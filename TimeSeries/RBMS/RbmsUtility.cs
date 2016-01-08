using Reclamation.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries.RBMS
{
    public class RbmsUtility
    {

        


        /// <summary>
        /// Used to transfer the custom properties from custom SiteCatalog.csv 
        /// to Pisces v2 seriesproperties table.
        /// </summary>
        /// <param name="args"></param>
            public static void SetProperties()
            {
                // import properties from old SiteCatalog
                var props = new string[] {"DrillHole",
"Riser",
"InstType",
"Area",
"Xcoord",
"Strata",
"Acoord",
"Bcoord",
"Units",
"Limit1",
"Limit2",
"Limit3",
"Limit4",
"Limit5"};

                CsvFile csv = new CsvFile(@"C:\TEMP\rbmsdump\sitecatalog.csv");

            // SQLiteServer svr = new SQLiteServer(@"C:\temp\rbmsdump\rbms.pdb");
            SqlServer svr = new SqlServer("ibr1gcpdb003","Pisces");
                var db = new TimeSeriesDatabase(svr);

                //var sc = db.GetSeriesCatalog();
                var sp = db.GetSeriesProperties();
//M - manually read static water level
                foreach (DataRow item in csv.Rows)
                {
                    if (item["isfolder"].ToString().ToLower() == "true")
                        continue;

                    var id = Convert.ToInt32(item["SiteDataTypeID"].ToString());
                    for (int i = 0; i < props.Length; i++)
                    {

                        var p = item[props[i]].ToString().Trim();
                        if (p != "")
                            sp.Set(props[i], p, id);

                        Console.WriteLine(props[i] + ": " + p);
                    }
                }
                Console.WriteLine("Saving");
                db.Server.SaveTable(sp);
            }
        }
}
