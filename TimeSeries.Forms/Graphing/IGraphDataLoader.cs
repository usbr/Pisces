using System;
namespace Reclamation.TimeSeries.Graphing
{
    public interface IGraphDataLoader
    {
     //   Steema.TeeChart.Styles.Line CreateTChartSeries(string legendText);
        void DrawSorted(Reclamation.TimeSeries.SeriesList list, string title, string subTitle, string xAxisTitle);
        void DrawTimeSeries(Reclamation.TimeSeries.SeriesList list, string title, string subTitle, bool undoZoom, bool multiLeftAxis = false, bool multiYear = true);
        void DrawWaterYears(Reclamation.TimeSeries.SeriesList list, string title, string subTitle, bool multiLeftAxis = false);
       // void FillTimeSeries(Reclamation.TimeSeries.Series s, Steema.TeeChart.Styles.Series tSeries);
    }
}
