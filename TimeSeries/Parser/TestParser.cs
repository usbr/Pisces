using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reclamation.TimeSeries;
//using Reclamation.TimeSeries.Parser;
using Reclamation.Core;
using Reclamation.TimeSeries.Excel;
using Reclamation.TimeSeries.Parser;
namespace ReclamationTesting.TimeSeries
{
    [TestClass]
    public class TestParser
    {
        TimeSeriesDatabase db;
        public TestParser()
        {
           string fn = FileUtility.GetTempFileName(".sdf");
           SqlServerCompact.CreateNewDatabase(fn);
           SqlServerCompact svr = new SqlServerCompact(fn);

           db = new TimeSeriesDatabase(svr);



        }

        [TestMethod]
        public void SimpleMath()
        {
            var p = new SeriesExpressionParser();

            Assert.AreEqual(4, p.Evaluate("10 - 3*2").Val);
            Assert.AreEqual(4, p.Evaluate("10 - 2 *3").Val);
            Assert.AreEqual(8, p.Evaluate("2^3").Val);
            Assert.AreEqual(8, p.Evaluate("10 - 2 * 3^0").Val);
            Assert.AreEqual(-2, p.Evaluate(" -(2+0*5)").Val);
            Assert.AreEqual(16, p.Evaluate("-2^(10-2*3)").Val);

        }

        [TestMethod]
        public void ConvertToCelcius()
        {

            var fn = TestData.DataPath + "\\CalculationTests.xlsx";
            Series s = new SpreadsheetGearSeries(fn, "ConvertToCelcius", "Date", "value", false);
            s.Units = "degrees C";
            s.Expression = "5/9*(this-32)";

            s.Read();
            
            s.WriteToConsole();
            s.Clear();
            s.Calculate(); //t,t.AddDays(3)); 
            s.WriteToConsole();
            Assert.AreEqual(0, s[0].Value,0.01);
            Assert.AreEqual(100, s[1].Value,0.01);
        }


        [TestMethod]
        public void UnregulatedFlow()
        {
            var fn = TestData.DataPath +"\\CalculationTests.xlsx";

            Series af = new SpreadsheetGearSeries(fn, "jck_qu", "Date", "jck af", false);
            Series qd = new SpreadsheetGearSeries(fn, "jck_qu", "Date", "jck qd", false);

            Series qu = new Series();
            qu.Parser.VariableResolver.Add("af", af);
            qu.Parser.VariableResolver.Add("qd", qd);
            qu.Expression = "(af[t]-af[t-1])/1.98347+qd";

            DateTime t1 = DateTime.Parse("12/25/2010");
            DateTime t2 = DateTime.Parse("12/27/2010");

            qu.Calculate(t1, t2);

            qu.WriteToConsole();
            Assert.AreEqual(674.39, qu["12/26/2010"].Value, .01);

        }
    }
}
