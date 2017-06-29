using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedGraph;

namespace Depth
{
    public class ChartProcessor
    {
        RatingTable m_rt;
        GraphPane m_chart;

        public ChartProcessor(RatingTable rt, ZedGraph.GraphPane chart)
        {
            m_chart = chart;
            m_rt = rt;
            InitAxis("", "");
            
        }

        public void AddLogRegressionLine()
        {
            if (m_rt.Points.Length == 0)
                return;
            // Log base-10 regression line
            var xData = m_rt.Points.Select(x => x.x + m_rt.Offset).ToArray();
            var yData = m_rt.Points.Select(y => y.y).ToArray();

            var logEquation = RegressionTools.SimpleRegression(xData, yData, 1, true, 10);
            PointPairList logPointsArray = new PointPairList();
            for (int i = 0; i < xData.Length; i++)
            {
                logPointsArray.Add( logEquation.EvalTransform(xData[i]),xData[i]);
            }
            var l = m_chart.AddCurve(logEquation.Name, logPointsArray, Color.Red, SymbolType.None);
            l.Line.IsVisible = true;
            l.Symbol.IsVisible = false;
        }



        public void AddPoints()
        {
            m_chart.CurveList.Clear();

            m_chart.Title.Text = m_rt.Title;
            PointPairList points = new PointPairList();
            foreach (var pt in m_rt.Points)
            {
                points.Add(pt.y, pt.x + m_rt.Offset, pt.tag);
            }

            var line = m_chart.AddCurve("data", points, Color.Green);
        }

        public void SetupAxis()
        {
            if (m_rt.Scaling == Scaling.LogLog)
            {
                m_chart.XAxis.Type = AxisType.Log;


                m_chart.YAxis.Type = AxisType.Log;
                m_chart.AxisChange();
                m_chart.YAxis.Scale.IsUseTenPower = true;

            }
            else
            {
                m_chart.XAxis.Type = AxisType.Linear;
                m_chart.YAxis.Type = AxisType.Linear;
                m_chart.YAxis.Scale.IsUseTenPower = false;
                m_chart.AxisChange();
                m_chart.YAxis.Scale.IsUseTenPower = false;
            }

            m_chart.AxisChange();
        }

        private void InitAxis(string xTitle, string yTitle)
        {
            m_chart.XAxis.Scale.IsUseTenPower = false;
            m_chart.XAxis.Scale.Mag = 0;
            m_chart.XAxis.Scale.Format = "#,#";
            m_chart.XAxis.Scale.MinGrace = 0.05;
            m_chart.AxisChange();
            m_chart.XAxis.Title.Text = xTitle;
            m_chart.AxisChange();

            m_chart.YAxis.Scale.IsUseTenPower = false;
            m_chart.YAxis.Scale.Mag = 0;
            m_chart.YAxis.Scale.Format = "#,#";
            m_chart.YAxis.Scale.MinGrace = 0.05;
            m_chart.YAxis.Title.Text = yTitle;
            m_chart.AxisChange();
        }
    }
}
