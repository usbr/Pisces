using System;
using Reclamation.Core;
using System.Data;
using System.Linq;
using HydrometTools.Decodes;

namespace Reclamation.TimeSeries.Decodes
{
    public class DecodesUtility
    {

        static string[] tableNames = {
         "site","site_property", "sitename","platformconfig","configsensor","platform","platformproperty",
         "transportmedium","decodesscript","formatstatement","platformsensor","platformsensorproperty",
         "unitconverter","scriptsensor","datatype","configsensordatatype","equipmentmodel","equipmentproperty",
                                     "networklistentry","networklist"};

        public static void GenerateDataSet(string filename, PostgreSQL svr)
        {
            DataSet ds = new DataSet();
            foreach (var tn in svr.TableNames())
            {
                var t = svr.Table(tn, "select * from " + tn + " where 1=2");
                ds.Tables.Add(t);
            }

            ds.WriteXmlSchema(filename);
        }


        public static DecodesDataSet GetDataSet(PostgreSQL svr)
        {
            var ds = new DecodesDataSet();

            for (int i = 0; i < tableNames.Length; i++)
			{
                string sql = "select * from "+tableNames[i];
                var tbl = ds.Tables[tableNames[i]];

                // check for schema changes

                var tblx = svr.Table(tableNames[i], sql+" where 1=2");
                var cnx = tblx.Columns.Cast<DataColumn>()
                    .Select(x => x.ColumnName).ToArray();
                var cn = tbl.Columns.Cast<DataColumn>()
                    .Select(x => x.ColumnName).ToArray(); 

                if( cnx.Length != cn.Length)
                {
                    Logger.WriteLine("database: "+String.Join(",",cnx));
                    Logger.WriteLine("dataset : "+String.Join(",",cn));
                }


                svr.FillTable(tbl,sql);

                if (tbl.Rows.Count > 0)
                {
                    Console.WriteLine("Notice Existing data. "+tableNames[i]+" has "+tbl.Rows.Count+" rows ");
                }
            }
            return ds;
        }

        ///// <summary>
        ///// Used to create DataSET.
        ///// </summary>
        ///// <param name="svr"></param>
        ///// <returns></returns>
        //public static DecodesDataSet GetEmptyDataSet(PostgreSQL svr)
        //{
        //    var ds = new DecodesDataSet();

        //    for (int i = 0; i < tableNames.Length; i++)
        //    {
        //        string sql = "select * from " + tableNames[i] + " where 2=1";
        //        svr.FillTable(ds.Tables[tableNames[i]], sql);
        //    }

        //    return ds;
        //}

        public static void CreateDecodesXSD(PostgreSQL svr, string outputFilename)
        {
        
            DataSet ds = new DataSet("DecodesDataSet");
            foreach (var tableName in tableNames)
            {
                string sql = "select * from "+tableName + " where 1=2";
                ds.Tables.Add(svr.Table(tableName,sql));
            }

            ds.WriteXmlSchema(outputFilename);
        }


        public static void UpdateServer(PostgreSQL svr, DecodesDataSet decodes)
        {
            
            foreach (var tableName in tableNames)
            {
                Console.Write("Saving " + tableName);
                var c = svr.SaveTable(decodes.Tables[tableName]);
                Console.WriteLine(" " + c.ToString() + " records");
            }
        }

        public  static void UpdateSequences(PostgreSQL svr)
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


        /// <summary>
        /// Runs a DECODES routing spec, for a specific site
        /// </summary>
        /// <returns></returns>
        public static void RunDecodesRoutingSpec(PostgreSQL svr,string routingSpecName, 
            DateTime t1, DateTime t2,
            string siteid, string outputFileName)
        {


        }

    }
}
