using System;
using System.Collections.Generic;
using System.Text;
using Reclamation.Core;
using NUnit.Framework;
using Reclamation.TimeSeries;
using System.IO;
using Reclamation.TimeSeries.DataLogger;

namespace Pisces.NunitTests.SeriesTypes
{
    [TestFixture]
    public class Cr10xTest
    {

        public static void Main()
        {
            Cr10xTest t = new Cr10xTest();
            t.AshlandCreek();
        }

        [Test]
        public void JulianDate()
        {
            DateTime t = Cr10xSeries.JulianToDate(2005, 155);

            Assert.AreEqual(6, t.Month, "june");
            Assert.AreEqual(4, t.Day, "day 4");

            t = Cr10xSeries.JulianToDate(2004, 61);
            Assert.AreEqual(3, t.Month, "march");
            Assert.AreEqual(1, t.Day, "march");
        }

        [Test]
        public void ParseMinutes()
        {
            Assert.AreEqual(23 * 60 + 59.999, ParseMinutes("2400"), 0.01, "2400");
            Assert.AreEqual(60, ParseMinutes("100"), 0.01);
            Assert.AreEqual(90, ParseMinutes("130"), 0.01);
            Assert.AreEqual(20*60+30, ParseMinutes("2030"), 0.01);

        }

        private double ParseMinutes(string hhmm)
        {
            double minutes = 0;
            bool is2400 = false;
            if (Cr10xSeries.TryParseMinutes(hhmm, "", out minutes,out is2400))
                return minutes;

            return -1;

        }
        //[Test]
        //public void DataProbe()
        //{
        //    string fn = Path.Combine(GetDataPath(), "ASH_CRK.DAT");
        //    CsvFile file = Cr10xTextFile.ReadFile(fn);
        //    Assert.AreEqual("-159.3", file.Rows[13][9].ToString());
        //    Assert.AreEqual(DBNull.Value, file.Rows[13][10]);
        //    Assert.AreEqual((2351).ToString(), file.Rows[14][33]);
        //}


        //[Test]
        //public void TestColumnCount()
        //{
        //    string fn = Path.Combine(GetDataPath(),"ASH_CRK.DAT");
        //    CsvFile file = Cr10xTextFile.ReadFile(fn);
        //    Assert.AreEqual(34,file.Columns.Count);
        //}


        [Test]
        public void AshlandCreek()
        {
            string dataPath = GetDataPath();
            //Series s = new Cr10xSeries(dataPath, "filename=ASH_CRK.DAT;interval=24;ColumnNumber=5");
            Series s = new Cr10xSeries(Path.Combine(dataPath, "ASH_CRK.dat"), 24, 5);

            s.Read();
            Assert.IsTrue(s.Count > 0);
            Assert.AreEqual(12.63, s[0].Value);
            // Average Creek_Cfs line 68 in file
            //Series s2 = new Cr10xSeries( dataPath,"filename=ASH_CRK.DAT;interval=30;ColumnNumber=9");
            Series s2 = new Cr10xSeries(Path.Combine(dataPath, "ASH_CRK.dat"), 30, 9);

            s2.Read();
            DateTime t = new DateTime(2005,6,4,1,30,0);
            Assert.AreEqual(12.41, s2.Lookup(t), "june 4 2005 creek_cfs");
            //

        }

        private static string GetDataPath()
        {
           return TestData.DataPath + "\\Cr10x";
            //string path = AssemblyUtility.GetAssemblyPath("Reclamation.TimeSeries");
            //Console.WriteLine(path);
            //path = System.IO.Directory.GetParent(path).Parent.Parent.FullName;
            //path = Path.Combine(path, "HydrometPisces");
            //string dataPath = path + "\\Rogue Data Logger";
            //Console.WriteLine(dataPath);
            //return dataPath;
        }

    }
}
