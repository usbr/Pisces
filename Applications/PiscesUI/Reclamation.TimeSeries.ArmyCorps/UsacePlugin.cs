using Reclamation.TimeSeries.Usace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reclamation.Core;
using System.IO;
using System.Drawing;
namespace Reclamation.TimeSeries.Usace
{
    class UsacePlugin: Reclamation.TimeSeries.IPlugin
    {

        public System.Drawing.Bitmap GetImage()
        {
            var fn = Path.Combine(FileUtility.GetExecutableDirectory(), "images");
            fn = Path.Combine(fn, "hecdss.ico");
            if (!File.Exists(fn))
                return null;
            var b1 = new Bitmap(fn);
            var b = new Bitmap(b1, new Size(16, 16));
            return b1;
        }

        public string GetAddMenuText()
        {
            return "US Army Corps Web Query (northwestern division) ...";
        }

        public void ModifyDatabase(Reclamation.TimeSeries.TimeSeriesDatabase db, PiscesFolder selectedFolder)
        {
            var dlg = new ImportCorpsDataQuery();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var s = new Usace.UsaceSeries(dlg.DssPath);
                db.AddSeries(s, selectedFolder);
            }
        }
    }
}
