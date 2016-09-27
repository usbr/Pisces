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

            var tbl = xls.ReadDataTable(0, true, true);

            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                var row = tbl.Rows[i];
                var format = ReadString(row, "format");
                var units = ReadString(row, "units");
                var folderName = ReadString(row, "folder");
                var filename = ReadString(row, "filename");
                if (!Path.IsPathRooted(filename))
                {
                     string dir = Path.GetDirectoryName(db.DataSource);
                    filename = Path.Combine(dir, filename);
                }
                var siteID = ReadString(row, "siteid");
                var name = ReadString(row, "name");
                var sheetName = ReadString(row, "sheet");

                Series s = null;
                

                if (format == "csv" || format == "txt")
                {
                    s = new TextSeries(filename);
                    s.Read();
                }
//                else if( format == "xls-monthly-wateryear")
  //              {
    //                throw new NotImplementedException("oops the programmer forgot to finish up some work");
      //          }
                else if( format == "xls-daily-yearlysheets")
                {
                    s = ImportMultiSheetDailySeriesExcel.ImportSpreadsheet(filename);
                }

                s.Units = units;
                s.Name = name;
                s.SiteID = siteID;

                var folder = db.RootFolder;
                if( folderName != "")
                {
                    folder = db.GetOrCreateFolder(folder.Name, folderName);
                }

                db.AddSeries(s, folder);
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
