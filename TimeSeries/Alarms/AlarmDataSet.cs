using Reclamation.Core;
using System;
using System.Data;
namespace Reclamation.TimeSeries.Alarms
{
}
namespace Reclamation.TimeSeries.Alarms {
    
    
    public partial class AlarmDataSet {
        private Core.BasicDBServer m_server;

        public Core.BasicDBServer Server
        {
            get { return m_server; }
            set { m_server = value; }
        }


        public void SaveTable(DataTable table)
        {
            m_server.SaveTable(table);
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

    }
    
}



