using Reclamation.Core;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Parser;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ImportCsv
{
    /// <summary>
    /// Program to Import CSV time series data.
    /// math expressions allow calculations on imported data
    /// this allows math expressions between columns being imported.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Usage: ImportCsv site_list.csv instant|daily daysBack");
                return;
            }
             FileUtility.CleanTempPath();
             Logger.EnableLogger();

            CsvFile csv = new CsvFile(args[0]);
            int daysBack = int.Parse(args[2]);
/*
 * 
interval,filename,date_column,value_column,cbtt,pcode
instant,kid.csv,Time,MB1.Hw_FlowMeter,ACHO,QC1
instant,kid.csv,Time,MB1.Hw_FlowCalc,ACHO,QC2
instant,kid.csv,Time,MB1.MillerHill_FlowPump1,HMPO,QP1
instant,kid.csv,Time,MB1.MillerHill_FlowPump2,HMPO,QP2
instant,kid.csv,Time,MB1.MillerHill_FlowPump3,HMPO,QP3

 */
            var rows = csv.Select("interval = '" + args[1] + "'");
            var interval = TimeInterval.Daily;
            if( args[1] == "instant")
                interval = TimeInterval.Irregular;

            Console.WriteLine("Processing "+rows.Length+" parameters");
            for (int i = 0; i < rows.Length; i++)
            {
                var filename = rows[i]["filename"].ToString();
                var dateColumn = rows[i]["date_column"].ToString();
                var valueColumn = rows[i]["value_column"].ToString();
                var cbtt = rows[i]["cbtt"].ToString();
                var pcode = rows[i]["pcode"].ToString();

                DataTable tbl = new CsvFile(filename);

                CalculationSeries s = new CalculationSeries();
                s.TimeInterval = interval;
                s.Expression = valueColumn;
                s.Parser.VariableResolver = new DataTableVariableResolver(tbl, dateColumn);
                s.Calculate(DateTime.Now.AddDays(-daysBack), DateTime.Now);

                Console.WriteLine("Processing " + cbtt + "_" + pcode + " " + s.Count + " records");

                TimeSeriesTransfer.Import(s,cbtt,pcode);

            }

        }

       
    }
}
