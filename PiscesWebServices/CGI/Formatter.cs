using Reclamation.TimeSeries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PiscesWebServices.CGI
{
    /// <summary>
    /// base class for different output formats.
    /// </summary>
     public abstract class Formatter
    {

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

    }
}
