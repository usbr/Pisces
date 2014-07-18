using System;
using System.Collections.Generic;
using System.Text;
using Reclamation.Core;
using System.Configuration;
using ZedGraph;
using System.Drawing;

namespace Reclamation.TimeSeries.Graphing
{
    /// <summary>
    /// Loads TimeSeries data into a ZedGraph Graph
    /// </summary>
    public class ZedGraphDataLoader
    {
        private ZedGraph.ZedGraphControl chart1;
        private GraphPane pane; 
        public ZedGraphDataLoader(ZedGraph.ZedGraphControl chart)
        {
            chart1 = chart;
            pane = chart1.GraphPane;
        }

        public void DrawTimeSeries(SeriesList list, string title, string subTitle,
            bool undoZoom,bool multiLeftAxis=false)
        {
            CreateSeries(list, title, subTitle,undoZoom,multiLeftAxis);

            for (int i = 0; i < list.Count; i++)
			{
                
              FillTimeSeries(list[i],chart1.GraphPane.CurveList[i]);
			}
            
            FormatBottomAxisStandard();
            chart1.RestoreScale(chart1.GraphPane);
            pane.YAxis.Scale.Mag = 0;
            pane.YAxis.Scale.Format = "#,#";

            chart1.Refresh();
        }

        public void DrawSorted(SeriesList list, string title, string subTitle,string xAxisTitle)
        {
            Clear();
            for (int i = 0; i < list.Count; i++)
            {
                PointPairList pairs = new PointPairList();
                foreach (var pt in list[i])
                {
                    pairs.Add(pt.Percent, pt.Value);
                }
                LineItem myCurve = pane.AddCurve(list[i].Appearance.LegendText,
                    pairs, Color.Red);//,SymbolType.Diamond);
                myCurve.Symbol.Fill.Type = FillType.None;
            }
            pane.XAxis.Title.Text = xAxisTitle;
           
            pane.XAxis.Type = AxisType.Linear;


            pane.YAxis.Scale.Mag = 0; 
            pane.YAxis.Scale.Format = "#,#";
            pane.XAxis.Scale.Format = "";
            //pane.XAxis.Scale.MajorUnit = DateUnit.Day;
            //pane.XAxis.Scale.MajorStep = 1;

            pane.YAxis.Title.Text = String.Join(", ", list.Text.UniqueUnits);
           chart1.AxisChange();
            chart1.Refresh();
            
        }
        public void DrawWaterYears(SeriesList list, string title, string subTitle, bool multiLeftAxis = false)
        {
            CreateSeries(list, title, subTitle,true,multiLeftAxis);
            for (int i = 0; i < list.Count; i++)
            {
                FillTimeSeries(list[i], chart1.GraphPane.CurveList[i]);
            }
            FormatBottomAxisWaterYearStyle();
            chart1.Refresh();
        }

        private void FormatBottomAxisStandard()
        {
            var myPane = chart1.GraphPane;
            myPane.XAxis.Title.Text = "Date";
            myPane.XAxis.Type = AxisType.Date;
            myPane.XAxis.Scale.Format = "dd-MMM-yy";
            myPane.XAxis.Scale.MajorUnit = DateUnit.Day;
            myPane.XAxis.Scale.MajorStep = 1;
            //myPane.XAxis.Scale.Min = new XDate(DateTime.Now.AddDays(-NumberOfBars));
            //myPane.XAxis.Scale.Max = new XDate(DateTime.Now);
            myPane.XAxis.MajorTic.IsBetweenLabels = true;
            myPane.XAxis.MinorTic.Size = 0;
            myPane.XAxis.MajorTic.IsInside = false;
            myPane.XAxis.MajorTic.IsOutside = true;


        }
        private void FormatBottomAxisWaterYearStyle()
        {
            var pane = chart1.GraphPane;
            pane.XAxis.Type = AxisType.Date;
            pane.XAxis.Scale.Format = "MMM d";
        }

        internal void DrawCorrelation(Series s1, Series s2, string title, string subTitle)
        {
            Clear();
            chart1.GraphPane.XAxis.Type = AxisType.Linear;

            var pane = chart1.GraphPane;
            pane.Title.Text = title + "\n" + subTitle;
            pane.XAxis.Title.Text = s1.Units + " " + s1.Appearance.LegendText;
            pane.YAxis.Title.Text = s2.Units + " "+ s2.Appearance.LegendText;

            var series1 = CreateSeries("");

            FillCorrelation(s1, s2, series1);
            chart1.GraphPane.CurveList.Add(series1);
        }
        /// <summary>
        /// Creates basic graph with empty series
        /// </summary>
        private void CreateSeries(SeriesList list, string title, string subTitle, 
                        bool undoZoom, bool multiLeftAxis)
        {
            Clear(undoZoom);
            var pane = chart1.GraphPane;

            chart1.Text = title + "\n" + subTitle;
            LineItem series = new LineItem("");
            for (int i = 0; i < list.Count; i++)
            {
               series = CreateSeries(list.Text.Text[i]);
                //string units = list[i].Units;
                
            }
            pane.CurveList.Add(series);
        }



        internal void Clear( )
        {
            Clear(true);
        }

        internal void Clear(bool undoZoom )
        {

            var pane = this.chart1.GraphPane;
            pane.Title.Text = "";
            pane.XAxis.Title.Text = "";
            pane.Y2Axis.Title.Text = "";
            pane.CurveList.Clear();
        }


        private Reclamation.TimeSeries.Forms.Properties.Settings Default
        {
            get { return Reclamation.TimeSeries.Forms.Properties.Settings.Default; }
        }

        LineItem CreateSeries(string legendText)
        {
            var pane = this.chart1.GraphPane;
            var series1 = new LineItem(legendText);
            series1.Symbol.Size = 2;
            series1.Color =  Default.GetSeriesColor(pane.CurveList.Count);
            series1.Line.Width = Default.GetSeriesWidth(pane.CurveList.Count);
            return series1;
        }

        
        /// <summary>
        /// copy data from TimeSeries.Series into ZedGraph CurveItem
        /// </summary>
        /// <param name="s"></param>
        /// <param name="tSeries"></param>
         void FillTimeSeries(Series s,CurveItem tSeries)
        {
            if (s.Count == 0)
            {
                return;
            }

            var pane = this.chart1.GraphPane;
            pane.XAxis.Type = AxisType.Date;

            double avg = TimeSeries.Math.AverageOfSeries(s);
            int sz = s.Count;
            
            for (int i = 0; i < sz; i++)
            {
                Point pt = s[i];
                double x = pt.DateTime.ToOADate();
            
                if (!pt.IsMissing)
                {
                    tSeries.AddPoint(x, pt.Value);
                }
                else
                {
                    //list.Add(x, avg, System.Drawing.Color.Transparent);
                }
            }
        }

          


        private  void FillCorrelation(Series s1, Series s2, LineItem series1)
        {

            
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
                            series1.AddPoint(pt.Value, pt.Value);
                        }
                    }
                }
            }
        }

        
    }
}
