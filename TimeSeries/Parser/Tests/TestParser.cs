using NUnit.Framework;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Excel;
using Reclamation.TimeSeries.Parser;
using DateTime = System.DateTime;

namespace Reclamation.TimeSeries.Parser.Tests
{
    [TestFixture]
    public class TestParser
    {


        [Test]
        public void NestedFunctionCalls()
        {

            string expression = "Merge(daily,EstimateDailyFromMonthly(daily,monthly))";

            string subExpression;
            ParserFunction f;
            ParserUtility.TryGetFunctionCall(expression, out subExpression, out f);

            Assert.AreEqual("Merge", f.Name);
            Assert.AreEqual(2, f.Parameters.Length);

            Assert.AreEqual("daily", f.Parameters[0]);
            Assert.AreEqual("EstimateDailyFromMonthly(daily,monthly)", f.Parameters[1]);

        }

        [Test]
        public void StringParameter()
        {

            string expression = "HydrometRuleCurve(\"prvo\",fc))";

            string subExpression;
            ParserFunction f;
            ParserUtility.TryGetFunctionCall(expression, out subExpression, out f);

            Assert.AreEqual("HydrometRuleCurve", f.Name);
            Assert.AreEqual(2, f.Parameters.Length);

            Assert.AreEqual("\"prvo\"", f.Parameters[0]);
            Assert.AreEqual("fc", f.Parameters[1]);

            CalculationSeries cs = new CalculationSeries("test");
            cs.Expression = expression;
            var vars = cs.GetDependendVariables();
            // "prvo"  is a string NOT a variable.
            foreach (var item in vars)
            {
                System.Console.WriteLine("'"+item+"'");
            }

            Assert.IsTrue(vars.Length == 1,"string parameter is not a variable");

            Assert.IsTrue(vars[0] == "fc");
        }




        [Test]
        public void SimpleDailyAverageWithInterval()
        {
            var mm = new CalculationSeries();
            mm.SiteName = "pici"; // test feautre that replaces %site% with SiteID
            mm.Expression = "DailyAverage(instant_%site%_ob)";
            mm.TimeInterval = TimeInterval.Daily;
            mm.Parser.VariableResolver = new HydrometVariableResolver();
            DateTime t1 = DateTime.Parse("5/2/2013");
            DateTime t2 = DateTime.Parse("5/2/2013");
            mm.Calculate(t1, t2);
            Assert.AreEqual(44.56, mm["5/2/2013"].Value,0.01);
            mm.WriteToConsole();
        }


        [Test]
        public void SimpleDailyAverageNoPrefix()
        {
            var mm = new CalculationSeries();
            mm.SiteName = "pici";
            mm.Expression = "DailyAverage(%site%_ob)";
            mm.TimeInterval = TimeInterval.Irregular;
            mm.Parser.VariableResolver = new HydrometVariableResolver();
            DateTime t1 = DateTime.Parse("5/2/2013");
            DateTime t2 = DateTime.Parse("5/2/2013");
            mm.Calculate(t1, t2);
            Assert.AreEqual(44.56, mm["5/2/2013"].Value, 0.01);
            mm.WriteToConsole();
        }


        
        [Test]
        public void ConvertToCelcius()
        {

            var fn = TestData.DataPath + "\\CalculationTests.xlsx";
            Series s = new ExcelDataReaderSeries(fn, "ConvertToCelcius", "Date", "value");
            s.Units = "degrees C";
            s.Name = "series1";

            CalculationSeries c = new CalculationSeries();
            c.Parser.VariableResolver.Add("series1", s);
            c.Expression = "5/9*(series1-32)";
            c.Read();
            
           // c.WriteToConsole();
            c.Clear();
            c.Calculate(); //t,t.AddDays(3)); 
            c.WriteToConsole();
            Assert.AreEqual(0, c[0].Value,0.01);
            Assert.AreEqual(100, c[1].Value,0.01);
        }


        [Test]
        public void UnregulatedFlow()
        {
            var fn = TestData.DataPath +"\\CalculationTests.xlsx";

            Series af = new ExcelDataReaderSeries(fn, "jck_qu", "Date", "jck af");
            Series qd = new ExcelDataReaderSeries(fn, "jck_qu", "Date", "jck qd");

            var qu = new CalculationSeries();

            qu.Parser.VariableResolver.Add("jck_af", af);
            qu.Parser.VariableResolver.Add("jck_qd", qd);

            qu.Expression = "(jck_af[t]-jck_af[t-1])/1.98347+jck_qd";

            DateTime t1 = DateTime.Parse("12/25/2010");
            DateTime t2 = DateTime.Parse("12/27/2010");

            qu.Calculate(t1, t2);

            qu.WriteToConsole();
            Assert.AreEqual(674.39, qu["12/26/2010"].Value, .01);

        }

        [Test]
        public void MpollQU()
        {
            var qu = new CalculationSeries();
            qu.Expression = "WOD_AF-WOD_AF[t-1]+WOD_OM";
            qu.TimeInterval = TimeInterval.Monthly;
            qu.Parser.VariableResolver = new HydrometVariableResolver();
              DateTime t1 = DateTime.Parse("9/1/1957");
            DateTime t2 = DateTime.Parse("12/23/1957");
            qu.Calculate(t1, t2);
            Assert.AreEqual(4120, qu["10/1/1957"].Value);
            qu.WriteToConsole();

        }


    }
}
