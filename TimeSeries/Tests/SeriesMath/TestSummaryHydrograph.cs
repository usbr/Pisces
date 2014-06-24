using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Reclamation.TimeSeries;

namespace Pisces.NunitTests.SeriesMath
{   
    [TestFixture]
    public class TestSummaryHydrograph
    {

        [Test]
        public void SummaryHydrograph()
        {
            /*
 	value	near Date
10%	Missing	min value
20%	532.18	3/25/1997
80%	186.57	3/25/2002
90%	145.8	3/25/2001
             
             * */
            Series s = TestData.EntiatRiver;

            Console.WriteLine(s.Count);
            Assert.AreEqual(3642, s.Count, "data count has changed!");
            SeriesList list = Reclamation.TimeSeries.Math.SummaryHydrograph(s, new int[] { 5, 10, 20, 80, 90, 99 },
                                new DateTime(2000, 10, 1),false,false,false,false); // water year 2001

            Assert.AreEqual(6, list.Count, "expected 6 series one each for 5%, 10%, 20%, 80%, 90% and 99%");

            Series s5 = list[0];
            Series s10 = list[1];
            Series s20 = list[2];
            Series s80 = list[3];
            Series s90 = list[4];
            Series s99 = list[5];


            //s20.WriteToConsole(true, true);
            DateTime date = new DateTime(2001, 3, 25); // march 25

            Assert.AreEqual(Point.MissingValueFlag, s5.Lookup(date), "5% should be missing we don't extrapolate");

            Assert.AreEqual(651.5, s10.Lookup(date), .05, " 10% interpolated value");



            Assert.AreEqual(532.2, s20.Lookup(date), .05, " 20% exceedance value");
            Assert.AreEqual(193.8, s80.Lookup(date), .05, " 80% exceedance value");
            Assert.AreEqual(145.8, s90.Lookup(date), .05, " 90% exceedance value ");

            //Assert.AreEqual(1997, Convert.ToDateTime(s20[date].Notes).Year, " 20% exceedance Year");
            //Assert.AreEqual(2002, Convert.ToDateTime(s80[date].Notes).Year, " 80% exceedance Year");
            //Assert.AreEqual(2001, Convert.ToDateTime(s90[date].Notes).Year, " 90% exceedance Year");

            Assert.IsTrue(s99[s99.LookupIndex(date)].IsMissing, " 99% should be mising");



        }


    }
}
