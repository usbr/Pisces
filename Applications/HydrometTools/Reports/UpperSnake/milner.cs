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
    public partial class milner : UserControl
    {
        public milner()
        {
            InitializeComponent();
        }
        internal void LoadData(Dictionary<string, double> today, Dictionary<string, double> yesterday)
        {
            string feet = " feet";
            string cfs = " cfs";
            Date.Text = DateTime.Now.ToString("dddd MMMM dd") + ", " + DateTime.Now.ToString("yyyy");
            if (today["mil fb"] > yesterday["mil fb"])
            {
                double chng = today["mil fb"] - yesterday["mil fb"];
                milFBtextBox.Text = today["mil fb"].ToString("F2") + feet + " Rising at a rate of " + chng.ToString("F2") + " ft/day";
            }
            else if (today["mil fb"] < yesterday["mil fb"])
            {
                double chng = yesterday["mil fb"] - today["mil fb"];
                milFBtextBox.Text = today["mil fb"].ToString("F2") + feet + " Falling at a rate of " + chng.ToString("F2") + " ft/day";
            }
            else
            {
                milFBtextBox.Text = today["mil fb"].ToString("F2") + feet + " and is Steady";
            }

            if (today["mil fb"] >= 11)
            {
                milpatterntextBox.Text = "The reservoir is currently full";
            }
            else
            {
                double down = 11 - today["mil fb"];
                double prct = (today["mil fb"] / 11) * 100;
                milpatterntextBox.Text = "The reservoir is currently down " + down.ToString("F2") + feet + " and is " + prct.ToString("F0") + "% full";
            }
            milQtextBox.Text = (today["mhpi q"]+today["mili q"]).ToString("F0") + cfs;
        }
    }
}
