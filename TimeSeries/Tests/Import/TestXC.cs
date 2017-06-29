using System;
using NUnit.Framework;
using Reclamation.TimeSeries;
using System.IO;
using Reclamation.TimeSeries.Import;
using Reclamation.Core;

namespace Pisces.NunitTests.Import
{
	[TestFixture]
    public class TestXC
    {
        [Test]
		public void TestXCSample1()
		{
            var fn = Path.Combine(TestData.DataPath,"xc", "pollasci.txt");
            Console.WriteLine(fn);
            Assert.IsTrue(File.Exists(fn));
            TextFile tf = new TextFile(fn);
            if ( XConnectTextFile.IsValidFile(tf) )
            {
                var xc = new XConnectTextFile(tf);
               var sl = xc.ToSeries();

               Assert.AreEqual(5, sl.Count);

               var idx = sl.IndexOfTableName("instant_nryw_bv");

               var s = sl[idx];

               Assert.AreEqual(12.60, s[0].Value, 0.01);
            }
		}


	}
}
