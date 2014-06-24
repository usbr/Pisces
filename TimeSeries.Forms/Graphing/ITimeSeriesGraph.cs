using System;
using System.Windows.Forms;
namespace Reclamation.TimeSeries.Graphing
{
    public interface ITimeSeriesGraph 
    {
        void Add(Reclamation.TimeSeries.Series s);
        Reclamation.TimeSeries.AnalysisType AnalysisType { set; }
        void Clear();
        void Draw(bool undoZoom);
        double MissingDataValue { get; set; }
        bool MultiLeftAxis { get; set; }
        Reclamation.TimeSeries.SeriesList Series { get; set; }
        string SubTitle { get; set; }
        string Title { get; set; }
    }
}
