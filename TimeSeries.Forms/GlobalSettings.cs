using System;
using System.Collections.Generic;
using System.Text;

namespace Reclamation.TimeSeries.Forms
{
    public static class GlobalSettings
    {
        public static void Save()
        {
            Properties.Settings.Default.Save();
        }

     
    }
}
