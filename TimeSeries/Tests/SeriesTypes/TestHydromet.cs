using System;
using NUnit.Framework;
using Reclamation.Core;

using System.IO;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.TimeSeries;

namespace Pisces.NunitTests.SeriesTypes
{
    [TestFixture]
    public class TestHydromet
    {
        /*
         * https://www.usbr.gov/pn-bin/webarccsv.pl?parameter=AGA%20AF,AGA%20QD,AGA%20QJ,ANTO%20QD,ANTO%20QJ&syer=2001&smnth=1&sdy=5&eyer=2001&emnth=1&edy=12&format=2
BEGIN DATA
DATE      ,    AGA AF  ,    AGA QD  ,    AGA QJ  ,   ANTO QD  ,   ANTO QJ  
01/05/2001,     1203.12,        0.00,MISSING     ,NO RECORD   ,NO RECORD   
01/06/2001,     1205.91,        0.00,MISSING     ,NO RECORD   ,NO RECORD   
01/07/2001,     1208.70,        0.00,MISSING     ,NO RECORD   ,NO RECORD   
01/08/2001,     1211.49,        0.00,MISSING     ,NO RECORD   ,NO RECORD   
01/09/2001,     1213.35,        0.00,MISSING     ,NO RECORD   ,NO RECORD   
01/10/2001,     1252.92,        0.00,MISSING     ,NO RECORD   ,NO RECORD   
01/11/2001,     1301.42,        0.00,MISSING     ,NO RECORD   ,NO RECORD   
01/12/2001,     1350.70,        0.00,MISSING     ,NO RECORD   ,NO RECORD   
END DATA
         */



        [Test]
        public void SiteDescription()
        {
            Logger.EnableLogger();
            string d = HydrometInfoUtility.LookupSiteDescription("ACAO");
            Console.WriteLine(d);
            Assert.AreEqual("Ashland Creek Mouth, near Ashland", d);
        }

        [Test]
        public void DayFileWebQuery()
        {
            DateTime t1 = new DateTime(2004, 1, 1);
            DateTime t2 = new DateTime(2004, 1, 2);

            Series s = HydrometInfoUtility.Read("jck", "af", t1, t2, TimeInterval.Irregular, HydrometHost.PN);
            s.WriteToConsole();
            Assert.AreEqual(192, s.Count, "number of records read");
            //  143482.03 
            DateTime t = DateTime.Parse("01/01/2004 15:15");
            Assert.AreEqual(143482.03, s.Lookup(t), 0.01, "value on jan 1, 2005 15:15");
        }


        [Test]
        public void GPBuffaloBillReservoirDaily()
        {
            var s = new HydrometDailySeries("bbr", "af",HydrometHost.GreatPlains);
            s.Read(new DateTime(2007, 10, 1), new DateTime(2007, 10, 5));

            Assert.IsTrue(s.Count == 5);
            Assert.AreEqual(415879, s["2007-10-1"].Value,.01);
        }

        [Test]
        public void JacksonLakeDaily()
        {
            var s = new HydrometDailySeries("jck", "qd");
            s.Read(new DateTime(2007, 10, 1), new DateTime(2007, 10, 5));

            Assert.IsTrue(s.Count == 5);
            Assert.AreEqual(1910.0, s["2007-10-1"].Value);
        }

        [Test]
        public void YakimaTICW()
        {
            
            Series s = HydrometDailySeries.Read("ticw", "qd",
                DateTime.Parse("2006-09-15"),
                DateTime.Parse("2006-09-20"), HydrometHost.Yakima);


            Series s2 = HydrometDailySeries.Read("ticw", "qd",
                DateTime.Parse("2006-09-15"),
                DateTime.Parse("2006-09-20"), HydrometHost.Yakima);


            DateTime[] dates ={
            DateTime.Parse("09/15/2006"),
            DateTime.Parse("09/16/2006"),
            DateTime.Parse("09/17/2006"),
            DateTime.Parse("09/18/2006"),
            DateTime.Parse("09/19/2006"),
            DateTime.Parse("09/20/2006")};
            double[] values = { 2208, 2066, 1950, 1795, 1635, 1592 };

            //double[] values ={ 2016.55, 1885.63, 1795.19, 1652.62, 1515.23, 1479.95 };

            for (int i = 0; i < s.Count; i++)
            {
                Assert.AreEqual(dates[i], s[i].DateTime, "internal server");
                Assert.AreEqual(values[i], s[i].Value, 0.01, "internal server");
            }

            for (int i = 0; i < s.Count; i++)
            {
                Assert.AreEqual(dates[i], s[i].DateTime, "external server");
                Assert.AreEqual(values[i], s[i].Value, 0.01, "external server");
            }
        }

        [Test]
        public void ShefA()
        {
            var s = new HydrometDailySeries("LUC", "QU");
            DateTime t1 = new DateTime(2012, 10, 27);
            DateTime t2 = new DateTime(2012, 10, 27);
            s.Read(t1, t2);

            var fn = FileUtility.GetTempFileName(".csv");
            File.AppendAllText(fn,"cbtt,pcode,shef_locid,shef_tag,time_zone,scale\n"
+"LUC,QU,LUCI1,QADRZ,M,0.001");
            
            var txt = HydrometDailySeries.CreateSHEFA(fn, t1, t2);

            Assert.AreEqual(".A LUCI1 20121027 M DH2400/QADRZ 0.802", txt[0]);


        }

        /// <summary>
        /// Hydromet mpoll stores the average for water years 1960-1989
        /// in the year 6189
        /// </summary>
        //[Test]
        public void Monthly30yrAverage()
        {
            HydrometMonthlySeries s = new HydrometMonthlySeries("jkpi", "sem");
            DateTime t1 = new DateTime(6189, 10, 1);
            DateTime t2 = new DateTime(6190, 9, 1);

            double[] sem = { 0, 3.5, 9.6, 16.6, 23.0, 28.8, 33.3, 23.0, 0, 0, 0, 0 };
            s.Read(t1, t2);
            Assert.AreEqual(12, s.Count);
            DateTime t = t1;
            for (int i = 0; i < sem.Length; i++)
            {
                Assert.AreEqual(sem[i], s[i].Value);
                Assert.AreEqual(t.Month, s[i].DateTime.Month);
                Assert.AreEqual(t.Year, s[i].DateTime.Year);
                Assert.AreEqual("S", s[i].Flag);
                t = t.AddMonths(1);
            }
            //m.CreateSeries(row);


        }



        [Test]
        public void MonthlyMissingRecords()
        {
            HydrometMonthlySeries s = new HydrometMonthlySeries("jkpi", "se");
            DateTime t1 = new DateTime(3000, 10, 1);
            DateTime t2 = new DateTime(3001, 9, 1);

            s.Read(t1, t2);
            s.WriteToConsole();
            Assert.AreEqual(12, s.Count);
            Assert.AreEqual(12, s.CountMissing());

        }


        /*
         * 
         * 
         * g/sem/6190  jkpi
JKPI
      DATE          SEM 
 OCT  6189         0.00S
 NOV  6189         3.50S
 DEC  6189         9.60S
 JAN  6190        16.60S
 FEB  6190        23.00S
 MAR  6190        28.80S
 APR  6190        33.30S
 MAY  6190        23.00S
 JUN  6190         0.00S
 JUL  6190         0.00S
 AUG  6190         0.00S
 SEP  6190         0.00S
       
         */

    }
}

