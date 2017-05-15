using System;
using Reclamation.Core;
using System.Data;
using Reclamation.TimeSeries.Hydromet;
using System.IO;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Decodes
{
    /// <summary>
    /// Creates a decodes setup for a cbtt from hydromet MCF files
    /// </summary>
    public class McfToDecodes
    {
        public static void Import(string serverIP, string database,string password, string networkListName,
            string[] siteList)
        {

            string fn = @"C:\temp\mcf.xml";
            Performance perf = new Performance();
            var pnMcf = new McfDataSet();

            if (File.Exists(fn) && File.GetLastWriteTime(fn).Date == DateTime.Now.Date)
            { // read existing file.
                Logger.WriteLine("Reading existing XML");
                pnMcf.ReadXml(fn);
            }
            else
            {
             Logger.WriteLine("Reading csv files from MCF");
             pnMcf = McfUtility.GetDataSetFromCsvFiles();
             pnMcf.WriteXml(fn);
            }

            DumpInfo(pnMcf);

            Application.DoEvents();
            
            Logger.WriteLine(serverIP);

            
            var svr = PostgreSQL.GetPostgresServer(database, serverIP, "decodes",password) as PostgreSQL;
            DecodesUtility.UpdateSequences(svr);
            DecodesUtility.GenerateDataSet(@"c:\temp\decodes.xsd",svr);

            var decodes = DecodesUtility.GetDataSet(svr);
            McfDecodesConverter c = new McfDecodesConverter(svr, pnMcf, decodes,siteList);

            c.importMcf(networkListName);

      
        }

        /// <summary>
        /// prints number of rows for each table in dataset.
        /// </summary>
        /// <param name="pnMcf"></param>
        private static void DumpInfo(McfDataSet pnMcf)
        {
            for (int i = 0; i < pnMcf.Tables.Count; i++)
            {
                Logger.WriteLine(pnMcf.Tables[i].TableName+"  has "+pnMcf.Tables[i].Rows.Count );
            }
        }

        /// <summary>
        /// Appends source mcf data to dest
        /// </summary>
        /// <param name="pnMcf"></param>
        /// <param name="yakMcf"></param>
        private static void AppendMcf(McfDataSet dest, McfDataSet source)
        {
            for (int i = 0; i < dest.Tables.Count; i++)
            {
                var dt = dest.Tables[i];
                var st = source.Tables[dt.TableName];

                string keyColumnName = dt.Columns[0].ColumnName;

                for (int r = 0; r < st.Rows.Count; r++)
                {
                    // check if row allready exists in dest
                    DataRow destRow = null;
                    

                    var keyValue = st.Rows[r][keyColumnName].ToString();
                    //if( keyValue == "AMRW")
                      //  Console.WriteLine();

                    var rows = dt.Select(keyColumnName +" = '"+ keyValue+ "'");
                    if( rows.Length >1)
                        throw new Exception("keyColumn contains "+rows.Length.ToString() + " matches");
                    if (rows.Length == 1)
                        destRow = rows[0];
                    else
                    {// or add row
                            destRow = dt.NewRow();
                            dt.Rows.Add(destRow);
                    }
                    
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        string cn = dt.Columns[c].ColumnName;
                        destRow[cn] = st.Rows[r][cn];
                    }
                }
            }
        }


       
        
    }
}
