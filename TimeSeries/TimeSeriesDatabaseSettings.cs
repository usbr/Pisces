using System;
using Reclamation.Core;
using System.Data;

namespace Reclamation.TimeSeries
{
    public class TimeSeriesDatabaseSettings
    {

        BasicDBServer m_server;
        DataTable piscesInfo;
        
        public TimeSeriesDatabaseSettings(BasicDBServer db)
        {
            m_server = db;
            

            piscesInfo = db.Table("piscesinfo");
            piscesInfo.PrimaryKey = new DataColumn[] { piscesInfo.Columns[0] };
        }

        public int GetDBVersion()
        {
            DataTable tbl = m_server.Table("version", "Select value from piscesinfo where name = 'FileVersion'");
            return Convert.ToInt32(tbl.Rows[0][0]);
        }


        public void Save()
        {
            m_server.SaveTable(piscesInfo);
        }

        public string Get(string name, string defaultIfMissing)
        {
            DataRow row = piscesInfo.Rows.Find(name);
            if (row == null)
                piscesInfo.Rows.Add(name, defaultIfMissing);

            return piscesInfo.Rows.Find(name)["value"].ToString();
        }

        private void UpdateValue(string name, string value)
        {
            DataRow row = piscesInfo.Rows.Find(name);
            if (row == null)
                row = piscesInfo.Rows.Add(name, value);

            row["value"] = value;
        }

        public void Set(string name, object value)
        {
            if (value is DateTime)
            {
                DateTime t = (DateTime)value;
                UpdateValue(name, t.ToString(Series.DateTimeFormatInstantaneous));
            }
            else
            {
                UpdateValue(name, value.ToString());
            }
        }

        public bool ReadBoolean(string name, bool defaultIfMissing)
        {
            return Convert.ToBoolean(Get(name, defaultIfMissing.ToString()));
        }

        public DateTime ReadDateTime(string name, DateTime defaultIfMissing)
        {
            return Convert.ToDateTime(
                Get(name, defaultIfMissing.ToString(Series.DateTimeFormatInstantaneous)));
        }

        public decimal ReadDecimal(string name, decimal defaultIfMissing)
        {
            return Convert.ToDecimal(Get(name, defaultIfMissing.ToString()));
        }

        public string ReadString(string name, string defaultIfMissing)
        {
            return Convert.ToString(Get(name, defaultIfMissing));
        }
    }
}