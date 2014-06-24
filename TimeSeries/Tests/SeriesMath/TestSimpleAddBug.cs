using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Reclamation.TimeSeries;

using Reclamation.Core;
using Reclamation.TimeSeries.Hydromet;

namespace Pisces.NunitTests.SeriesMath
{
    [TestFixture]
    public class TestSimpleAddBug
    {

        [Test]
        public void AddBug()
        {
           
            DateTime t1 = new DateTime(2013,1,1);
            var t2 = t1.AddDays(365);
            var fn = FileUtility.GetTempFileNameInDirectory(@"c:\temp\",".pdb");
            SQLiteServer svr = new SQLiteServer(fn);
            TimeSeriesDatabase db = new Reclamation.TimeSeries.TimeSeriesDatabase(svr);
            var s = new HydrometDailySeries("pal","af");
            s.Name = "pal_af";
            s.Read(t1,t2);
            db.AddSeries(s);

            var cs = new CalculationSeries("add_test");
            cs.Expression = "pal_af + pal_af";
            db.AddSeries(cs);


            cs = db.GetSeriesFromName("add_test") as CalculationSeries;
            cs.Calculate(t1, t2);


            cs = db.GetSeriesFromName("add_test") as CalculationSeries;
            cs.Read();

            Assert.IsTrue(cs.Count > 0);
            
        }
        
    }
}
