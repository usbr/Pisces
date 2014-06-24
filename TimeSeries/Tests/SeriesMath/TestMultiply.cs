using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Reclamation.TimeSeries;


namespace Pisces.NunitTests.SeriesMath
{
    [TestFixture]
    public class TestMultiply
    {

        [Test]
        public void Mult()
        {
            Series s1 = new Series();
            Series s2 = new Series();
            DateTime t = new DateTime(2006, 7, 11);
            
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
            Console.WriteLine("");
            s2.WriteToConsole();
            Console.WriteLine("");
            Series s3 = s1 * s2;
            s3.WriteToConsole();
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
                    Assert.AreEqual(s1.Lookup(t)*2, s1.Lookup(t) * s2.Lookup(t) );
                }
                t = t.AddDays(1);

            }

        }
        
    }
}
