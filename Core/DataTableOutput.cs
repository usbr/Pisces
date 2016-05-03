using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
namespace Reclamation.Core
{
    public static class DataTableOutput
    {


        /// <summary>
        /// Writes a DataTable to file.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="filename"></param>
        /// <param name="WriteSchema"></param>
        public static void Write(DataTable table, string filename, bool WriteSchema)
        {
            Write(table, filename, WriteSchema, false);
        }

        /// <summary>
        /// Saves contents of DataTable to comma seperated file.
        /// TO DO:  check for internal commas, in string types.
        /// this is a test using DataView to preserver order
        /// of a sorted DataView
        /// </summary>
        public static void Write(DataTable table, string filename,
          bool WriteSchema, bool ConsoleOutput)
        {
            if (table == null)
                return;
            DataView view = table.DefaultView;
            StreamWriter sr = new StreamWriter(filename, false);
            int sz = view.Count;
            int cols = view.Table.Columns.Count;
            bool[] IsStringColumn = new Boolean[cols];
            int c;
            for (c = 0; c < cols; c++)
            {
                if (c < cols - 1)
                {
                    sr.Write(CsvFile.EncodeCSVCell( table.Columns[c].ColumnName.Trim()) + ",");
                    if (ConsoleOutput)
                        Console.Write(table.Columns[c].ColumnName.Trim() + ",");
                }
                else
                {
                    sr.WriteLine(CsvFile.EncodeCSVCell(table.Columns[c].ColumnName.Trim())); // no comma on last
                    if (ConsoleOutput)
                        Console.WriteLine(table.Columns[c].ColumnName.Trim()); // no comma on last
                }
                if (table.Columns[c].DataType.ToString() == "System.String")
                    IsStringColumn[c] = true;
            }


            if (WriteSchema && cols > 0)
            {
                for (c = 0; c < cols - 1; c++)
                {
                    sr.Write(table.Columns[c].DataType);
                    sr.Write(",");
                    if (ConsoleOutput)
                    {
                        Console.Write(table.Columns[c].DataType);
                        Console.Write(",");
                    }
                }

                sr.Write(table.Columns[c].DataType); // no comma on last
                sr.WriteLine();
                if (ConsoleOutput)
                {
                    Console.Write(table.Columns[c].DataType); // no comma on last
                    Console.WriteLine();
                }
            }

            for (int r = 0; r < sz; r++)
            {
                for (c = 0; c < cols; c++)
                {
                    if (IsStringColumn[c])
                    {
                        sr.Write("\"" + view[r][c] + "\"");
                        if (ConsoleOutput)
                            Console.Write("\"" + view[r][c] + "\"");
                    }
                    else
                    {
                        sr.Write(view[r][c]);
                        if (ConsoleOutput)
                            Console.Write(view[r][c]);
                    }
                    if (c < cols - 1)
                    {
                        sr.Write(",");
                        if (ConsoleOutput)
                            Console.Write(",");
                    }

                }
                sr.WriteLine();
                if (ConsoleOutput)
                    Console.WriteLine();
            }
            sr.Close();
        }

        /// <summary>
        /// Creates HTML from a DataTable
        /// http://stackoverflow.com/questions/19682996/datatable-to-html-table
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToHTML(DataTable dt, Func<string, string> f )
        {
            
            StringBuilder html = new StringBuilder();
            html.Append("<table>");
            //add header row
            html.Append( "<tr>");
            for (int i = 0; i < dt.Columns.Count; i++)
                html.Append("<td>" + dt.Columns[i].ColumnName + "</td>");
             html.Append("</tr>");
            //add rows
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                html.Append("<tr>");
                for (int j = 0; j < dt.Columns.Count; j++)
                    html.Append("<td>" + dt.Rows[i][j].ToString() + "</td>");
               html.Append( "</tr>");
            }
            html.Append("</table>");
            return html.ToString();
        }


        /// <summary>
        /// Convert to JSON
        /// http://stackoverflow.com/questions/36062991/how-to-use-odata-in-nancyfx
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToJson(DataTable dt)
        {
        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                serializer.MaxJsonLength = Int32.MaxValue;
                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }
                var resultString = serializer.Serialize(rows);
            return resultString;
        }
    }
}
