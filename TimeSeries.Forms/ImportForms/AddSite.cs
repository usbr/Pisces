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
           m_seriesCatalog = new TimeSeriesDatabaseDataSet.SeriesCatalogDataTable();
            
        }

        TimeSeriesDatabaseDataSet.SeriesCatalogDataTable m_seriesCatalog;

        public TimeSeriesDatabaseDataSet.SeriesCatalogDataTable SeriesCatalog
        {
            get { return m_seriesCatalog; }
        }

        public string SiteID
        {
            get { return this.textBoxSiteID.Text; }
        }

        public string SiteName
        {
            get { return this.textBoxdescription.Text; }
        }

        public string Elevation
        {
            get { return this.textBoxElevation.Text; }
        }
        public string Lat
        {
            get { return this.textBoxlatitude.Text; }
        }
        public string Lon
        {
            get { return this.textBoxlongitude.Text; }
        }
        public string TimeZone
        {
            get { return this.textBoxTimezone.Text; }
        }
        public string Install
        {
            get { return this.textBoxInstall.Text; }
        }
        public string State
        {
            get { return this.textBoxState.Text; }
        }

        private void AddTemplateClick(object sender, EventArgs e)
        {

            if (this.comboBox1.SelectedValue == null)
            {
                labelError.Text = "a template must be selected";
                return;
            }

            if (this.textBoxSiteID.Text.Trim() == "")
            {
                labelError.Text = "a siteid must be entered";
                return;
            }
            labelError.Text = "";

            string templateName = this.comboBox1.SelectedValue.ToString().ToLower();
            string newName = this.SiteID.ToLower();
            // find all folders with same name as siteID
            m_seriesCatalog = m_db.GetSeriesCatalog("siteid = '" + templateName + "'");
            // replace template name with new siteid.
            foreach (var item in m_seriesCatalog)
            {
                item.Name = item.Name.Replace(templateName, newName);
                item.siteid = newName;
                item.TableName = item.TableName.Replace(templateName,newName);
            }


            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = m_seriesCatalog;
            this.dataGridView1.Columns["id"].Visible = false;
            this.dataGridView1.Columns["parentid"].Visible = false;
            this.dataGridView1.Columns["isfolder"].Visible = false;
            this.dataGridView1.Columns["sortorder"].Visible = false;
            this.dataGridView1.Columns["iconname"].Visible = false;
            this.dataGridView1.Columns["Units"].Visible = false;
            this.dataGridView1.Columns["connectionstring"].Visible = false;
            this.dataGridView1.Columns["notes"].Visible = false;
            this.dataGridView1.Columns["enabled"].Visible = false;
        }






    }
}

