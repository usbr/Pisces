using System;
using System.Collections.Generic;
using System.Text;

namespace Reclamation.TimeSeries
{
    public interface IExplorerSettings
    {
        void WriteToSettings(PiscesEngine settings);
        void ReadFromSettings(PiscesEngine settings);
    }
}
