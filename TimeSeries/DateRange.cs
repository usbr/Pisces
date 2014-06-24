using System;
using Reclamation.Core;

namespace Reclamation.TimeSeries
{
    /// <summary>
    /// DateRange represents range within a year or water year.
    /// DateRange can count the number of days between two Dates.
    /// </summary>
    public class DateRange
    {
        DateTime date1;
        DateTime date2;
        int beginningMonth;

        public bool IsValid
        {
            get { return date1 != DateTime.MinValue; }
        }


        public DateRange()
        {
            date1 = DateTime.MinValue;
            date2 = DateTime.MaxValue;

        }
        public DateRange(DateTime date1, DateTime date2)
        {
            this.date1 = date1;
            this.date2 = date2;
            this.beginningMonth = 1;//january
        }

        public DateRange(MonthDayRange range, int year, int beginningMonth)
        {
            this.beginningMonth = beginningMonth;
            if (!range.ValidBeginningMonth(beginningMonth))
            {
                throw new ArgumentOutOfRangeException(
                    "Please check the date range you entered. It needs to be consistent\n"
                    +"with the type of year: i.e. 'water year', 'calendar year', or custom year\n"
                    +"your range is "
                    +range.ToString()+ " and your beginning month is "+beginningMonth);
            }

            YearRange yearRng = new YearRange(year, beginningMonth);
            
            int idx = yearRng.Months.IndexOf(range.Month1);
            if (idx < 0)
            {
                throw new Exception();
            }
            date1 = LeapSafeDate(yearRng.Years[idx], range.Month1, range.Day1);

            idx = yearRng.Months.IndexOf(range.Month2);
            if (idx < 0)
            {
                throw new Exception();
            }
            date2 = LeapSafeDate(yearRng.Years[idx], range.Month2, range.Day2);
            date2 =date2.AddHours(23).AddMinutes(59).AddSeconds(59);

            if (date2 < date1)
            {
                throw new ArgumentOutOfRangeException("The range " + range.ToString() +
                     " is not valid ");
            }

        }

        /// <summary>
        /// get Beginning Date
        /// </summary>
        public DateTime DateTime1
        {
            get { return date1; }
        }

        /// <summary>
        /// get Ending Date
        /// </summary>
        public DateTime DateTime2
        {
            get { return date2; }
        }
	
        /// <summary>
        /// Returns number of days in this range
        /// </summary>
        public int Count
        {
            get
            {
                TimeSpan ts = new TimeSpan(date2.Ticks - date1.Ticks);
                return ts.Days + 1;
            }
        }
        /// <summary>
        /// If possible creates a DateTime that is valid
        /// for any year.
        /// </summary>
        private DateTime LeapSafeDate(int year, int month, int day)
        {
          DateTime rval = new DateTime();

          if (!DateTime.IsLeapYear(year) && month == 2 && day == 29)
          {
              rval = new DateTime(year, month, 28);
          }
          else
          {
              rval = new DateTime(year, month, day);
          }
          return rval;
        }
    }
}
