using NUnit.Framework;
using Reclamation.Core;
using Reclamation.TimeSeries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydrometServer.Tests
{
    [TestFixture]
    public class TestRecursive
    {
        TimeSeriesDatabase db;

        string dir;
        public TestRecursive()
        {
            string fn = FileUtility.GetTempFileName(".pdb-recursive");
            Console.WriteLine(fn);
            var svr = new SQLiteServer(fn);
            db = new TimeSeriesDatabase(svr, Reclamation.TimeSeries.Parser.LookupOption.TableName, false);
            dir = FileUtility.GetTempPath();
            CreateInputFiles();
        }

        private void CreateInputFiles()
        {


            File.WriteAllLines(Path.Combine(dir, "file1.wafi")
                , new string[] {

"yyyyMMMdd hhmm cbtt     PC        NewValue   OldValue   Flag user:hydromet # DECODES output",
"2017SEP15 2245 WAFI     CH        -1.05      998877.00  -20",
"2017SEP15 2230 WAFI     CH        -1.05      998877.00  -20",
"2017SEP15 2215 WAFI     CH        -1.05      998877.00  -20",
"2017SEP15 2200 WAFI     CH        -1.05      998877.00  -20",
"2017SEP15 2200 WAFI     BATVOLT   12.47      998877.00  -01"});

            File.WriteAllLines(Path.Combine(dir, "file1.clk")
                , new string[] {

"2017SEP27 2345 CLK FB        4529.97    998877.00 - 01",
"2017SEP27 2330 CLK FB        4529.97    998877.00 - 01",
"2017SEP27 2315 CLK FB        4529.97    998877.00 - 01",
"2017SEP27 2300 CLK FB        4529.97    998877.00 - 01",
"2017SEP27 2345 CLK TV        61.70      998877.00 - 01",
"2017SEP27 2330 CLK TV        61.70      998877.00 - 01",
"2017SEP27 2315 CLK TV        61.70      998877.00 - 01",
"2017SEP27 2300 CLK TV        61.70      998877.00 - 01",
"2017SEP27 2300 CLK BATVOLT   12.80      998877.00 - 01",
"2017SEP27 2349 CLK PARITY    0.00       998877.00 - 01",
"2017SEP27 2349 CLK POWER     48.00      998877.00 - 01",
"2017SEP27 2349 CLK MSGLEN    30.00      998877.00 - 01",
"2017SEP27 2349 CLK LENERR    0.00       998877.00 - 01",
"2017SEP27 2349 CLK TIMEERR   0.56       998877.00 - 01" });

            File.WriteAllLines(Path.Combine(dir, "file2.clk")
                , new string[] {
"2017SEP28 0045 CLK      FB        4529.97    998877.00  -01",
"2017SEP28 0030 CLK      FB        4529.96    998877.00  -01",
"2017SEP28 0015 CLK      FB        4529.97    998877.00  -01",
"2017SEP28 0000 CLK      FB        4529.96    998877.00  -01",
"2017SEP28 0045 CLK      TV        59.90      998877.00  -01",
"2017SEP28 0030 CLK      TV        60.98      998877.00  -01",
"2017SEP28 0015 CLK      TV        61.34      998877.00  -01",
"2017SEP28 0000 CLK      TV        61.52      998877.00  -01",
"2017SEP28 0000 CLK      BATVOLT   12.80      998877.00  -01",
"2017SEP28 0049 CLK      PARITY    0.00       998877.00  -01",
"2017SEP28 0049 CLK      POWER     48.00      998877.00  -01",
"2017SEP28 0049 CLK      MSGLEN    30.00      998877.00  -01",
"2017SEP28 0049 CLK      LENERR    0.00       998877.00  -01",
"2017SEP28 0049 CLK      TIMEERR   0.55       998877.00  -01" });


        }

        /// <summary>
        /// This test is intentionally has recursive input, to try to crash
        /// </summary>
        [Test]
        public void SimpleRecursive_wafi()
        {
            var c = new CalculationSeries("instant_wafi_ch");
            c.Expression =  "GenericWeir(wafi_ch,0,16.835,1.5)";
            db.AddSeries(c);

            FileImporter fi = new FileImporter(db);
            
            fi.Import(dir, true, true, "*.wafi");
            
        }

        /// <summary>
        /// This test reproduces failed daily calculations,
        /// all daily_clk_* did not compute daily data.
        /// </summary>
        [Test]
        public void BugDailyForebay()
        {
            AddDailyCalculation("daily_clk_fb", "DailyMidnight(instant_%site%_fb)");
            AddDailyCalculation("daily_clk_af", "DailyMidnight(instant_%site%_af)");
            AddDailyCalculation("daily_clk_fd", "DailyAverage(instant_%site%_fb)");

            FileImporter fi = new FileImporter(db);

            fi.Import(dir, true, true, "*.clk");
        }

        private void AddDailyCalculation(string name, string exp)
        {
            var c = new CalculationSeries(name);
            TimeSeriesName tn = new TimeSeriesName(name);
            c.Expression = exp;
            c.SiteID = tn.siteid;
            c.TimeInterval = TimeInterval.Daily;
            db.AddSeries(c);
        }
    }
}
