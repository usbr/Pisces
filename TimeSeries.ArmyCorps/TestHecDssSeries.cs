using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using Reclamation.TimeSeries.Hec;
using Reclamation.TimeSeries;
using Reclamation.Core;
namespace Pisces.NunitTests.SeriesTypes
{
    /// <summary>
    /// Summary description for TestHecDssSeries
    /// </summary>
    [TestFixture]
    public class TestHecDssSeries
    {

        public static string DataPath
        {
            get
            {
                return Globals.TestDataPath;
            }
        }

        public TestHecDssSeries()
        {
        }

        [Test]
        public void Catalog()
        {
            string fn = DataPath + "\\HecDss\\sample.dss";
            string[] stuff = HecDssTree.GetCatalog(fn);
            Console.WriteLine(String.Join("\n",stuff));
        }

        [Test]
        public void GreenRiver()
        {
            string fn = DataPath + "\\HecDss\\sample.dss";
            HecDssSeries s = new HecDssSeries(fn, "/GREEN RIVER/OAKVILLE/FLOW-RES OUT//1HOUR/OBS/");
            s.Read();

            Assert.AreEqual(TimeInterval.Hourly, s.TimeInterval);
            Assert.AreEqual("CFS", s.Units);
            Assert.AreEqual(504, s.Count);
            Assert.AreEqual(2580.0, s[s.Count - 1].Value, 0.01);
        }
    }
}
