using NUnit.Framework;
using Reclamation.Core;
using System;
using System.IO;
using DateTime = System.DateTime;

namespace Reclamation.TimeSeries.Parser.Tests
{
    [TestFixture]
    public class TestParser
    {
        [Test]
        public void SimpleMath()
        {
            var p = new SeriesExpressionParser();

            Assert.AreEqual(4, p.Evaluate("10 - 3*2").Double);
            Assert.AreEqual(4, p.Evaluate("10 - 2 *3").Double);
            Assert.AreEqual(8, p.Evaluate("2^3").Double);
            Assert.AreEqual(8, p.Evaluate("10 - 2 * 3^0").Double);
            Assert.AreEqual(-2, p.Evaluate(" -(2+0*5)").Double);
            Assert.AreEqual(16, p.Evaluate("-2^(10-2*3)").Double);

        }


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

            string expression = "HydrometRuleCurve(\"c:\\temp\\prvo\",fc))";

            string subExpression;
            ParserFunction f;
            ParserUtility.TryGetFunctionCall(expression, out subExpression, out f);

            Assert.AreEqual("HydrometRuleCurve", f.Name);
            Assert.AreEqual(2, f.Parameters.Length);

            Assert.AreEqual("\"c:\\temp\\prvo\"", f.Parameters[0]);
            Assert.AreEqual("fc", f.Parameters[1]);

            CalculationSeries cs = new CalculationSeries("test");
            cs.Expression = expression;
            var vars = cs.GetDependentVariables();
            // "prvo"  is a string NOT a variable.
            foreach (var item in vars)
            {
                System.Console.WriteLine("'"+item+"'");
            }

            Assert.IsTrue(vars.Count == 1,"string parameter is not a variable");

            Assert.IsTrue(vars[0] == "fc");
        }




        [Test]
        public void SimpleDailyAverageWithInterval()
        {
            var mm = new CalculationSeries();
            mm.SiteID = "pici"; // test feautre that replaces %site% with SiteID
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
            mm.SiteID = "pici";
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

            var fn = Path.Combine(TestData.DataPath, "calculationtests_convert.csv");
            var csv = new CsvFile(fn);
            Series s = new DataTableSeries(csv,  TimeInterval.Daily, "Date", "value");
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
            var fn = Path.Combine(TestData.DataPath, "calculationtests_jck_qu.csv");
            var csv = new CsvFile(fn);
            Series af = new DataTableSeries(csv, TimeInterval.Daily, "Date", "jck af");
            Series qd = new DataTableSeries(csv, TimeInterval.Daily, "Date", "jck qd");

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

        [Test]
        public void TestIf()
        {
            Series gh = new Series("emi_gh"); // gauge height
            Series zs = new Series("emi_zs"); // switch

            DateTime t = DateTime.Parse("2001-01-01");
            gh.Add(t,2.0); 
            zs.Add(t,0); //  regular weir 
            t = t.AddMinutes(15);
            gh.Add(t,2.0);
            zs.Add(t,1); // weir blade is up (low flows)

            var regular = Math.GenericWeir(gh,0.07175,64.49,1.742);
            var lowFlow = Math.RectangularContractedWeir(gh,3.0);

            var x = Math.If(zs, lowFlow, regular);

            x.WriteToConsole();
            Assert.AreEqual(229.378145, x[0].Value,0.01);
            Assert.AreEqual(24.48852205, x[1].Value,0.01);


        }

        [Test]
        public void Conditionals()
        {
            SeriesExpressionParser.Debug = true;
            var tiew_qj = new Series("tiew_qj");
            var nscw_qj = new Series("nscw_qj");

            tiew_qj.TimeInterval = TimeInterval.Daily;
            nscw_qj.TimeInterval = TimeInterval.Daily;

            DateTime t1 = DateTime.Parse("1/1/2015");
            DateTime t2 = DateTime.Parse("1/10/2015");
            
            tiew_qj.AfterRead += new EventHandler(delegate(object o, EventArgs a)
            {
                tiew_qj.AddRange(t1, new double[] { 10, 10, 1, 1 });
            });
            nscw_qj.AfterRead += new EventHandler(delegate(object o, EventArgs a)
            {
                nscw_qj.AddRange(t1, new double[] { 11, 10, 1, 100 });
            });
            var expected = new double[]  { 35, 0,  0, 35 };
            var expected2 = new double[] {  0, 0, 35, 0 };

            var qu = new CalculationSeries();
            
            qu.Expression = "If( tiew_qj + nscw_qj > 20.0 , 35.0, 0.0)";
            qu.TimeInterval = TimeInterval.Daily;         
            qu.Parser.VariableResolver.Add("tiew_qj", tiew_qj);
            qu.Parser.VariableResolver.Add("nscw_qj", nscw_qj);
            qu.Calculate(t1,t2);
            qu.WriteToConsole();
            Assert.AreEqual(4, qu.Count, " expected 4 QU values");


            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], qu[i].Value, 0.001);
            }
            expected = null;
            qu.Expression = "If( tiew_qj + nscw_qj < 20.0 , 35.0, 0.0)";
            qu.Calculate(t1, t2);
            qu.WriteToConsole();
            Assert.AreEqual(4, qu.Count, " expected 4 QU values");

            for (int i = 0; i < expected2.Length; i++)
            {
                Assert.AreEqual(expected2[i], qu[i].Value, 0.001);
            }

        }


    }
}

