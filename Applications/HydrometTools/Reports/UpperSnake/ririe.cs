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
    public partial class ririe : UserControl
    {
        public ririe()
        {
            InitializeComponent();
        }
        internal void LoadData(Dictionary<string, double> today, Dictionary<string, double> yesterday)
        {
            string feet = " feet";
            string cfs = " cfs";
            Date.Text = DateTime.Now.ToString("dddd MMMM dd") + ", " + DateTime.Now.ToString("yyyy");
            WTXIQtextBox.Text = today["wtxi q"].ToString("F0") + cfs;
            double rirqu = ((today["rir af"] - yesterday["rir af"]) / 1.98347) + today["riri q"];
            if (rirqu < 0)
            {
                rirqu = 0;
            }
            RIRQUtextBox.Text = rirqu.ToString("F0") + cfs;

            if (today["rir fb"] > yesterday["rir fb"])
            {
                double chng = today["rir fb"] - yesterday["rir fb"];
                RIRFBtextBox.Text = today["rir fb"].ToString("F2") + feet + " Rising at a rate of " + chng.ToString("F2") + " ft/day";
            }
            else if (today["rir fb"] < yesterday["rir fb"])
            {
                double chng = yesterday["rir fb"] - today["rir fb"];
                RIRFBtextBox.Text = today["rir fb"].ToString("F2") + feet + " Falling at a rate of " + chng.ToString("F2") + " ft/day";
            }
            else
            {
                RIRFBtextBox.Text = today["rir fb"].ToString("F2") + feet + " and is Steady";
            }

            if (today["rir fb"] >= 5112.8)
            {
                RIRpatterntextBox.Text = "The reservoir is currently full";
            }
            else
            {
                double down = 5112.8 - today["rir fb"];
                double prct = (today["rir af"] / 80500) * 100;
                RIRpatterntextBox.Text = "The reservoir is currently down " + down.ToString("F2") + feet + " and is " + prct.ToString("F0") + "% full";
            }
            RIRIQtextBox.Text = today["riri q"].ToString("F0") + cfs;

            if (today["rir fb"] >= 5019)
            {
                BlackBoat.Text = "The Blacktail boat ramp is " + (today["rir fb"] - 5019).ToString("F1") + " feet under water";
            }
            else
            {
                BlackBoat.Text = "The Blacktail boat ramp is currently out of service";
            }

            if (today["rir fb"] >= 5030)
            {
                JuniperBoat.Text = "The Juniper ramp is " + (today["rir fb"] - 5030).ToString("F1") + " feet under water";
            }
            else
            {
                JuniperBoat.Text = "The Juniper ramp is currently out of service";
            }            
           
        }
    }
}
