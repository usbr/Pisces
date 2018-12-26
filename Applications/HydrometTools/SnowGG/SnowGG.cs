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

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                timeSeriesGraph1.AnnotationOnMouseMove = checkBoxAnnotate.Checked;
                Cursor = Cursors.WaitCursor;
                Application.DoEvents();
                string pcode = DeterminePcode();

                timeSeriesGraph1.Clear();

                string cbtt = comboBoxCbtt.Text.Trim();

                if (cbtt.Trim() == "" || pcode.Trim() == "")
                    return;

                UserPreference.Save("Snowgg->cbtt", cbtt);
                UserPreference.Save("Snowgg->pcode", comboBoxPcode.Text.Trim());

                int[] waterYears = this.yearSelector1.SelectedYears;
                var server = HydrometInfoUtility.HydrometServerFromPreferences();
                var range = monthRangePicker1.MonthDayRange;

                Series s = new HydrometDailySeries(cbtt, pcode, server);
                var sl = new SeriesList();
                sl.Add(s);

                var wyList = PiscesAnalysis.WaterYears(sl, waterYears, false, 10,true);

                AddStatistics(wyList);

                if (checkBoxGP.Checked)
                {
                    GPAverage(cbtt, server, range, wyList);
                }

                var mp = ReadMpollData(pcode, cbtt);
                mp.RemoveMissing();
                if( mp.Count >0)
                    wyList.Add(mp);

                // remove months outside selected range
                var list = FilterBySelectedRange(range, wyList);

                this.timeSeriesGraph1.AnalysisType = AnalysisType.WaterYears;
                this.timeSeriesGraph1.Series = list;
                this.timeSeriesGraph1.Title = HydrometInfoUtility.LookupSiteDescription(cbtt) + "  Elevation:" + HydrometInfoUtility.LookupElevation(cbtt);
                this.timeSeriesGraph1.Draw(true);


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


    }
}
