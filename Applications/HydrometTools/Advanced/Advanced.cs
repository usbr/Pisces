using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.TimeSeries.Decodes;
using Reclamation.TimeSeries.Forms.MetaData;
using Reclamation.TimeSeries;
using HydrometTools.Decodes;

namespace HydrometTools.Advanced
{
    public partial class AdvancedControl : UserControl
    {
        public AdvancedControl()
        {
            InitializeComponent();
        }

        private void archiverInput1_Load(object sender, EventArgs e)
        {
            
        }

        private void buttonDownloadQuality_Click(object sender, EventArgs e)
        {
            string cbtt = this.textBoxCbttQuality.Text.Trim();
            /// TO get nessdi
            var nessid = HydrometInfoUtility.LookupNessid(cbtt);
            //var nessid = "34748654";
            var tbl = GoesMessageDataSet.GetEDDNMessages(nessid, textBoxHoursBack.Text);
            dataGridViewQuality.DataSource = tbl;

        }

        private void linkLabelHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"http://eddn.usgs.gov/dcpformat.html");
            
        }

        private void buttonDownloadNessid_Click(object sender, EventArgs e)
        {
            var tbl = GoesMessageDataSet.GetEDDNMessages(this.textBoxNessid.Text, textBoxHoursBack.Text);
            dataGridViewQuality.DataSource = tbl;
        }

        SiteProperties m_siteMetaData;
        private void tabControlLinux_SelectedIndexChanged(object sender, EventArgs e)
        {

           
        }

        EquationEditorTable m_equationEditorTable1;
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabControl1.SelectedTab == tabPageLinux
               && m_siteMetaData == null)
            {
                var db = Database.DB();
                m_siteMetaData = new SiteProperties(db);
                m_siteMetaData.Parent = tabPageSites;
                m_siteMetaData.Dock = DockStyle.Fill;
            }
            if( this.tabControl1.SelectedTab == tabPageEquations
                && m_equationEditorTable1 == null)
            {
                m_equationEditorTable1 = new EquationEditorTable();
                m_equationEditorTable1.Parent = tabPageEquations;
                m_equationEditorTable1.Dock = DockStyle.Fill;
            }
        }
    }
}
