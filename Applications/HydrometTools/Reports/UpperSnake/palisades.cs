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
    public partial class palisades : UserControl
    {
        public palisades()
        {
            InitializeComponent();
        }
        internal void LoadData(Dictionary<string, double> today, Dictionary<string, double> yesterday)
        {
            string feet = " feet";
            string cfs = " cfs";
            Date.Text = DateTime.Now.ToString("dddd MMMM dd") + ", " + DateTime.Now.ToString("yyyy");
            double palid = ((today["pal af"] - yesterday["pal af"]) / 1.98347) + today["pali q"];
            if (palid < 0)
            {
                palid = 0;
            }
            PALIDtextBox.Text = palid.ToString("F0") + cfs;

            if (today["pal fb"] > yesterday["pal fb"])
            {
                double chng = today["pal fb"] - yesterday["pal fb"];
                PALFBtextBox.Text = today["pal fb"].ToString("F2") + feet + " Rising at a rate of " + chng.ToString("F2") + " ft/day";
            }
            else if (today["pal fb"] < yesterday["pal fb"])
            {
                double chng = yesterday["pal fb"] - today["pal fb"];
                PALFBtextBox.Text = today["pal fb"].ToString("F2") + feet + " Falling at a rate of " + chng.ToString("F2") + " ft/day";
            }
            else
            {
                PALFBtextBox.Text = today["pal fb"].ToString("F2") + feet + " and is Steady";
            }

            if (today["pal fb"] >= 5620)
            {
                PALpatterntextBox.Text = "The reservoir is currently full";
            }
            else
            {
                double down = 5620 - today["pal fb"];
                double prct = (today["pal af"] / 1200000) * 100;
                PALpatterntextBox.Text = "The reservoir is currently down " + down.ToString("F2") + feet + " and is " + prct.ToString("F0") + "% full";
            }
            PALIQtextBox.Text = today["pali q"].ToString("F0") + cfs;

            if (today["pal fb"] >= 5552.8)
            {
                BlowoutBoat.Text = "The Blowout boat ramp is " + (today["pal fb"] - 5552.8).ToString("F1") + " feet under water";
            }
            else
            {
                BlowoutBoat.Text = "The Blowout boat ramp is currently out of service";
            }

            if (today["pal fb"] >= 5512.0)
            {
                CalamityBoat.Text = "The Calamity ramp is " + (today["pal fb"] - 5512.0).ToString("F1") + " feet under water";
            }
            else
            {
                CalamityBoat.Text = "The Calamity ramp is currently out of service";
            }          

        }
          
    }
}          
