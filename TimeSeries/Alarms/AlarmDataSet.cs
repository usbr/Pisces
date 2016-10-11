using Reclamation.Core;
using System;
using System.Data;
namespace Reclamation.TimeSeries.Alarms
{
}
namespace Reclamation.TimeSeries.Alarms {
    
    
    public partial class AlarmDataSet {
        private Core.BasicDBServer m_server;

        public static AlarmDataSet CreateInstance(BasicDBServer server = null)
        {
            AlarmDataSet rval;
            if (server == null)
            { // create from config files.
                var db = TimeSeriesDatabase.InitDatabase(new Arguments(new string[] { }));
                rval = new AlarmDataSet();
                rval.m_server = db.Server;
            }
            else
            {// create using server
                rval = new AlarmDataSet();
                rval.m_server = server;
            }
            return rval;
        }


        public void SaveTable(DataTable table)
        {
            m_server.SaveTable(table);
        }

        public alarm_listDataTable GetList()
        {
            var tbl = new alarm_listDataTable();
            m_server.FillTable(tbl, "select * from alarm_list order by list");
            return tbl;
        }
        
        /// <summary>
        /// Gets a list of alarms in priority order for processing
        /// only alarms with status (new, or unconfirmed)
        /// </summary>
        /// <returns></returns>
        public alarm_phone_queueDataTable GetNewAlarms()
        {
            AlarmDataSet.alarm_phone_queueDataTable tbl = new alarm_phone_queueDataTable();
            string sql = "select * from alarm_phone_queue where status='new' or status = 'unconfirmed' order by priority";
            m_server.FillTable(tbl, sql);
            return tbl;
        }


        public string[] GetPhoneNumbers(string list)
        {
            string sql = "select phone from alarm_recipient where list='"+list+"' order by call_order";
            var tbl = m_server.Table("alarm_recipient", sql);
            return DataTableUtility.Strings(tbl,"","phone");
        }


        public partial class alarm_scriptsDataTable
        {
            public int NextID()
            {
                if (this.Rows.Count > 0)
                {
                    return ((int)this.Compute("Max(id)", "") + 1);
                }
                return 1;
            }
        }


         public alarm_recipientDataTable GetRecipients(string label)
        {
            var tbl = new alarm_recipientDataTable();
            m_server.FillTable(tbl, "select * from alarm_recipient where list = '" + label + "' order by call_order");
            tbl.idColumn.AutoIncrementSeed = m_server.NextID("alarm_recipient", "id");
            tbl.listColumn.DefaultValue = label;
            return tbl;
        }


         public void AddNewAlarmGroup(string label)
         {
             var tbl = GetList();
             tbl.Addalarm_listRow(label);
             m_server.SaveTable(tbl);
         }

         public void DeleteAlarmGroup(string label)
         {
             DeleteRecipients(label);
             var tbl = GetList();
             var rows =tbl.Select("list = '"+label+"'");
             if( rows.Length ==1)
             {
                 rows[0].Delete();
             }
             m_server.SaveTable(tbl);
         }

        public void DeleteRecipients(string label)
         {
             var tbl = GetRecipients(label);
             for (int i = tbl.Rows.Count - 1; i >= 0; i--)
             {
                 DataRow dr = tbl.Rows[i];
                     dr.Delete();
             }
             m_server.SaveTable(tbl);
         }

        public AlarmDataSet.alarm_phone_queueDataTable GetAlarmQueue(bool everything=false)
        {
            var tbl = new AlarmDataSet.alarm_phone_queueDataTable();
            string sql = "select * from alarm_phone_queue ";

            if( ! everything)
            {
                sql += " where status in ('new', 'unconfirmed')";
            }

            m_server.FillTable(tbl,sql);

            return tbl;

        }
    }
    
}



