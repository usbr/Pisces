using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Reclamation.TimeSeries;
using System.IO;
using Reclamation.Core;
using Reclamation.TimeSeries.Excel;
using Reclamation;
namespace Pisces.NunitTests.SeriesTypes
{
    [TestFixture]
   public class TestExcelReaderSeries
    {

        public static void Main()
        {
            var x = new TestExcelReaderSeries();
            x.ReadOneYearFiltering();
        }
       
        [Test]
        public void Create()
        {
            string fn = Path.GetTempFileName();
            File.Delete(fn);
            fn = Path.ChangeExtension(fn, ".xls");
            Console.WriteLine("creating " + fn);
            ExcelUtility.CreateXLS(fn);
            Assert.IsTrue(File.Exists(fn));

        }


        [Test]
        public void SimpleRead()
        {
            string filename = TestData.DataPath + "\\SpecificationTestData.xls";
            Assert.IsTrue(System.IO.File.Exists(filename),"missing file "+ filename);
            var s = new ExcelDataReaderSeries(filename, "Sheet1", "Date", "JulianDay");
            s.Read();
            for (int i = 0; i < s.Messages.Count; i++)
            {
                Console.WriteLine(s.Messages[i]);                
            }

            Assert.IsTrue(s.Count > 1000, "not enough data in " + filename);

        }

        [Test]
       public void LargeRead()
       {
           Performance perf = new Performance();
           string filename = TestData.DataPath+@"\UnregulationUpperSnake.xls";
           Assert.IsTrue(System.IO.File.Exists(filename), "missing file " + filename);
           ExcelDataReaderSeries s = new ExcelDataReaderSeries(filename, "data", "Date", "HEII QD");
           s.Read();
           for (int i = 0; i < s.Messages.Count; i++)
           {
               Console.WriteLine(s.Messages[i]);
           }

           Assert.AreEqual(28942, s.Count, "wrong number of points from " + filename);
           perf.Report("large read took "); // 5.7 seconds ExcelOle (full startup)
       }

       [Test]
       public void ReadOneYearFiltering()
       {
           string filename = TestData.DataPath+ @"\UnregulationUpperSnake.xls";
           Assert.IsTrue(System.IO.File.Exists(filename), "missing file " + filename);
           var s = new ExcelDataReaderSeries(filename, "data", "Date", "HEII QD");
           s.Read(DateTime.Parse("2005-01-01"), DateTime.Parse("2005-12-31"));
           for (int i = 0; i < s.Messages.Count; i++)
           {
               Console.WriteLine(s.Messages[i]);
           }

           Assert.AreEqual(365, s.Count, "wrong number of points from " + filename);

       }

      
    }
}
