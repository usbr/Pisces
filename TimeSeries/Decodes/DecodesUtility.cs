using System;
using Reclamation.Core;
using System.Data;

namespace Reclamation.TimeSeries.Decodes
{
    public class DecodesUtility
    {

        static string[] tableNames = {
         "site","site_property", "sitename","configsensor","platformconfig","platform","platformproperty",
         "transportmedium","decodesscript","formatstatement","platformsensor","platformsensorproperty",
         "scriptsensor","datatype","configsensordatatype","equipmentmodel","equipmentproperty","unitconverter",
                                     "networklistentry","networklist"};


        public static DecodesDataSet GetDataSet(PostgreSQL svr)
        {
            var ds = new DecodesDataSet();

            for (int i = 0; i < tableNames.Length; i++)
			{
                string sql = "select * from "+tableNames[i];
                var tbl = ds.Tables[tableNames[i]];
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

        //public static void TruncateTablesaa(PostgreSQL svr)
        //{
        //    foreach (var tableName in tableNames)
        //    {
        //        string sql = "delete from " + tableName ;
        //        svr.RunSqlCommand(sql);
        //    }
        //}

    //    public static void AddEquipment(DecodesDataSet decodes)  {


    //        var e = decodes.equipmentmodel;
    //        var ep = decodes.equipmentproperty; 

    //        e.AddequipmentmodelRow(1, "MCF-S", "Sutron", "", "", "DCP");
    //        e.AddequipmentmodelRow(2, "MCF-A", "FTX", "TX312", "", "DCP");
    //        e.AddequipmentmodelRow(3, "MCF-D", "Design Analysis", "", "", "DCP");
    //        e.AddequipmentmodelRow(4, "MCF-V", "Vitel", "", "", "DCP");
    //        e.AddequipmentmodelRow(5, "CR1000", "CSV", "", "", "TransportMedium");



    //        ep.AddequipmentpropertyRow(1, "DataOrder", "D");
    //        ep.AddequipmentpropertyRow(2, "DataOrder", "A");
    //        ep.AddequipmentpropertyRow(3, "DataOrder", "D");
    //        ep.AddequipmentpropertyRow(4, "DataOrder", "D");
    //        ep.AddequipmentpropertyRow(5, "DataOrder", "A");


    //}

        public static void UpdateServer(PostgreSQL svr, DecodesDataSet decodes)
        {
            
            foreach (var tableName in tableNames)
            {
                Console.Write("Saving " + tableName);
                var c = svr.SaveTable(decodes.Tables[tableName]);
                Console.WriteLine(" " + c.ToString() + " records");
            }
        }
    }
}
