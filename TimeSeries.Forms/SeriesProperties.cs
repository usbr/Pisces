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


        public SeriesProperties()
        {
            m_series = new Series();
            InitializeComponent();

        }
        Series m_series;

        public SeriesProperties(Series  s, string[] DBunits)
        {
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
        }

        private void ReadSeriesProperties()
        {
            textBoxName.Text = m_series.Name;
            textBoxSiteName.Text = m_series.SiteName;
            textBoxConnectString.Text = m_series.ConnectionString;
            textBoxProvider.Text = m_series.Provider;
            textBoxNotes.Text = m_series.Notes;
            textBoxExpression.Text = m_series.Expression;
            checkBoxActive.Checked = m_series.Enabled;

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

            tblSeriesProperties = m_series.Properties;
            tblSeriesProperties.Columns["id"].AutoIncrement = true;
            tblSeriesProperties.Columns["id"].AutoIncrementSeed = m_series.Properties.NextID();
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
            m_series.Enabled = this.checkBoxActive.Checked;
            m_series.TimeInterval = TimeSeriesDatabase.TimeIntervalFromString(this.comboBoxTimeInterval.Text);
            //m_series.Alias = this.textBoxAlias.Text;
            if (tblSeriesProperties != null && m_series.TimeSeriesDatabase != null)
            {
                m_series.TimeSeriesDatabase.Server.SaveTable(tblSeriesProperties);
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
    }
}