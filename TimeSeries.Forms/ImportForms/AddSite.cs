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
        DataTable properties = new DataTable("properties");
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
           comboBox1.SelectedValue = "boii";
           m_seriesCatalog = new TimeSeriesDatabaseDataSet.SeriesCatalogDataTable();


         
           properties.Columns.Add("name");
           properties.Columns.Add("value");
           properties.Rows.Add("program", "hydromet");
           properties.Rows.Add("basin", "");

           dataGridViewProperties.DataSource = properties;
            

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

        public DataTable Program
        {
            get {
                return properties;
            }
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
            var tbl = m_db.GetSeriesCatalog("siteid = '" + templateName + "' and isfolder = 0");
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
            foreach (var item in tbl)
            {
                var r = m_seriesCatalog.NewSeriesCatalogRow();
                r.ItemArray = item.ItemArray;
                m_seriesCatalog.AddSeriesCatalogRow(r);
            }
        }

        private void SetupDataGridView()
        {
            this.dataGridViewSeries.DataSource = null;
            this.dataGridViewSeries.DataSource = m_seriesCatalog;
            this.dataGridViewSeries.Columns["id"].Visible = false;
            this.dataGridViewSeries.Columns["parentid"].Visible = false;
            this.dataGridViewSeries.Columns["isfolder"].Visible = false;
            this.dataGridViewSeries.Columns["sortorder"].Visible = false;
            this.dataGridViewSeries.Columns["iconname"].Visible = false;
            this.dataGridViewSeries.Columns["Units"].Visible = false;
            this.dataGridViewSeries.Columns["connectionstring"].Visible = false;
            this.dataGridViewSeries.Columns["notes"].Visible = false;
            this.dataGridViewSeries.Columns["enabled"].Visible = false;

            dataGridViewSeries.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void buttonIndividuals_Click(object sender, EventArgs e)
        {
            var siteID =this.textBoxSiteID.Text.Trim(); 
              if (siteID == "")
            {
                labelError.Text = "a siteid must be entered";
                return;
            }
              bool daily = checkBoxDaily.Checked;

            labelError.Text = "";
            if (this.checkBoxQ.Checked)
            {
                AddInstantRow(siteID, "feet", "gh");
                AddInstantRow(siteID, "cfs", "q","FileRatingTable(%site%_gh,\"%site%.csv\")");
                AddInstantRow(siteID, "feet", "hj", "FileRatingTable(%site%_gh,\"%site%_shift.csv\")");
                if (daily)
                {
                    AddDailyRow(siteID, "cfs", "qd", "DailyAverage(instant_%site%_q,10)");
                    AddDailyRow(siteID, "cfs", "gd", "DailyAverage(instant_%site%_gh,10)");
                    AddDailyRow(siteID, "feet", "hj", "DailyAverage(instant_%site%_hj,10)");
                }
            }
            if (this.checkBoxCanal.Checked)
            {
                AddInstantRow(siteID, "feet", "ch");
                AddInstantRow(siteID, "cfs", "qc", "FileRatingTable(%site%_ch,\"%site%.csv\")");
                AddInstantRow(siteID, "feet", "hh", "FileRatingTable(%site%_ch,\"%site%_shift.csv\")");
                if (daily)
                {
                    AddDailyRow(siteID, "cfs", "qj", "DailyAverage(instant_%site%_qc,10)");
                    AddDailyRow(siteID, "cfs", "gj", "DailyAverage(instant_%site%_ch,10)");
                    AddDailyRow(siteID, "feet", "hh", "DailyAverage(instant_%site%_hh,10)");
                }
            }
            if (this.checkBoxWaterTemp.Checked)
            {
                AddInstantRow(siteID, "degF", "wf");
                if (daily)
                {
                    AddDailyRow(siteID, "degF", "wi", "DailyMin(instant_%site%_wf,10)");
                    AddDailyRow(siteID, "degF", "wk", "DailyMax(instant_%site%_wf,10)");
                    AddDailyRow(siteID, "degF", "wz", "DailyAverage(instant_%site%_wf,10)");
                }
            }

            if( this.checkBoxAirTemp.Checked)
            {
                AddInstantRow(siteID, "degF", "ob");
                if (daily)
                {
                    AddDailyRow(siteID, "degF", "mn", "DailyMin(instant_%site%_ob,10)");
                    AddDailyRow(siteID, "degF", "mm", "DailyMax(instant_%site%_ob,10)");
                    AddDailyRow(siteID, "degF", "mx", "DailyAverage(instant_%site%_ob,10)");
                }
            }

            if (this.checkBoxPrecip.Checked)
            {
                AddInstantRow(siteID, "in", "pc");
                if (daily)
                {
                    AddDailyRow(siteID, "in", "pc", "DailyMidnight(instant_%site%_pc)");
                    AddDailyRow(siteID, "in", "pp", "daily_%site%_pc-daily_%site%_pc[t-1]");
                    AddDailyRow(siteID, "in", "pu", "DailyWaterYearRunningTotal(daily_%site%_pp,daily_%site%_pu)");
                }
            }




            if (this.checkBoxWaterTempC.Checked)
            {
                AddInstantRow(siteID, "degC", "wc");
                if (daily)
                {
                    AddDailyRow(siteID, "degF", "wm", "DailyMax(instant_%site%_wc,10)");
                    AddDailyRow(siteID, "degF", "wn", "DailyMin(instant_%site%_wc,10)");
                    AddDailyRow(siteID, "degF", "wy", "DailyAverage(instant_%site%_wc,10)");
                }
            }

            if (this.checkBoxGenericWeir.Checked)
            {
                AddInstantRow(siteID, "feet", "ch");
                AddInstantRow(siteID, "cfs", "qc", "GenericWeir(%site%_ch,mcf_offset,mcf_scale(width),mcf_base(exponent))");
                AddInstantRow(siteID, "feet", "hh", "LookupShift(%site%_ch)");

            }
            if (this.checkBoxRectWeir.Checked)
            {
                AddInstantRow(siteID, "feet", "ch");
                AddInstantRow(siteID, "cfs", "qc", "RectangularContractedWeir("+siteID+"_ch, mcf_scale)");

                if (daily)
                {
                    AddDailyRow(SiteID, "ft", "gj", "DailyAverage(instant_%site%_ch)");
                    AddDailyRow(SiteID, "cfs", "qj", "DailyAverage(instant_%site%_qc)");
                }

                //AddInstantRow(siteID, "feet", "hh", "");

            }

            if (this.checkBoxReservoir.Checked)
            {
                AddInstantRow(siteID, "feet", "fb");
                AddInstantRow(siteID, "acre-feet", "af", "FileRatingTable(%site%_fb,\"%site%.csv\")");

                if( daily)
                {
                    AddDailyRow(SiteID, "ft", "fb", "DailyMidnight(instant_%site%_fb)");
                    AddDailyRow(SiteID, "acre-feet", "af", "DailyMidnight(instant_%site%_af)");
                }
            }

            if( this.checkBoxCustom.Checked)
            {
                AddInstantRow(siteID,textBoxCustomUnits.Text.Trim(), textBoxCustom.Text.Trim());
            }

            //if( this.checkBoxQuality.Checked)
            //{
            //    foreach (var item in QualityParameters)
            //    {
            //        AddInstantRow(siteID, "", item.ToLower());  
            //    }
            //}
            SetupDataGridView();
        }

        private void AddInstantRow(string siteID, string units, string pcode, string expression = "")
        {
            m_seriesCatalog.AddInstantRow(siteID, 0, units, pcode, expression);
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
            m_seriesCatalog.AddSeriesCatalogRow(m_seriesCatalog.NextID(), 0, 0, 1, iconName, siteID + "_" + pcode, siteID, units, "Daily",
             pcode, "daily_" + siteID + "_" + pcode, provider, "", expression, "", 1);
        }

        TimeSeriesDatabaseDataSet.sitecatalogDataTable m_siteCatalog;
        TimeSeriesDatabaseDataSet.sitecatalogRow m_siteRow;

        private void textBoxSiteID_TextChanged(object sender, EventArgs e)
        {

            if (textBoxdescription.Text.Trim() == "" && textBoxSiteID.Text.Trim().Length >2)
            {
                if (m_siteCatalog == null)
                {
                    m_siteCatalog = m_db.GetSiteCatalog();
                }

                m_siteRow = m_siteCatalog.FindBysiteid(textBoxSiteID.Text.Trim());
                if (m_siteRow != null)
                {
                    this.textBoxdescription.Text = m_siteRow.description;
                    this.textBoxElevation.Text = m_siteRow.elevation.ToString();
                    this.textBoxlatitude.Text = m_siteRow.latitude.ToString();
                    this.textBoxlongitude.Text = m_siteRow.longitude.ToString();
                    this.textBoxInstall.Text = m_siteRow.install;
                    this.textBoxTimezone.Text = m_siteRow.timezone;
                    this.textBoxState.Text = m_siteRow.state;
                }

            }

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }






    }
}

