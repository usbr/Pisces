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
        private DataRowEditor dataRowEditor1;

        public SiteProperties(TimeSeriesDatabase db )
        {
            
            m_db = db;
            InitializeComponent();
            InitDataRowEditor();
            SetupComboBox();
        }

        private void SetupComboBox()
        {
            comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
            m_props = m_db.GetSiteProperties();

            m_sites = m_db.GetSiteCatalog();

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

        private void InitDataRowEditor()
        {
            dataRowEditor1 = new DataRowEditor();
            this.dataRowEditor1.Location = new System.Drawing.Point(3, 58);
            this.dataRowEditor1.Margin = new System.Windows.Forms.Padding(4);
            this.dataRowEditor1.Name = "dataRowEditor1";
            this.dataRowEditor1.Size = new System.Drawing.Size(352, 402);
            dataRowEditor1.Parent = this;
            this.dataRowEditor1.TabIndex = 2;
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
                dataGridViewSiteProperties.DataSource = null;

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

        private void buttonNew_Click(object sender, EventArgs e)
        {
            if (this.textBoxSiteID.Text.Trim() == ""
              || this.textBoxDescription.Text.Trim() == "")
                return;

            try
            {
                var newRow = m_sites.NewsitecatalogRow();
                newRow.siteid = this.textBoxSiteID.Text.ToLower().Trim();
                newRow.description = this.textBoxDescription.Text;
                newRow.latitude = 0;
                newRow.longitude = 0;
                newRow.elevation = 0;
                m_sites.Rows.Add(newRow);

                m_db.Server.SaveTable(m_sites);
                SetupComboBox();
                Draw(textBoxSiteID.Text.ToLower().Trim());

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
