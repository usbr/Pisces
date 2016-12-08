using System.Data;
using System.Collections.Generic;
using Reclamation.TimeSeries.RatingTables;
namespace Reclamation.TimeSeries
{
    public interface IExplorerView
    {

        SeriesList SeriesList { set; get;}
        string Title { set;}
        string SubTitle { set;}
       bool MultipleYAxis { set; }
        DataTable DataTable { set; get;}
        AnalysisType AnalysisType { set;}
        bool MonthlySummaryMultiYear { set; }
        List<string> Messages { get; set;}
        void Draw();
        bool UndoZoom { set; }

        void Clear();
    }
}
