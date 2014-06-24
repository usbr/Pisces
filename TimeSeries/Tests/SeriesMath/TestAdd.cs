using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Reclamation.TimeSeries;


namespace Pisces.NunitTests.SeriesMath
{
    [TestFixture]
    public class TestAdd
    {

        [Test]
        public void AddToEmpty()
        {
            Series s1 = new Series();
            Series s2 = new Series();
            DateTime t = new DateTime(2006, 7, 11);

            Series s3 = AddTwoSeriesWithDifferentLengths(s1, s2, ref t);

            Assert.AreEqual(s3.Count, s1.Count);

            t = new DateTime(2006, 7, 11);
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(t);
                bool missing = true;
                if (i > 5 && i < 8)
                    missing = false;
                
                Assert.AreEqual(s3[i].IsMissing,missing);

                if (!missing)
                {
                    Assert.AreEqual(s1.Lookup(t) + s2.Lookup(t),3 );
                }
                t = t.AddDays(1);

            }

        }

        private static Series AddTwoSeriesWithDifferentLengths(Series s1, Series s2, ref DateTime t)
        {
            for (int i = 0; i < 10; i++)
            {
                s1.Add(t, 1);

                if (i > 5 && i < 8)
                {
                    s2.Add(t, 2);
                }
                t = t.AddDays(1);
            }
            s1.WriteToConsole();
            s2.WriteToConsole();
            Series s3 = s1 + s2;
            return s3;
        }
        
    }
}
