using System;
using NUnit.Framework;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Excel;
using System.IO;
using Reclamation.Core;

namespace Pisces.NunitTests.SeriesTypes
{
    [TestFixture]
   public class TestSpreadsheetGearSeries
    {

        [Test]
        [ExpectedException(typeof(System.Data.DuplicateNameException))]
        public void ColumnNames()
        {
            string filename = TestData.DataPath + "\\ImportWithUpdate.xls";
            SpreadsheetGearExcel db = new SpreadsheetGearExcel(filename);

            string[] cols = db.ColumnNames("Duplicates");
            foreach (var s in cols)
            {
                Console.WriteLine(s);
            }

            Assert.AreEqual(34, cols.Length);
        }

        [Test]
        public void MonthlyIntervalDetection()
        {
            string filename = TestData.DataPath + "\\unregulation calculations.xls";
            SpreadsheetGearExcel db = new SpreadsheetGearExcel(filename);
            SpreadsheetGearSeries s = new SpreadsheetGearSeries(filename, "Riverware returns", "A", "B");
            s.Read();
            Assert.AreEqual(TimeInterval.Monthly, s.TimeInterval);
        }
        


        [Test]
        public void ColumnReferenceNames()
        {
            int i = SpreadsheetGearSeries.ColumnIndexFromRef("A");
            Assert.AreEqual(0, i);
            i = SpreadsheetGearSeries.ColumnIndexFromRef("AA");
            Assert.AreEqual(26, i);
            i = SpreadsheetGearSeries.ColumnIndexFromRef("IV");
            Assert.AreEqual(255, i);


            string s = SpreadsheetGearExcel.ReferenceFromIndex(0);
            Assert.AreEqual("A",s);
            s = SpreadsheetGearExcel.ReferenceFromIndex(26);
            Assert.AreEqual("AA", s);
            s = SpreadsheetGearExcel.ReferenceFromIndex(255);
            Assert.AreEqual("IV", s);


        }


        

        [Test]
        public void MissingTimeBug()
        {
            string filename = TestData.DataPath + "\\Basin.xls";
            SpreadsheetGearSeries s = new SpreadsheetGearSeries(filename, "Basin", "Date/Time", "Temperature   (*C)");

            s.Read();
            Assert.AreEqual(6722, s.Count, "series count");
            Console.WriteLine(s.Count);
        }

        private static SpreadsheetGearSeries GetSimpleSeries(string filename)
        {
          SpreadsheetGear.IWorkbook workbook = SpreadsheetGear.Factory.GetWorkbook(filename);
          string sheetName = workbook.Sheets[0].Name;
          SpreadsheetGearSeries s = new SpreadsheetGearSeries(workbook, sheetName, "A", "B", "cfs");
          return s;
        }

        [Test]
        public void BasicRead()
        {
            string filename = TestData.DataPath + "\\MILIrunning.xls";
            SpreadsheetGearSeries s = GetSimpleSeries(filename);

            s.Read();
            Assert.AreEqual(2.61, s["02/03/05 04:47:14.0"].Value, 0.01);
            Console.WriteLine(s.Count);
        }

        [Test]
        public void DailyAverageBug()
        {
            string filename = TestData.DataPath + "\\Deadwood at SF.xls";
            SpreadsheetGearSeries s = GetSimpleSeries(filename);
            s.Read();

            //Assert.AreEqual(31740, s.Count);
            Assert.AreEqual(33812, s.Count);

            Point pt = Reclamation.TimeSeries.Math.TimeWeightedAverageForDay(s, DateTime.Parse("10/3/2006"));
            Console.WriteLine(pt);

            Assert.IsTrue(pt.IsMissing);
            pt = Reclamation.TimeSeries.Math.TimeWeightedAverageForDay(s, DateTime.Parse("10/4/2006"));
            Assert.IsTrue(pt.IsMissing);

        }


        [Test]
        public void SimpleRead()
        {
            string filename = TestData.DataPath+"\\SpecificationTestData.xls";
            Assert.IsTrue(System.IO.File.Exists(filename),"missing file "+ filename);
            Series s = SpreadsheetGearSeries.ReadFromFile(filename, "Sheet1", "Date", "JulianDay");

            for (int i = 0; i < s.Messages.Count; i++)
            {
                Console.WriteLine(s.Messages[i]);                
            }

            Assert.IsTrue(s.Count> 789, "not enough points" + filename);

        }

        [Test]
       public void LargeRead()
       {
           string filename = TestData.DataPath+@"\UnregulationUpperSnake.xls";
           Assert.IsTrue(System.IO.File.Exists(filename), "missing file " + filename);
            SpreadsheetGearSeries s =
               new SpreadsheetGearSeries(filename, "data", "Date", "HEII QD");
           s.Read();
           //s.WriteToConsole();
           for (int i = 0; i < s.Messages.Count; i++)
           {
               Console.WriteLine(s.Messages[i]);
           }

           Assert.AreEqual(28942, s.Count, "wrong number of points from " + filename);

       }
        [Test]
        public void UsingColumnReferenceName()
        {
            string filename = TestData.DataPath + @"\QCBID BIOP Data_kt.xls";
            Assert.IsTrue(System.IO.File.Exists(filename), "missing file " + filename);
            SpreadsheetGearSeries s = new SpreadsheetGearSeries(filename, "CC at 5-mile", "C", "J");
            s.Read();
            for (int i = 0; i < s.Messages.Count; i++)
            {
                Console.WriteLine(s.Messages[i]);
            }

            Assert.AreEqual(126.3, s[11].Value, .01);
        }

        /// <summary>
        /// Test read from connection string
        /// </summary>
        [Test]
        public void ConnectionString()
        {
            string filename = TestData.DataPath + "\\SpecificationTestData.xls";
            Assert.IsTrue(System.IO.File.Exists(filename), "missing file " + filename);
            Series s = SpreadsheetGearSeries.ReadFromFile(filename, "Sheet1", "Date", "JulianDay");
            int count = s.Count;
            Assert.IsTrue(s.Count > 100);
            s.Clear();
            Assert.IsTrue(s.Count == 0);
            Series s2 = SpreadsheetGearSeries.CreateFromConnectionString(s.ConnectionString,"");
            s2.Read();
            Assert.AreEqual(count, s2.Count);

        }

        [Test]
        public void WaterYearFormat()
        {
            string filename = TestData.DataPath + "\\wateryear.xls";
            Assert.IsTrue(System.IO.File.Exists(filename), "missing file " + filename);

            SpreadsheetGearSeries xls = new SpreadsheetGearSeries(filename,
                "Sheet1", "WaterYear", "Oct",true);
            xls.Read();

            Assert.AreEqual(12, xls[0].Value, 0.001);
            Assert.AreEqual(DateTime.Parse("1889-10-1"), xls[0].DateTime);

            Assert.AreEqual(1100, xls[DateTime.Parse("1934-6-1")].Value);

            xls.WriteCsv(@"c:\temp\a.csv");
        }

        [Test]
        public void DatabaseFormat()
        {
            string filename = Path.Combine(TestData.DataPath, "DatabaseStyleExcelIdwr.xlsx");
            Assert.IsTrue(System.IO.File.Exists(filename), "missing file " + filename);

            SpreadsheetGearSeries xls = new SpreadsheetGearSeries(filename,
                "Data", "DataDate", "Flow",false, "SiteID","13202995");
            xls.Read();

            Assert.AreEqual(5696, xls.Count);
            Assert.AreEqual(0, xls[0].Value, 0.001);
            Assert.AreEqual(10, xls["1986-6-1"].Value, 0.001);
        }



        [Test, Category("Internal")]
        public void LargeImport()
        {

            Performance perf = new Performance();
            string dataPath = TestData.DataPath;
            string fn = @"T:\PN6200\Staff\KTarbet\PiscesSampleData\CraigAddley\Testout.txt";
            Console.WriteLine(fn);
            Assert.IsTrue(File.Exists(fn),"Missing file "+fn);
            SpreadsheetGearExcel xls = new SpreadsheetGearExcel(fn);
            perf.Report("done reading " + fn);
            Series s = SpreadsheetGearSeries.ReadFromWorkbook(xls.Workbook,"sheet1", "DateTime", "flow",false,"cfs");
            s.Read();
            Assert.AreEqual(666282, s.Count);
            //Series s = new ExcelDataReaderSeries(fn, "sheet1", "DateTime", "flow","cfs");


            perf.Report("done importing excel file");
            /*
             * 666282 records
             * 
             *  SaveTable    ==> 286 seconds
             *  InsertTable  ==> 64 seconds
             */

            // perform exceedance calculation
            //ExplorerView v = new ExplorerView();
            //Explorer explorer = new Explorer(v, db);

            //explorer.MonthDayRange 

        }

    }
}
