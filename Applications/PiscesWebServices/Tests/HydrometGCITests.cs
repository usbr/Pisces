using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Reclamation.Core;
using Reclamation.TimeSeries;
using System.IO;
using System.Text.RegularExpressions;
using PiscesWebServices.CGI;

namespace PiscesWebServices.Tests
{
    [TestFixture]    
    class HydrometGCITests
    {
        public static void Main()
        {
            //Logger.EnableLogger();
            HydrometGCITests t = new HydrometGCITests();
            t.CGI_StationFormat();
            t.CGI_PerfTestLarge();
        }

        [Test]
        public void CGI_PerfTestLarge()
        {
            string payload = "parameter=mddo ch,wcao q,boii Z,boii ob,&syer=2015&smnth=4&sdy=13&eyer=2015&emnth=9&edy=1&format=2";
            RunTest(payload, TimeInterval.Irregular);
        }
        [Test]
        public void CGI_PerfTestSmall()
        {
            string payload = "parameter=mddo ch,wcao q,boii Z,boii ob,&syer=2015&smnth=4&sdy=13&eyer=2015&emnth=4&edy=13&format=2";
            RunTest(payload, TimeInterval.Irregular);
        }

        [Test]
        public void CGI_StationFormat()
        {
        //https://www.usbr.gov/pn-bin/webdaycsv.pl?station=cedc&pcode=ob&pcode=obx&back=10&format=2
            string payload = "station=cedc&pcode=ob&pcode=obx&back=10&format=2";
            RunTest(payload, TimeInterval.Irregular);
        }

        private static string RunTest(string payload, TimeInterval interval)
        {
            Performance p = new Performance();
            TimeSeriesDatabase db = TimeSeriesDatabase.InitDatabase(new Arguments(new string[] { }));
            WebTimeSeriesWriter c = new WebTimeSeriesWriter(db,interval,payload);
            var fn = FileUtility.GetTempFileName(".txt");
            c.Run(fn);

            if (File.Exists(fn))
                p.Report(File.ReadAllLines(fn).Length + " lines read");
            else
                p.Report();
            return fn;
        }

        /// <summary>
        /// This query is a simplified/new recommended method to query.
        /// </summary>
        [Test]
        public void CGI_Daily_CleanerDesign()
        {

            string payload = "parameter=luc   fb,luc af&start=2016-5-19&end=2016-5-24&format=csv";
            var fn = RunTest(payload, TimeInterval.Daily);
/*            DATE      ,  LUC     FB      ,  LUC     AF      
5/24/2016 4:10:00 PM: CreateSQL
5/24/2016 4:10:00 PM: list of 2 series
05/19/2016,,  253349.11
05/20/2016,,  253698.69
05/21/2016,,  254910.41
05/22/2016,,  255826.27
05/23/2016,,  255933.44
 */
            TextFile tf = new TextFile(fn);
            int idx = tf.IndexBeginningWith("05/19/2016", 0);
            var s = tf[idx].Split(',')[2];
            double d = Convert.ToDouble(s);
            Assert.AreEqual(253349.11, d, 1.0);
            

        }

        [Test]
        public void CGI_Daily_LuckyPeak()
        {
            string payload = "parameter=luc   fb,luc   af&syer=2016&smnth=5&sdy=19&eyer=2016&emnth=5&edy=23&format=2";
            CompareLinuxToVMSCGI(payload, TimeInterval.Daily);
        }

        [Test]
        public void CGI_Daily_NMPI()
        {
            string payload = "parameter=nmpi et,nmpi   sr,nmpi ym&syer=2016&smnth=5&sdy=19&eyer=2016&emnth=5&edy=23&format=2";
            CompareLinuxToVMSCGI(payload, TimeInterval.Daily);
        }
        [Test]
        public void CGI_Instant_boii()
        {
            string payload = "parameter=mddo ch,wcao q,boii Z,boii ob,&syer=2015&smnth=10&sdy=30&eyer=2015&emnth=11&edy=4&format=2";
            CompareLinuxToVMSCGI(payload);
        }

        [Test]
        public void CGI_Instant_mddo()
        {
            string payload = "parameter=mddo ch,wcao q,boii Z,boii ob,&syer=2015&smnth=10&sdy=30&eyer=2015&emnth=11&edy=4&format=2";
            CompareLinuxToVMSCGI(payload);
        }

        
        public static void CompareLinuxToVMSCGI(string payload,TimeInterval interval= TimeInterval.Irregular)
        {
            
            //Program.Main(new string[] { "--cgi=instant", "--payload=?"+payload });

            TimeSeriesDatabase db = TimeSeriesDatabase.InitDatabase(new Arguments(new string[]{}));
            WebTimeSeriesWriter c = new WebTimeSeriesWriter(db,interval,payload);
            var fn = FileUtility.GetTempFileName(".txt");
            Console.WriteLine("linux temp file:"+fn);
            c.Run(fn);

            TextFile tf = new TextFile(fn);
            tf.DeleteLines(0, 1);

            var fnhyd0 = FileUtility.GetTempFileName(".txt");
            Console.WriteLine("vms temp file:" + fnhyd0);
            string url = "https://www.usbr.gov/pn-bin/webarccsv.pl?";

            if( interval == TimeInterval.Irregular || interval == TimeInterval.Hourly)
                url = "https://www.usbr.gov/pn-bin/webdaycsv.pl?";
            Web.GetFile(url + payload, fnhyd0);

          var tf2 = new TextFile(fnhyd0);

          if (!CompareHydrometData(tf, tf2))
          {
              // do detailed comparision.
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
        }

        private static bool CompareHydrometData(TextFile a, TextFile b)
        {
            int idx1 = a.IndexOf("BEGIN DATA");
            a.DeleteLines(0,idx1);
            int idx2 = a.IndexOf("END DATA");
            a.DeleteLines(idx2, a.Length - 1);

            idx1 = b.IndexOf("BEGIN DATA");
            b.DeleteLines(0, idx1);
            idx2 = b.IndexOf("END DATA");
            b.DeleteLines(idx2, b.Length - 1);

            

            if (a.Length != b.Length)
                return false;

            for (int i = 0; i < a.Length; i++)
            {
                var x = a[i].Split(',');
                var y = b[i].Split(',');

                if (x.Length != y.Length)
                    return false;

                for (int j = 0; j < x.Length; j++)
                {
                    if (i == 0) // cbtt parameter
                    {// allow different spacing
                        // 'jck fb'   or 'jck     fb'
                        RegexOptions options = RegexOptions.None;
                        Regex regex = new Regex("[ ]{2,}", options);

                        x[j] = regex.Replace(x[j], " ");
                        y[j] = regex.Replace(y[j], " ");
                    }
                    if (x[j].Trim() != y[j].Trim())
                            return false;
                }
            }

            return true;
        }

        [Test]
        public void DumpTest()
        {
            Program.Main(new string[] { "--cgi=sites" });
        }
    }
}
