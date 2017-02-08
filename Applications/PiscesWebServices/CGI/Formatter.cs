using Reclamation.TimeSeries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PiscesWebServices.CGI
{
    /// <summary>
    /// base class for different output formats.
    /// </summary>
    public abstract class Formatter
    {

        private bool m_orderByDate = true;

        public bool OrderByDate
        {
            get { return m_orderByDate; }
            set { m_orderByDate = value; }
        }

        string delimeter = ",";

        public string Delimeter
        {
            get { return delimeter; }
            set { delimeter = value; }
        }

        bool m_hourlyOnly = false;

        public bool HourlyOnly
        {
            get { return m_hourlyOnly; }
            set { m_hourlyOnly = value; }
        }


        bool m_printFlags = true;

        public bool PrintFlags
        {
            get { return m_printFlags; }
            set { m_printFlags = value; }
        }
        protected TimeInterval m_interval = TimeInterval.Irregular;

        public TimeInterval Interval
        {
            get { return m_interval; }
            set { m_interval = value; }
        }

        public Formatter(TimeInterval interval, bool printFlags)
        {
            m_interval = interval;
            m_printFlags = printFlags;
        }
        public abstract string FormatFlag(object o);
        public abstract string FormatNumber(object o);
        public abstract string FormatDate(object o);
        public abstract void WriteSeriesHeader(SeriesList list);
        public abstract void WriteSeriesTrailer();

        public abstract void PrintRow(string t0, string[] vals, string[] flags);

        public abstract void WriteLine(string s);

        /// <summary>
        /// Print DataTable composed of tablename,datetime,value[,flag]
        /// with columns for each tablename
        /// </summary>
        /// <param name="list"></param>
        /// <param name="table"></param>
        public virtual void PrintDataTable(SeriesList list, DataTable table )
        {
            var t0 = "";

            if (table.Rows.Count > 0)
                t0 = FormatDate(table.Rows[0][1]);

            var vals = new string[list.Count];
            var flags = new string[list.Count];
            var dict = new Dictionary<string, int>();
            for (int i = 0; i < list.Count; i++)
            {
                dict.Add(list[i].Table.TableName, i);
            }

            string t = "";
            bool printThisRow = false;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                var row = table.Rows[i];

                t = FormatDate(row[1]);

                if (t != t0)
                {
                    if (printThisRow)
                        PrintRow(t0, vals, flags);
                    vals = new string[list.Count];
                    flags = new string[list.Count];
                    t0 = t;
                }

                vals[dict[row[0].ToString()]] = FormatNumber(row[2]);
                flags[dict[row[0].ToString()]] = FormatFlag(row[3]);

                DateTime date = Convert.ToDateTime(row[1]);
                bool topOfHour = date.Minute == 0;
                printThisRow = HourlyOnly == false || (HourlyOnly && topOfHour);

            }
            if (printThisRow)
                PrintRow(t, vals, flags);
        }

    }
}
