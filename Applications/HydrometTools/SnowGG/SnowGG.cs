using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Reclamation.Core;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.TimeSeries.Analysis;
using Math = Reclamation.TimeSeries.Math;
using Reclamation.TimeSeries.Forms.Graphing;
using System.IO;

namespace HydrometTools.SnowGG
{
    public partial class SnowGG : UserControl
    {
        public SnowGG()
        {
            InitializeComponent();
            this.comboBoxCbtt.Text = UserPreference.Lookup("Snowgg->cbtt");
            this.comboBoxPcode.Text = UserPreference.Lookup("Snowgg->pcode");

            
            Reset();
            this.timeSeriesGraph1.AfterEditGraph += timeSeriesGraph1_AfterEditGraph;
            timeSeriesGraph1.AnnotationOnMouseMove = true;
        }

        void timeSeriesGraph1_AfterEditGraph(object sender, EventArgs e)
        {
            var gs = GetGraphSettings();
            gs.Merge(timeSeriesGraph1.GraphSettings);
            gs.Save(GraphSettingFileName());

        }

        void Reset()
        {
            
            var t = DateTime.Now;
            int wy = t.Year;
            if( t.Month >= 10)
                wy++;

            this.yearSelector1.SelectedYears = new Int32[]{wy--,wy};
            var t2 = t.AddMonths(1);


            if (t.Month == 9)
            {
                this.monthRangePicker1.MonthDayRange = new MonthDayRange(10, 1, 9, 30);
            }
            else
            {
                this.monthRangePicker1.MonthDayRange = new MonthDayRange(10, 1, t2.Month, DateTime.DaysInMonth(t2.Year, t2.Month));
            }

            //this.comboBoxPcode.SelectedIndex = 0;
            this.checkBoxGP.Visible = HydrometInfoUtility.HydrometServerFromPreferences() == HydrometHost.GreatPlains;
            this.checkBoxMpoll.Visible = HydrometInfoUtility.HydrometServerFromPreferences() != HydrometHost.GreatPlains;
            if( this.checkBoxMpoll.Visible )
               this.checkBoxGP.Checked = false;
            if (this.checkBoxGP.Visible)
                this.checkBoxMpoll.Checked = false;
        }

        private List<string> snowGgColors = new List<string> {
            "Blue",
            "ForestGreen",
            "Fuchsia",
            "DarkRed",
            "LimeGreen",
            "GoldenRod",
            "Aqua",
            "SlateGray",
            "DarkSalmon",
            "Peru"
        };

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                timeSeriesGraph1.AnnotationOnMouseMove = checkBoxAnnotate.Checked;
                Cursor = Cursors.WaitCursor;
                Application.DoEvents();
                string pcodeOrig = DeterminePcode();

                timeSeriesGraph1.Clear();

                string cbttOrig = comboBoxCbtt.Text.Trim();
                string cbtt = cbttOrig, pcode = pcodeOrig;

                var seriesList = new List<string>();
                if ((cbttOrig.Trim() == "" || pcodeOrig.Trim() == "") && textBoxMultiple.Text == "")
                {
                    return;
                }
                else
                {
                    if (!checkBoxUseList.Checked)
                    {
                        UserPreference.Save("Snowgg->cbtt", cbttOrig);
                        UserPreference.Save("Snowgg->pcode", comboBoxPcode.Text.Trim());
                        seriesList.Add(cbttOrig + "_" + pcodeOrig);
                    }
                    else
                    {
                        var seriesItems = textBoxMultiple.Text.Split(',');
                        foreach (string item in seriesItems)
                        {
                            if (item.Trim().Split(' ').Length == 2)
                            {
                                seriesList.Add(item.Trim().Split(' ')[0] + "_" + item.Trim().Split(' ')[1]);
                            }
                        }
                    }
                }

                int[] waterYears = this.yearSelector1.SelectedYears;
                SeriesList finalSeriesCollection = new SeriesList();
                foreach (string series in seriesList)
                {
                    cbtt = series.Split('_')[0];
                    comboBoxCbtt.Text = cbtt;
                    pcode = series.Split('_')[1];
                    comboBoxPcode.Text = pcode;
                    var server = HydrometInfoUtility.HydrometServerFromPreferences();
                    var range = monthRangePicker1.MonthDayRange;

                    Series s = new HydrometDailySeries(cbtt, pcode, server);
                    var sl = new SeriesList();
                    sl.Add(s);

                    var wyList = PiscesAnalysis.WaterYears(sl, waterYears, false, 10, true);

                    foreach (var item in wyList)
                    {
                        item.Name = cbtt + " " + pcode;
                    }


                    wyList = ApplyDeltas(wyList, waterYears);
                    AddStatistics(wyList);

                    if (checkBoxGP.Checked)
                    {
                        GPAverage(cbtt, server, range, wyList);
                    }

                    var mp = ReadMpollData(pcode, cbtt);
                    mp.RemoveMissing();
                    if (mp.Count > 0)
                        wyList.Add(mp);

                    // remove months outside selected range
                    var list = FilterBySelectedRange(range, wyList);
                    finalSeriesCollection.Add(list);
                }

                // Set series line colors
                var uniqueSeriesNames = new List<string>();
                var uniqueSeriesColors = new List<string>();
                int colorCounter = 0;
                foreach (var item in finalSeriesCollection)
                {
                    // set line color by year which is identified in the legendtext field
                    if (!uniqueSeriesNames.Contains(item.Appearance.LegendText) && !item.Appearance.LegendText.Contains("%") && 
                        !item.Appearance.LegendText.Contains("avg") && !item.Appearance.LegendText.Contains("max") && !item.Appearance.LegendText.Contains("min"))
                    {
                        uniqueSeriesNames.Add(item.Appearance.LegendText);//.Name);
                        uniqueSeriesColors.Add(snowGgColors[colorCounter]);
                        colorCounter = (colorCounter + 1) % snowGgColors.Count;
                    }
                }
                foreach (var item in finalSeriesCollection)
                {
                    try
                    {
                        int colIdx = uniqueSeriesNames.IndexOf(item.Appearance.LegendText);//.Name);
                        item.Appearance.Color = uniqueSeriesColors[colIdx];
                    }
                    catch
                    {
                        item.Appearance.Color = "Black";
                    }
                }

                this.timeSeriesGraph1.AnalysisType = AnalysisType.WaterYears;
                this.timeSeriesGraph1.Series = finalSeriesCollection;
                if (seriesList.Count == 1)
                {
                    this.timeSeriesGraph1.Title = HydrometInfoUtility.LookupSiteDescription(cbtt) + "  Elevation:" + HydrometInfoUtility.LookupElevation(pcode);
                }
                //timeSeriesGraph1.GraphSettings = GetGraphSettings();

                this.timeSeriesGraph1.Draw(true);

                comboBoxCbtt.Text = cbttOrig;
                comboBoxPcode.Text = pcodeOrig;

                timeSeriesGraph1.GraphSettings = GetGraphSettings();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private string GraphSettingFileName()
        {
            return Path.Combine(FileUtility.GetTempPath(), "hydromet_graph.txt");
        }
        GraphSettings GetGraphSettings()
        {
            GraphSettings gs = new GraphSettings();

            var fn = GraphSettingFileName();

            if (File.Exists(fn))
                gs.Read(fn);
            return gs;
        }

        private SeriesList ApplyDeltas(SeriesList wyList, int[] waterYears)
        {
            int sCount = wyList.Count;
            if (checkBoxDeltas.Checked && sCount > 1)
            {
                var deltaList = new SeriesList();
                Series s1 = wyList[0];
                s1.RemoveMissing();
                deltaList.Add(s1);

                for (int sIdx = 1; sIdx < sCount; sIdx++)
                {
                    var ithS = wyList[sIdx];
                    var deltaS = new Series();
                    var deltaNoZeroS = new Series();
                    int deltaCounter = 0;
                    for (int ptIdx = 1; ptIdx < ithS.Count; ptIdx++)
                    {
                        if (ithS[ptIdx].DateTime < s1.MaxDateTime)
                        {
                            deltaS.Add(ithS[ptIdx].DateTime,double.NaN);
                            deltaNoZeroS.Add(ithS[ptIdx].DateTime, double.NaN);
                        }
                        else if (ithS[ptIdx].DateTime == s1.MaxDateTime)
                        {
                            deltaS.Add(s1[ithS[ptIdx].DateTime]);
                            deltaNoZeroS.Add(s1[ithS[ptIdx].DateTime]);
                        }
                        else if (ithS[ptIdx].DateTime > s1.MaxDateTime)
                        {
                            // Filter out projected zeros but keep the running negatives in terms of calculating the evolving
                            //  delta-differenced values
                            var calcVal = deltaS[deltaCounter - 1].Value + ithS[ptIdx].Value - ithS[ptIdx - 1].Value;
                            deltaS.Add(ithS[ptIdx].DateTime, calcVal);
                            if (calcVal < 0)
                            {
                                deltaNoZeroS.Add(ithS[ptIdx].DateTime, 0);
                            }
                            else
                            {
                                deltaNoZeroS.Add(ithS[ptIdx].DateTime, calcVal);
                            }                            
                        }
                        deltaCounter++;
                    }
                    deltaNoZeroS.Units = s1.Units;
                    deltaNoZeroS.Name = waterYears[sIdx].ToString("F0") + " deltas";
                    // [JR] Displays original data in addition to delta curves
                    //wyList.Add(deltaS);
                    // [JR] Only shows delta curves
                    deltaList.Add(deltaNoZeroS);
                }
                return deltaList;
            }
            else
            {
                return wyList;
            }
        }

        private void AddStatistics(SeriesList wyList)
        {

            bool anyStats = checkBoxMax.Checked || checkBoxMin.Checked || checkBoxAvg.Checked || checkBoxPctls.Checked;

            if (!anyStats)
                return;

            int y1 =1990;
            int y2 =2011;
            int[] pctls = new int[] { };

            int.TryParse(this.textBoxWY1.Text, out y1);
            int.TryParse(this.textBoxWY2.Text, out y2);

            if (checkBoxPctls.Checked)
            {
                try
                {
                    string values = textBoxPctls.Text;
                    string[] tokens = values.Split(',');
                    pctls = Array.ConvertAll<string, int>(tokens, int.Parse);
                }
                catch
                {
                    pctls = new int[] { 10, 50, 90 };
                }

            }

            DateTime t1 = new DateTime(y1 - 1, 10, 1);
            DateTime t2 = new DateTime(y2  , 9, 30);

            var server = HydrometInfoUtility.HydrometServerFromPreferences();
            Series s = new HydrometDailySeries(comboBoxCbtt.Text.Trim(), DeterminePcode(),server);
            s.Read(t1, t2);
            s.RemoveMissing();
            s.Appearance.LegendText = "";
           
            YearRange yr = new YearRange(2000, 10);
            var list = Math.SummaryHydrograph(s, pctls, yr.DateTime1, checkBoxMax.Checked, checkBoxMin.Checked, checkBoxAvg.Checked, false); //, false);
           
            
            wyList.Add(list);

        }

        private static SeriesList FilterBySelectedRange(MonthDayRange range, SeriesList wyList)
        {
            SeriesList list = new SeriesList();
            foreach (Series item in wyList)
            {
                //// bug fix: leap years shifted to year 2000
                ////          have extra data point in October of next year.
                ////          delete this.
                //if (item.Count > 0 && item[0].DateTime.Month == 10
                //    && item[item.Count - 1].DateTime.Month == 10
                //    && range.Month2 != 10)
                //{
                //    item.ReadOnly = false;
                //    item.RemoveAt(item.Count - 1);

                //}

                list.Add(Math.ShiftToYear( Math.Subset(item, range),2000));

            }

            return list;
        }

        private static void GPAverage(string cbtt, HydrometHost server, MonthDayRange range, SeriesList wyList)
        {
            var se_avg = new HydrometDailySeries(cbtt, "se_avg", server);
            var sl2 = new SeriesList();
            sl2.Add(se_avg);
            var list1 = PiscesAnalysis.WaterYears(sl2, new int[] { 2002 }, false, range.Month1, true);
            if (list1.Count == 1)
            {
                var se = list1[0];
                se.Appearance.LegendText = "SE_AVG";
                wyList.Add(se);
            }
        }

        private  Series ReadMpollData(string pcode, string cbtt)
        {

            if (!checkBoxMpoll.Checked)
                return new Series();

            Series m = new HydrometMonthlySeries(cbtt, pcode);
            DateTime t1 = new DateTime(6189, 10, 1);
            DateTime t2 = new DateTime(6190, 9, 1);


           m.Read(t1, t2);
           //m = Math.Subset(m, monthRangePicker1.MonthDayRange);
           //if (range.Month1 < 9)
           //{
           //    m = Math.ShiftToYear(m, 2001);
           //}
         //  else
           //{
           //    m = Math.ShiftToYear(m, 2000);
           //}

           m.Appearance.Style = Styles.Point;
           m.Appearance.LegendText += " 30 yr average";
           
            return m;
        }

        private string DeterminePcode()
        {
            string rval = "se";
            string input = comboBoxPcode.Text.ToLower().Trim();

            

            if (input.StartsWith("snow", StringComparison.CurrentCultureIgnoreCase))
            {

                if (HydrometInfoUtility.ArchiveParameterExists(comboBoxCbtt.Text.Trim().ToUpper(), "SE"))
                    rval = "se";
                else if (HydrometInfoUtility.ArchiveParameterExists(comboBoxCbtt.Text.Trim().ToUpper(), "SP"))
                    rval = "sp";
            }
            else
            if (input.StartsWith("flow", StringComparison.CurrentCultureIgnoreCase))
            {
                rval = "qd";
            }
            else if( input.StartsWith("stor", StringComparison.CurrentCultureIgnoreCase) )
            {
                rval = "af";
            }
            else if (input.StartsWith("prec", StringComparison.CurrentCultureIgnoreCase))
            {
                rval = "pu";
            }
            else
            {
                rval = input;
            }
            return rval;
        }

        

        private void SnowGG_Load(object sender, EventArgs e)
        {
            Reset();
        }

        private void buttonSelectGroup_Click(object sender, EventArgs e)
        {
            var dlg = new SelectBasin(SnowGGUtility.GetSnowggGroupList());

            dlg.ShowDialog();
                // get group name.
                string grp = dlg.SelectedGroup;
                if (grp.Trim() != "")
                {
                    this.buttonSelectGroup.Text = grp;
                    comboBoxCbtt.Items.Clear();
                    // get list of items.

                    string[] list = SnowGGUtility.GetCbttList(grp);
                    
                    for (int i = 0; i < list.Length; i++)
                    {
                        // check if first itme is 'pcode:pc'
                        if (i == 0 && list[0].IndexOf("pcode:") == 0)
                        {// set pcode
                            this.comboBoxPcode.Text = list[i].Substring(6);
                        }
                        else
                        {
                            // add other items to cbtt combo box
                            comboBoxCbtt.Items.Add(list[i]);
                        }

                    }
                    comboBoxCbtt.SelectedIndex = 0;
                }
        }

        private void comboBoxCbtt_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonRefresh_Click(this, EventArgs.Empty);
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {

            if (comboBoxCbtt.SelectedIndex < comboBoxCbtt.Items.Count - 1)
            {
                comboBoxCbtt.SelectedIndex++;
            }
        }

        private void SnowGG_VisibleChanged(object sender, EventArgs e)
        {
           // if (Visible == true)
               // Reset();
        }

        private void useList_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxCbtt.Enabled = !comboBoxCbtt.Enabled;
            comboBoxPcode.Enabled = !comboBoxPcode.Enabled;
            textBoxMultiple.Enabled = !textBoxMultiple.Enabled;
        }

    }
}
