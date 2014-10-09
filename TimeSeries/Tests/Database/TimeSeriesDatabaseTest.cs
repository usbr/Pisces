using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using Reclamation.TimeSeries;
using Reclamation.Core;
using System.IO;
using Math = Reclamation.TimeSeries.Math;
using Reclamation.TimeSeries.Excel;
using Reclamation.TimeSeries.Hydromet;

namespace Pisces.NunitTests.Database
{
    /// <summary>
    /// Summary description for DatabaseSeriesFactory
    /// </summary>
    [TestFixture]
    public class TimeSeriesDatabaseTest
    {


        TimeSeriesDatabase db;
       // string dataPath;
        string textFileName = "el68d_DigitizedChart.txt";
        string excelFileName = "ImportWithUpdate.xls";
        string updatedExcelFileName = "ImportWithUpdate2.xls";

        string tmpDir = @"C:\temp\db_test"; // test relative paths

        public TimeSeriesDatabaseTest()
        {
            if (!Directory.Exists(tmpDir))
            {
                Directory.CreateDirectory(tmpDir);
            }
            string fn =Path.Combine(@"C:\temp","factory.pdb");
            FileUtility.GetTempFileNameInDirectory(@"C:\temp\",".pdb");

            SQLiteServer.CreateNewDatabase(fn);
            SQLiteServer svr = new SQLiteServer(fn);
            db = new TimeSeriesDatabase(svr);

            //string dataPath = ReclamationTesting.Properties.Settings.Default.DataPath;
            string dataPath = TestData.DataPath;
           
            File.Copy(Path.Combine(dataPath, textFileName), Path.Combine(tmpDir, textFileName),true);
            textFileName = Path.Combine(tmpDir, textFileName);

            File.Copy(Path.Combine(dataPath, excelFileName), Path.Combine(tmpDir, excelFileName),true);
            excelFileName = Path.Combine(tmpDir, excelFileName);

            File.Copy(Path.Combine(dataPath, updatedExcelFileName), Path.Combine(tmpDir, updatedExcelFileName), true);
            updatedExcelFileName = Path.Combine(tmpDir, updatedExcelFileName);
   


            // Add some data for export test
            Series s;
            int c;
            int sdi;
            AddExcelSeries(out s, out c, out sdi);
            AddTextSeries(out s, out c, out sdi);


        }

        
       
        /// <summary>
        /// TextSeries can be stored in the database and automatically update from
        /// the original text file if file has changed, and file still exists
        /// </summary>
        [Test]
        public void TextFactory()
        {
            Series s;
            int c;
            int sdi;
            AddTextSeries(out s, out c, out sdi);

            s = db.GetSeries(sdi);
            s.Read();
            Assert.AreEqual(c, s.Count);
            Assert.AreEqual("cfs", s.Units);
            Console.WriteLine(s.Units);

            s = db.GetSeries(sdi);
            DateTime t1 = new DateTime(1999,1,1);
            DateTime t2 = new DateTime(1999,1,1,23,59,59);
            s.Read(t1, t2);

            Assert.AreEqual(43, s.Count);
        }

        private void AddTextSeries(out Series s, out int c, out int sdi)
        {
            s = TextSeries.ReadFromFile(textFileName);
            s.Units = "cfs";
            //s.Table.TableName = "ts"+;
            c = s.Count;
            Assert.IsTrue(s.Count > 0);
            sdi = db.AddSeries(s);
        }


        [Test]
        public void ExcelFactory()
        {
            Series s;
            int c;
            int sdi;
            AddExcelSeries(out s, out c, out sdi);
            

            s = db.GetSeries(sdi);
            s.Read();
            Assert.AreEqual(c, s.Count);

            s = db.GetSeries(sdi);
            DateTime t1 = new DateTime(2007, 4, 30);
            s.Read(t1, t1.AddDays(1));

            Assert.AreEqual(1, s.Count);

            // simulate updating excel file.
            File.Copy(updatedExcelFileName, excelFileName,true);
            // copy doesn't update LaswWriteTime so cache doesn't see a change
            File.SetLastWriteTime(excelFileName, DateTime.Now);
            s = db.GetSeries(sdi);
            s.Read();
            Console.WriteLine("After update count = "+s.Count);
            Assert.AreEqual(c + 1, s.Count);
        }

        
        private void AddExcelSeries(out Series s, out int c, out int sdi)
        {
            var fn = excelFileName;
            s = new ExcelDataReaderSeries(fn, "NoDuplicates", "Date Sampled", "Field Temp C");
            s.Read();
            c = s.Count;
            Console.WriteLine("Count = "+c);
            Assert.IsTrue(s.Count > 0);
            sdi = db.AddSeries(s);

           
        }


        [Test]
        public void HydrometDailyFactory()
        {
            Series s = new Reclamation.TimeSeries.Hydromet.HydrometDailySeries("jck", "af");
            s.Read(DateTime.Now.AddDays(-365), DateTime.Now.Date);
            int sdi = db.AddSeries(s);
            Assert.AreEqual("acre-feet", s.Units);

            s = db.GetSeries(sdi);
            s.Read(DateTime.Now.AddDays(-365),DateTime.Now.Date);

            Assert.IsTrue(s.Count> 100);
            Assert.IsTrue(s.ConnectionString.Contains("jck"));

        }


        [Test]
        public void HydrometAutoUpdate()
        {
            var t1 = new DateTime(1980, 10, 1);
            var t2 = new DateTime(1980, 10, 2);
            Series s1 = new Reclamation.TimeSeries.Hydromet.HydrometDailySeries("jck", "af");
            s1.Read(t1, t2);
            int sdi1 = db.AddSeries(s1);

            Series s2 = new Reclamation.TimeSeries.Hydromet.HydrometDailySeries("pal", "af");
            s2.Read(t1, t2);
            int sdi2 = db.AddSeries(s2);

            s1 = db.GetSeries(sdi1);
            s2 = db.GetSeries(sdi2);

            HydrometInfoUtility.AutoUpdate = true;
            t2 = t2.AddHours(24);// reservoir contents are stored at midnight
            Console.WriteLine(t2);
            s1.Read(t1, t2);
            s2.Read(t1, t2);

            s1.WriteToConsole();
            Assert.AreEqual(515150.0, s1[0].Value);
            Assert.AreEqual(817782.0, s2[0].Value);
            Assert.AreEqual(3, s1.Count);
            Assert.AreEqual(3, s2.Count);

            SeriesList sl = new SeriesList();
            sl.Add(s1);
            sl.Add(s2);
            SimpleMathSeries c1 = new SimpleMathSeries("",sl,new MathOperation[]{ MathOperation.Add});
            SimpleMathSeries c2 = new SimpleMathSeries("",sl, new MathOperation[] {MathOperation.Subtract});

            int sdi3 = db.AddSeries(c1);
            int sdi4 = db.AddSeries(c2);

            Series s3 = db.GetSeries(sdi3);
            Series s4 = db.GetSeries(sdi4);

            s3.Read(t1, t2);
            s4.Read(t1, t2);

            Assert.AreEqual(515150.0 + 817782.0, s3[0].Value);
            Assert.AreEqual(515150.0 - 817782.0, s4[0].Value);

        }




        [Test]
        public void ExportDatabase()
        {
            db.Export(@"C:\temp\export1");
           // db.Import(@"C:\temp\export1");
        }

        

    }
}
