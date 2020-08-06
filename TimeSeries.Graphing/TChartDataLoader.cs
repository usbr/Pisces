using System;
using System.Collections.Generic;
using System.Text;
using Steema.TeeChart;
//using System.Drawing;
using Reclamation.Core;
using System.Configuration;
using System.Data;
using System.Drawing;

namespace Reclamation.TimeSeries.Graphing
{
    /// <summary>
    /// Loads TimeSeries data into a TeeChart Graph
    /// </summary>
    public class TChartDataLoader : Reclamation.TimeSeries.Graphing.IGraphDataLoader
    {
        private TChart chart1;
        public TChartDataLoader(TChart chart)
        {
            chart1 = chart;
        }

        public void DrawTimeSeries(SeriesList list, string title, string subTitle,
            bool undoZoom,bool multiLeftAxis=false,bool multiYear=true)
        {
            CreateSeries(list, title, subTitle,undoZoom,multiLeftAxis);
            for (int i = 0; i < list.Count; i++)
			{
			 FillTimeSeries(list[i],chart1[i]);
			}
            FormatBottomAxisStandard(multiYear);
            chart1.Refresh();
        }

        public void DrawSorted(SeriesList list, string title, string subTitle,string xAxisTitle)
        {
            CreateSeries(list, title, subTitle,true,false);
            for (int i = 0; i < list.Count; i++)
            {
                FillSortedSeries(list[i], chart1[i]);
            }
            chart1.Refresh();
            chart1.Axes.Bottom.Title.Text = xAxisTitle;

        }
        public void DrawWaterYears(SeriesList list, string title, string subTitle, bool multiLeftAxis = false)
        {
            CreateSeries(list, title, subTitle,true,multiLeftAxis);
            for (int i = 0; i < list.Count; i++)
            {
                FillTimeSeries(list[i], chart1[i]);
            }
            FormatBottomAxisStandard(false);
            chart1.Refresh();
        }

        private void FormatBottomAxisStandard(bool multiYear)
        {
            if (multiYear)
            {
                chart1.Axes.Bottom.Labels.DateTimeFormat = "";// Oct 1
                chart1.Axes.Bottom.Labels.ExactDateTime = true;
                chart1.Axes.Bottom.Increment = 0;
            }
            else
            {
                chart1.Axes.Bottom.Labels.DateTimeFormat = "MMM d";// Oct 1
                chart1.Axes.Bottom.Labels.ExactDateTime = true;
                chart1.Axes.Bottom.Increment = Steema.TeeChart.Utils.GetDateTimeStep(Steema.TeeChart.DateTimeSteps.OneMonth);
            }
        }

        internal void DrawCorrelation(Series s1, Series s2, string title, string subTitle)
        {
            Clear();
            chart1.Text = title + "\n" + subTitle;
            chart1.Axes.Bottom.Title.Text = s1.Units + " "+ s1.Appearance.LegendText;
            chart1.Axes.Left.Title.Text = s2.Units + " "+ s2.Appearance.LegendText;

            Steema.TeeChart.Styles.Line series1 = CreateTChartSeries("");
            series1.Pointer.Visible = true;
            series1.LinePen.Visible = false;
             //series1.LineHeight = 0;
            FillCorrelation(s1, s2, series1);
            chart1.Series.Add(series1);
        }
        /// <summary>
        /// Creates basic graph with empty series
        /// </summary>
        private void CreateSeries(SeriesList list, string title, string subTitle, 
                        bool undoZoom, bool multiLeftAxis)
        {
            Clear(undoZoom);
            chart1.Legend.LegendStyle = LegendStyles.Series;
            chart1.Legend.Alignment = LegendAlignments.Top;
            // remove custom axis
            chart1.Axes.Custom.RemoveAll();
            chart1.Panel.MarginLeft = 3D;

            chart1.Text = title + "\n" + subTitle;
            if (list.Count > 1 || list.Type == SeriesListType.WaterYears)
            {
                chart1.Legend.Visible = true;
            }
            else
            {
                chart1.Legend.Visible = false;
            }

            chart1.Axes.Left.Title.Text = "";
            for (int i = 0; i < list.Count; i++)
            {
                //Steema.TeeChart.Styles.Line series = CreateTChartSeries(list[i].Appearance.LegendText);
                Steema.TeeChart.Styles.Line series = CreateTChartSeries(list.Text.Text[i]);//.Appearance.LegendText);
                if (list[i].Appearance.Style == Styles.Point)
                {
                    series.LinePen.Visible = false;
                    series.Pointer.Visible = true;
                }
                series.Stairs = list[i].Appearance.StairStep;
                string units = list[i].Units;

                // [JR] Hydromet Tools Data Analysis tab - catch and color same years
                if (System.Drawing.Color.FromName(list[i].Appearance.Color) != System.Drawing.Color.Black)
                {
                    series.Color = System.Drawing.Color.FromName(list[i].Appearance.Color);
                }

                if (multiLeftAxis)
                    SetupMultiLeftAxis(chart1,series, units);
                else
                    SetupAxisLeftRight(chart1,series, units);


                chart1.Series.Add(series);
            }
        }

        public Steema.TeeChart.Styles.Line CreateSeries(DataTable table, string columnName, TimeInterval interval, bool showBadData)
        {
            Steema.TeeChart.Styles.Line series1 = new Steema.TeeChart.Styles.Line();

            double avg = AverageOfColumn(table, columnName, interval, showBadData);
            series1.XValues.DateTime = true;
            series1.Legend.Visible = true;
            series1.Pointer.Visible = true;
            series1.Pointer.HorizSize = 2;
            series1.Pointer.VertSize = 2;

            //Color[] oldColors = {Color.Red,Color.Green,Color.Blue,Color.Black,Color.Orange, Color.Aquamarine,
            //    Color.DarkGreen,Color.Purple,Color.Aqua,Color.BlueViolet,Color.Brown,Color.BurlyWood,
            //    Color.CadetBlue,Color.Chartreuse, Color.Chocolate,Color.Coral,Color.CornflowerBlue };

            // High-contrast color palette from https://sashamaps.net/docs/tools/20-colors/
            Color[] colors =
            {
                ColorTranslator.FromHtml("#4363d8"),
                ColorTranslator.FromHtml("#f58231"),
                ColorTranslator.FromHtml("#e6194B"),
                ColorTranslator.FromHtml("#3cb44b"),
                ColorTranslator.FromHtml("#ffe119"),
                ColorTranslator.FromHtml("#911eb4"),
                ColorTranslator.FromHtml("#42d4f4"),
                ColorTranslator.FromHtml("#f032e6"),
                ColorTranslator.FromHtml("#bfef45"),
                ColorTranslator.FromHtml("#fabed4"),
                ColorTranslator.FromHtml("#469990"),
                ColorTranslator.FromHtml("#dcbeff"),
                ColorTranslator.FromHtml("#9A6324"),
                ColorTranslator.FromHtml("#fffac8"),
                ColorTranslator.FromHtml("#800000"),
                ColorTranslator.FromHtml("#aaffc3"),
                ColorTranslator.FromHtml("#808000"),
                ColorTranslator.FromHtml("#ffd8b1"),
                ColorTranslator.FromHtml("#000075"),
                ColorTranslator.FromHtml("#a9a9a9")
            };

            if (chart1.Series.Count < colors.Length)
            {
                series1.Color = colors[chart1.Series.Count];
            }

            series1.Title = columnName;

            int sz = table.Rows.Count;
            for (int i = 0; i < sz; i++)
            {
                DateTime date = (DateTime)table.Rows[i][0];

                bool plotPoint = true;
                if (interval == TimeInterval.Irregular)
                {
                    string flag = " ";
                    int idx = table.Columns.IndexOf(columnName);
                    idx++; // flag column is next
                    if (!showBadData && table.Rows[i][idx] != DBNull.Value)
                    {
                        flag = table.Rows[i][idx].ToString().Trim();
                        plotPoint = (flag == "" || flag == " " || flag == "e");
                    }
                }
                if (table.Rows[i][columnName] != System.DBNull.Value && plotPoint)
                {
                    double val = (double)table.Rows[i][columnName];
                    series1.Add((double)date.ToOADate(), val);
                }
                else
                {
                    series1.Add((double)date.ToOADate(), avg, Color.Transparent);
                }
            }

            return series1;
        }


        private static double AverageOfColumn(DataTable table, string columnName, TimeInterval interval, bool includeBadData)
        {
            int sz = table.Rows.Count;
            int counter = 0;
            double rval = 0;
            for (int i = 0; i < sz; i++)
            {

                bool plotPoint = true;
                if (!includeBadData && interval == TimeInterval.Irregular)
                {
                    string flag = "";
                    int idx = table.Columns.IndexOf(columnName);
                    idx++; // flag column is next
                    if (table.Rows[i][idx] != DBNull.Value)
                    {
                        flag = table.Rows[i][idx].ToString().Trim();
                        plotPoint = (flag == "" || flag == " " || flag == "e");
                    }
                }

                if (table.Rows[i][columnName] != System.DBNull.Value
                    && plotPoint)
                {
                    double x = (double)table.Rows[i][columnName];
                    rval += x;
                    counter++;
                }
            }
            if (counter > 0)
                return rval / counter;
            else return 0;
        }


        /// <summary>
        /// Find an existing axis with the same units, or create one, and assign this series to it.  
        /// Also enter units as the axis text
        /// </summary>
        /// <param name="series"></param>
        /// <param name="units"></param>
        public static void SetupMultiLeftAxis( TChart chart1, Steema.TeeChart.Styles.Line series, string units)
        {

            string leftUnits = chart1.Axes.Left.Title.Text;
            if (  leftUnits == "" ||  leftUnits == units)
            {
                series.VertAxis = Steema.TeeChart.Styles.VerticalAxis.Left;
                if (leftUnits == "")
                    chart1.Axes.Left.Title.Text = units;

                return; // default axis ok.
            }
            //string rightUnits = chart1.Axes.Right.Title.Text;
            //if (rightUnits == "" || rightUnits == units)
            //{
            //    series.VertAxis = Steema.TeeChart.Styles.VerticalAxis.Right;
            //    if (rightUnits == "")
            //        chart1.Axes.Right.Title.Text = units;

            //    return; // default axis ok.
            //}

            int customCount = 0;
            for (int i = 0; i < chart1.Axes.Custom.Count; i++)
            {
                if (chart1.Axes.Custom[i].IsCustom())
                    customCount++;
                if (chart1.Axes.Custom[i].Title.Text.IndexOf(units) >= 0)
                {
                    series.CustomVertAxis = chart1.Axes.Custom[i];
                    return;
                }
            }
            // add new custom axis.

            var axis1 = new Steema.TeeChart.Axis();
            chart1.Axes.Custom.Add(axis1);

            //axis1.OtherSide = false;
            chart1.Panel.MarginLeft += 5D;
            axis1.RelativePosition = -10 * (customCount+1);
            axis1.Grid.Visible = false;
            axis1.Title.Angle = 90;
            //axis1.Title.Caption = "custom";
            series.CustomVertAxis = axis1;
            axis1.Title.Text = units;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chart1"></param>
        /// <param name="series"></param>
        /// <param name="units"></param>
        public static void SetupAxisLeftRight(TChart chart1, Steema.TeeChart.Styles.Line series, string units)
        {
            string leftAxisUnits = chart1.Axes.Left.Title.Text;
            List<string> leftUnitList;
            if (chart1.Axes.Left.Title.Tag == null || leftAxisUnits =="")
            {
                leftUnitList = new List<string>();
                chart1.Axes.Left.Title.Tag = leftUnitList;
            }
            else
            {
                leftUnitList = chart1.Axes.Left.Title.Tag as List<string>;
            }

            if ( leftUnitList.IndexOf(units) <0  && leftAxisUnits != "")
                { // right axis
                    series.VertAxis = Steema.TeeChart.Styles.VerticalAxis.Right; 
                    if (chart1.Axes.Right.Title.Text.IndexOf(units) < 0)
                    {
                        chart1.Axes.Right.Title.Text += units + " ";
                        chart1.Axes.Right.Visible = true;
                    }
                }
                else
                { // left axis
                    if (  leftUnitList.IndexOf(units) <0 )//chart1.Axes.Left.Title.Text.IndexOf(units) < 0)
                    {
                        chart1.Axes.Left.Title.Text += units + " ";
                        leftUnitList.Add(units);
                    }
                }
        }


        internal void Clear( )
        {
            Clear(true);
        }

        internal void Clear(bool undoZoom )
        {
            chart1.Series.Clear();
            chart1.Aspect.View3D = false;
            chart1.Text = "";
            chart1.Legend.Visible = false;
            chart1.Aspect.Orthogonal = false;
            
            chart1.Axes.Right.Title.Text = "";
            chart1.Axes.Left.Title.Text = "";
            chart1.Axes.Left.Title.Tag = null;

            chart1.Axes.Bottom.Title.Text = "";

            chart1.Axes.Bottom.MinimumOffset = 3;
            chart1.Axes.Bottom.MaximumOffset = 3;

            chart1.Axes.Left.MinimumOffset = 3;
            chart1.Axes.Left.MaximumOffset = 3;
            if (undoZoom)
            {
                chart1.Zoom.Undo();
            }

        }


        public  Steema.TeeChart.Styles.Line CreateTChartSeries(string legendText)
        {
            Steema.TeeChart.Styles.Line series1 = new Steema.TeeChart.Styles.Line();

            series1.Legend.Visible = true;
            
            series1.Pointer.HorizSize = 2;
            series1.Pointer.VertSize = 2;
            series1.Title = legendText;
            series1.Pointer.Visible = false;
            //series1.Stairs = true;
            series1.Marks.Visible = false;

            
            
            series1.Color = Properties.Settings.Default.GetSeriesColor(chart1.Series.Count);
            series1.LinePen.Width = Properties.Settings.Default.GetSeriesWidth(chart1.Series.Count);
           // Logger.WriteLine("Color = " + series1.Color.ToKnownColor().ToString());
            return series1;
        }

        
        /// <summary>
        /// copy data from TimeSeries.Series into Steema.TeeChart.Styles.Series
        /// </summary>
        /// <param name="s"></param>
        /// <param name="tSeries"></param>
        public  void FillTimeSeries(Series s,Steema.TeeChart.Styles.Series tSeries)
        {
            if (s.Count == 0)
            {
                return;
            }
            
            tSeries.XValues.DateTime = true;
            double avg = TimeSeries.Math.AverageOfSeries(s);
            int sz = s.Count;
            for (int i = 0; i < sz; i++)
            {
                Point pt = s[i];
                double x = pt.DateTime.ToOADate();
                if (!pt.IsMissing)
                {
                    tSeries.Add(x, pt.Value);
                }
                else
                {
                    tSeries.Add(x, avg, System.Drawing.Color.Transparent);
                }
            }

        }

        private static void FillSortedSeries(Series s, Steema.TeeChart.Styles.Series tSeries)
        {
            tSeries.XValues.DateTime = false;
            int sz = s.Count;
            for (int i = 0; i < sz; i++)
            {
                Point pt = s[i];
                double x = pt.Percent;
                tSeries.Add(x, pt.Value);
            }
        }

        private  void FillCorrelation(Series s1, Series s2, Steema.TeeChart.Styles.Line series1)
        {

            series1.XValues.DateTime = false;
            int sz = s1.Count;

            for (int i = 0; i < s1.Count; i++)
            {
                Point pt = s1[i];

                if (!pt.IsMissing)
                {
                    int idx = s2.IndexOf(pt.DateTime);
                    if (idx >= 0)
                    {
                        Point pt2 = s2[idx];
                        if (!pt2.IsMissing)
                        {
                            series1.Add(pt.Value, pt2.Value);
                        }
                    }
                }
            }
        }

        
    }
}
