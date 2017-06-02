using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Reclamation.Core;
using System.IO;
using Reclamation.TimeSeries.Hydromet;

namespace HydrometForecast
{
    public partial class RunForecast : UserControl
    {
        public RunForecast()
        {
            InitializeComponent();
            this.comboBoxSubsequent.SelectedIndex = 0;
            DateTime t = dateTimePickerForecastDate.Value;
            this.dateTimePickerForecastDate.Value = new DateTime(t.Year, 1, 1);
            OnMessage += RunForecast_OnMessage;
            HydrometInfoUtility.WebCaching = true;
            Logger.OnLogEvent +=Logger_OnLogUIEvent;
            toolStripStatusLabel1.Text = "";
        }

        private void Logger_OnLogUIEvent(object sender, StatusEventArgs e)
        {
            if (e.Tag == "ui")
            {
                toolStripStatusLabel1.Text = e.Message;
                statusStrip1.Refresh();
                Application.DoEvents();
            }

        }

        void RunForecast_OnMessage(object sender, StatusEventArgs e)
        {

            UpdateTextBox(textBoxOutput, e.Message);
            UpdateTextBox(textBoxSummary, e.Message);
            Application.DoEvents();

        }

        private void UpdateTextBox(TextBox tb, string message)
        {
            var data = new List<string>();
            var lines = tb.Lines;
            data.AddRange(lines);
            data.Add(message);
            tb.Lines = data.ToArray();
        }

        ForecastSpreadsheetEditor xls;

        string currentForecast = "";


        string errors = "";
        private void buttonRun_Click(object sender, EventArgs e)
        {
            Logger.LogHistory.Clear();
            Logger.EnableLogger(true);
            FileUtility.CleanTempPath();
            
            if (forecastList1.SelectedItems.Length == 0)
                return;
            string currentForecast="";
            try
            {
                Logger.OnLogEvent += Logger_OnLogEvent;
                string[] forecastToRun = forecastList1.SelectedItems;
                Cursor = Cursors.WaitCursor;
                this.textBoxOutput.Lines = new string[] { "calculating " + forecastToRun.Length + " forecasts." };
                this.textBoxSummary.Lines = new string[] { "calculating " + forecastToRun.Length + " forecasts." };

             
                Reclamation.TimeSeries.Parser.SeriesExpressionParser.Debug = true;
                var t = dateTimePickerForecastDate.Value;

                List<ForecastResult> normalConditions = RunForecasts(forecastToRun, t,1.0);
                List<ForecastResult> highConditions = null;
                List<ForecastResult> lowConditions  = null;

                 var query = (from res in normalConditions.AsEnumerable() select res.Details).SelectMany(i => i).ToArray();
                 this.textBoxOutput.Lines = query;

                 if (this.comboBoxSubsequent.Text == "100% 120%  80%")
                 {
                     highConditions = RunForecasts(forecastToRun,t, 1.2);
                     lowConditions = RunForecasts(forecastToRun, t, 0.8);
                 }
                 if (this.comboBoxSubsequent.Text == "100% 150%  50%")
                 {
                     highConditions = RunForecasts(forecastToRun, t, 1.5);
                     lowConditions = RunForecasts(forecastToRun, t, 0.5);
                 }

                 var summary = CreateSummary(normalConditions,highConditions,lowConditions);


                 this.textBoxSummary.Lines = summary.ToArray();


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: while running "+currentForecast+"\n"+ ex.Message+"\n"+errors);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        void Logger_OnLogEvent(object sender, StatusEventArgs e)
        {
            errors += "\n" + e.Message;
        }

        

        private List<string> CreateSummary(List<ForecastResult> normal, List<ForecastResult> high, List<ForecastResult> low)
        {
           var t = dateTimePickerForecastDate.Value;
           var summary = new List<string>();

           string header = CreateHeader(high, low, t);
           summary.AddRange(header.Split('\n'));


           bool la = this.checkBoxLookAhead.Checked;

           for (int i = 0; i < normal.Count; i++)
            {
                var n = normal[i];

                var s = "| " + n.Equation.Name.PadRight(25)
                    + n.ForecastPeriod.PadRight(10)
                    + n.AverageRunoff.ToString("F1").PadLeft(7)
                    + n.GetForecastForSummary(la).ToString("F1").PadLeft(11)
                    + PercentOfNormal(n.AverageRunoff, n.GetForecastForSummary(la)).PadLeft(8);

                if (high != null)
                {
                    s += high[i].GetForecastForSummary(la).ToString("F1").PadLeft(11)
                        + PercentOfNormal(n.AverageRunoff, high[i].GetForecastForSummary(la)).PadLeft(8)
                        + " " + low[i].GetForecastForSummary(la).ToString("F1").PadLeft(11)
                        + PercentOfNormal(n.AverageRunoff, low[i].GetForecastForSummary(la)).PadLeft(8);
                }

                s+= "    |";
                summary.Add(s);
            }
           var footer = CreateFooter(high != null);
           summary.AddRange(footer);
            return summary;
        }

        private string[] CreateFooter(bool wide)
        {
            string rval = "|                                                                  |\n"
                         +"|__________________________________________________________________|";

            if (wide)
            {
                rval = "|                                                                                                         |\n"
                      + "|_________________________________________________________________________________________________________|";
            }

            return rval.Split('\n');
        }

        public string PercentOfNormal(double avg, double forecast)
        {
            int percent = 0;
            if (avg > 0)
            {
                percent = Convert.ToInt32(Math.Round(forecast / avg * 100.0, 0));
            }

            return percent.ToString();
        }
        private static string CreateHeader(List<ForecastResult> high, List<ForecastResult> low, DateTime t)
        {
            string header = "";
            
            if (high == null)
            {
                header = ""
              + "                              CURRENT\n"
              + "                         MONTHLY FORECAST SUMMARY\n"
              + "                         " + t.ToString("MMMM dd yyyy") + "\n"
              + "+------------------------------------------------------------------+\n"
              + "|                        |          |           |     NORMAL       |\n"
              + "|                        |          |           |   SUBSEQUENT     |\n"
              + "|                        |          |           |   CONDITIONS     |\n"
              + "|                        |          |1981-2010  |                  |\n"
              + "|                        |FORECAST  | AVERAGE   |FORECAST  PERCENT |\n"
              + "| FORECAST               |PERIOD    | (1000AF)  |(1000AF)  NORMAL  |\n"
              + "|------------------------------------------------------------------|\n"
              + "|                                        AVE       NORM      %     |\n"
              + "|                                                                  |";
            }
            else
            {
                header = ""
               + "                                                   CURRENT\n"
               + "                                              MONTHLY FORECAST SUMMARY\n"
               + "                                              " + t.ToString("MMMM dd yyyy") + "\n"
               + "+---------------------------------------------------------------------------------------------------------+\n"
               + "|                        |          |           |     NORMAL       |   120% NORMAL     |   80% NORMAL     |\n"
               + "|                        |          |           |   SUBSEQUENT     |   SUBSEQUENT      |   SUBSEQUENT     |\n"
               + "|                        |          |           |   CONDITIONS     |   CONDITIONS      |   CONDITIONS     |\n"
               + "|                        |          |1981-2010  |                  |                   |                  |\n"
               + "|                        |FORECAST  | AVERAGE   |FORECAST  PERCENT |FORECAST  PERCENT  |FORECAST  PERCENT |\n"
               + "| FORECAST               |PERIOD    | (1000AF)  |(1000AF)  NORMAL  |(1000AF)  NORMAL   |(1000AF)  NORMAL  |\n"
               + "|---------------------------------------------------------------------------------------------------------|\n"
               + "|                                        AVE       NORM      %       HIGH       %         LOW        %    |\n"
               + "|                                                                                                         |";
                header = header.Replace("120%", (high[0].EstimationFactor * 100).ToString("F0") + "%");
                header = header.Replace("80%", (low[0].EstimationFactor * 100).ToString("F0") + "%");


            }
            return header;
        }

        

        private List<ForecastResult> RunForecasts(string[] forecastToRun, 
            DateTime t, double estimationFactor)
        {
            List<ForecastResult> allResults = new List<ForecastResult>();
            for (int i = 0; i < forecastToRun.Length; i++)
            {
                ForecastEquation eq=null;
                try
                {
                    eq = GetEquation(forecastToRun[i]);
                    FireOnLogEvent(eq.Name,"");
                    ForecastResult result = eq.Evaluate(t, checkBoxLookAhead.Checked, estimationFactor);
                    allResults.Add(result);
                }
                    catch(Exception e)
                {
                    var res = new ForecastResult();
                    string msg = e.Message;
                    if (eq != null)
                    {
                        res.Equation.Name = " -->> ERROR " + eq.Name;
                        msg = eq.Name + " " + msg;
                        FireOnLogEvent(msg, "");
                    }
                    res.Details.Add(msg);
                    allResults.Add(res);

                }
            }
            return allResults;
        }

        public static event StatusEventHandler OnMessage;

        static void FireOnLogEvent(string message, string tag)
        {
            if (OnMessage != null)
            {
                OnMessage(null, new StatusEventArgs(message, tag));
            }
        }

        private ForecastEquation GetEquation(string forecastName)
        {
            currentForecast = forecastName;
            var fileName = FileUtility.GetTempFileName(".csv");
            xls.SaveSheetToCsv(forecastName, fileName);
            Logger.WriteLine("Run " + forecastName);

            ForecastEquation eq = new ForecastEquation(fileName);
            return eq;
        }

        internal void Reload(ForecastSpreadsheetEditor xls)
        {
            this.xls = xls;
            this.forecastList1.SetSheetNames(xls.SheetNames);

        }


        
        private void buttonTestAllYears_Click(object sender, EventArgs e)
        {
            Logger.LogHistory.Clear();
            Logger.EnableLogger(true);
            FileUtility.CleanTempPath();
            string currentForecast = "";
            try
            {
                Performance perf = new Performance();
                Reclamation.TimeSeries.Parser.SeriesExpressionParser.Debug = false;
                
                string[] forecastToRun = forecastList1.SelectedItems;
                Cursor = Cursors.WaitCursor;
                this.textBoxOutput.Lines = new string[] { "calculating " + forecastToRun.Length + " forecasts." };
                Application.DoEvents();

                var t = dateTimePickerForecastDate.Value;
                double[] estimationFactors = { 1.0 };
                if (this.comboBoxSubsequent.Text == "100% 120%  80%")
                {
                    estimationFactors = new double[]{ 1.0,1.2,0.8 };
                }
                else
                if (this.comboBoxSubsequent.Text == "100% 150%  50%")
                {
                    estimationFactors = new double[]{ 1.0,1.5,0.5 };
                }
                
                for (int i = 0; i < forecastToRun.Length; i++)
                {
                    ForecastEquation eq = GetEquation(forecastToRun[i]);
                    var tbl = eq.RunHistoricalForecasts(eq.StartYear, eq.EndYear,checkBoxLookAhead.Checked, t.Month,estimationFactors);
                    var fn = FileUtility.GetTempFileName(".csv");
                    CsvFile.WriteToCSV(tbl, fn,false);
                    List<string> output = new List<string>();
                    output.AddRange(this.textBoxOutput.Lines);
                    output.Add("completed " + forecastToRun[i] +" elapsed seconds = "+perf.ElapsedSeconds.ToString("F0"));
                    this.textBoxOutput.Lines = output.ToArray();
                    Application.DoEvents();
                    System.Diagnostics.Process.Start(fn);

                }
                perf.Report("done with forecast performance .");

                //this.textBoxOutput.Lines = query;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: while running " + currentForecast + "\n" + ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void linkLabelNotepad_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string fn = FileUtility.GetTempFileName(".txt");
            if (tabControl1.SelectedTab == tabPageDetails)
            {
                File.WriteAllLines(fn, textBoxOutput.Lines);
            }
            else
            {
                File.WriteAllLines(fn, textBoxSummary.Lines);
            }
            System.Diagnostics.Process.Start(fn);
        }

        private void linkLabelLogfiles_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var fn = FileUtility.GetTempFileName(".txt");
            File.WriteAllLines(fn,Logger.LogHistory.ToArray());
            System.Diagnostics.Process.Start(fn);
        }
    }
}
