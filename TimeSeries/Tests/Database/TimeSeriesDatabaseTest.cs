using NUnit.Framework;
using Reclamation.Core;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Hydromet;
using System;
using System.IO;

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

        string tmpDir;

        public TimeSeriesDatabaseTest()
        {
            string path;
            if (LinuxUtility.IsLinux())
            {
                path = "/tmp";
            }
            else
            {
                path = "C:\\Temp\\";
            }

            tmpDir = Path.Combine(path, "db_test");

            if (!Directory.Exists(tmpDir))
            {
                Directory.CreateDirectory(tmpDir);
            }
            string fn = Path.Combine(path, "factory.pdb");
            FileUtility.GetTempFileNameInDirectory(path, ".pdb");

            SQLiteServer.CreateNewDatabase(fn);
            SQLiteServer svr = new SQLiteServer(fn);
            db = new TimeSeriesDatabase(svr, false);

            //string dataPath = ReclamationTesting.Properties.Settings.Default.DataPath;
            string dataPath = TestData.DataPath;

            File.Copy(Path.Combine(dataPath, textFileName), Path.Combine(tmpDir, textFileName), true);
            textFileName = Path.Combine(tmpDir, textFileName);



            // Add some data for export test
            Series s;
            int c;
            int sdi;
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
            DateTime t1 = new DateTime(1999, 1, 1);
            DateTime t2 = new DateTime(1999, 1, 1, 23, 59, 59);
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
        public void HydrometDailyFactory()
        {
            Series s = new Reclamation.TimeSeries.Hydromet.HydrometDailySeries("jck", "af", HydrometHost.PNLinux);
            s.Read(DateTime.Now.AddDays(-365), DateTime.Now.Date);
            int sdi = db.AddSeries(s);
            Assert.AreEqual("acre-feet", s.Units);

            s = db.GetSeries(sdi);
            s.Read(DateTime.Now.AddDays(-365), DateTime.Now.Date);

            Assert.IsTrue(s.Count > 100, "count = " + s.Count);
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
            SimpleMathSeries c1 = new SimpleMathSeries("", sl, new MathOperation[] { MathOperation.Add });
            SimpleMathSeries c2 = new SimpleMathSeries("", sl, new MathOperation[] { MathOperation.Subtract });

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
            string path;
            if (LinuxUtility.IsLinux())
            {
                path = "/tmp";
            }
            else
            {
                path = "C:\\Temp\\";
            }

            db.Export(Path.Combine(path, "export1"));
        }
    }
}
