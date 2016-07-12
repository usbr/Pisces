using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries
{
    public class TimeRange
    {
        private List<TimeRange> rval = null;
        private DateTime start;
        private DateTime end;
        private DateTime temp;
        private int days;

        public TimeRange(DateTime start, DateTime end)
        {
            this.start = start;
            this.end = end.EndOfDay();
        }

        public List<TimeRange> Split(int days = 60)
        {
            rval = new List<TimeRange>();
            this.days = days;
            while (start < end)
            {
                temp = start.AddDays(days).EndOfDay();
                if (temp > end)
                {
                    temp = end;
                }
                rval.Add(new TimeRange(start, temp));
                start = temp.NextDay();
            }
            return rval;
        }

        public DateTime StartDate {
            get
            {
                return start;
            }
            set
            {
                StartDate = value;
            }
        }

        public DateTime EndDate {
            get
            {
                return end;
            }
            set
            {
                EndDate = value;
            } 
        }
    }
}
