using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Reclamation.TimeSeries.Usgs;

namespace Pisces.NunitTests.SeriesTypes
{
    [TestFixture]
    public class TestUsgs
    {

        [Test]
        public void TestRealTime()
        {
            string site_no = "13069500"; // / SNAKE RIVER NR BLACKFOOT ID
            UsgsRealTimeParameter parameter = UsgsRealTimeParameter.Discharge;
            UsgsRealTimeSeries s = new UsgsRealTimeSeries(site_no, parameter);
            s.Read(new DateTime(2011,1,12),new DateTime(2011,1,13));
            Assert.AreEqual(192, s.Count);
            Assert.AreEqual(2240, s[0].Value);
        }

        [Test]
        public void TestDaily()
        {
            UsgsDailyValueSeries s = new UsgsDailyValueSeries("13190500",UsgsDailyParameter.DailyMeanDischarge);
            s.Read(new DateTime(2003, 10, 1), DateTime.Now);
            s.WriteCsv(@"C:\temp\andi_qd_usgs.csv");

        }

        [Test]
        public void TestGroundWaterLevels()
        {
            UsgsGroundWaterLevelSeries s = new UsgsGroundWaterLevelSeries("444401116463001");
            s.Read();

            Assert.AreEqual(1974, s[0].DateTime.Year);

            Assert.AreEqual(14, s[95].DateTime.Hour);
            //Point pt = s["1988-10-04"]
        }


        [Test]
        public void TestUsgsQualificationCodes()
        {
            UsgsDailyValueSeries s = new UsgsDailyValueSeries("13168500", UsgsDailyParameter.DailyMeanTemperature);

            var t1 = new DateTime(1997,8,1);
            var t2 = new DateTime(1997,8,15);
            s.Read(t1, t2);
            s.RemoveMissing();
            Assert.AreEqual(23.5, s[0].Value);
            Assert.AreEqual("A", s[0].Flag);
            s.WriteToConsole(true);
            /*
USGS	13168500	1997-08-02	177	A	26.6	A	21.1	A	23.5	A
USGS	13168500	1997-08-03	163	A	27.5	A	20.8	A	23.9	A
USGS	13168500	1997-08-04	153	A	25.7	A	22.8	A	23.9	A
USGS	13168500	1997-08-05	144	A	27.5	A	21.6	A	24.2	A
USGS	13168500	1997-08-06	149	A	28.2	A	21.9	A	24.7	A
USGS	13168500	1997-08-07	140	A	27.5	A	22.4	A	25.0	A
USGS	13168500	1997-08-08	135	A	26.9	A	22.3	A	24.4	A
USGS	13168500	1997-08-09	131	A	26.0	A	20.9	A	23.2	A
USGS	13168500	1997-08-10	130	A	27.1	A	21.6	A	23.9	A

             */
        }



    }
}
