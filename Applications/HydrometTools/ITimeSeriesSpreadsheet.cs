using Reclamation.TimeSeries;
using System;
namespace HydrometTools
{
    interface ITimeSeriesSpreadsheet
    {
        bool AutoFlagDayFiles { get; set; }
        void SetCellValue(int rowIndex, int columnIndex, double val);
        void SetDataTable(System.Data.DataTable tbl, TimeInterval db, bool scrollToTop);
        bool SuspendUpdates { get; }
        void Clear();
        event EventHandler<EventArgs> UpdateCompleted;
    }
}
