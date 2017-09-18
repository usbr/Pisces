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

        public TestRecursive()
        {
            string fn = FileUtility.GetTempFileName(".pdb-recursive");
            Console.WriteLine(fn);
            var svr = new SQLiteServer(fn);
            db = new TimeSeriesDatabase(svr, Reclamation.TimeSeries.Parser.LookupOption.TableName, false);
        }

        [Test]
        public void SimpleRecursive_wafi()
        {
            var c = new CalculationSeries("instant_wafi_ch");
            c.Expression =  "GenericWeir(wafi_ch,0,16.835,1.5)";
            db.AddSeries(c);

            FileImporter fi = new FileImporter(db);
            var dir = FileUtility.GetTempPath();

            File.WriteAllLines(Path.Combine(dir,"file1.wafi")
                ,new string[] {

"yyyyMMMdd hhmm cbtt     PC        NewValue   OldValue   Flag user:hydromet # DECODES output",
"2017SEP15 2245 WAFI     CH        -1.05      998877.00  -20",
"2017SEP15 2230 WAFI     CH        -1.05      998877.00  -20",
"2017SEP15 2215 WAFI     CH        -1.05      998877.00  -20",
"2017SEP15 2200 WAFI     CH        -1.05      998877.00  -20",
"2017SEP15 2200 WAFI     BATVOLT   12.47      998877.00  -01"
                }
                );

            fi.Import(dir, true, true, "*.wafi");
        }
    }
}
