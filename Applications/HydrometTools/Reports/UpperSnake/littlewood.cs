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
    public partial class littlewood : UserControl
    {
        public littlewood()
        {
            InitializeComponent();
        }
        internal void LoadData(Dictionary<string, double> today, Dictionary<string, double> yesterday)
        {
            string feet = " feet";
            string cfs = " cfs";
            Date.Text = DateTime.Now.ToString("dddd MMMM dd") + ", " + DateTime.Now.ToString("yyyy");
            double wodqu = ((today["wod af"] - yesterday["wod af"]) / 1.98347) + today["wodi q"];
            if (wodqu < 0)
            {
                wodqu = 0;
            }
            wodQUtextBox.Text = wodqu.ToString("F0") + cfs;

            if (today["wod fb"] > yesterday["wod fb"])
            {
                double chng = today["wod fb"] - yesterday["wod fb"];
                wodFBtextBox.Text = today["wod fb"].ToString("F2") + feet + " Rising at a rate of " + chng.ToString("F2") + " ft/day";
            }
            else if (today["wod fb"] < yesterday["wod fb"])
            {
                double chng = yesterday["wod fb"] - today["wod fb"];
                wodFBtextBox.Text = today["wod fb"].ToString("F2") + feet + " Falling at a rate of " + chng.ToString("F2") + " ft/day";
            }
            else
            {
                wodFBtextBox.Text = today["wod fb"].ToString("F2") + feet + " and is Steady";
            }

            if (today["wod fb"] >= 5237.3)
            {
                wodpatterntextBox.Text = "The reservoir is currently full";
            }
            else
            {
                double down = 5237.3 - today["wod fb"];
                double prct = (today["wod af"] / 30000) * 100;
                wodpatterntextBox.Text = "The reservoir is currently down " + down.ToString("F2") + feet + " and is " + prct.ToString("F0") + "% full";
            }
            wodQtextBox.Text = today["wodi q"].ToString("F0") + cfs;

            if (today["wod fb"] >= 5176)
            {
                Boat.Text = "The boat ramp is " + (today["wod fb"] - 5176).ToString("F1") + " feet under water";
            }
            else
            {
                Boat.Text = "The boat ramp is currently out of service";
            }
            
        }
    }
}
