using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Forms
{
    public partial class YearTypeSelector : UserControl
    {
        


        public YearTypeSelector()
        {
            InitializeComponent();
            this.comboBox1.SelectedIndex = 9;
            Enabling();

        }

        
        public event EventHandler<EventArgs> BeginningMonthChanged;

        private void OnBeginninMonthChanged(object sender, EventArgs e)
        {
            // Copy to a temporary variable to be thread-safe.
            EventHandler<EventArgs> temp = BeginningMonthChanged;
            if (temp != null)
                temp(this, EventArgs.Empty);

        }

        private int m_beginningMonth;

        public int BeginningMonth
        {
            get {
                return m_beginningMonth;
            
                }
            set {
                if (value < 0 || value > 12)
                {
                    this.radioButtonWaterYear.Checked = true;
                }
                else
                if (value == 10)
                {
                    this.radioButtonWaterYear.Checked = true;
                }
                else
                if (value == 1)
                {
                    this.radioButtonCalendarYear.Checked = true;
                }
                else
                {
                    this.comboBox1.SelectedIndex = (value - 1);
                }

              // m_beginningMonth = value;
                DetermineCurrentValue();
              }
        }

        private void DetermineCurrentValue()
        {
            int rval = 1;
            if (this.radioButtonCalendarYear.Checked)
            {
                rval = 1;
            }
            else
            if (this.radioButtonWaterYear.Checked)
            {
                rval = 10;
            }
            else
            {
              rval = this.comboBox1.SelectedIndex + 1;
            }
            if (m_beginningMonth != rval)
            {
                m_beginningMonth = rval; // SET BEFORE EVENT
                OnBeginninMonthChanged(this, EventArgs.Empty);
            }
          //  m_beginningMonth = rval;

            this.labelDebug.Text = m_beginningMonth.ToString();
           // return rval;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // go ahead 11 steps to find ending month for custom year
            int i = this.comboBox1.SelectedIndex;
            for (int j = 1; j <= 11; j++)
            {
                if (i == this.comboBox1.Items.Count - 1)
                {
                    i = 0;
                }
                else
                {
                    i++;
                }
            }
            string txt = this.comboBox1.Items[i].ToString();
            this.labelEndingCustomMonth.Text = " - "+txt+ ")";
            DetermineCurrentValue();
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            Enabling();
            DetermineCurrentValue();
        }

        private void Enabling()
        {
            if (this.radioButtonCustom.Checked)
            {
                this.comboBox1.Enabled = true;
            }
            else
            {
                this.comboBox1.Enabled = false;
            }
        }
    }
}
