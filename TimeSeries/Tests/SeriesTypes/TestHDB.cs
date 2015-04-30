using System;
using NUnit.Framework;
using Reclamation.Core;

using System.IO;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.HDB;

namespace Pisces.NunitTests.SeriesTypes
{
    [TestFixture]
    public class TestHDB
    {

        [Test, Category("Internal")]
        public void SDI1930()
        {
            var s = new HDBSeries(1930,TimeInterval.Daily ,HDBServer.LCHDB);
            
            s.Read(new DateTime(2015, 4, 1), new DateTime(2015,4, 5));

            Assert.IsTrue(s.TimeInterval == TimeInterval.Daily);
            Assert.IsTrue(s.Count == 5);
            Assert.AreEqual(1084.17, s["2015-4-5"].Value,0.01);
            /*
                     DATETIME,     SDI_1930
04/02/2015 00:00,      1084.79
04/03/2015 00:00,      1084.69
04/04/2015 00:00,      1084.37
04/05/2015 00:00,      1084.17
04/06/2015 00:00,      1084.06
04/07/2015 00:00,      1083.93
04/08/2015 00:00,      1083.65
04/09/2015 00:00,      1083.47
04/10/2015 00:00,      1083.25
04/11/2015 00:00,      1082.98
04/12/2015 00:00,      1082.85
04/13/2015 00:00,      1082.63
04/14/2015 00:00,      1082.38
04/15/2015 00:00,      1082.23
04/16/2015 00:00,      1081.91
             */
        }

    }
}

