using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Reclamation.Core;
using Reclamation.TimeSeries;
using System.IO;
using Math = Reclamation.TimeSeries.Math;

namespace Pisces.NunitTests.Estimation
{
    [TestFixture]
    public class TestUofIEstimation
    {

        public static void Main()
        {
            TestUofIEstimation t = new TestUofIEstimation();
            t.UofIDisaggregation();
        }

        static DateTime t1 = new DateTime(1997, 1, 1);
        static DateTime t2 = new DateTime(1997, 12, 31);

        string path = "";
        public TestUofIEstimation()
        {
            string zipFile = Path.Combine(TestData.DataPath, "UofIDisaggregationTest.zip");
             path = FileUtility.GetTempPath() + @"\UofIDisaggregationTest.pdb";
            ZipFileUtility.UnzipFile(zipFile,path);
        }

        [Test]
        public void UofIDisaggregation()
        {
            SQLiteServer pDB = new SQLiteServer(path);
            TimeSeriesDatabase DB = new TimeSeriesDatabase(pDB,false);

            // Reads input data required by the calculation
            Series daily = DB.GetSeriesFromName("SS_Day_Mean");
            daily.Read();// Source Station Daily data
            Series monthly = DB.GetSeriesFromName("TS_Mon_Mean");
            monthly.Read();// Target Station Monthly data
            Series known= DB.GetSeriesFromName("C#Disaggregated");
            known.Read(t1,t2);
            
            Series infilled = Math.RMSEInterp(daily, monthly);
            var s = infilled.Subset(t1, t2);
            s.TimeInterval = TimeInterval.Daily;
            
            var diff = Math.Sum(known - s);
            Assert.IsTrue(System.Math.Abs(diff) < 0.01,"Error");
        }

        [Test]
        public void UofIInterpolation()
        {
            SQLiteServer pDB = new SQLiteServer(path);
            TimeSeriesDatabase DB = new TimeSeriesDatabase(pDB,false);

            // Reads input data required by the calculation
            Series daily = DB.GetSeriesFromName("SS_Day_Mean");
            Series monthly = DB.GetSeriesFromName("TS_Mon_Mean");
            Series known = DB.GetSeriesFromName("C#Disaggregated-Interpolated");
            daily.Read();
            monthly.Read();
            known.Read(t1, t2);

            Series infilled = Math.UofIStreamflowDisaggregation(daily, monthly);
            var s = infilled.Subset(t1, t2);
            double diff = 0.0;
            for (int i = 0; i < known.Count; i++)
            { 
                diff += (known[i].Value - s[i].Value);
            }

            Assert.AreEqual(0.00, diff, 0.01);
        }

        [Test]
        public void TestRMSEMassBalance()
        {
            SQLiteServer pDB = new SQLiteServer(path);
            TimeSeriesDatabase DB = new TimeSeriesDatabase(pDB,false);

            // Reads input data required by the calculation
            Series daily = DB.GetSeriesFromName("CHEI_QD");
            Series monthly = DB.GetSeriesFromName("CHEI_QM");
            daily.Read();
            monthly.Read();

            // disaggregated daily summed to monthly acre-feet
            Series infilled = Math.RMSEInterp(daily, monthly);
            Series infilledMonthlySumAcreFeet = Math.MonthlySum(infilled) * 1.98347;
            infilledMonthlySumAcreFeet.TimeInterval = TimeInterval.Monthly;
            
            // get equal time period for infilled data to original monthly data
            Series s = infilledMonthlySumAcreFeet.Subset(monthly.MinDateTime, monthly.MaxDateTime);

            var diff = System.Math.Abs(Math.Sum(monthly - s));
            Assert.IsTrue(diff < 0.01, "UofI RMSEInterp mass balance failed by: " + diff);
        }

        [Test]
        public void TestMergeMassBalance()
        {
            SQLiteServer pDB = new SQLiteServer(path);
            TimeSeriesDatabase DB = new TimeSeriesDatabase(pDB,false);

            // Reads input data required by the calculation
            Series daily = DB.GetSeriesFromName("CHEI_QD");
            Series monthly = DB.GetSeriesFromName("CHEI_QM");
            daily.Read();
            monthly.Read();

            // disaggregate and merge
            Series infilled = Math.RMSEInterp(daily, monthly);
            Math.MergeCheckMassBalance(daily, infilled);

            // generate series of monthly volumes only for months with a computed value,
            // these will be compared to the observed monthly
            Series partialMonthlyEstimated = new Series();
            for (int i = 0; i < infilled.Count; i++)
            {
                Point p = infilled[i];

                // Gets the data for the month
                int numDays = DateTime.DaysInMonth(p.DateTime.Year, p.DateTime.Month);
                DateTime t1 = new DateTime(p.DateTime.Year, p.DateTime.Month, 1);
                DateTime t2 = new DateTime(p.DateTime.Year, p.DateTime.Month, numDays);

                if (p.Flag == PointFlag.Computed && partialMonthlyEstimated.IndexOf(t1) < 0)
                {
                    partialMonthlyEstimated.Add(t1, Math.Sum(infilled.Subset(t1, t2)) * 1.98347);
                }
            }

            // check observed against infilled for months where data was infilled
            double diff = 0.0;
            for (int i = 0; i < partialMonthlyEstimated.Count; i++)
            {
                DateTime estDate = partialMonthlyEstimated[i].DateTime;
                if (monthly.IndexOf(estDate) > 0)
                {
                    diff += (monthly[estDate].Value - partialMonthlyEstimated[estDate].Value);
                }
            }

            Assert.IsTrue(diff < 0.01, "UofI merge mass balance failed by: " + diff);
        }
    }
}