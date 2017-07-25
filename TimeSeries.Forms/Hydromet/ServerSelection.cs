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

        }

        private void ReadSettings()
        {
            var svr = HydrometInfoUtility.HydrometServerFromPreferences();
            if (svr == HydrometHost.PN)
            {
                this.radioButtonPnHydromet.Checked = true;
            }
            else
                if (svr == HydrometHost.PNLinux)
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

        }

        private void serverChanged(object sender, EventArgs e)
        {
            SaveToUserPref();
        }
    }
}
