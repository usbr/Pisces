using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Reclamation.Core;
using Reclamation.TimeSeries;
using System.Text.RegularExpressions;

namespace HydrometTools.Import
{
    public partial class ImportFDRTemperature : UserControl
    {

        private Series m_series;

        public ImportFDRTemperature()
        {
            InitializeComponent();
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var fn = openFileDialog1.FileName;
                this.textBoxFilename.Text = fn;

                DataTable tbl = null;
                if (Path.GetExtension(fn).IndexOf("xls") >= 0)
                {// xls or xlsx (Excel)
                   
                    NpoiExcel xls = new NpoiExcel(fn);
                    DataTable template = new DataTable("watertemp");
                    template.Columns.Add("DateTime", typeof(DateTime));
                    template.Columns.Add("val", typeof(double));

                    tbl = xls.ReadDataTable(0, template, true);
                }
                else if(Path.GetExtension(fn).IndexOf("csv") >= 0)
                { // csv
                    //tbl = new CsvFile(fn, CsvFile.FieldTypes.AllText);
                    var s = new TextSeries(fn);
                    s.Read();
                    tbl = s.Table;
                }
                m_series = CreateSeries(tbl);
                this.dataGridView1.DataSource = m_series.Table;
                this.timeSeriesTeeChartGraph1.Series.Clear();
                this.timeSeriesTeeChartGraph1.Series.Add(m_series);
                this.timeSeriesTeeChartGraph1.Draw(true);
                this.comboBoxPcode.SelectedIndex = -1;
            }

        }
        

      

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                var db = Database.DB();
                SetTableName(m_series);
                Application.DoEvents();
                db.ImportSeriesUsingTableName(m_series);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void SetTableName(Series s)
        {
            s.Table.TableName = "instant_fdrwq_" + GetPcode();
            s.Name = "fdrwq_" + GetPcode();
        }
        private Series CreateSeries(DataTable tbl)
        {
            int duplicateCount = 0;
            var rval = new Series();

            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                DateTime t;
                if( ! DateTime.TryParse(tbl.Rows[i][0].ToString(), out t))
                    continue;
                double d;
                if( ! double.TryParse(tbl.Rows[i][1].ToString(), out d))
                    continue;
                t = Reclamation.TimeSeries.Math.RoundToNearestHour(t);

                if( rval.IndexOf(t)>=0 )
                {
                    Logger.WriteLine("duplicate date (rounded to hour): " + t.ToString(),"ui");
                    duplicateCount++; 
                    continue;
                }

                rval.Add(t,d);
            }

            Logger.WriteLine("duplicate count = " + duplicateCount, "ui");
            SetTableName(rval);
            return rval;
        }

        private string GetPcode()
        {
            var tokens = this.comboBoxPcode.Text.Split(' ');
            return tokens[0].Trim().ToLower();
        }

        private void comboBoxPcode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GetPcode() != "")
                buttonSave.Enabled = true;
        }
    }
}
