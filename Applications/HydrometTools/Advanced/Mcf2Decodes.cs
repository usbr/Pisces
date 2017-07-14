using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reclamation.TimeSeries.Decodes;
using Reclamation.Core;
using System.IO;

namespace HydrometTools.Advanced
{
    public partial class Mcf2Decodes : UserControl
    {
        public Mcf2Decodes()
        {
            InitializeComponent();
        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
          
            
            Reclamation.Core.Logger.LogHistory.Clear();
            Reclamation.Core.Logger.EnableLogger(true);
            Reclamation.Core.Logger.OnLogEvent += Logger_OnLogEvent;
            try
            {
                string[] siteList = textBoxSiteList.Text.Trim().Split(',');
                if( File.Exists(textBoxSiteList.Text))
                {
                    siteList = File.ReadAllLines(textBoxSiteList.Text);
                }
                  
                McfToDecodes.Import(textBoxServer.Text, textBoxDbName.Text, 
                   textBoxPass.Text,textBoxNetworklist.Text, siteList,textBoxMrdbPath.Text);
            }
            catch (Exception ex)
            {
                Logger.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
            }
        }

        void Logger_OnLogEvent(object sender, Reclamation.Core.StatusEventArgs e)
        {
            this.textBoxLogger.Lines = Reclamation.Core.Logger.LogHistory.ToArray();
        }

       
    }
}
