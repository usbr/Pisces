using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries
{
    public class TimeRange
    {
        private List<TimeRange> rval = new List<TimeRange>();
        private DateTime start;
        private DateTime end;
        private DateTime temp;
        private int maxDaysInMemory;

        public TimeRange(DateTime start, DateTime end, int maxDaysInMemory = 60)
        {
            this.start = start;
            this.end = end.EndOfDay();
            this.maxDaysInMemory = maxDaysInMemory;

        }

        public List<TimeRange> List()
        {
            while (start < end)
            {
                temp = start.AddDays(maxDaysInMemory).EndOfDay();
                if (temp > end)
                {
                    temp = end;
                }
                rval.Add(new TimeRange(start, temp, maxDaysInMemory));
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
