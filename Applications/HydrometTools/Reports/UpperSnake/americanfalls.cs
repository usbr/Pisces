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
    public partial class americanfalls : UserControl
    {
        public americanfalls()
        {
            InitializeComponent();
        }
        internal void LoadData(Dictionary<string, double> today, Dictionary<string, double> yesterday)
        {
            string feet = " feet";
            string cfs = " cfs";
            Date.Text = DateTime.Now.ToString("dddd MMMM dd") + ", " + DateTime.Now.ToString("yyyy");
            double amfqu = ((today["amf af"] - yesterday["amf af"]) / 1.98347) + today["amfi q"];
            if (amfqu < 0)
            {
                amfqu = 0;
            }
            AMFQUtextBox.Text = amfqu.ToString("F0") + cfs;

            if (today["amf fb"] > yesterday["amf fb"])
            {
                double chng = today["amf fb"] - yesterday["amf fb"];
                AMFFBtextBox.Text = today["amf fb"].ToString("F2") + feet + " Rising at a rate of " + chng.ToString("F2") + " ft/day";
            }
            else if (today["amf fb"] < yesterday["amf fb"])
            {
                double chng = yesterday["amf fb"] - today["amf fb"];
                AMFFBtextBox.Text = today["amf fb"].ToString("F2") + feet + " Falling at a rate of " + chng.ToString("F2") + " ft/day";
            }
            else
            {
                AMFFBtextBox.Text = today["amf fb"].ToString("F2") + feet + " and is Steady";
            }

            if (today["amf fb"] >= 4354.5)
            {
                AMFpatterntextBox.Text = "The reservoir is currently full";
            }
            else
            {
                double down = 4354.5 - today["amf fb"];
                double prct = (today["amf af"] / 1672590) * 100;
                AMFpatterntextBox.Text = "The reservoir is currently down " + down.ToString("F2") + feet + " and is " + prct.ToString("F0") + "% full";
            }
            AMFQtextBox.Text = today["amfi q"].ToString("F0") + cfs;
            
            if (today["amf fb"] >= 4311.6)
            {
                WestsideBoat.Text = "The Westside boat ramp is " + (today["amf fb"] - 4311.6).ToString("F1") + " feet under water";
            }
            else
            {
                WestsideBoat.Text = "The Westside boat ramp is currently out of service";
            }
            
            if (today["amf fb"] >= 4326.9)
            {
                SportBoat.Text = "The Sportsmans Park ramp is " + (today["amf fb"] - 4326.9).ToString("F1") + " feet under water";
            }
            else
            {
                SportBoat.Text = "The Sportsmans Park ramp is currently out of service";
            }
            
            if (today["amf fb"] >= 4330)
            {
                SeagulBoat.Text = "The Seagul Bay ramp is " + (today["amf fb"] - 4330).ToString("F1") + " feet under water";
            }
            else
            {
                SeagulBoat.Text = "The Seagul Bay ramp is currently out of service";
            }
            
            if (today["amf fb"] >= 4327)
            {
                CityBoat.Text = "The City Marina ramp is " + (today["amf fb"] - 4327).ToString("F1") + " feet under water";
            }
            else
            {
                CityBoat.Text = "The City Marina ramp is currently out of service";
            }
            
        }
    }
}
