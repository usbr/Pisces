using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Reclamation.TimeSeries.Parser;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.TimeSeries;
using Math = Reclamation.TimeSeries.Math;
using Pisces.NunitTests.Database;
namespace Pisces.NunitTests.SeriesMath
{
    [TestFixture]
    public class DailyCalculatorTest
    {

        [Test]
        public void MathMax()
        {
            //DailyCalculator.
            DateTime t = new DateTime(2012, 12, 6);
            ConstantSeries s1 = new ConstantSeries("s1", "aa", -1, TimeInterval.Daily);
            s1.TimeInterval = TimeInterval.Daily;
            Reclamation.TimeSeries.Parser.SeriesExpressionParser.Debug = true;
            var s = new CalculationSeries();
            s.Expression = "Max(s1,0)";
            s.TimeInterval = TimeInterval.Daily;
            s.Parser.VariableResolver = new VariableResolver();
            s.Parser.VariableResolver.Add("s1", s1);

            s.Calculate(t, t.AddDays(2));
            s.WriteToConsole();
            Assert.AreEqual(3, s.Count);

        }

        [Test]
        public void ValeQU()
        { // daily data based on other daily data
            //DailyCalculator.
            Reclamation.TimeSeries.Parser.SeriesExpressionParser.Debug = true;
            var s = new CalculationSeries();
            s.Expression = "(WAR_AF[t-2]-WAR_AF[t-3]+BEU_AF[t-2]-BEU_AF[t-3]+BUL_AF-BUL_AF[t-1])/1.98347+VALO_QD";
            s.TimeInterval = TimeInterval.Daily;
            s.Parser.VariableResolver = new HydrometVariableResolver();

            DateTime t = new DateTime(2012, 12, 6);
            s.Calculate(t,t);

            Assert.AreEqual(1, s.Count);
            Assert.AreEqual(573.75, s[t].Value, 0.01);

        }


        [Test]
        public void JacksonQU()
        { // daily data based on other daily data
            //DailyCalculator.
            Reclamation.TimeSeries.Parser.SeriesExpressionParser.Debug = true;
            var s = new CalculationSeries();
            s.Expression = "(jck_af-jck_af[t-1])/1.98347+jck_qd";
            s.TimeInterval = TimeInterval.Daily;
            s.Parser.VariableResolver = new HydrometVariableResolver();

            s.Calculate(new DateTime(2012,11,1),new DateTime(2012,11,15));

            Assert.AreEqual(298, s["2012-11-15"].Value, 0.01);

        }


        private static CalculationSeries ComputeMonthlyValue(string cbtt, string pcode,
            TimeSeriesCalculator c,
            int year, int month)
        {
            var cs = c.Create(cbtt, pcode, TimeInterval.Monthly);
            //var cs = db.GetSeries(cbtt, pcode, TimeInterval.Monthly) as CalculationSeries;
            DateTime t1 = new DateTime(year, month, 1);
            DateTime t2 = t1.EndOfMonth();
            cs.Calculate(t1, t2);
            //cs.Read(2011, 7);
            return cs;
        }



    }
}
