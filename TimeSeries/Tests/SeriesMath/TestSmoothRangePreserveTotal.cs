using System;
using System.Text;
using NUnit.Framework;
using Reclamation.TimeSeries;

namespace Pisces.NunitTests.SeriesMath
{
   

    /// <summary>
    /// see the associated excel spreadsheet 'Smooth preserving sum.xls'
    /// </summary>
    [TestFixture]
    public class TestSmoothRangePreserveTotal
    {


        /// <summary>
        /// Test with 15 as midpoint  [20,17.5,15.0,16.6667,18.333,20]  = 107.5
        /// Solution is 4 as midponit with values [20,12,4,9.33333,14.66667,20] = 80
        /// </summary>
        [Test]
        public void Basic()
        {

            Series s = new Series();
            s.Add("1/1/2006", 20);
            s.Add("1/2/2006", 20);
            s.Add("1/3/2006", 20);
            s.Add("1/4/2006", 20);
            s.Add("1/5/2006", 20);
            s.Add("1/6/2006", 20);
            s.Add("1/7/2006", -40);
            s.Add("1/8/2006", 10);
            s.Add("1/9/2006", 50);
            s.Add("1/10/2006", 20);
            s.Add("1/11/2006", 20);
            s.Add("1/12/2006", 20);
            s.Add("1/13/2006", 20);
            s.Add("1/14/2006", 20);
            s.Add("1/15/2006", 20);

            DateTime t1 = DateTime.Parse("1/6/2006");
            DateTime t2 = DateTime.Parse("1/11/2006");
           // s.WriteToConsole();

            SeriesRange sr = new SeriesRange(s, t1, t2);

            double d = sr.ComputeSum(15);

            Console.WriteLine(" \n\n sr.ComputeSum(15)= "+d);
            Assert.AreEqual(107.5, d, " summed using 15 as guess for midpoint");

            sr.SmoothPreservingSum();


            Assert.AreEqual(4,sr.MidPointValue,0.01);

        }

        [Test]
        public void Negative()
        {
            Series s = new Series();
            s.Add("1/6/2006", -1);
            s.Add("1/7/2006", -40);
            s.Add("1/8/2006", -50);
            s.Add("1/9/2006", -30);

            DateTime t1 = DateTime.Parse("1/6/2006");
            DateTime t2 = DateTime.Parse("1/9/2006");

            SeriesRange sr = new SeriesRange(s, t1, t2);
            double d = sr.ComputeSum(1.0);
            Assert.AreEqual(-44.5, d,.01, " summed using 1 as guess for midpoint");

            sr.SmoothPreservingSum();
        
            Assert.AreEqual(-50,sr.MidPointValue, 0.001);


            Assert.AreEqual(-1, s[0].Value,0.0001);
            Assert.AreEqual(-50, s[1].Value, 0.0001);
            Assert.AreEqual(-40, s[2].Value, 0.0001);
            Assert.AreEqual(-30, s[3].Value, 0.0001);

        }



        [Test]
        public void NegativeCrossYAxis()
        {
            Series s = new Series();
      
            s.Add("1/6/2006", -1);
            s.Add("1/7/2006", -40);
            s.Add("1/8/2006", -50);
            s.Add("1/9/2006", -30);
            s.Add("1/10/2006", 30);

            //s.WriteToConsole();
            DateTime t1 = DateTime.Parse("1/6/2006");
            DateTime t2 = DateTime.Parse("1/10/2006");

            SeriesRange sr = new SeriesRange(s, t1, t2);
            double d = sr.ComputeSum(-10);
            Assert.AreEqual(23.5, d, .01, " summed using -10  as guess for midpoint");

            sr.SmoothPreservingSum();
            Assert.AreEqual(-67.25, sr.MidPointValue, 0.001);

            Assert.AreEqual(-1, s[0].Value, 0.0001);
            Assert.AreEqual(-67.25, s[2].Value, 0.0001);
            Assert.AreEqual(30, s[4].Value, 0.0001);


        }
        [Test]
        public void NegativeCrossYAxis2()
        {
            Series s = new Series();
           
            s.Add("1/6/2006", 5);
            s.Add("1/7/2006", -15);
            s.Add("1/8/2006", -10);
            s.Add("1/9/2006", 30);
            
           // s.WriteToConsole();
            DateTime t1 = DateTime.Parse("1/6/2006");
            DateTime t2 = DateTime.Parse("1/9/2006");

            SeriesRange sr = new SeriesRange(s, t1, t2);
            double d = sr.ComputeSum(-25);
            Assert.AreEqual(12.5, d, .01, " summed using -25 as guess for midpoint");

            sr.SmoothPreservingSum();
            Assert.AreEqual(-26.6667, sr.MidPointValue, 0.001);

            Assert.AreEqual(5, s[0].Value, 0.0001);
            Assert.AreEqual(1.6667, s[2].Value, 0.0001);
            Assert.AreEqual(30, s[3].Value, 0.0001);
        }



    }
}
