using System;
using System.Collections.Generic;
using System.Text;

namespace Reclamation.TimeSeries
{
    /// <summary>
    /// Represents the range and amount of time series data
    /// </summary>
    public class PeriodOfRecord
    {
       

        public PeriodOfRecord(DateTime t1, DateTime t2, int count)
        {
            m_t1 = t1;
            m_t2 = t2;
            m_count = count;
        }


        DateTime m_t1;

        public DateTime T1
        {
            get { return m_t1; }
            set { m_t1 = value; }
        }
        DateTime m_t2;

        public DateTime T2
        {
            get { return m_t2; }
            set { m_t2 = value; }
        }
        int m_count;

        public int Count
        {
            get { return m_count; }
            set { m_count = value; }
        }

        public override string ToString()
        {
            return T1.ToShortDateString().PadLeft(10) + " "
                   + T2.ToShortDateString().PadLeft(10) + " ";
        }

        /// <summary>
        /// returns true of time is within tha max and min DateTime of this series.
        /// </summary>
        public bool InRange(DateTime time)
        {
            if (time >= this.T1 && time <= this.T2)
                return true;
            return false;
        }
       
    }
}
