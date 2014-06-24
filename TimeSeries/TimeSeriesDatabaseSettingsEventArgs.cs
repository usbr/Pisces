using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries
{
    public class TimeSeriesDatabaseSettingsEventArgs:EventArgs
    {
        public TimeWindow Window { get; set; }
        public TimeSeriesDatabaseSettings Settings { get; set; }

        public TimeSeriesDatabaseSettingsEventArgs(TimeSeriesDatabaseSettings settings, TimeWindow window)
        {
            this.Window = window;
            this.Settings = settings;
        }


    }
}
