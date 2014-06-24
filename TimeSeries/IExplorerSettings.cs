using System;
using System.Collections.Generic;
using System.Text;

namespace Reclamation.TimeSeries
{
    public interface IExplorerSettings
    {
        void WriteToSettings(PiscesSettings settings);
        void ReadFromSettings(PiscesSettings settings);
    }
}
