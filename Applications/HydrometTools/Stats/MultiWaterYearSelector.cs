using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HydrometTools.Stats
{
    public partial class MultiWaterYearSelector : UserControl
    {
        public MultiWaterYearSelector()
        {
            InitializeComponent();
            labelYearCount.Text = "";
        }

        public string SetGroupBoxTitle
        {
            get
            {
                return this.groupBox1.Text;
            }
            set
            {
                this.groupBox1.Text = value;
            }
        }

        public string cbtt {

            get { return this.textBoxCbtt.Text; }
       }

        public string pcode
        {

            get { return this.textBoxPcode.Text; }
        }

        public DateTime T1
        {
            get
            {
                var year1 = Convert.ToInt32(textBoxYear.Text);
                var t = new DateTime(year1 - 1, 10, 1);
                return t;
            }
        }
        public DateTime T2
        {
            get
            {
                var year2 = Convert.ToInt32(textBoxEndYear.Text);
                var t = new DateTime(year2, 9, 30);
                return t;
            }
        }

        private void updateYearLabel(object sender, EventArgs e)
        {
            labelYearCount.Text = ((T2 - T1).Days / 365).ToString() + " years";
        }
    }
}
