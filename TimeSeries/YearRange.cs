using System;
using System.Collections.Generic;
using System.Text;

namespace Reclamation.TimeSeries
{
    /// <summary>
    /// YearRange represents a 12 month period (water year)
    /// </summary>
    public class YearRange
    {
        int year;
        List<int> yearList = new List<int>();

        public List<int> Years
        {
            get { return yearList; }
            //set { yearList = value; }
        }
        List<int> monthList = new List<int>();

        public List<int> Months
        {
            get { return monthList; }
            //set { monthList = value; }
        }

      /// <summary>
      /// Constructor for Year Range, using named water year 
      /// </summary>
      /// <param name="year">named year</param>
      /// <param name="beginningMonth">month where the year increments (USGS uses 10 for October)</param>
        public YearRange(int waterYear, int beginningMonth)
        {
            this.year = waterYear;
            this.beginningMonth = beginningMonth;
            DateTime date1 = new DateTime(2000, beginningMonth, 1);
            int endMonth = date1.AddMonths(11).Month; // compute ending month

            t2 = new DateTime(waterYear,endMonth, DateTime.DaysInMonth(waterYear,endMonth),23,59,59);
            t1 = t2.AddYears(-1).AddDays(1);
            t1 = new DateTime(t1.Year, t1.Month, t1.Day, 0, 0, 0);

            BuildYearAndMonthList();
        }

        private void BuildYearAndMonthList()
        {
            DateTime t = t1;
            for (int i = 0; i < 12; i++)
            {
                Years.Add(t.Year);
                Months.Add(t.Month);
                t = t.AddMonths(1);
            }
        }

        public override string ToString()
        {
            return year + " " + DateTime1.ToString("MMM") + " - " + DateTime2.ToString("MMM");
        }
      
        /// <summary>
        /// determine the water year that has a DateTime t and begins in
        /// specified beginningMonth
        /// used to determine the first water year, when doing something
        /// like sum,min,max for each water year.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="beginningMonth">month where year increments</param>
        public YearRange(DateTime t, int beginningMonth )
        {
            this.beginningMonth = beginningMonth;

            if (beginningMonth > 1 && t.Month >= beginningMonth)
            {
                this.year = t.Year + 1;
            }
            else
            {
                this.year = t.Year;
            }
            DateTime date1 = new DateTime(2000, beginningMonth, 1);
            int endMonth = date1.AddMonths(11).Month; // compute ending month

            t2 = new DateTime(year, endMonth, DateTime.DaysInMonth(year, endMonth), 23, 59, 59);
            t1 = t2.AddYears(-1).AddDays(1);
            t1 = new DateTime(t1.Year, t1.Month, t1.Day, 0, 0, 0);

            BuildYearAndMonthList();
        }


        private int beginningMonth;

        private DateTime t1;

       

        public int Year
        {
            get { return year; }
        }
	
        public DateTime DateTime1
        {
            get { return t1; }
            set { t1 = value; }
        }

        private DateTime t2;

        public DateTime DateTime2
        {
            get { return t2; }
            set { t2 = value; }
        }

        public int BeginningMonth
        {
            get { return beginningMonth; }
        }


        
    }
}
