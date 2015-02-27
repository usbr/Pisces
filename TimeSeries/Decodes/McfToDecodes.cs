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
        public static void Import(string serverIP, string database,string siteFilter,string networkListName)
        {

            string fn = @"C:\temp\mcf.xml";
            Performance perf = new Performance();
            var pnMcf = new McfDataSet();

            if (File.Exists(fn) && File.GetLastWriteTime(fn).Date != DateTime.Now.Date)
            { // read existing file.
                Logger.WriteLine("Reading existing XML");
                pnMcf.ReadXml(fn);
            }
            else
            {
             Logger.WriteLine("Reading csv files from MCF");
             pnMcf = McfUtility.GetDataSetFromCsvFiles(Globals.LocalConfigurationDataPath);
             pnMcf.WriteXml(fn);
            }

            DumpInfo(pnMcf);

            Application.DoEvents();
            
            Logger.WriteLine(serverIP);

            var cs = PostgreSQL.CreateADConnectionString(serverIP, database, "decodes");
            PostgreSQL svr = new PostgreSQL(cs);
            var decodes = DecodesUtility.GetDataSet(svr);
            McfDecodesConverter c = new McfDecodesConverter(svr, pnMcf, decodes);

            c.importMcf(siteFilter.Split(','),networkListName);

            UpdateSequences(svr);
      
        }

        /// <summary>
        /// prints number of rows for each talble in dataset.
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


        private static void UpdateSequences(PostgreSQL svr)
        {
            // fix sequences
            string sql = ""

            + "SELECT setval('datapresentationidseq', (SELECT MAX(id) FROM datapresentation)); "
            + "SELECT setval('datasourceidseq', (SELECT MAX(id) FROM datasource));"
            + "SELECT setval('datatypeidseq', (SELECT MAX(id) FROM datatype)); "
            + "SELECT setval('decodesscriptidseq', (SELECT MAX(id) FROM decodesscript)); "
            + "SELECT setval('enumidseq', (SELECT MAX(id) FROM enum)); "
            + "SELECT setval('equipmentidseq', (SELECT MAX(id) FROM equipmentmodel)); "
            + "SELECT setval('networklistidseq', (SELECT MAX(id) FROM networklist)); "
            + "SELECT setval('platformconfigidseq', (SELECT MAX(id) FROM platformconfig)); "
            + "SELECT setval('platformidseq', (SELECT MAX(id) FROM platform)); "
            + "SELECT setval('presentationgroupidseq', (SELECT MAX(id) FROM presentationgroup)); "
            + "SELECT setval('routingspecidseq', (SELECT MAX(id) FROM routingspec)); "
            + "SELECT setval('siteidseq', (SELECT MAX(id) FROM site)); "
            + "SELECT setval('unitconverteridseq', (SELECT MAX(id) FROM unitconverter)); ";
            Console.WriteLine(sql);
            svr.RunSqlCommand(sql);
            PostgreSQL.ClearAllPools();
        }

        
    }
}
