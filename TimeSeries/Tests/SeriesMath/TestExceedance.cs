using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Reclamation.TimeSeries;
using Math = Reclamation.TimeSeries.Math;
using Reclamation.Core;
namespace Pisces.NunitTests.SeriesMath
{
    [TestFixture]
    public class TestExceedance
    {

        [Test]
        public void SouthForkBoiseSeptember()
        {
            Series s  = TestData.SouthForkBoise;

            Assert.AreEqual(s.Count, 22346, " unexpected number of points");

            Series s2  = Reclamation.TimeSeries.Math.Subset(s, new int[] { 7 }); // july.

            Series e = Reclamation.TimeSeries.Math.Sort(s2, RankType.Weibul);

            Point pt = e[0];

            Assert.AreEqual(4030.0, pt.Value,0.01);
            Assert.AreEqual(.05, pt.Percent,0.01);


            Point min = e[e.Count - 1];

            Assert.AreEqual(99.95, min.Percent, 0.01);
            Assert.AreEqual(117.00, min.Value, 0.01);


            // again with exceedance method.
            Series s3 = new TextSeries(TestData.DataPath + "\\" + "SouthForkOfBoiseNearFeatherville.txt");

            Series e2 = s3.Exceedance(TimeSeriesDatabase.MinDateTime, TimeSeriesDatabase.MaxDateTime,
                new MonthDayRange(7, 1, 7, 31), RankType.Weibul);

            pt = e2[0];
            Assert.AreEqual(4030.0, pt.Value, 0.01);
            Assert.AreEqual(.05, pt.Percent, 0.01);


        }
        


        [Test]
        public void EntiatRiver()
        {
            Series s = TestData.EntiatRiver;

            Console.WriteLine(s.Count);
            Assert.AreEqual(3642, s.Count, "data count has changed!");
            
            Series sorted = Math.Sort(s, RankType.Weibul);

            TestEntiatRiverSortedValues(sorted);
            //sorted.WriteToConsole(false, true);
            //1822	50.01372495
        }

        public static void TestEntiatRiverSortedValues(Series sorted)
        {
            //sorted.WriteToConsole(true, true);
            Assert.AreEqual(4810, sorted[0].Value, 0.01, "Max value not sorted to top");

            Assert.AreEqual(58, sorted[sorted.Count - 1].Value, 0.01, "Max value not sorted to top");


            //36	0.988196541	2950
            Assert.AreEqual(2950, sorted[35].Value, 0.001, " value ");
            Assert.AreEqual(0.988196541, sorted[35].Percent, 0.01, " percentage ");

        }

        /*
Date	cfs	Rank	percent	Flow (cfs)
3/15/1996	675	1	4.0	762
3/16/1996	746	2	8.0	761
3/17/1996	760	3	12.0	760
3/18/1996	761	4	16.0	757
3/19/1996	762	5	20.0	749
3/20/1996	757	6	24.0	749
3/21/1996	749	7	28.0	746
3/22/1996	749	8	32.0	731
3/23/1996	731	9	36.0	700
3/24/1996	700	10	40.0	675
3/25/1996	663	11	44.0	663
3/26/1996	650	12	48.0	650
3/27/1996	628	13	52.0	642
3/28/1996	599	14	56.0	628
3/29/1996	579	15	60.0	599
3/30/1996	559	16	64.0	579
3/31/1996	545	17	68.0	559
4/1/1996	540	18	72.0	545
4/2/1996	521	19	76.0	540
4/3/1996	497	20	80.0	521
4/4/1996	487	21	84.0	517
4/5/1996	489	22	88.0	497
4/6/1996	517	23	92.0	489
4/7/1996	642	24	96.0	487

         */

        [Test]
        public void EntiatRiver24Points()
        {
            Series s = TestData.EntiatRiver24Points;

            Console.WriteLine(s.Count);
            Assert.AreEqual(24, s.Count, "data count has changed!");

            Series sorted = Math.Sort(s, RankType.Weibul);//new ExceedanceSeries(s);

            sorted.WriteToConsole(true, true);
            Assert.AreEqual(762, sorted[0].Value, 0.01, "Max value not sorted to top");

            Assert.AreEqual(487, sorted[sorted.Count - 1].Value, 0.01, "Max value not sorted to top");

            Assert.AreEqual(642, sorted[12].Value, 0.001, "  value ");
            Assert.AreEqual(52, sorted[12].Percent, 0.01, " percentage ");
        }

    }
}
