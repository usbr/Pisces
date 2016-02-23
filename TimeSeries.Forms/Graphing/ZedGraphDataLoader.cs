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
        private ZedGraphControl chart1;
        private GraphPane pane;
        private float defaultLineWidth = 2;
        private float defaultSymbolSize = 2;
        private System.Drawing.Point mouseUpLoc;
        private System.Drawing.Point mouseDownLoc;

        public ZedGraphDataLoader(ZedGraphControl chart)
        {
            chart1 = chart;
            pane = chart1.GraphPane;
            pane.IsFontsScaled = false;
            pane.YAxis.Scale.MinGrace = 0;
            pane.YAxis.Scale.MaxGrace = 0;
            pane.XAxis.Scale.MinGrace = 0;
            pane.XAxis.Scale.MaxGrace = 0;
            pane.XAxis.Scale.MagAuto = false;
            pane.YAxis.Scale.MagAuto = false;
            
            SetPaneVisible(false);

            chart1.ZoomEvent += chart1_ZoomEvent;
            chart1.MouseDownEvent += chart1_MouseDownEvent;
            chart1.MouseUpEvent += chart1_MouseUpEvent;
        }

        private void SetPaneVisible(bool visible)
        {
            pane.Title.Text = "";

            pane.Border.IsVisible = visible;
            pane.Legend.IsVisible = visible;
            pane.XAxis.IsVisible = visible;
            pane.YAxis.IsVisible = visible;
            pane.Y2Axis.IsVisible = !pane.Y2Axis.IsAxisSegmentVisible;

            pane.XAxis.MajorGrid.IsVisible = visible;
            pane.YAxis.MajorGrid.IsVisible = visible;
            pane.XAxis.MajorGrid.DashOn = 5f;
            pane.YAxis.MajorGrid.DashOn = 5f;
            pane.XAxis.MajorGrid.Color = Color.LightGray;
            pane.YAxis.MajorGrid.Color = Color.LightGray;
        }

        private bool chart1_MouseUpEvent(ZedGraphControl sender, System.Windows.Forms.MouseEventArgs e)
        {
            mouseUpLoc = e.Location;
            return false;
        }

        private bool chart1_MouseDownEvent(ZedGraphControl sender, System.Windows.Forms.MouseEventArgs e)
        {
            mouseDownLoc = e.Location;
            return false;
        }

        private void chart1_ZoomEvent(ZedGraphControl sender, ZoomState oldState, ZoomState newState)
        {
            if (mouseDownLoc.IsEmpty || mouseUpLoc.IsEmpty)
            {
                return;
            }

            // if zoom rectangle is drawn from right to left, clear all zooming
            if (mouseDownLoc.X > mouseUpLoc.X && mouseDownLoc.Y > mouseUpLoc.Y )
            {
                sender.ZoomOutAll(sender.GraphPane);
                RefreshChart(sender);
            }
        }

        private void LabelYaxis(SeriesList list)
        {
            pane.YAxis.Title.Text = String.Join(", ", list.Text.UniqueUnits);
        }

        public void DrawTimeSeries(SeriesList list, string title, string subTitle,
            bool undoZoom,bool multiLeftAxis=false)
        {
            Clear();

            if (list.Count == 0)
            {
                return;
            }

            CreateSeries(list, title, subTitle,undoZoom,multiLeftAxis);

            for (int i = 0; i < list.Count; i++)
            {
                
              FillTimeSeries(list[i],chart1.GraphPane.CurveList[i]);
            }

            SetPaneVisible(true);
            FormatBottomAxisStandard();
            pane.YAxis.Scale.Format = "#,#";
            pane.YAxis.Scale.MinAuto = true;
            pane.YAxis.Scale.MaxAuto = true;
            LabelYaxis(list);
            RefreshChart(chart1);
        }

        public void DrawSorted(SeriesList list, string title, string subTitle,string xAxisTitle)
        {
            Clear();

            if (list.Count == 0)
            {
                return;
            }

            for (int i = 0; i < list.Count; i++)
            {
                PointPairList pairs = new PointPairList();
                foreach (var pt in list[i])
                {
                    pairs.Add(pt.Percent, pt.Value);
                }

                var color = Default.GetSeriesColor(pane.CurveList.Count);
                LineItem myCurve = pane.AddCurve(list[i].Appearance.LegendText,
                    pairs, color);
                myCurve.Symbol.IsVisible = false;
                myCurve.Line.Width = defaultLineWidth;
            }
            pane.XAxis.Title.Text = xAxisTitle;
           
            pane.XAxis.Type = AxisType.Linear;
            pane.YAxis.Scale.Format = "#,#";
            pane.XAxis.Scale.Format = "";
            pane.XAxis.Scale.MajorStep = 5;
            pane.YAxis.Scale.MinAuto = true;
            pane.YAxis.Scale.MaxAuto = true;
            SetPaneVisible(true);
            LabelYaxis(list);
            RefreshChart(chart1);
            
        }

        private void RefreshChart(ZedGraphControl chart)
        {
            chart.GraphPane.AxisChange();
            chart.Refresh();
        }

       
        public void DrawWaterYears(SeriesList list, string title, string subTitle, bool multiLeftAxis = false)
        {
            Clear();

            if (list.Count == 0)
            {
                return;
            }

            CreateSeries(list, title, subTitle,true,multiLeftAxis);
            for (int i = 0; i < list.Count; i++)
            {
                FillTimeSeries(list[i], chart1.GraphPane.CurveList[i]);
            }
            FormatBottomAxisStandard();
            pane.XAxis.Scale.Format = "MMM d";
            pane.YAxis.Scale.MinAuto = true;
            pane.YAxis.Scale.MaxAuto = true;
            SetPaneVisible(true);
            RefreshChart(chart1);
        }

        private void FormatBottomAxisStandard()
        {
            pane.XAxis.Title.Text = "Date";
            pane.XAxis.Type = AxisType.Date;
            pane.XAxis.Scale.Format = "M/d/yyyy";
            pane.XAxis.Scale.MajorUnit = DateUnit.Month;
            pane.XAxis.Scale.MajorStep = 1;
            pane.XAxis.Scale.MinGrace = 0;
            pane.XAxis.Scale.MaxGrace = 0;
            pane.XAxis.MajorTic.IsBetweenLabels = true;
            pane.XAxis.MinorTic.Size = 0;
            pane.XAxis.MajorTic.IsInside = false;
            pane.XAxis.MajorTic.IsOutside = true;
        }

        internal void DrawCorrelation(Series s1, Series s2, string title, string subTitle)
        {
            Clear();
            pane.XAxis.Type = AxisType.Linear;

            pane.Title.Text = title + "\n" + subTitle;
            pane.XAxis.Title.Text = s1.Units + " " + s1.Appearance.LegendText;
            pane.YAxis.Title.Text = s2.Units + " "+ s2.Appearance.LegendText;

            var series1 = CreateCorrelationSeries("");

            FillCorrelation(s1, s2, series1);
            pane.CurveList.Add(series1);
            pane.XAxis.Scale.Format = "#,#";
            pane.XAxis.Scale.MajorStepAuto = true;

            SetPaneVisible(true);
            RefreshChart(chart1);
        }
        /// <summary>
        /// Creates basic graph with empty series
        /// </summary>
        private void CreateSeries(SeriesList list, string title, string subTitle, 
                        bool undoZoom, bool multiLeftAxis)
        {
            Clear(undoZoom);

            chart1.Text = title + "\n" + subTitle;
            LineItem series = new LineItem("");
            for (int i = 0; i < list.Count; i++)
            {
               series = CreateSeries(list.Text.Text[i]);
                //string units = list[i].Units;
               pane.CurveList.Add(series);    
            }
            
        }



        internal void Clear( )
        {
            Clear(true);
        }

        internal void Clear(bool undoZoom )
        {
            if (undoZoom)
                chart1.ZoomOutAll(pane);

            SetPaneVisible(false);
            pane.CurveList.Clear();
            RefreshChart(chart1);
        }


        private Reclamation.TimeSeries.Forms.Properties.Settings Default
        {
            get { return Reclamation.TimeSeries.Forms.Properties.Settings.Default; }
        }

        LineItem CreateSeries(string legendText)
        {
            var series1 = new LineItem(legendText);
            series1.Symbol.IsVisible = false;
            series1.Color =  Default.GetSeriesColor(pane.CurveList.Count);
            series1.Line.Width = Default.GetSeriesWidth(pane.CurveList.Count);
            series1.Line.Width = defaultLineWidth;
            return series1;
        }

        LineItem CreateCorrelationSeries(string legendText)
        {
            var series1 = new LineItem(legendText);
            series1.Line.IsVisible = false;
            series1.Symbol.IsVisible = true;
            series1.Symbol.Size = defaultSymbolSize;
            series1.Symbol.Fill.Type = FillType.Solid;
            series1.Color = Default.GetSeriesColor(pane.CurveList.Count);
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

            
            for (int i = 0; i < sz; i++)
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
