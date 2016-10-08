using System;
namespace Reclamation.TimeSeries.Forms.RatingTables
{
    interface IRatingTableGraph
    {
        void Clear();
        void Draw(Reclamation.TimeSeries.RatingTables.BasicMeasurement[] measurements);
        void DrawDemo();
        string SubTitle { get; set; }
        string Title { get; set; }
    }
}
