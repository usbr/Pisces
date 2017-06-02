using Reclamation.TimeSeries.Forms.Calculations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Forms
{
    public partial class SeriesProperties : Form
    {
        Quality m_quality;
        public SeriesProperties()
        {
            m_series = new Series();
            InitializeComponent();
        }
        Series m_series;
        TimeSeriesDatabase m_db;
        public SeriesProperties(Series  s, TimeSeriesDatabase db)
        {
            m_db = db;
            m_quality = new Quality(m_db);
            string[] DBunits = db.GetUniqueUnits();
            if (s == null)
            {
                throw new ArgumentNullException();
            }
            m_series = s;
            InitializeComponent();
            this.LoadList(comboBoxUnits, DBunits);
            ReadSeriesProperties();
            this.textBoxExpression.Enabled =  s is CalculationSeries;
            buttonBuildExpression.Enabled = s is CalculationSeries;
            LoadQualityLimits();
        }

        private void LoadQualityLimits()
        {
            this.textBoxHigh.Text = "";
            this.textBoxLow.Text = "";

            var quality_row  = m_quality.GetRow(m_series.Table.TableName);
            if( quality_row != null)
            {
                if (!quality_row.IshighNull() )
                    this.textBoxHigh.Text = quality_row.high.ToString("F2");
                if (!quality_row.IslowNull())
                    this.textBoxLow.Text = quality_row.low.ToString("F2");
            }
        }

        private void ReadSeriesProperties()
        {
            textBoxName.Text = m_series.Name;
            textBoxSiteName.Text = m_series.SiteID;
            textBoxConnectString.Text = m_series.ConnectionString;
            textBoxProvider.Text = m_series.Provider;
            textBoxNotes.Text = m_series.Notes;
            textBoxExpression.Text = m_series.Expression;
            checkBoxActive.Checked = m_series.Enabled == 1;

            textBoxSource.Text = m_series.Source;
            if (m_series.Table == null)
                textBoxDBTableName.Text = "";
            else
                textBoxDBTableName.Text = m_series.SeriesCatalogRow.TableName;

            textBoxParameter.Text = m_series.Parameter;
            textBoxParentID.Text = m_series.Parent.ID.ToString();
            PeriodOfRecord por = m_series.GetPeriodOfRecord();
            textBoxPOR1.Text = por.T1.ToString();
            textBoxPOR2.Text = por.T2.ToString();
            textBoxRecordCount.Text = por.Count.ToString();
            textBoxSiteID.Text = m_series.ID.ToString();
            textBoxSortOrder.Text = m_series.SortOrder.ToString();
            //textBoxAlias.Text = s.Alias;
            
            comboBoxUnits.SelectedItem = m_series.Units;

            //comboBoxDisplayUnits.Items.Clear();
            //comboBoxDisplayUnits.Items.Add("");
            //comboBoxDisplayUnits.Items.Add("degrees C");
            //comboBoxDisplayUnits.SelectedItem = s.DisplayUnits;
            //this.textBoxMath.Text = m_series.Expression;
            comboBoxTimeInterval.SelectedIndex = 0;
            comboBoxTimeInterval.SelectedItem = m_series.TimeInterval.ToString();

            tblSeriesProperties = m_db.GetSeriesProperties();
            tblSeriesProperties.Columns["id"].AutoIncrement = true;
            tblSeriesProperties.Columns["id"].AutoIncrementSeed = tblSeriesProperties.NextID();
            tblSeriesProperties.Columns["seriesid"].DefaultValue = m_series.ID;
            dgvProperties.DataSource = tblSeriesProperties;
            tblSeriesProperties.DefaultView.RowFilter = "seriesid=" + m_series.ID;
            dgvProperties.Columns["id"].Visible = false;
            dgvProperties.Columns["seriesid"].Visible = false;

        }

        TimeSeriesDatabaseDataSet.seriespropertiesDataTable tblSeriesProperties;



        private void LoadList(ComboBox owner, string[] list)
        {
            owner.Items.Clear();
            for (int i = 0; i < list.Length; i++)
            {
                owner.Items.Add(list[i]);
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            SaveSeriesProperties();
        }

        private void SaveSeriesProperties()
        {
            m_series.Name = this.textBoxName.Text;
            m_series.Units = this.comboBoxUnits.Text;
            m_series.Notes = this.textBoxNotes.Text;
            m_series.Expression = this.textBoxExpression.Text;
            m_series.ConnectionString = this.textBoxConnectString.Text;
            m_series.Enabled = (short)(this.checkBoxActive.Checked ? 1 : 0);
            m_series.TimeInterval = TimeSeriesDatabase.TimeIntervalFromString(this.comboBoxTimeInterval.Text);
            //m_series.Alias = this.textBoxAlias.Text;
            if (tblSeriesProperties != null && m_series.TimeSeriesDatabase != null)
            {
                m_series.TimeSeriesDatabase.Server.SaveTable(tblSeriesProperties);
                SaveQualityLimits();
            }
        }

        private void SaveQualityLimits()
        {

            if (this.textBoxHigh.Modified || this.textBoxLow.Modified)
            {
                double high, low;
                if( double.TryParse(this.textBoxHigh.Text,out high)
                    && double.TryParse(this.textBoxLow.Text,out low))
                   m_quality.SaveLimits(m_series.Table.TableName,high,low,0);
                else
                    MessageBox.Show("Error: could not read High and Low limits. They must be numbers.");
            }

        }

        private void buttonBuildExpression_Click(object sender, EventArgs e)
        {
            var s = m_series as CalculationSeries;
                string tmpExp = s.Expression;
               var DB = s.TimeSeriesDatabase;
                CalculationProperties p = new CalculationProperties(s, new TimeSeriesTreeModel(DB), DB.GetUniqueUnits());
                if (p.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.textBoxExpression.Text = s.Expression;
                }
                else
                {
                    this.textBoxExpression.Text = tmpExp;
                }

            
        }

        private void textBoxParameter_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void label23_Click(object sender, EventArgs e)
        {

        }

        private void label27_Click(object sender, EventArgs e)
        {

        }

        private void checkBoxEnable_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void tabPageAlarm_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }
    }
}