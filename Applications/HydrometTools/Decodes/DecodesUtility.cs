using System;
using Reclamation.Core;
using System.Data;
using System.Linq;
using HydrometTools.Decodes;
using System.Configuration;
using System.IO;

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
            string siteid, string outputFileName, string logFileName)
        {
            

            if (!UpdateNessidInRoutingProperties(svr, siteid))
                return;

            // update the output filename
            var sql = "select * from routingspec where name = 'hydromet-tools'";
            var tbl = svr.Table("routingspec", sql);

            if( tbl.Rows.Count != 1)
            {
                Logger.WriteLine("Error: routing spec named 'hydromet-tools' was not found");
                return;
            }
            tbl.Rows[0]["consumerarg"] = outputFileName;
            tbl.Rows[0]["sincetime"] = t1.Year.ToString() + "/" + t1.DayOfYear.ToString("000") + t1.ToString(" HH:mm:ss");
            tbl.Rows[0]["untiltime"] = t2.Year.ToString() + "/" + t2.DayOfYear.ToString("000") + t2.ToString(" HH:mm:ss");
            svr.SaveTable(tbl);

            // run the routing spec.
            //C:\Hydromet\OPENDCS\bin>rs  hydromet-tools
            string dir = ConfigurationManager.AppSettings["DECODES_INSTALL_DIR"];

            if( string.IsNullOrEmpty(dir) || !Directory.Exists(dir))
            {
                Logger.WriteLine("Error: DECODES_INSTALL_DIR is not define in app.config or path not found ");
            }

            var exe = Path.Combine(dir, "bin", "rs.bat");

            var args = "-d 3 -l "+logFileName+" hydromet-tools";
            var rval = ProgramRunner.RunExecutable(exe, args);
            foreach (var item in rval)
            {
                Logger.WriteLine(item);
            }

        }
         
        /// <summary>
        /// Sets the nessid (mediumid) in the decodes properties 
        /// for the routing spec called 'hydromet-tools'
        /// Assuming there is a property name='sc:DCP_ADDRESS_0000'  that contains the 
        /// nessid for routing a single site.
        /// </summary>
        /// <param name="svr"></param>
        /// <param name="siteid"></param>
        /// <returns></returns>
        private static bool UpdateNessidInRoutingProperties(PostgreSQL svr, string siteid)
        {

            // lookup mediumid in decodes
            var sql = "select id,platformdesignator,mediumid "
         + " from platform P join transportmedium T "
         + " on P.id = T.platformid "
         + " where platformdesignator ~*'\\m" + siteid + "\\M'";

            var tbl = svr.Table("a", sql);
            if (tbl.Rows.Count != 1)
            {
                Logger.WriteLine("Error: could not find '" + siteid + "' in the DECODES database");
                return false;
            }
            var mediumid = tbl.Rows[0]["mediumid"].ToString();

            // update medium id in routing spec property
            sql = "SELECT routingspecid, prop_name, prop_value "
                  + " FROM public.routingspecproperty "
                  + " where routingspecid in (select id from routingspec where name='hydromet-tools') "
                  + " and prop_name ='sc:DCP_ADDRESS_0000'";

            tbl = svr.Table("routingspecproperty", sql);
            if (tbl.Rows.Count != 1)
            {
                Logger.WriteLine("Error: could not find 'sc:DCP_ADDRESS_0000' in the DECODES database");
                return false;
            }
            tbl.Rows[0]["prop_value"] = mediumid;

            return svr.SaveTable(tbl) > 0;

        }

    }
}
