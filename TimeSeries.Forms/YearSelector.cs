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
    public partial class YearSelector : UserControl
    {
        public YearSelector()
        {
            InitializeComponent();
        }


        public int[] SelectedYears
        {
            get
            {
                string input = this.textBox1.Text;

                try
                {
                    return WaterYear.WaterYearsFromRange(input);
                }
                catch (Exception)
                {
                    MessageBox.Show("formatting error with water years selected in display type " + input);
                }
                return new int[] { };

            }
            set
            {
                string s = "";
                for (int i = 0; i < value.Length; i++)
                {
                    s += value[i].ToString() + " ";
                }
                this.textBox1.Text = s;
            }
        }

       

    }
}
