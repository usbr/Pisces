using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reclamation.TimeSeries;
using Reclamation.Core;
using System.Data;
using Reclamation.TimeSeries.Owrd;

namespace ImportOWRD
{
    /// <summary>
    /// Imports  time series data from the Oregon Department of Water Resources Web page
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {

            if (args.Length != 2)
            {
                Console.WriteLine("Usage: ImportOwrd site_list.csv hoursBack");
                return;
            }

            int hoursBack = Convert.ToInt32(args[1]);

            Point.MissingValueFlag = 998877;
            bool errors = false;

            CsvFile csv = new CsvFile(args[0], CsvFile.FieldTypes.AllText);
            foreach (DataRow row in csv.Rows)
            {
                string site_id = row["site_id"].ToString();
                string pcode = row["pcode"].ToString();
                string cbtt = row["cbtt"].ToString();
            
                
                   var s = new OwrdSeries(site_id, OwrdSeries.OwrdDataSet.Instantaneous_Flow,true);
                    try
                    {
                        s.Read(DateTime.Now.AddHours(-hoursBack), DateTime.Now);

                        s.RemoveMissing();
                    Console.WriteLine(cbtt+" "+s.Count +" points");
                    if (s.Count > 0)
                        {
                            TimeSeriesTransfer.Import(s, cbtt, pcode);
                        }
                    }
                    catch (Exception e)
                    {
                        errors = true;
                        Console.WriteLine(e.Message);
                    }
                }

            if (errors)
                Console.WriteLine("Error reading one or more sites");
        }

    }
}
