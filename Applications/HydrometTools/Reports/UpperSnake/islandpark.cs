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
    public partial class islandpark : UserControl
    {
        public islandpark()
        {
            InitializeComponent();
        }
        internal void LoadData(Dictionary<string, double> today, Dictionary<string, double> yesterday)
        {
            string feet = " feet";
            string cfs = " cfs";
            Date.Text = DateTime.Now.ToString("dddd MMMM dd") + ", " + DateTime.Now.ToString("yyyy");
            double islqu = ((today["isl af"] - yesterday["isl af"]) / 1.98347) + today["isli q"];
            if (islqu < 0)
            {
                islqu = 0;
            }
            ISLQUtextBox.Text = islqu.ToString("F0") + cfs;

            if (today["isl fb"] > yesterday["isl fb"])
            {
                double chng = today["isl fb"] - yesterday["isl fb"];
                ISLFBtextBox.Text = today["isl fb"].ToString("F2") + feet + " Rising at a rate of " + chng.ToString("F2") + " ft/day";
            }
            else if (today["isl fb"] < yesterday["isl fb"])
            {
                double chng = yesterday["isl fb"] - today["isl fb"];
                ISLFBtextBox.Text = today["isl fb"].ToString("F2") + feet + " Falling at a rate of " + chng.ToString("F2") + " ft/day";
            }
            else
            {
                ISLFBtextBox.Text = today["isl fb"].ToString("F2") + feet + " and is Steady";
            }

            if (today["isl fb"] >= 6303)
            {
                ISLpatterntextBox.Text = "The reservoir is currently full";
            }
            else
            {
                double down = 6303 - today["isl fb"];
                double prct = (today["isl af"] / 135205) * 100;
                ISLpatterntextBox.Text = "The reservoir is currently down " + down.ToString("F2") + feet + " and is " + prct.ToString("F0") + "% full";
            } 
            ISLQtextBox.Text = today["isli q"].ToString("F0") + cfs;
            
            if (today["isl fb"] >= 6293.8)
            {
                MccreaBoat.Text = "The Mc Crea boat ramp is " + (today["isl fb"] - 6293.8).ToString("F1") + " feet under water";
            }
            else
            {
                MccreaBoat.Text = "The Mc Crea boat ramp is currently out of service";
            }

            if (today["isl fb"] >= 6296.5)
            {
                MillBoat.Text = "The Mill Creek ramp is " + (today["isl fb"] - 6296.5).ToString("F1") + " feet under water";
            }
            else
            {
                MillBoat.Text = "The Mill Creek ramp is currently out of service";
            }

            if (today["isl fb"] >= 6294.7)
            {
                WestBoat.Text = "The West End ramp is " + (today["isl fb"] - 6294.7).ToString("F1") + " feet under water";
            }
            else
            {
                WestBoat.Text = "The West End ramp is currently out of service";
            }

            if (today["isl fb"] >= 6281.4)
            {
                ButterBoat.Text = "The Buttermilk ramp is " + (today["isl fb"] - 6281.4).ToString("F1") + " feet under water";
            }
            else
            {
                ButterBoat.Text = "The Buttermilk ramp is currently out of service";
            }

            if (today["isl fb"] >= 6278.4)
            {
                PondsBoat.Text = "The Pond's Lodge ramp is " + (today["isl fb"] - 6278.4).ToString("F1") + " feet under water";
            }
            else
            {
                PondsBoat.Text = "The Pond's Lodge ramp is currently out of service";
            }
                      
            HENDate.Text = DateTime.Now.ToString("dddd MMMM dd") + ", " + DateTime.Now.ToString("yyyy");
            double henid = ((today["hen af"] - yesterday["hen af"]) / 1.98347) + today["heni q"];
            if (henid < 0)
            {
                henid = 0;
            }
            HENQUtextBox.Text = henid.ToString("F0") + cfs;

            if (today["hen fb"] > yesterday["hen fb"])
            {
                double chng = today["hen fb"] - yesterday["hen fb"];
                HENFBtextBox.Text = today["hen fb"].ToString("F2") + feet + " Rising at a rate of " + chng.ToString("F2") + " ft/day";
            }
            else if (today["hen fb"] < yesterday["hen fb"])
            {
                double chng = yesterday["hen fb"] - today["hen fb"];
                HENFBtextBox.Text = today["hen fb"].ToString("F2") + feet + " Falling at a rate of " + chng.ToString("F2") + " ft/day";
            }
            else
            {
                HENFBtextBox.Text = today["hen fb"].ToString("F2") + feet + " and is Steady";
            }

            if (today["hen fb"] >= 16.6)
            {
                HENpatterntextBox.Text = "The reservoir is currently full";
            }
            else
            {
                double down = 16.6 - today["hen fb"];
                double prct = (today["hen af"] / 89737) * 100;
                HENpatterntextBox.Text = "The reservoir is currently down " + down.ToString("F2") + feet + " and is " + prct.ToString("F0") + "% full";
            }
            HENIQtextBox.Text = today["heni q"].ToString("F0") + cfs;
        }
    }
}
