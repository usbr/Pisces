using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reclamation.Core;
using Reclamation.TimeSeries.Hydromet;

namespace Project1
{
    public partial class jackson : UserControl
    {
        public jackson()
        {
            InitializeComponent();
        }
       
        internal void LoadData(Dictionary<string, double> today, Dictionary<string, double> yesterday)
        {
            string feet = " feet";
            string cfs = " cfs";
            Date.Text = DateTime.Now.ToString("dddd MMMM dd") + ", " + DateTime.Now.ToString("yyyy");
            FLGYQtextBox.Text = today["flgy q"].ToString("F0") + cfs;
            double jckqu = ((today["jck af"] - yesterday["jck af"]) / 1.98347) + today["jck q"];
            if (jckqu < 0)
            {
                jckqu = 0;
            }
            JCKQUtextBox.Text = jckqu.ToString("F0") + cfs;

            if (today["jck fb"] > yesterday["jck fb"])
            {
                double chng = today["jck fb"] - yesterday["jck fb"];
                JCKFBtextBox.Text = today["jck fb"].ToString("F2") + feet + " Rising at a rate of " + chng.ToString("F2") + " ft/day";
            }
            else if (today["jck fb"] < yesterday["jck fb"])
            {
                double chng = yesterday["jck fb"] - today["jck fb"];
                JCKFBtextBox.Text = today["jck fb"].ToString("F2") + feet + " Falling at a rate of " + chng.ToString("F2") + " ft/day";
            }
            else
            {
                JCKFBtextBox.Text = today["jck fb"].ToString("F2") + feet + " and is Steady";
            }

            if (today["jck fb"] >= 6769)
            {
                JCKpatterntextBox.Text = "The reservoir is currently full";
            }
            else
            {
                double down = 6769 - today["jck fb"];
                double prct = (today["jck af"] / 847000) * 100;
                JCKpatterntextBox.Text = "The reservoir is currently down " + down.ToString("F2") + feet + " and is " + prct.ToString("F0") + "% full";
            }

            JCKQtextBox.Text = today["jck q"].ToString("F0") + cfs;
            PCKYQtextBox.Text = today["pcky q"].ToString("F0") + cfs;
            BFKYtextBox.Text = today["bfky q"].ToString("F0") + cfs;
            JKSYtextBox.Text = today["jksy q"].ToString("F0") + cfs;
            ALPYtextBox.Text = today["alpy q"].ToString("F0") + cfs;
            
            if (today["jck fb"] >= 6737.7)
            {
                SgnMtBoat.Text = "The Signal Mt. boat ramp is " + (today["jck fb"] - 6737.7).ToString("F1") + " feet under water";
            }
            else
            {
                SgnMtBoat.Text = "The Signal Mt. boat ramp is currently out of service";
            }
            if (today["jck fb"] >= 6735.7)
            {
                LeekBoat.Text = "The Leek's Marina ramp is " + (today["jck fb"] - 6735.7).ToString("F1") + " feet under water";
            }
            else
            {
                LeekBoat.Text = "The Leek's Marina ramp is currently out of service";
            }
            if (today["jck fb"] >= 6753.7)
            {
                CoultBoat.Text = "The Coulter Bay ramp is " + (today["jck fb"] - 6753.7).ToString("F1") + " feet under water";
            }
            else
            {
                CoultBoat.Text = "The Coulter Bay ramp is currently out of service";
            }
                      
            GRSDate.Text = DateTime.Now.ToString("dddd MMMM dd") + ", " + DateTime.Now.ToString("yyyy");

            if (today["grs fb"] > yesterday["grs fb"])
            {
                double chng = today["grs fb"] - yesterday["grs fb"];
                GRSFBtextBox.Text = today["grs fb"].ToString("F2") + feet + " Rising at a rate of " + chng.ToString("F2") + " ft/day";
            }
            else if (today["grs fb"] < yesterday["grs fb"])
            {
                double chng = yesterday["grs fb"] - today["grs fb"];
                GRSFBtextBox.Text = today["grs fb"].ToString("F2") + feet + " Falling at a rate of " + chng.ToString("F2") + " ft/day";
            }
            else
            {
                GRSFBtextBox.Text = today["grs fb"].ToString("F2") + feet + " and is Steady";
            }

            if (today["grs fb"] >= 7210)
            {
                GRSpatterntextBox.Text = "The reservoir Is currently full";
            }
            else
            {
                double down = 7210 - today["grs fb"];
                double prct = (today["grs af"] / 15204) * 100;
                GRSpatterntextBox.Text = "The reservoir Is currently down " + down.ToString("F2") + feet + " and is " + prct.ToString("F0") + "% full";
            }
            GRSQtextBox.Text = (Math.Pow((today["grs fb"] - 7210), 1.5) * 141).ToString("F0") + cfs;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
