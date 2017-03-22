using NUnit.Framework;
using Reclamation.TimeSeries;
using System;

namespace Reclamation.TimeSeries.Hydromet.Tests
{
    [TestFixture]
    class TestForecasting
    {
     //   TimeSeriesDatabase db;
        public TestForecasting()
        {
            Reclamation.Core.Logger.EnableLogger();
        }



        [Test]
        public void TestHydrometResidualForecast()
        {
            DateTime t1 = new DateTime(2014, 2, 1);
            DateTime t2 = new DateTime(2014, 2, 28);

            var qu = new HydrometDailySeries("prvo", "qu");
            var fc = new HydrometMonthlySeries("prvo", "fc");
            var fcm = new HydrometMonthlySeries("prvo", "fcm");
            var residual = new HydrometDailySeries("prv", "fcresid");

            Assert.AreEqual(TimeInterval.Daily, residual.TimeInterval);

            qu.Read(t1, t2);
            fc.Read(t1, t2);
            fcm.Read(t1, t2);
            var forecast = Math.HydrometForecastMonthlyToDaily(fc, fcm);
            residual.Read(t1, t2);

            var fcresid = Reclamation.TimeSeries.Math.HydrometResidualForecast(forecast, qu, residual);
            fcresid.WriteToConsole();
        }


        [Test, Category("Internal")]
        public void TestHydrometRuleCurve()
        {
            var residual = new HydrometDailySeries("prv", "fcresid");

            DateTime t1 = new DateTime(2013, 10, 1);
            DateTime t2 = new DateTime(2014, 3, 4);
            residual.Read(t1, t2);
            DateTime feb1 = new DateTime(2014,2,1);

            var space = Reclamation.TimeSeries.Math.HydrometRuleCurve("prvo", residual);

            //Assert.AreEqual(0, space[t1].Value);

            Assert.AreEqual(60000, space[feb1].Value,1);
            space.WriteToConsole();
        }

    }
}
