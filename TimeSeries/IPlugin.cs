using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries
{

   public interface IPlugin
    {
        System.Drawing.Bitmap GetImage();
        string GetAddMenuText();
        void ModifyDatabase(TimeSeriesDatabase db, PiscesFolder selectedFolder);
    }
}
