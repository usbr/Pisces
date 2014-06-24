using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Forms
{
    public partial class MonthDayPicker : UserControl
    {
        public MonthDayPicker()
        {
            InitializeComponent();
        }

        public int Month
        {
            get
            {
                TryParse();
                return m;
            }
        }

        public override string Text
        {
            get { return this.maskedTextBox1.Text; }
            set { this.maskedTextBox1.Text = value; }
        }
        public int Day
        {
            get {
                TryParse();
                return d; }
        }


        int m, d;
        bool TryParse()
        {
          string s = maskedTextBox1.Text;

          int idx =  s.IndexOf('/');
           if( idx <=0)
               return false;

           string s1 = s.Substring(0, idx);
           if (!int.TryParse(s1, out m))
           {
               return false;
           }
           string s2 = s.Substring(idx + 1);
           if (!int.TryParse(s2, out d))
           {
               return false;
           }

           string dateString = "2000-" + m + "-" + d;
            DateTime date;
            if (!DateTime.TryParse(dateString, out date))
            {
                return false;
            }

            return true;
        }

        private void MonthDayPicker_Validating(object sender, CancelEventArgs e)
        {
            if (!TryParse())
            {
                this.toolTip1.IsBalloon = true;
                 toolTip1.Show("month/day is not valid",
                 maskedTextBox1, maskedTextBox1.Location.X, 
                 maskedTextBox1.Location.Y, 5000);
                 e.Cancel = true;
            }

           
        }

        private void maskedTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            toolTip1.Hide(maskedTextBox1);

        }
       
    }
}
