using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Usgs;
using System.Xml.XPath;
using System.Xml;
using Reclamation.Core;
using System.Data;
using Reclamation.TimeSeries.Hydromet;

namespace ImportUsgs
{
    class Program
    {
        /// <summary>
        /// reads USGS instant streamflow data and saves in Hydromet format
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {

            if (args.Length != 2)
            {
                Console.WriteLine("Usage: ImportUsgs site_list.csv hoursBack");
                return;
            }

            int hoursBack = Convert.ToInt32(args[1]);

            Point.MissingValueFlag = 998877;
            bool errors = false;

            CsvFile csv = new CsvFile(args[0], CsvFile.FieldTypes.AllText);
            foreach (DataRow row in csv.Rows)
            {
                var interval = GetInterval(row);
                string site_id = row["site_id"].ToString();
                string usgs_parameter = row["usgs_parameter"].ToString();
                string pcode = row["pcode"].ToString();
                string cbtt = row["cbtt"].ToString();
                Series s;

                Console.WriteLine(cbtt);
                if (interval == TimeInterval.Daily)
                {
                    if (usgs_parameter.ToLower() == "gageheight")
                        s = new UsgsDailyValueSeries(site_id, UsgsDailyParameter.DailyMeanDischarge);
                    else
                    {
                        Logger.WriteLine("Error: the parameter '"+usgs_parameter+"' is not yet supported");
                        s = new Series();
                    }
                    s.Read(DateTime.Now.AddHours(-hoursBack), DateTime.Now);

                    var fn = TimeSeriesExport.GetIncommingFileName("daily",cbtt,pcode);
                    HydrometDailySeries.WriteToArcImportFile(s, cbtt, pcode, fn);

                }
                else if( interval == TimeInterval.Irregular)
                {
                    if (usgs_parameter.ToLower() == "watertemp")
                        s = new UsgsRealTimeSeries(site_id, UsgsRealTimeParameter.Temperature);
                    else
                    if (usgs_parameter.ToLower() == "gageheight")
                        s = new UsgsRealTimeSeries(site_id, UsgsRealTimeParameter.GageHeight);
                    else if (usgs_parameter.ToLower() == "discharge")
                        s = new UsgsRealTimeSeries(site_id, UsgsRealTimeParameter.Discharge);
                    else
                    {
                        Logger.WriteLine("Error: the parameter '" + usgs_parameter + "' is not yet supported");
                        s = new Series();
                    }

                    try
                    {
                        s.Read(DateTime.Now.AddHours(-hoursBack), DateTime.Now);

                        if (usgs_parameter.ToLower() == "watertemp" && pcode.ToLower() == "wf")
                        {
                            //(°C × 9/5) + 32 = °F
                            s = s * 9.0 / 5.0 + 32.0;
                        }

                        s.RemoveMissing();
                        if (s.Count > 0)
                        {
                            var fn = TimeSeriesExport.GetIncommingFileName("instant", cbtt, pcode);
                            HydrometInstantSeries.WriteToHydrometFile(s, cbtt, pcode, WindowsUtility.GetShortUserName(), fn);
                        }
                    }
                    catch(Exception e)
                    {
                        errors = true;
                        Console.WriteLine(e.Message);
                    }
                }
            }



            if (errors)
                throw new Exception("Error reading one or more sites");
        }

        private static TimeInterval GetInterval(DataRow row)
        {
            string interval = row["interval"].ToString();
            if (interval.ToLower() == "daily")
                return TimeInterval.Daily;

            else if (interval.ToLower() == "instant")
                return TimeInterval.Irregular;
            else
            {
                Logger.WriteLine("Error: invalid interval: '"+interval+"'");
                throw new Exception();
            }

        }
        
    }
}
