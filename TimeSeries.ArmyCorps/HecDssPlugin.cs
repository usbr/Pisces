using Reclamation.TimeSeries.Usace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reclamation.Core;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
namespace Reclamation.TimeSeries.Usace
{
    class HecDSSPlugin: Reclamation.TimeSeries.IPlugin
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
            return "Add Hec &Dss File (*.dss)";
        }

        public void ModifyDatabase(Reclamation.TimeSeries.TimeSeriesDatabase db, PiscesFolder selectedFolder)
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "Hec Dss File (*.dss)|*.dss|All Files (*.*)|*.*";
            dlg.DefaultExt = ".dss";

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                 Hec.HecDssTree.AddDssFileToDatabase(
                    dlg.FileName, selectedFolder, db);
                
            }
        }
    }
}
