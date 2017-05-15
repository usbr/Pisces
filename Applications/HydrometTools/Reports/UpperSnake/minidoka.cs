using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Project1
{
    public partial class minidoka : UserControl
    {
        public minidoka()
        {
            InitializeComponent();
        }
        internal void LoadData(Dictionary<string, double> today, Dictionary<string, double> yesterday)
        {
            string feet = " feet";
            string cfs = " cfs";
            Date.Text = DateTime.Now.ToString("dddd MMMM dd") + ", " + DateTime.Now.ToString("yyyy");
            double minqu = ((today["min af"] - yesterday["min af"]) / 1.98347) + today["mini q"]+today["nmci qc"]+today["smci qc"];
            if (minqu < 0)
            {
                minqu = 0;
            }
            minQUtextBox.Text = minqu.ToString("F0") + cfs;

            if (today["min fb"] > yesterday["min fb"])
            {
                double chng = today["min fb"] - yesterday["min fb"];
                minFBtextBox.Text = today["min fb"].ToString("F2") + feet + " Rising at a rate of " + chng.ToString("F2") + " ft/day";
            }
            else if (today["min fb"] < yesterday["min fb"])
            {
                double chng = yesterday["min fb"] - today["min fb"];
                minFBtextBox.Text = today["min fb"].ToString("F2") + feet + " Falling at a rate of " + chng.ToString("F2") + " ft/day";
            }
            else
            {
                minFBtextBox.Text = today["min fb"].ToString("F2") + feet + " and is Steady";
            }

            if (today["min fb"] >= 4245)
            {
                minpatterntextBox.Text = "The reservoir is currently full";
            }
            else
            {
                double down = 4245 - today["min fb"];
                double prct = (today["min af"] / 95200) * 100;
                minpatterntextBox.Text = "The reservoir is currently down " + down.ToString("F2") + feet + " and is " + prct.ToString("F0") + "% full";
            }
            minQtextBox.Text = today["mini q"].ToString("F0") + cfs;

            if (today["min fb"] >= 4240.1)
            {
                WalcottBoat.Text = "The Walcott boat ramp is " + (today["min fb"] - 4240.1).ToString("F1") + " feet under water";
            }
            else
            {
                WalcottBoat.Text = "The Walcott boat ramp is currently out of service";
            }
            
        }
    }
}
