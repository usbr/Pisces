using System;
namespace Reclamation.TimeSeries
{
    public interface IScenarioSelector
    {
        bool IncludeBaseline { get; }
        bool IncludeSelected { get; }
        bool MergeSelected { get; }
        event EventHandler OnApply;
        event EventHandler OnCancel;
        //string[] Selected { get; }
        bool SubtractFromBaseline { get; }
        void Show();
    }
}
