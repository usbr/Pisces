using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Forms
{
    public partial class StatisiticalMethodOptions : UserControl
    {
        public StatisiticalMethodOptions()
        {
            InitializeComponent();
        }

        public StatisticalMethods StatisticalMethods
        {
            get 
            {
               StatisticalMethods rval = StatisticalMethods.None;
               if (checkBoxMax.Checked)
                   rval = rval | StatisticalMethods.Max;
               if (checkBoxMin.Checked)
                   rval = rval | StatisticalMethods.Min;
               if (checkBoxSum.Checked)
                   rval = rval | StatisticalMethods.Sum;
               if (checkBoxCount.Checked)
                   rval = rval | StatisticalMethods.Count;
               if (checkBoxMean.Checked)
                   rval = rval | StatisticalMethods.Mean;


               return rval;
            }
            set { 
                 StatisticalMethods sm = value;
                checkBoxMax.Checked = (value == StatisticalMethods.Max);
                checkBoxMin.Checked = (value == StatisticalMethods.Min);
                checkBoxSum.Checked = (value == StatisticalMethods.Sum);
                checkBoxCount.Checked = (value == StatisticalMethods.Count);
                checkBoxMean.Checked = (value == StatisticalMethods.Mean);
            
               }
        }
    }
}
