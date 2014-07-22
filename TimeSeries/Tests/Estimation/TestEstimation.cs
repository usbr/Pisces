using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Estimation;


namespace Pisces.NunitTests.SeriesMath
{
    /// Tests for basic Multiple Linear Regression
    /// Feb and May have missing data
    ///
    ///  The relationship between Series1 and Series2
    ///  is different for different months.
    ///
    [TestFixture]
    public class TestEstimation
    {

        Series s1, s2;
        SeriesList list;
        DateTime t1, t2;
        public TestEstimation()
        {

        }
        /// <summary>
        /// Test Multiple Linear Regression (MLR) 
        /// with simple two series example. 
        /// Series1 is twice the value of Series2 (Jan,Feb,Mar)
        /// Series1 is 3 times the value of Series2 (Apr...Dec)
        /// </summary> 
        [Test]
        public void SimpleMonthlyMLRJanFebMar()
        {
            CreateSeries();
            
           var x = MultipleLinearRegression.MlrInterpolation(list, t1, t2, new int[] { 1, 2,3 }, .7);
           Console.WriteLine("Results:");
           x.EstimatedSeries.WriteToConsole();
           Console.WriteLine(String.Join("\n", x.Report));
           Point feb = x.EstimatedSeries["2000-2-1"];
           Assert.AreEqual(2, feb.Value,0.01, "Feb value should be 2");
           Point may = x.EstimatedSeries["2000-5-1"];
           Assert.IsTrue(may.IsMissing, "Value in May should not be estimated");
        }
        /// <summary>
        /// Test Multiple Linear Regression (MLR) 
        /// with simple two series example. 
        /// </summary> 
        [Test]
        public void SimpleMonthlyMLRAprMayJun()
        {
            CreateSeries();

            var y = MultipleLinearRegression.MlrInterpolation(list, t1, t2, new int[] { 4, 5, 6 }, .7);
            var may = y.EstimatedSeries["2000-5-1"];
            Assert.AreEqual(5, may.Value,0.01, "may value should be 5");
            var feb = y.EstimatedSeries["2000-2-1"];
            Assert.IsTrue(may.IsMissing, "Value in Feb should not be estimated");

        }
        /// Series1 is twice the value of Series2 (Jan,Feb,Mar)
        /// Series1 is 3 times the value of Series2 (Apr...Dec)
        private void CreateSeries()
        {
            s1 = new Series("", TimeInterval.Monthly);
            s1.Name = "snuggles";
            s2 = new Series("", TimeInterval.Monthly);
            s2.Name = "myza";
            DateTime t = new DateTime(2000, 1, 1);
            double val1 = t.Month;
            double val2 = 0;
            while (t.Year == 2000)
            {
                val1 = t.Month;
                if (t.Month <= 3)
                    val2 = val1 * 2;
                else
                    val2 = val1 * 3;

                if (t.Month == 5)
                    s1.AddMissing(t);
                else if (t.Month == 2)
                    s1.AddMissing(t);
                else
                    s1.Add(t, val1, "");

                s2.Add(t, val2, "");


                t = t.AddMonths(1).FirstOfMonth();
            }
            list = new SeriesList();
            list.Add(s1);
            list.Add(s2);
            t1 = s1.MinDateTime;
            t2 = s1.MaxDateTime;
            Console.WriteLine("Input Series 1");
            s1.WriteToConsole();
            Console.WriteLine("Input Series 2");
            s2.WriteToConsole();
        }

    }
}
