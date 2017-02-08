using System.Configuration;
using Reclamation.Core;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using Reclamation.TimeSeries.Hydromet;
using System.Data;
namespace HydrometNotifications
{
    
    
    public partial class AlarmDataSet {


        

        public partial class alarm_definitionRow
        {
            //string expression1 = @"(?<database>[id]{1})\:(?<cbtt>\%?\w+)\s+(?<pcode>\w+)";
            string expression1 = @"(?<database>\w+)\:\s*(?<cbtt>\%?\w+)\s+(?<pcode>\w+)";
            // i:jck af     # instant data (jackson lake content)
            // d:jck fb      # daily data (jackson lake forebay)
            public string pcode
            {
                get
                {
                    return Regex.Match(this.data_source, expression1).Groups["pcode"].Value;
                }
            }
            public string cbtt
            {
                get
                {
                    return Regex.Match(this.data_source, expression1).Groups["cbtt"].Value;
                }
            }

            public bool IsVirtual = false;


            public string database
            {
                get {
                    return Regex.Match(this.data_source, expression1).Groups["database"].Value;
                }
            }

        }
       

        public static void PreloadInstantHydrometData(AlarmDataSet.alarm_definitionDataTable alarmdef)
        {
            // find all instant data, and largest hours_back, to make a single cache of data
            var cbttPcodes = (from row in alarmdef.AsEnumerable()
                         where row.database.ToLower() == "i"  // instant hydromet data
                         select row.cbtt.ToLower() + " " + row.pcode.ToLower()).Distinct().ToArray();

            if (cbttPcodes.Length == 0)
                return;

            // TO DO.  throw error if mixing quality and 'regular' data.
            //if (MixedQualityData(cbttPcodes))
            //{
            //    throw new ArgumentException("Error: quality and Mixing qual");
            //}

            var hours_back = (from row in alarmdef.AsEnumerable()
                              where row.database.ToLower() == "i" 
                              select row.hours_back).Max();

    

            DateTime t1 = DateTime.Now.AddHours(-hours_back);
            DateTime t2 = DateTime.Now;
           // HydrometInstantSeries.KeepFlaggedData = true;

            var cache = new HydrometDataCache();
            cache.Add(String.Join(",", cbttPcodes).Split(','), t1, t2,
                HydrometHost.PN, Reclamation.TimeSeries.TimeInterval.Irregular, hours_back);

            HydrometInstantSeries.Cache = cache;
            Console.WriteLine(cbttPcodes);
        }

        static BasicDBServer m_svr;
        public static BasicDBServer DB
        {
            get
            {
                return m_svr;
            }
            set
            {
                m_svr = value;
            }
        }


       public static int GetNextID(string tableName)
       {
           DataTable tbl = DB.Table(tableName,"Select Max(id) from "+tableName);

                if ( tbl.Rows.Count >0)
                {
                    return Convert.ToInt32(tbl.Rows[0][0]) +1;
                }
                return 1;
       }


       public static alarm_definitionDataTable GetAlarmDefinitionTable(string alarmGroup)
       {

           var rval = new alarm_definitionDataTable();

           string sql = "select * from alarm_definition where group_name = '" + alarmGroup + "' ";
           DB.FillTable(rval, sql);
           rval.group_nameColumn.DefaultValue = alarmGroup;

           return rval;
       }

        public static alarm_definitionDataTable GetActiveAlarmDefinitionTable(string alarmGroup)
        {
            var rval = new alarm_definitionDataTable();

            string sql = "select * from alarm_definition where group_name = '"+alarmGroup +"' and enabled is true ";
            DB.FillTable(rval,sql);
            rval.group_nameColumn.DefaultValue = alarmGroup;
          

            var virtualRows = new List<alarm_definitionRow>();

            int counter = -1;
            foreach (var row in rval)
            {
                if  ( row.cbtt.IndexOf("%") == 0 )
                { 
                    // parse out group name preceded by % ending with a space
                    string siteGroup_name = row.cbtt.Substring(1); 
                    // lookup the list of sites in this group
                    
                    foreach (var cbtt in GetSiteList(siteGroup_name))
                    {
                        // insert new 'virtual' rows in the alarm_definitionDataTable
                        var newRow = rval.Newalarm_definitionRow();
                        newRow.ItemArray = row.ItemArray;
                        newRow.data_source = row.data_source.Replace("%" + siteGroup_name, cbtt);
                        newRow.id = counter;
                        newRow.IsVirtual = true; // so we won't try to make changes (and therefor try to save)
                        virtualRows.Add(newRow);
                        counter--;
                    }
                    // throw away this row
                    row.Delete();
                }
            }

            foreach (var row in virtualRows)
            {
                rval.Rows.Add(row);
            }

            rval.AcceptChanges();

            return rval;
        }

        private static string[] GetSiteList(string group_name)
        {
            var tbl = DB.Table("tmp", "select site_list from alarm_sites where group_name = '" + group_name + "'");
            if (tbl.Rows.Count > 0)
            {
                return tbl.Rows[0][0].ToString().Split(',');
            }
            throw new Exception("Error: could not find a site list for group_name '" + group_name + "'");
        }
        public partial class alarm_definitionDataTable
        {
            public void Save()
            {
                // TO DO .remove 'virtual' rows
                //this.Select('
                DB.SaveTable(this);
            }
        }

        internal static string[] EmailAddresses(string emailGroup)
        {
           var tbl = DB.Table("tmp", "select email_list from alarm_group where group_name = '"+emailGroup+"'");
            if( tbl.Rows.Count == 0)
                tbl = DB.Table("tmp", "select email_list from alarm_group where group_name = 'default'");

           if (tbl.Rows.Count > 0)
           {
               return tbl.Rows[0][0].ToString().Split(',');
           }
           throw new Exception("Error: could not find group_name '" + emailGroup + "'");
        }

        internal static int SaveEmailAddresses(string emailGroup, string emailAddresses)
        {
            var tbl = DB.Table("alarm_group", "select * from alarm_group where group_name = '" + emailGroup + "'");
            
            if (tbl.Rows.Count == 0)
            {
                DataRow row = tbl.NewRow();
                row["group_name"] = emailGroup;
                row["email_list"] = emailAddresses;
                tbl.Rows.Add(row);
            }
            else
            {
                tbl.Rows[0]["group_name"] = emailGroup;
                tbl.Rows[0]["email_list"] = emailAddresses;
            }

           return DB.SaveTable(tbl);
 
        }

        internal static DataTable GetGroups()
        {
            var sql = "select distinct group_name from alarm_definition order by group_name";
            return DB.Table("alarm_definition", sql);
        }

        internal static DataTable alarm_sites()
        {
            return DB.Table("alarm_sites");
        }
    }
}


