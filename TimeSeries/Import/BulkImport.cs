using Reclamation.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries.Import
{
    public static class BulkImport
    {

        /// <summary>
        /// Imports multiple series using an excel control file.
        /// the control file has one entry per row(series)
        /// and specifies file format and other details for 
        /// the series.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="excelFilename"></param>
        public static void Import(TimeSeriesDatabase db, string excelFilename)
        {
            
            NpoiExcel xls = new NpoiExcel(excelFilename);

            var dirXls = Path.GetDirectoryName(excelFilename);
            var tbl = xls.ReadDataTable(0, true, true);

            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                var row = tbl.Rows[i];
                var format = ReadString(row, "format");
                var path = ReadString(row, "path").Split(new char[]{',', '/', '\\'},
                                            StringSplitOptions.RemoveEmptyEntries);
                var filename = ReadString(row, "filename");
                
                var tablename = ReadString(row, "tablename");
                if (filename != "" &&  Path.IsPathRooted(filename))
                {
                     string dir = Path.GetDirectoryName(db.DataSource);
                    filename = Path.Combine(dir, filename);
                }
                else
                {
                    filename = Path.Combine(dirXls, filename);
                }
                 
                
                var sheetName = ReadString(row, "sheet");

                var interval = TimeInterval.Daily;

                var timeinterval = ReadString(row, "timeinterval").ToLower();
               
                if (timeinterval.IndexOf("instant") >= 0)
                    interval = TimeInterval.Irregular;
                if (timeinterval.IndexOf("month") >= 0)
                    interval = TimeInterval.Monthly;
                if (timeinterval.IndexOf("daily") >= 0)
                    interval = TimeInterval.Daily;


                Series s = new Series("",interval);
              

                if (format == "csv" || format == "txt" )
                {
                    if (File.Exists(filename))
                    {
                        s = new TextSeries(filename);
                        s.Read();
                    }
                    else
                    {
                        if (filename.Trim() != "")
                            Logger.WriteLine("File not found: '" + filename + "'");
                    }
                }
                else if( format == "xls-daily-yearlysheets")
                {
                    s = ImportMultiSheetDailySeriesExcel.ImportSpreadsheet(filename);
                }
                s.Parameter = ReadString(row, "parameter");
                s.Name = ReadString(row, "name");
                s.SiteID = ReadString(row, "siteid");
                s.Units = ReadString(row, "units");
                s.Notes = ReadString(row, "notes");
                s.Expression = ReadString(row, "expression");

                if (s.Expression != "")
                    s.Provider = "CalculationSeries";


                if (tablename != "")
                {
                    s.Table.TableName = tablename;
                }
                else
                {
                    s.Table.TableName = "ts_" + s.Name.ToLower();
                }
                var folder = db.RootFolder;
                if( path.Length >0)
                {
                    folder = db.GetOrCreateFolder(path);
                }


                if( db.SeriesExists(s.Table.TableName))
                {
                    Console.WriteLine("Table already exists '"+s.Table.TableName+"'");
                    continue;
                }

                int id = db.AddSeries(s, folder);

                var prop = ReadString(row, "properties").Split(new char[] { ',' },
                                   StringSplitOptions.RemoveEmptyEntries);

                for (int p = 0; p < prop.Length; p++)
                {
                    var tokens = prop[p].Split(':');

                    if (tokens.Length == 2)
                    {
                        s.Properties.Set(tokens[0], tokens[1]);
                    }
                }
                s.Properties.Save();               
            }

        }

        private static string ReadString(DataRow row, string columnName)
        {
            if (!row.Table.Columns.Contains(columnName))
                return "";

            if (row[columnName] == DBNull.Value)
            {
                return "";
            }
            return row[columnName].ToString().Trim();
        }

    }
}
