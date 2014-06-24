using NUnit.Framework;
using System.IO;
using Reclamation.TimeSeries.Decodes;
using Reclamation.TimeSeries;

namespace Pisces.NunitTests.SeriesTypes
{
    [TestFixture]    
    public class TestDecodesImporter
    {

        public TestDecodesImporter()
        {

        }

        [Test]
        public void Simple()
        {
            string fn =  Path.Combine(TestData.DataPath, "ISL-20110429114510");

            var p = new DecodesParser(fn);
           // var series = p.GetSeries();


        }
    }
}
