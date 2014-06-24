using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Forms
{
    /// <summary>
    /// TimeWindow control allows user to select full period of
    /// record, range of dates, range to today, or last x number of days.
    /// </summary>
    public partial class TimeWindowSelector : UserControl
    {
        public TimeWindowSelector()
        {
            InitializeComponent();
            timeWindow1 = new TimeSeries.TimeWindow();
            EnableDates(this, EventArgs.Empty);
        }

        public event EventHandler<EventArgs> ClosePressed;
       

        /// <summary>
        /// Returns or sets 1 of 4 possible strings:
        /// FullPeriodOfRecord, FromToDates, FromDateToToday,
        /// or NumDaysFromToday
        /// </summary>
        TimeWindow timeWindow1;
        public TimeWindow TimeWindow
        {
            get
            {
                timeWindow1.FromToDatesT1 = this.dateTimePicker1.Value;
                timeWindow1.FromToDatesT2 = this.dateTimePicker2.Value;
                timeWindow1.FromDateToTodayT1 = this.dateTimePicker3.Value;
                timeWindow1.NumDaysFromToday = this.numericUpDown1.Value;

                if (this.radioButtonFullPeriodOfRecord.Checked)
                {
                    timeWindow1.WindowType = TimeWindowType.FullPeriodOfRecord;
                }
                else
                    if (this.radioButtonFromToDates.Checked)
                    {
                        timeWindow1.WindowType = TimeWindowType.FromToDates;
                    }
                    else
                        if (this.radioButtonFromDateToToday.Checked)
                        {
                            timeWindow1.WindowType = TimeWindowType.FromDateToToday;
                        }
                        else
                            if (this.radioButtonNumDaysFromToday.Checked)
                            {
                                timeWindow1.WindowType = TimeWindowType.NumDaysFromToday;
                            }

                return timeWindow1;
            }
            set
            {
                timeWindow1 = value;
                numericUpDown1.Value = timeWindow1.NumDaysFromToday;
                dateTimePicker1.Value = timeWindow1.FromToDatesT1;
                dateTimePicker2.Value = timeWindow1.FromToDatesT2;
                dateTimePicker3.Value = timeWindow1.FromDateToTodayT1;

                if (value.WindowType == TimeWindowType.FullPeriodOfRecord)
                {
                    this.radioButtonFullPeriodOfRecord.Checked = true;
                }
                else
                    if (value.WindowType == TimeWindowType.FromToDates)
                    {
                        this.radioButtonFromToDates.Checked = true;
                    }
                    else
                        if (value.WindowType == TimeWindowType.FromDateToToday)
                        {
                            this.radioButtonFromDateToToday.Checked = true;
                        }
                        else
                            if (value.WindowType == TimeWindowType.NumDaysFromToday)
                            {
                                this.radioButtonNumDaysFromToday.Checked = true;
                            }
            }
        }
        
        private void EnableDates(object sender, EventArgs e)
        {
            this.dateTimePicker1.Enabled = false;
            this.dateTimePicker2.Enabled = false;
            this.dateTimePicker3.Enabled = false;
            this.numericUpDown1.Enabled = false;

            if (this.radioButtonFromToDates.Checked)
            {
                this.dateTimePicker1.Enabled = true;
                this.dateTimePicker2.Enabled = true;
            }
            else
                if (this.radioButtonFromDateToToday.Checked)
                {
                    this.dateTimePicker3.Enabled = true;
                }
                else
                    if (this.radioButtonNumDaysFromToday.Checked)
                    {
                        this.numericUpDown1.Enabled = true;
                    }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            EventHandler<EventArgs> temp = ClosePressed;
            if( ClosePressed != null)
                ClosePressed(this,EventArgs.Empty);
            //this.ParentForm.Close();
        }

        private void TimeWindowOptions_Load(object sender, EventArgs e)
        {
            this.buttonClose.Visible = (this.ParentForm != null && ParentForm.Name == "PopupHost");
        }

       
        
    }
}