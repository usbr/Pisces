using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Reclamation.TimeSeries;


namespace Pisces.NunitTests.SeriesMath
{
    [TestFixture]
    public class TestEstimation
    {

        /// <summary>
        /// Test Multiple Linear Regression (MLR) 
        /// with simple two series example. 
        /// Series1 is twice the value of Series2 (Jan,Feb)
        /// Series1 is 3 times the value of Series2 (Mar...Dec)
        /// </summary> 
        [Test]
        public void SimpleMonthlyMLR()
        {
            var s1 = new Series("", TimeInterval.Monthly);
            s1.Name = "snuggles";
            var s2 =new Series("", TimeInterval.Monthly);
            s2.Name = "myza";
            DateTime t = new DateTime(2000,1,10);
            double val1 = 10;
            double val2 = 0;
            while (t.Year < 2002)
            {
                if (t.Month == 1 || t.Month == 2)
                    val2 = val1 * 2;
                else
                    val2 = val1 * 3;
                s1.Add(t, val1, "");
                s2.Add(t, val2, "");

                val1 +=10;
                t = t.AddMonths(1).FirstOfMonth();
            }
                
            SeriesList list = new SeriesList();
            list.Add(s1);
            list.Add(s2);
            DateTime t1 = s1.MinDateTime;
            DateTime t2 = s1.MaxDateTime;
           var sResult = Reclamation.TimeSeries.Math.MlrInterpolation(list, t1, t2, new int[] { 1, 2 }, .7);
        }

    }
}