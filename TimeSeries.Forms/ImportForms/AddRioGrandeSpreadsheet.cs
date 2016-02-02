using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Forms.ImportForms
{
    public partial class AddRioGrandeSpreadsheet : Form
    {
        public AddRioGrandeSpreadsheet()
        {
            InitializeComponent();
        }

        public string SeriesName
        {
            get { return this.textBoxName.Text; }
        }

        public string Units
        {
            get { return this.textBoxUnits.Text; }
        }
    }
}
