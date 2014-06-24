using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Reclamation.Core;

namespace Reclamation.TimeSeries.Forms
{
    public partial class MonthDayRangePicker : UserControl
    {
        private MonthDayRange range;
        public MonthDayRangePicker()
        {
            InitializeComponent();
            range = new MonthDayRange(10, 1, 9, 30);
            MonthDayRange = range;
        }

        public MonthDayRange MonthDayRange
        {
            get {
                UpdateRange();
                return range; 
            }
            set
            {
                this.range = value;
                this.monthDayPicker1.Text = range.Month1.ToString().PadLeft(2, '0')
                       + "/"
                       + range.Day1.ToString().PadLeft(2, '0');

                this.monthDayPicker2.Text = range.Month2.ToString().PadLeft(2, '0')
                                          + "/"
                                          + range.Day2.ToString().PadLeft(2, '0');
            }
        }

        public int BeginningMonth
        {
            get { return range.Month1; }
            set
            {
                if (!range.ValidBeginningMonth(value))
                {
                    DateTime t1 = new DateTime(2000, value, 1);
                    DateTime t2 = t1.AddMonths(11);
                    t2 = new DateTime(2000, t2.Month, DateTime.DaysInMonth(2000, t2.Month));

                    this.MonthDayRange = new MonthDayRange(t1.Month, t1.Day,
                        t2.Month, t2.Day);
                }
            }
         }

        private void monthDayPicker1_Validated(object sender, EventArgs e)
        {
            UpdateRange();

        }

        private void UpdateRange()
        {
            range.Reset(this.monthDayPicker1.Month,
                        this.monthDayPicker1.Day,
                        this.monthDayPicker2.Month,
                        this.monthDayPicker2.Day);

            //range.Month1 = this.monthDayPicker1.Month;
            //range.Day1 = this.monthDayPicker1.Day;
            //range.Month2 = this.monthDayPicker2.Month;
            //range.Day2 = this.monthDayPicker2.Day;
        }
       

        
    }
}
