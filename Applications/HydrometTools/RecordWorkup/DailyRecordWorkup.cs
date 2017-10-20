using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Reclamation.Core;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.TimeSeries.Graphing;

namespace HydrometTools.RecordWorkup
{
    public partial class DailyRecordWorkup : UserControl
    {
        private ITimeSeriesSpreadsheet timeSeriesSpreadsheet1;
        DataTable siteListTable;
        public DailyRecordWorkup()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            int yr = DateTime.Now.Year - 1;
            this.textBoxWaterYear.Text = yr.ToString();

            var fn = FileUtility.GetFileReference("yakima_record_list.csv");

            siteListTable = new CsvFile(fn, CsvFile.FieldTypes.AllText);

            comboBoxSiteList.DataSource = siteListTable;
            comboBoxSiteList.DisplayMember = "name";
            comboBoxSiteList.ValueMember = "siteid";
            comboBoxSiteList.SelectedIndex = -1;
            comboBoxSiteList.DropDownStyle = ComboBoxStyle.DropDownList;

#if SpreadsheetGear
            var uc = new TimeSeriesSpreadsheetSG();
#else
            var uc = new TimeSeriesSpreadsheet();
#endif
            uc.Parent = this.panelGraphTable;
            uc.BringToFront();
            uc.Dock = DockStyle.Fill;
            timeSeriesSpreadsheet1 = uc;
            uc.Dock = DockStyle.Fill;
            uc.BringToFront();



        }

        private void comboBoxSiteList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSiteList.SelectedIndex >=0  && comboBoxSiteList.ValueMember != "" )
            {
                labelSiteid.Text = comboBoxSiteList.SelectedValue.ToString();
            }
        }

        private void linkLabelRead_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            HydrometHost svr = HydrometInfoUtility.HydrometServerFromPreferences();

            int yr;
            if (!int.TryParse(textBoxWaterYear.Text, out yr))
            {
                MessageBox.Show("Error parsing water year '" + textBoxWaterYear.Text + "'");
                return;
            }
            if (comboBoxSiteList.SelectedIndex < 0)
                return;

            DateTime t1 = new DateTime(yr - 1, 10, 1);
            DateTime t2 = new DateTime(yr, 9, 30);

            int idx = this.comboBoxSiteList.SelectedIndex;
            string siteId = comboBoxSiteList.SelectedValue.ToString();

            string query = siteId + " " + siteListTable.Rows[idx]["parameters"].ToString();
            ReadSeries(svr, t1, t2, query);

        }

        private void ReadSeries(HydrometHost svr, DateTime t1, DateTime t2, string query)
        {
            var hydrometDataTable = HydrometDataUtility.ArchiveTable(svr, query, t1, t2);
            int sz = hydrometDataTable.Columns.Count;

            timeSeriesSpreadsheet1.Clear();
            bool ctrl = (Control.ModifierKeys & Keys.Control) != 0;
            timeSeriesSpreadsheet1.SetDataTable(hydrometDataTable,Reclamation.TimeSeries.TimeInterval.Daily,ctrl);

            tChart1.Series.Clear();
            tChart1.Zoom.Undo();
            TChartDataLoader loader = new TChartDataLoader(this.tChart1);
            for (int i = 1; i < sz; i += 1)
            {
                try
                {
                    string columnName = hydrometDataTable.Columns[i].ColumnName;
                    Steema.TeeChart.Styles.Line series = loader.CreateSeries(hydrometDataTable, columnName, Reclamation.TimeSeries.TimeInterval.Daily, true);

                    series.VertAxis = Steema.TeeChart.Styles.VerticalAxis.Left;
                    series.Pointer.Visible = true;

                    var tokens = TextFile.Split(columnName);
                    string pcode = "";
                    string cbtt = "";
                    if (tokens.Length == 2)
                    {
                        cbtt = tokens[0].Trim();
                        pcode = tokens[1].Trim();
                    }

                    string units = GetUnits(columnName);
                    TChartDataLoader.SetupAxisLeftRight(tChart1, series, units);


                    tChart1.Series.Add(series);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString() + " series index " + i);
                    Logger.WriteLine(ex.ToString(), "ui");
                }
            }
        }

        private static string GetUnits(string columnName)
        {
            var cn = columnName.ToLower().Trim();
            
            if (cn  == "qd" || cn == "qj")
              return "cfs";
            return "feet";
        }
    }
}
