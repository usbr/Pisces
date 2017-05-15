using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reclamation.Core;
using System.Text.RegularExpressions;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Hydromet;

namespace HydrometTools.Graphing
{
    public partial class DailyGraphing : UserControl
    {
        public DailyGraphing()
        {
            InitializeComponent();
            graphProperties1.Items = Properties.Settings.Default.DailyGraphProperties;
            graphProperties1.ItemsChanged += new EventHandler<EventArgs>(graphProperties1_ItemsChanged);
            graphProperties1.SelectedIndexChanged += new EventHandler(graphProperties1_SelectedIndexChanged);
        }

        void graphProperties1_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonRefresh_Click(sender, e);
        }

        void graphProperties1_ItemsChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.DailyGraphProperties = graphProperties1.Items;
            Properties.Settings.Default.Save();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                Application.DoEvents();
                timeSeriesGraph1.Clear();
                string data = graphProperties1.SelectedItem.Trim();
                string title = "";
                int[] wy = null;
                string[] cbtt;
                string[] pcode;
                ParseInput(data, out title, out wy, out cbtt, out pcode);

                var list = new SeriesList();
                for (int i = 0; i < cbtt.Length; i++)
                {
                    YearRange yr = new YearRange(wy[i], this.monthDayRangePicker1.BeginningMonth);
                    var s = new HydrometDailySeries(cbtt[i], pcode[i]);
                    DateTime t1 = new DateTime(yr.DateTime1.Year, monthDayRangePicker1.MonthDayRange.Month1, monthDayRangePicker1.MonthDayRange.Day1);
                    DateTime t2 = new DateTime(yr.DateTime2.Year, monthDayRangePicker1.MonthDayRange.Month2, monthDayRangePicker1.MonthDayRange.Day2);

                    s.Read(t1, t2);
                    Series s2 = Reclamation.TimeSeries.Math.ShiftToYear(s, 2000);
                    s2.Appearance.LegendText = cbtt[i] + " " + pcode[i] + " " + wy[i];
                    list.Add(s2);
                }

                this.timeSeriesGraph1.AnalysisType = AnalysisType.WaterYears;
                this.timeSeriesGraph1.Series = list;
                timeSeriesGraph1.MultiLeftAxis = UserPreference.Lookup("MultipleYAxis") == "True";
                this.timeSeriesGraph1.Title = title;
                this.timeSeriesGraph1.Draw(true);
            }
            finally
            {
                Cursor = Cursors.Default;       
            }
        }

        private void ParseInput(string s, out string title, out int[] wy, 
            out string[] cbtt, out string[] pcode)
        {
            List<int> waterYears = new List<int>();
            List<string> sites = new List<string>();
            List<string> pc = new List<string>();
            title = "";
            //graph 1 , 1997 YAKSYS AF, 2011 YAKSYS AF, 2011 KEE AF
            string[] tokens = s.Split(',');
            if (tokens.Length >= 2)
            {
                title = tokens[0];
                for (int i = 1; i < tokens.Length; i++)
                {
                    var parts = Regex.Split(tokens[i].Trim(), @"\s+");
                    if (parts.Length != 3)
                        continue;
                    int y =0;
                    if( int.TryParse(parts[0],out y))
                    {
                        waterYears.Add(y);
                        sites.Add(parts[1]);
                        pc.Add(parts[2]);
                    }
                }
            }

            wy = waterYears.ToArray();
            cbtt = sites.ToArray();
            pcode = pc.ToArray();


        }
    }
}
