using System;
using System.Collections.Generic;
using System.Text;

namespace Reclamation.Core
{
    /// <summary>
    /// MonthDayRange contains a range of dates
    /// such as oct01 to sep30
    /// </summary>
    public class MonthDayRange
    {
        int month1, day1, month2, day2;
        DateTime d1;
        DateTime d2;

        /// <summary>
        /// Creates a MonthDayRange for water year range. Oct-Sep
        /// </summary>
        public MonthDayRange()
        {
            Init(10, 1, 9, 30);
        }
        public MonthDayRange(int month1, int day1, 
            int month2, int day2)
        {
            Init(month1, day1, month2, day2);
        }

        private void Init(int month1, int day1, int month2, int day2)
        {
            Reset(month1, day1, month2, day2);
        }
        
        private void UpdateInternalDates()
        {
            d1 = new DateTime(2000, month1, day1);
            d2 = new DateTime(2000, month2, day2);
        }

        public bool Contains(int month, int day)
        {
            DateTime d = new DateTime(2000, month,day);
            if (d2 < d1)
            {
                return !(d > d2 && d < d1); // exclusion range
            }
            return (d >= d1 && d <= d2);
        }

        public bool Contains(DateTime date)
        {
            return Contains(date.Month, date.Day);
        }

        public override string ToString()
        {
            return d1.ToString("MM/dd") + " - " + d2.ToString("MM/dd");
        }


        /// <summary>
        /// returns beginning month day in MMMDD format i.e. OCT01
        /// </summary>
        public string MMMDD1
        {
            get { return d1.ToString("MMMdd"); }
        }

        /// <summary>
        /// returns ending month day in MMMDD format i.e. OCT01
        /// </summary>
        public string MMMDD2
        {
            get { return d2.ToString("MMMdd"); }
        }

        public int Month1
        {
            get { return this.month1;  }
            //set
            //{
            //    this.month1 = value;
            //    UpdateInternalDates();
            //}
        }
        public int Month2
        {
            get { return this.month2; }
            //set
            //{
            //    this.month2 = value;
            //    UpdateInternalDates();
            //}
        }
        public int Day1
        {
            get { return this.day1;}
            //set
            //{
            //    this.day1 = value;
            //    UpdateInternalDates();
            //}
        }
        public int Day2
        {
            get { return this.day2; }
            //set
            //{
            //    this.day2 = value;
            //    UpdateInternalDates();
            //}
        }


        public void Reset(int m1, int d1, int m2, int d2)
        {
            this.month1 = m1;
            this.day1 = d1;
            this.month2 = m2;
            this.day2 = d2;
            UpdateInternalDates();
        }

        /// <summary>
        /// Gets list of dates for this range(based on year 2000)
        /// </summary>
        /// <returns></returns>
        public DateTime[] GetDates()
        {
            var rval = new List<DateTime>();

            DateTime t = d1;

            if (d2.Month == 2 && d2.Day == 29)
                throw new NotImplementedException("feb 29 not supported");

            while ( !(t.Day == d2.Day && t.Month == d2.Month))
            {
                rval.Add(t);
                t = t.AddDays(1);
            }

            return rval.ToArray();
        }

        public string[] GetDateStrings(string format)
        {
            var rval = new List<string>();
            DateTime[] dates = GetDates();

            for (int i = 0; i < dates.Length; i++)
            {
                rval.Add(dates[i].ToString(format));
            }
            return rval.ToArray();
        }

        public bool ValidBeginningMonth(int beginningMonth)
        {
            List<int> months = new List<int>();
            int m = beginningMonth;
            for (int i = 0; i < 12; i++)
            {
                months.Add(m);
                m++;
                if (m > 12)
                {
                    m = 1;
                }
            }

            int idx1 = months.IndexOf(Month1);
            int idx2 = months.IndexOf(Month2);
            return (idx2 >= idx1);
        }
    }
}
