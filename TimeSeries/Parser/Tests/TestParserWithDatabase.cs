using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Reclamation.Core;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Parser;

namespace Reclamation.TimeSeries.Parser.Tests
{
    [TestFixture]
    class TestParserWithDatabase
    {
        TimeSeriesDatabase db;
        public TestParserWithDatabase()
        {
           string fn = FileUtility.GetTempFileName(".pdb");
           SQLiteServer.CreateNewDatabase(fn);
           var svr = new SQLiteServer(fn);

           db = new TimeSeriesDatabase(svr);
        }


        [Test]
        public void Shift()
        {
            // nov 14, 1999,  250 cfs
            // nov 15, 1999   249 cfs
            Series s = new HydrometDailySeries("bigi", "qd");
            DateTime t = DateTime.Parse("1999-11-15");

            s.Read(t, t);
            Assert.AreEqual(1, s.Count);

            s =Reclamation.TimeSeries.Math.Shift(s, 1);

            Assert.AreEqual(249, s[0].Value);
            Assert.AreEqual(16, s[0].DateTime.Day);
        }
        /// <summary>
        /// Test expression  bigi_qd[t-1]
        /// where t= nov 15, 1999
        /// 
        /// result should read and shift data so we have results
        /// on nov 15, 1999 -- but data from nov 14, 1999
        /// </summary>
        [Test]
        public void ReadAndShift()
        {
            // nov 14, 1999,  250 cfs
            // nov 15, 1999   249 cfs
            Series s = new HydrometDailySeries("bigi", "qd");
            DateTime t = DateTime.Parse("1999-11-15");
            int offset = -1;
            s = SeriesExpressionParser.ReadAndShiftData(-offset,s,t, t);

            Assert.AreEqual(1,s.Count);
            Assert.AreEqual(250, s[0].Value);
            Assert.AreEqual(15, s[0].DateTime.Day);
        }


        [Test]
        public void Yakima11146_QC()
        {

            string expression = "BUM_AF-BUM_AF[t-1]+RIM_AF-RIM_AF[t-1]+CLE_AF-CLE_AF[t-1]+KAC_AF-KAC_AF[t-1]+KEE_AF-KEE_AF[t-1]+SNCW_QC+ROZW_QC+RSDW_QC+RSCW_QC+KTCW_QC+'1146_QC'+TIEW_QC+WOPW_QC+NACW_QM";
            var vp = new VariableParser();
            var vars = vp.GetAllVariables(expression);

            Console.WriteLine(string.Join("\n",vars));
            Assert.AreEqual(19, vars.Length);
            Assert.AreEqual("1146_QC", vars[15]);
        }

        [Test]
        public void Constant()
        {

            CalculationSeries c = new CalculationSeries();
            c.TimeInterval = TimeInterval.Daily;
            c.Name = "constant_series";
            c.Expression = "15.0";
            db.AddSeries(c);

            DateTime t1 = new DateTime(2014, 10, 1);
            DateTime t2 = new DateTime(2014,10,5);

            c.Calculate(t1,t2);

            Assert.AreEqual(5, c.Count);
            Assert.AreEqual(15,c[0].Value,0.001);
        }


        [Test]
        public void VariableNames()
        {
            string equation = "Merge()"; // not a variable
            Assert.AreEqual(0, VariableParser.Default().GetAllVariables(equation).Length);

            equation = "Merge(series1,'series 2')+'Series 5'";
            var vars = VariableParser.Default().GetAllVariables(equation);
            Assert.AreEqual("series1",vars[0] );
            Assert.AreEqual("series 2",vars[1] );
            Assert.AreEqual("Series 5",vars[2]);


            equation = "'jck af'+two";
            vars = VariableParser.Default().GetAllVariables(equation);
            Assert.AreEqual(2, vars.Length);

        }
        

        [Test]
        public void FunctionNames()
        {
          ParserFunction f;
          string subExpr = "";
          bool ok = ParserUtility.TryGetFunctionCall("Merge(series1,'series 2')+'Series 5'",out subExpr, out f);
          Assert.IsTrue(ok);
          Assert.AreEqual("Merge", f.Name);
          Assert.AreEqual(2, f.Parameters.Length);

          Series observed = new Series();
          observed.Name = "observed";
          observed.Add(DateTime.Parse("2001-01-01"), 1);
          observed.AddMissing(DateTime.Parse("2001-1-02"));
          observed.Add(DateTime.Parse("2001-01-04"), 1);
          observed.TimeInterval = TimeInterval.Daily;
          Series estimated = new Series();
          estimated.Name = "estimated";
          estimated.Add(DateTime.Parse("2001-1-02"), 2);
          estimated.Add(DateTime.Parse("2001-1-03"), 2);
          estimated.Add(DateTime.Parse("2000-12-25"), 2);
          estimated.Add(DateTime.Parse("2001-12-26"), 2);
          estimated.TimeInterval = TimeInterval.Daily;

          CalculationSeries c = new CalculationSeries();
          //c.SetMissingDataToZero = true;
          c.TimeInterval = TimeInterval.Daily;
          c.Name = "merged"; 
          c.Expression = "Merge(observed, estimated)";
          db.AddSeries(observed);
          db.AddSeries(estimated);
          db.AddSeries(c);

          c.Calculate();

          Assert.AreEqual(2, c["2001-1-03"].Value, 0.0001);

          c.WriteToConsole();
        }

       

        [Test]
        public void MathInDatabase()
        {
            Series one = new Series();
            one.Name = "o";
            one.Add(DateTime.Parse("2001-01-01"), 1);
            one.Add(DateTime.Parse("2001-01-02"), 1);

            Series two = new Series();
            two.Name = "two";
            two.Add(DateTime.Parse("2001-01-01"), 2);
            two.Add(DateTime.Parse("2001-01-02"), 2);


            CalculationSeries onePlusTwo = new CalculationSeries();
            onePlusTwo.Name = "one+two"; // this name will match 'one%' in SQL
            onePlusTwo.Expression = "o+two";
            db.AddSeries(one);
            db.AddSeries(two);
            db.AddSeries(onePlusTwo);

            onePlusTwo.Calculate();
            Assert.AreEqual(2, onePlusTwo.Count);
            Assert.AreEqual(3, onePlusTwo[0].Value);
            Assert.AreEqual(3, onePlusTwo[1].Value);
            onePlusTwo.WriteToConsole();
        }

        public void SpaceInVariableName()
        {
            Series one = new Series();
            one.Name = "o";
            one.Add(DateTime.Parse("2001-01-01"), 1);
            one.Add(DateTime.Parse("2001-01-02"), 1);


            SeriesExpressionParser.Debug = true;
            one.Name = "jck af"; // put a space in the name
            db.AddSeries(one);

            CalculationSeries onePlusTwo = new CalculationSeries();
            onePlusTwo.Name = "space_in_expression"; 
            onePlusTwo.Expression = "'jck af'+two"; // using single quotes '
            onePlusTwo.Calculate();
            Assert.AreEqual(2, onePlusTwo.Count);
            Assert.AreEqual(3, onePlusTwo[0].Value);
            Assert.AreEqual(3, onePlusTwo[1].Value);
        }


        [Test]
        public void TestFunctions()
        {
            Series s = new Series();
            DateTime t = DateTime.Now.Date; // midnight

            s.Add(t, 1.0);
            s.Add(t.AddMinutes(15), 2.0);
            s.Add(t.AddMinutes(30), 3.0);

            s.Expression = "SimpleDailyAverage(qd)";
            var avg = Reclamation.TimeSeries.Math.DailyAverage(s);

           //Math.DailyAverage(qd, t1, t2);

            DateTime t1 = DateTime.Parse("12/25/2010");
            DateTime t2 = DateTime.Parse("12/27/2010");
           
           // s.Calculate(t1, t2);


        }

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

    }
}
