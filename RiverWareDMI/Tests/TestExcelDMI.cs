using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Reclamation.Core;
using Reclamation.Riverware;
using System.IO;

namespace ReclamationTesting.RiverWareDmiTest
{
    [TestFixture]
    public class TestExcelDMI
    {
        [Test]
        public void MultiYearTest()
        {
            Performance p = new Performance();
            string path = Reclamation.Core.Globals.TestDataPath;

            for (int i = 1; i <= 2; i++)
            {
                string dir = @"C:\Temp\dmitest";
                Directory.CreateDirectory(dir);
                List<string> args = new List<string>();
                args.Add(path + "\\RiverWare\\boiseControl.txt");
                args.Add(dir);
                args.Add("1927-10-04");// date for RiverWare
                args.Add("12:00");
                args.Add("1928-07-01");
                args.Add("12:00");
                args.Add("-UXlsFileName=" + path + "\\RiverWare\\BoiseModelData.xls");
                args.Add("-sTrace="+i);

                
                if( Directory.Exists(dir))
                   Directory.Delete(dir,true);
                Directory.CreateDirectory(dir);
                Reclamation.RiverwareDmi.Program.Main(args.ToArray());

                string fn = dir + "\\Inflow.Anderson Ranch.txt";
                TextFile tf = new TextFile(fn);
                int idx = tf.IndexOf("start_date");


                if (i == 1) // 1918
                {
                    double val = Convert.ToDouble(tf[idx + 1]);
                    Assert.AreEqual(429, val, .5);
                }
                if (i == 2) // 1919
                {
                    double val = Convert.ToDouble(tf[idx + 1]);
                    Assert.AreEqual(348, val, .5);
                }

                Directory.Delete(dir, true);
            }

        }


//Jackson.Inflow: obj=StorageReservoir file=c:\temp\Inflow.Jackson.txt units=cfs scale=0.504167 import=resize excel_sheet=unregulation excel_data_column=jck_qu
//MinidokaToMilner:Milner.Local Inflow: obj=Reach file="c:\temp\Local Inflow.MinidokaToMilner_Milner.txt" units=cfs scale=0.504167 import=resize excel_sheet=unregulation excel_data_column=mili_ql

        [Test]
        public void WaterYear1931()
        {
            Performance p = new Performance();
            string path = Globals.TestDataPath;
            //controlfile tempPath yyyy-mm-dd hh:mm yyyy-mm-dd hh:mm 1DAY -UxlsFileName=file.xls [-STrace=n]");
            //-UxlsFileName=V:\PN6200\Models\BoiseRiverWare\BoiseModelData.xls -UDebugLevel=1 -UWaterYear=1943 -UFirstWaterYear=1919 
            List<string> args = new List<string>();
            args.Add(path + "\\RiverWare\\snakeControl.txt");
            args.Add("c:\\temp");
            args.Add("1927-10-04");// start date for RiverWare 
            args.Add("12:00");
            args.Add("1928-07-01");// end date for riverware
            args.Add("12:00");
            args.Add("1DAY");
            args.Add("-UXlsFileName=" + path + "\\Riverware\\SnakeTestData.xls");
            args.Add("-UWaterYear=1931"); // actual xls data begins 10/4/1930  (water year 1931)
            args.Add("-UFirstWaterYear=1928");
            Reclamation.RiverwareDmi.Program.Main(args.ToArray());

            string fn = @"c:\temp\Inflow.Jackson.txt";
            TextFile tf = new TextFile(fn);
            File.Delete(fn);

            Assert.AreEqual("# import began on line index: 1113", tf[tf.IndexOf("# import")]);
            Assert.AreEqual("start_date: 1927-10-04 24:00", tf[tf.IndexOf("start_date")]);

            int idx = tf.IndexOf("start_date");
            Assert.AreEqual(691.4, Convert.ToDouble(tf[idx+1]),.1); 
            Assert.AreEqual(689.4, Convert.ToDouble(tf[idx + 2]), .1);
            Assert.AreEqual(689.4, Convert.ToDouble(tf[idx + 3]), .1);
            Assert.AreEqual(4211.4, Convert.ToDouble(tf[idx + 4]), .1);
            p.Report("completed FullDataDump()");
                      
        }

        [Test]
        public void WaterYearTrace()
        {

            string path = Globals.TestDataPath;
            List<string> args = new List<string>();
            //-UxlsFileName=V:\PN6200\Models\BoiseRiverWare\BoiseModelData.xls -UDebugLevel=1 -UWaterYear=1943 -UFirstWaterYear=1919 
            args.Add(path + "\\RiverWare\\snakeControl.txt");
            args.Add("c:\\temp");
            args.Add("1927-10-04");// start date for RiverWare 
            args.Add("12:00");
            args.Add("1928-07-01");
            args.Add("12:00");
            args.Add("1DAY");
            args.Add("-UXlsFileName=" +path + "\\RiverWare\\SnakeTestData.xls");
            args.Add("-STrace=4");
            Reclamation.RiverwareDmi.Program.Main(args.ToArray());

            string fn = @"c:\temp\Local Inflow.MinidokaToMilner_Milner.txt";
            TextFile tf = new TextFile(fn);
            File.Delete(fn);
            
            int idx = tf.IndexOf("start_date");
            Assert.AreEqual(-2906, Convert.ToDouble(tf[idx + 1]), .6);
            Assert.AreEqual(-2864, Convert.ToDouble(tf[idx + 2]), .6);
            
        }

    }
}
