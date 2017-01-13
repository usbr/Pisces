using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Forms
{
    

    public partial class SummaryOption : UserControl
    {
        public SummaryOption()
        {
            InitializeComponent();
        }
        public event EventHandler<EventArgs> AggregateTypeChanged;

        public StatisticalMethods StatisticalMethods
        {
            get {

                if (this.radioButtonNone.Checked)
                    return StatisticalMethods.None;
                else if (this.radioButtonAnnualMax.Checked)
                    return StatisticalMethods.Max;
                else if (this.radioButtonAnnualMin.Checked)
                    return StatisticalMethods.Min;
                else if (this.radioButtonSum.Checked)
                    return StatisticalMethods.Sum;
                else if (this.radioButtonAverage.Checked)
                    return StatisticalMethods.Average;
                else
                    throw new NotImplementedException("internal error, invalid Aggregate selection");

            }
            set {

                if (value == StatisticalMethods.None)
                    this.radioButtonNone.Checked = true;
                else if (value == StatisticalMethods.Max)
                    this.radioButtonAnnualMax.Checked = true;
                else if (value == StatisticalMethods.Min)
                    this.radioButtonAnnualMin.Checked = true;
                else if (value == StatisticalMethods.Sum)
                    this.radioButtonSum.Checked = true;
                else if (value == StatisticalMethods.Average)
                    this.radioButtonAverage.Checked = true;
               

            }

        }

        private void radioButtonCheckedChanged(object sender, EventArgs e)
        {

           // Copy to a temporary variable to be thread-safe.
            EventHandler<EventArgs> temp = AggregateTypeChanged;
            if (temp != null)
                temp(this, EventArgs.Empty);

        }


        public event EventHandler<EventArgs> OptionClicked;

        private void SummaryOption_MouseClick(object sender, MouseEventArgs e)
        {
            Console.WriteLine("SummaryOption_MouseClick");
        }

        private void SummaryOption_MouseDown(object sender, MouseEventArgs e)
        {
            Console.WriteLine("SummaryOption_MouseDown");
        }

        private void radioButtonNone_MouseClick(object sender, MouseEventArgs e)
        {
            EventHandler<EventArgs> temp = OptionClicked;
            if (OptionClicked != null)
                OptionClicked(this, EventArgs.Empty);
            Console.WriteLine("radioButtonNone_MouseClick");
        }
    }
}
