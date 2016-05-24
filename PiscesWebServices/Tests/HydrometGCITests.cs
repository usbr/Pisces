using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Reclamation.Core;
using Reclamation.TimeSeries;
using System.IO;

namespace PiscesWebServices.Tests
{
    [TestFixture]    
    class HydrometGCITests
    {
        public static void Main()
        {
            //Logger.EnableLogger();
            HydrometGCITests t = new HydrometGCITests();
            t.StationFormat();
            t.PerfTestLarge();
        }

        [Test]
        public void PerfTestLarge()
        {
            string payload = "parameter=mddo ch,wcao q,boii Z,boii ob,&syer=2015&smnth=4&sdy=13&eyer=2015&emnth=9&edy=1&format=2";
            RunTest(payload, TimeInterval.Irregular);
        }
        [Test]
        public void PerfTestSmall()
        {
            string payload = "parameter=mddo ch,wcao q,boii Z,boii ob,&syer=2015&smnth=4&sdy=13&eyer=2015&emnth=4&edy=13&format=2";
            RunTest(payload, TimeInterval.Irregular);
        }

        [Test]
        public void StationFormat()
        {
        //http://www.usbr.gov/pn-bin/webdaycsv.pl?station=cedc&pcode=ob&pcode=obx&back=10&format=2
            string payload = "station=cedc&pcode=ob&pcode=obx&back=10&format=2";
            RunTest(payload, TimeInterval.Irregular);
        }

        private static void RunTest(string payload, TimeInterval interval)
        {
            Performance p = new Performance();
            TimeSeriesDatabase db = TimeSeriesDatabase.InitDatabase(new Arguments(new string[] { }));
            CsvTimeSeriesWriter c = new CsvTimeSeriesWriter(db);
            var fn = FileUtility.GetTempFileName(".txt");
            fn = "";
            c.Run(interval, payload, fn);

            if (File.Exists(fn))
                p.Report(File.ReadAllLines(fn).Length + " lines read");
            else
                p.Report();
        }

        /// <summary>
        /// This query is a simplified/new recommended method to query.
        /// </summary>
        [Test]
        public void RecommendedQuery()
        {

        }

        [Test]
        public void Daily_boii()
        {
            string payload = "parameter=mddo ch,wcao q,boii Z,boii ob,&syer=2015&smnth=10&sdy=30&eyer=2015&emnth=11&edy=4&format=2";
            InstantCompareLinuxToVMSCGI(payload);
        }

        [Test]
        public void Instant_boii()
        {
            string payload = "parameter=mddo ch,wcao q,boii Z,boii ob,&syer=2015&smnth=10&sdy=30&eyer=2015&emnth=11&edy=4&format=2";
            InstantCompareLinuxToVMSCGI(payload);
        }

        [Test]
        public void Instant_mddo()
        {
            string payload = "parameter=mddo ch,wcao q,boii Z,boii ob,&syer=2015&smnth=10&sdy=30&eyer=2015&emnth=11&edy=4&format=2";
            InstantCompareLinuxToVMSCGI(payload);
        }

        
        public static void InstantCompareLinuxToVMSCGI(string payload,TimeInterval interval= TimeInterval.Irregular)
        {
            
            //Program.Main(new string[] { "--cgi=instant", "--payload=?"+payload });

            TimeSeriesDatabase db = TimeSeriesDatabase.InitDatabase(new Arguments(new string[]{}));
            CsvTimeSeriesWriter c = new CsvTimeSeriesWriter(db);
            var fn = FileUtility.GetTempFileName(".txt");
            c.Run( interval, payload,fn);

            TextFile tf = new TextFile(fn);
            tf.DeleteLines(0, 1);

            var fnhyd0 = FileUtility.GetTempFileName(".txt");
            string url = "http://www.usbr.gov/pn-bin/webarccsv.pl?";

            if( interval == TimeInterval.Irregular || interval == TimeInterval.Hourly)
                url = "http://www.usbr.gov/pn-bin/webdaycsv.pl?";
            Web.GetFile(url + payload, fnhyd0);

          var tf2 = new TextFile(fnhyd0);
          var diff = TextFile.Compare(tf, tf2);
            
          if (diff.Length > 0)
          {
                for (int i = 0; i < tf.Length; i++)
                {
                    Console.WriteLine(tf[i]);
                }
          }
            Assert.IsTrue(diff.Length == 0);

        }

        [Test]
        public void DumpTest()
        {
            Program.Main(new string[] { "--cgi=sites" });
        }
    }
}
