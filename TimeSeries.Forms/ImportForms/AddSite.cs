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
            get {
                m_seriesCatalog.AcceptChanges();
                return m_seriesCatalog; 
            }
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
            var tbl = m_db.GetSeriesCatalog("siteid = '" + templateName + "'");
            // replace template name with new siteid.
          //  m_seriesCatalog.Columns.Add("oldId", typeof(Int32));
            foreach (var item in tbl)
            {
                item.Name = item.Name.Replace(templateName, newName);
                item.siteid = newName;
                item.TableName = item.TableName.Replace(templateName,newName);
            }

            AddRows(tbl);
            SetupDataGridView();
        }

        private void AddRows(TimeSeriesDatabaseDataSet.SeriesCatalogDataTable tbl)
        {
            var r = m_seriesCatalog.NewSeriesCatalogRow();
            foreach (var item in tbl)
            {
                r.ItemArray = item.ItemArray;
                m_seriesCatalog.AddSeriesCatalogRow(r);
            }
        }

        private void SetupDataGridView()
        {
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

        private void buttonIndividuals_Click(object sender, EventArgs e)
        {
            var siteID =this.textBoxSiteID.Text.Trim(); 
              if (siteID == "")
            {
                labelError.Text = "a siteid must be entered";
                return;
            }
            labelError.Text = "";
            if (this.checkBoxQ.Checked)
            {
                AddInstantRow(siteID, "feet", "gh");
                AddInstantRow(siteID, "cfs", "q","FileRatingTable(%site%_gh,\"%site%.csv\")");
                AddInstantRow(siteID, "cfs", "hj", "FileRatingTable(%site%_gh,\"%site%_shift.csv\")");

                AddDailyRow(siteID, "cfs", "qd", "DailyAverage(instant_%site%_q,10)");
                AddDailyRow(siteID, "cfs", "gd", "DailyAverage(instant_%site%_gh,10)");
                AddDailyRow(siteID, "cfs", "hj", "DailyAverage(instant_%site%_hj,10)");
            }
            if (this.checkBoxWaterTemp.Checked)
            {

                AddInstantRow(siteID, "degF", "wf");

                AddDailyRow(siteID, "cfs", "wi", "DailyMin(instant_%site%_wf,10)");
                AddDailyRow(siteID, "cfs", "wk", "DailyMax(instant_%site%_wf,10)");
                AddDailyRow(siteID, "cfs", "wz", "DailyAverage(instant_%site%_wf,10)");
            }


            SetupDataGridView();
        }

        private void AddInstantRow(string siteID, string units, string pcode, string expression = "")
        {
            var provider = "Series";
            string iconName = "";
            if (expression != "")
            {
                provider = "CalculationSeries";
                iconName = "sum";
            }
            m_seriesCatalog.AddSeriesCatalogRow(m_seriesCatalog.NextID(), 0, false, 1,iconName, siteID + "_" + pcode, siteID, units, "Irregular",
             pcode, "instant_" + siteID + "_" + pcode, provider,"", expression, "", true);
        }
        private void AddDailyRow(string siteID, string units, string pcode, string expression = "")
        {
            var provider = "Series";
            string iconName = "";
            if (expression != "")
            {
                provider = "CalculationSeries";
                iconName = "sum";
            }
            m_seriesCatalog.AddSeriesCatalogRow(m_seriesCatalog.NextID(), 0, false, 1, iconName, siteID + "_" + pcode, siteID, units, "Daily",
             pcode, "instant_" + siteID + "_" + pcode, provider, "", expression, "", true);
        }






    }
}

