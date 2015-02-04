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
    public partial class SiteMetaData : UserControl
    {
        public SiteMetaData()
        {
            InitializeComponent();
        }
        TimeSeriesDatabase m_db;
        TimeSeriesDatabaseDataSet.sitecatalogDataTable m_sites;
        public SiteMetaData(TimeSeriesDatabase db)
        {
            
            m_db = db;
            InitializeComponent();
            comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;

            m_sites = db.GetSiteCatalog();

            var temp = m_sites.Copy();
            for (int i = 0; i < temp.Rows.Count; i++)
            {
                var r = temp.Rows[i];
                r["description"] = r["siteid"].ToString().ToUpper() + " " + r["description"].ToString();
            }
            
            comboBox1.DataSource = temp;
            comboBox1.ValueMember = "description";
            comboBox1.DisplayMember = "description";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex < 0)
            {
                dataRowEditor1.SetDataRow(m_sites.NewsitecatalogRow());
                return;
            }

            var row =m_sites[comboBox1.SelectedIndex];
            this.dataRowEditor1.SetDataRow(row);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            var msg = m_db.Server.SaveTable(m_sites);
            labelStatus.Text = msg+ " rows saved ";
        }
    }
}
