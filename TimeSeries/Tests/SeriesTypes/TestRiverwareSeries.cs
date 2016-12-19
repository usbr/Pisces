using System;
using System.IO;
using System.Data;
using NUnit.Framework;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.RiverWare;

namespace Pisces.NunitTests.SeriesTypes
{
    [TestFixture]
    public class TestRiverwareSeries
    {

        [Test]
        public void SnapShotObject()
        {
            var s = new RiverWareSeries(Path.Combine(TestData.DataPath, "RiverWare", "SnapShotObj.rdf"), "", "Wolf Gage_Gage Outflow", -1, true);

            s.Read();

            Assert.AreEqual(15706, s.Count,"wrong number of time steps");

            Assert.AreEqual(5.001,s[0].Value,0.0001);
            Assert.AreEqual(5.0,s[1].Value,0.0001);

        }

        [Test]
        public void RDF()
        {
            var s = new RiverWareSeries(Path.Combine(TestData.DataPath, "RiverWare", "regular.rdf"), "DIVERSION KRD Irrigation", "Diversion", -1, false);
            s.Read();
            Assert.AreEqual(8035, s.Count,"wrong number of time steps");

            double[] v = { 0.001, 0.002, 0.003, 0.004, 0.005 };
            DateTime[] d = { };

            DateTime t = DateTime.Parse("1980-11-1 23:59:59");

            for (int i = 0; i < 5; i++)
			{
                Assert.AreEqual(v[i], s[i].Value);
                Assert.AreEqual(t, s[i].DateTime);
                t = t.AddDays(1);

			}

            Assert.AreEqual(true,s[s.Count-2].IsMissing);
            Assert.AreEqual(-0.0001, s[s.Count - 1].Value);

            Assert.AreEqual(DateTime.Parse("2002-10-31 23:59:59"), s[s.Count - 1].DateTime);


        }

        [Test]
        public void MMS()
        {
            var s = new RiverWareSeries(Path.Combine(TestData.DataPath, "RiverWare", "MMSTracesOutputWY05JunRun.rdf"), "TWSA PARW_Data", "Daily TWSA", 10, false);
            var parw = new RiverWareSeries(Path.Combine(TestData.DataPath, "RiverWare", "MMSTracesOutputWY05JunRun.rdf"), "PARW", "MMS PRMS Unregulated Flow", 51, false);

            Assert.AreEqual("cfs", parw.Units); // units before Read
            Assert.AreEqual("acre-ft", s.Units);

            Assert.AreEqual(TimeInterval.Daily, s.TimeInterval);
            s.Read();
            parw.Read();
            Assert.AreEqual(2040.66, parw[0].Value);
            Assert.AreEqual(934.89, parw[parw.Count - 1].Value);
            //Console.WriteLine(parw[parw.Count - 1]);
            Assert.AreEqual("acre-ft", s.Units);

            Assert.AreEqual("cfs", parw.Units);


        }

        [Test]
        public void LowerColoradoTraces()
        {
            string fn1 = Path.Combine(TestData.DataPath, "RiverWare", "LowerColoradoTraces.rdf");
            var s1 = new RiverWareSeries(fn1, "Powell", "Outflow", 1,false);
            var s2 = new RiverWareSeries(fn1, "Powell", "Outflow", 100,false);

            s1.Read();
            Assert.AreEqual(800001, s1[0].Value, 0.01);
            s2.Read();
            Assert.AreEqual(801965, s2[0].Value, 0.01);
        }


        [Test]
        public void UpperSnakeTraces()
        {

            string fn1 = Path.Combine(TestData.DataPath, "RiverWare", "UpperSnakeTraces.rdf");
            string fn2 = Path.Combine(TestData.DataPath, "RiverWare", "UpperSnakeTraces.rdf");

            var s1 = new RiverWareSeries(fn1, "Jackson", "Inflow", 1,false);
            var s2 = new RiverWareSeries(fn2, "Jackson", "Inflow", 15,false);

            s1.Read();
            s2.Read();
           
            Assert.AreEqual(s1.Units,"cfs");
            Assert.AreEqual(new DateTime(2006, 10, 20, 23, 59, 59),s1[0].DateTime);

            double[] expected1928 ={669.96,559.00,565.00,443.04,674.96 };
            double[] expected1942 ={749.04,537.29,749.04,436.46,537.29};

            for (int i = 0; i < expected1928.Length; i++)
            {
                Assert.AreEqual(expected1928[i], s1[i].Value, 0.01);
                Assert.AreEqual(expected1942[i], s2[i].Value, 0.01);
            }
        }
        //FileName=BOtargetsNoContFlow-50pct.rdf;ObjectName=Heron^Albuquerque;SlotName=Storage;ScenarioNumber=-1;IsSnapShot=False

        [Test]
        public void Albuquerque()
        {
            string fn = Path.Combine(TestData.DataPath, "RiverWare", "MRG-ESA-Output", "BOtargetsNoContFlow-50pct.rdf");
            var s = new RiverWareSeries(fn, "Heron^Albuquerque", "Storage", -1, false);
            s.Read();

            Assert.AreEqual(3652, s.Count);

        }

    }
}
