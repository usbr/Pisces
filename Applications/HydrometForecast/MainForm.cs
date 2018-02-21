using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reclamation.Core;
using System.IO;
using Reclamation.TimeSeries.Hydromet;

namespace HydrometForecast
{
    public partial class MainForm : Form
    {
        ForecastSpreadsheetEditor xls;
        public MainForm()
        {
            InitializeComponent();
            var fn = UserPreference.Lookup("FileName");
            if (File.Exists(fn))
            {
                OpenFile(fn);
            }

            Text += " " +  Application.ProductVersion;
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            FileUtility.CleanTempPath();
            
            Application.Run(new MainForm());
        }

        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            if (openExcelDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                OpenFile(openExcelDialog.FileName);
            }
        }

        private void OpenFile(string fileName)
        {
            this.textBoxExcelFileName.Text = fileName;
            InitilizeSpreadsheet();
            xls.Open(fileName);
            runForecast1.Reload(xls);
            UserPreference.Save("FileName", fileName);
        }

        private void InitilizeSpreadsheet()
        {
            if (xls != null)
            {
                xls.Visible = false;
                xls = null;
            }
            xls = new ForecastSpreadsheetEditor();
            xls.Parent = tabPageEdit;
            xls.Dock = DockStyle.Fill;
            xls.BringToFront();

        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (xls != null)
            {
                xls.Save();
                Logger.WriteLine("Save " + this.textBoxExcelFileName.Text);
            }
        }

        CoefficientCalculator R = null;
        ForecastEquation eq;

        private void buttonRecomputeCoeficients_Click(object sender, EventArgs e)
        {
  //          Performance perf = new Performance();
            string fn = FileUtility.GetTempFileName(".csv");
            xls.SaveSheetToCsv(xls.ActiveSheetName, fn);
            xls.Save(); // must save to get back to *.xls from *.csv
           eq = new ForecastEquation(fn);

            var cache = new HydrometDataCache();
            
            cache.Add(eq.GetCbttPcodeList().ToArray(),
                                  new DateTime(eq.StartYear - 1, 10, 1),
                                  new DateTime(eq.EndYear, 9, 30));

            HydrometMonthlySeries.Cache = cache;

            var dir = Path.GetDirectoryName(this.textBoxExcelFileName.Text);


            try
            {
                Cursor = Cursors.WaitCursor;
                Application.DoEvents();

                R = new CoefficientCalculator();
                var newCoefficients = R.ComputeCoefficients(eq, Path.GetDirectoryName(Application.ExecutablePath));

                var dlg = new RegressionResults();
                dlg.CompareToHistoryClicked += new EventHandler<EventArgs>(dlg_CompareToHistoryClicked);
                dlg.Output = R.Output;
                dlg.DataFile = R.dataFile;
                dlg.CoeficientsExisting = CoefficientCalculator.FormatCoefficients(eq.coefficients);
                dlg.CoefficientsComputed = CoefficientCalculator.FormatCoefficients(newCoefficients);

                if (dlg.ShowDialog() == DialogResult.OK) // save
                {
                // save back to excel.
                    xls.UpdateCoeficients(xls.ActiveSheetName, newCoefficients);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }

        }

        void dlg_CompareToHistoryClicked(object sender, EventArgs e)
        {
            if (R == null || eq == null)
                return;

            // look for history.out in app path.
            string fn = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "history.out");

            if (!File.Exists(fn))
            {
                MessageBox.Show("can't find history.out");
                return;
            }

            HistoricalTerms h = new HistoricalTerms(fn, eq.Name);

            if (h.Table.Rows.Count == 0)
            {
                MessageBox.Show("Error reading history.out -- could not locate " + eq.Name);
                return;
            }

            CompareTerms dlg = new CompareTerms();
            DataTable terms = new CsvFile(R.dataFile);
            dlg.LoadTables(terms, h.Table, Difference(terms, h.Table));

            dlg.ShowDialog();


        }

        /// <summary>
        /// subtracts two dataTables
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static DataTable Difference(DataTable a, DataTable b)
        {
            DataTable rval = a.Copy();

            for (int row = 0; row < Math.Min(a.Rows.Count,b.Rows.Count); row++)
            {
                for (int col = 1; col < a.Columns.Count; col++)
                {
                    rval.Rows[row][col] = Convert.ToDouble(a.Rows[row][col]) - Convert.ToDouble(b.Rows[row][col]);
                }
            }
            return rval;
        }

        private void buttonOpenLocalMpoll_Click(object sender, EventArgs e)
        {
        }
       

     
    }
}
