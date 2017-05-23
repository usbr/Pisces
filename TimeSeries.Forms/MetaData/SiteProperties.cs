using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Forms.MetaData
{
    public partial class SiteProperties : UserControl
    {
        public SiteProperties()
        {
            InitializeComponent();
        }
        TimeSeriesDatabase m_db;
        TimeSeriesDatabaseDataSet.sitecatalogDataTable m_sites;
        TimeSeriesDatabaseDataSet.sitepropertiesDataTable m_props;
        public SiteProperties(TimeSeriesDatabase db )
        {
            
            m_db = db;
            InitializeComponent();
            comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
            m_props = db.GetSiteProperties();
            
            m_sites = db.GetSiteCatalog();

            var temp = m_sites.Copy(); // copy for the combo box (selection only)
            for (int i = 0; i < temp.Rows.Count; i++)
            {
                var r = temp.Rows[i];
                r["description"] = r["siteid"].ToString().ToUpper() + " " + r["description"].ToString();
            }
            
            comboBox1.DataSource = temp;
            comboBox1.ValueMember = "siteid";
            comboBox1.DisplayMember = "description";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            if (comboBox1.SelectedIndex < 0)
            {
                dataRowEditor1.SetDataRow(m_sites.NewsitecatalogRow());
                return;
            }

            var row = m_sites[comboBox1.SelectedIndex];
            this.dataRowEditor1.SetDataRow(row);

            var siteid = row["siteid"].ToString();
            m_props.Columns["siteid"].DefaultValue = siteid;
            m_props.Columns["id"].AutoIncrement = true;
            m_props.Columns["id"].AutoIncrementSeed = m_props.NextID();
            m_props.DefaultView.RowFilter = "siteid = '" + siteid + "'";
            this.dataGridViewSiteProperties.DataSource = m_props;

            dataGridViewSiteProperties.Columns["siteid"].Visible = false;
            dataGridViewSiteProperties.Columns["id"].Visible = false;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            var msg = m_db.Server.SaveTable(m_sites);
            msg += m_db.Server.SaveTable(m_props);
            labelStatus.Text = msg+ " rows saved ";
        }

        public void Draw(string siteID)
        {
            comboBox1.SelectedValue = siteID;
            UpdateDisplay();
        }
    }
}
