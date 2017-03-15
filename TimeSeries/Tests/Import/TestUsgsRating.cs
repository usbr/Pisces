using System;
using NUnit.Framework;
using Reclamation.TimeSeries;
using System.IO;
using Reclamation.Core;
using Reclamation.TimeSeries.Import;
using Reclamation.TimeSeries.Usgs;

namespace Pisces.NunitTests.Import
{
	[TestFixture]
    public class TestUsgsRating
    {
        [Test]
		public void TestRatingWithoutBreakPoints()
		{
            var usgs = new UsgsRatingTable("12363000");
            var fn = Path.Combine(TestData.DataPath,"rating_tables","12363000.txt");
            TextFile tf = new TextFile(fn);
            usgs.CreateShiftAndFlowTables(tf);
        }

        [Test]
        public void TestRatingFromWeb()
        {
            var usgs = new UsgsRatingTable("13206000");
            usgs.CreateShiftAndFlowTablesFromWeb();
        }
    }
}
