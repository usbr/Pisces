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
    public partial class RangePicker : UserControl
    {
        public RangePicker()
        {
            InitializeComponent();
            Enabling(this, EventArgs.Empty);
        }
        public MonthDayRange MonthDayRange
        {
            get
            {
                if (this.radioButtonMonthDay.Checked)
                {
                    return this.monthDayRangePicker1.MonthDayRange;
                }
                else
                {
                    return this.monthRangePicker1.MonthDayRange;
                }
            }

            set
            {
              monthDayRangePicker1.MonthDayRange = value;
              monthRangePicker1.MonthDayRange = value;
            }
        }

        public int BeginningMonth
        {
            set
            {
                monthRangePicker1.BeginningMonth = value;
                monthDayRangePicker1.BeginningMonth = value;
            }
        }

        private void Enabling(object sender, EventArgs e)
        {
           if (radioButtonMonthDay.Checked)
            {
                monthDayRangePicker1.Enabled = true;
                monthRangePicker1.Enabled = false;
            }
            else
            {
              monthDayRangePicker1.Enabled = false;
              monthRangePicker1.Enabled = true;
            }
        }

    }
}
