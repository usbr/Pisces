using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Forms.ImportForms
{
    public partial class AddSite : Form
    {
        TimeSeriesDatabase m_db;
        public AddSite()
        {
            InitializeComponent();
        }
        public AddSite(TimeSeriesDatabase db)
        {
            InitializeComponent();
            this.m_db = db;

            Setup();
        }

        TimeSeriesDatabaseDataSet.sitecatalogDataTable m_sites;
        private void Setup()
        {
           m_sites = m_db.GetSiteCatalog();
           this.comboBox1.DataSource = m_sites;
           this.comboBox1.DisplayMember = "siteid";
           this.comboBox1.ValueMember = "siteid";
            
        }

        private void AddTemplateClick(object sender, EventArgs e)
        {

            // find all folders with same name as siteID
            var sc = m_db.GetSeriesCatalog("siteid = '"+this.comboBox1.SelectedValue.ToString()+"'");
            this.dataGridView1.DataSource = sc;

        }




    }
}
