using System;
using System.Data;
using Reclamation.Core;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using Reclamation.TimeSeries.Hydromet;

namespace HydrometTools.SnowGG
{
    internal class SnowGGUtility
    {

        public SnowGGUtility()
        {

        }

        static DataTable s_groups;
        private static DataTable SnowggGroups
        {
            get
            {
                {
                    var filename = "snowgg_groups.csv";
                    var svr = HydrometInfoUtility.HydrometServerFromPreferences();
                    if (svr == HydrometHost.GreatPlains)
                    {
                        filename = "snowgg_groups_gp.csv";
                    }

                    s_groups = new CsvFile(FileUtility.GetFileReference(filename));
                }

                return s_groups;
            }
        }

        public static string[] GetSnowggGroupList()
        {
            var tbl = DataTableUtility.SelectDistinct(SnowggGroups, "Group");
            return DataTableUtility.Strings(tbl, "", "Group");
        }

        public static string[] GetCbttList(string groupName)
        {
            DataRow[] rows = SnowggGroups.Select("group='" + groupName + "'");
            var rval = new List<string>();

            foreach (DataRow r in rows)
            {
                rval.Add(r["cbtt"].ToString());
            }
            return rval.ToArray();
        }
    }
}