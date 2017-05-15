using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reclamation.Core;
using Reclamation.AgriMet;

namespace HydrometTools.AgriMet
{
    public partial class AgriMetTab : UserControl
    {
        public AgriMetTab()
        {
            InitializeComponent();
#if SpreadsheetGear
            var uc = new Reclamation.AgriMet.UI.AgriMetCropChartEditor();
            uc.Parent = tabPageCropDates;
            uc.Dock = DockStyle.Fill;
#endif
        }

       
    }
}
