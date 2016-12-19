using System.IO;
using NUnit.Framework;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.BpaHydsim;

namespace Pisces.NunitTests.SeriesTypes
{
    [TestFixture]
    public class TestBpaHydsimSeries
    {
        [Test]
        public void TestBrnleeQout()
        {
            string plantName = "BRNLEE";
            string dataType = "QOUT";

            string fnMdb = Path.Combine(TestData.DataPath, "BpaHydsim", "Supp_A1F450.mdb");
            string fnCsv = Path.Combine(TestData.DataPath, "BpaHydsim", "BRNLEE_QOUT.csv");
            
            BpaHydsimSeriesAccess sMdb = new BpaHydsimSeriesAccess(fnMdb, plantName, dataType);
            sMdb.Read();
            Series sCsv = TextSeries.ReadFromFile(fnCsv);

            Assert.AreEqual(sMdb.Count, sCsv.Count, "wrong number of time steps");
            for (int i = 0; i < sMdb.Count; i++)
            {
                Assert.AreEqual(sMdb[i].Value, sCsv[i].Value);
            }
            Assert.AreEqual(Math.Sum(sMdb), Math.Sum(sCsv));
        }

        [Test]
        public void TestBrnleeStor()
        {
            string plantName = "BRNLEE";
            string dataType = "ENDSTO";

            string fnMdb = Path.Combine(TestData.DataPath, "BpaHydsim", "Supp_A1F450.mdb");
            string fnCsv = Path.Combine(TestData.DataPath, "BpaHydsim", "BRNLEE_ENDSTO.csv");

            BpaHydsimSeriesAccess sMdb = new BpaHydsimSeriesAccess(fnMdb, plantName, dataType);
            sMdb.Read();
            Series sCsv = TextSeries.ReadFromFile(fnCsv);

            Assert.AreEqual(sMdb.Count, sCsv.Count, "wrong number of time steps");
            for (int i = 0; i < sMdb.Count; i++)
            {
                Assert.AreEqual(sMdb[i].Value, sCsv[i].Value);
            }
            Assert.AreEqual(Math.Sum(sMdb), Math.Sum(sCsv));
        }
    }
}
