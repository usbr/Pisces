using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.Core;

namespace Reclamation.TimeSeries.Forms.Hydromet
{
    public partial class ServerSelection : UserControl
    {
        public ServerSelection()
        {
            InitializeComponent();
            ReadSettings();
        }

        private void SaveToUserPref()
        {
            if (this.radioButtonPnHydromet.Checked)
            {
                UserPreference.Save("HydrometServer", HydrometHost.PN.ToString());
            }
            else
                if (this.radioButtonBoiseLinux.Checked)
            {
                UserPreference.Save("HydrometServer", HydrometHost.PNLinux.ToString());
            }
            else
                if (this.radioButtonYakHydromet.Checked)
            {
                UserPreference.Save("HydrometServer", HydrometHost.Yakima.ToString());
            }
            else
                    if (this.radioButtonGP.Checked)
            {
                UserPreference.Save("HydrometServer", HydrometHost.GreatPlains.ToString());
            }
            else
                if (this.radioButtonLocal.Checked)
                {
                    UserPreference.Save("HydrometServer", HydrometHost.LocalSource.ToString());
                }

                else
                    if (this.radioButtonYakLinux.Checked)
                    {
                        UserPreference.Save("HydrometServer", HydrometHost.YakimaLinux.ToString());
                    }
            
            
            UserPreference.Save("TimeSeriesDatabaseName", this.textBoxDbName.Text);

            


        }

        private void ReadSettings()
        {
            var svr = HydrometInfoUtility.HydrometServerFromPreferences();

            // retiring PN 
           if (svr == HydrometHost.PNLinux || svr == HydrometHost.PN)
            {
                this.radioButtonBoiseLinux.Checked = true;
            }
            else
                if (svr == HydrometHost.Yakima)
            {
                this.radioButtonYakHydromet.Checked = true;
            }
            else
                    if (svr == HydrometHost.GreatPlains)
            {
                this.radioButtonGP.Checked = true;
            }
            else if (svr == HydrometHost.LocalSource)
            {
                radioButtonLocal.Checked = true;
            }

            this.textBoxDbName.Text = UserPreference.Lookup("TimeSeriesDatabaseName", "timeseries");

        }

        private void serverChanged(object sender, EventArgs e)
        {
            SaveToUserPref();
        }
    }
}
