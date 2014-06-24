using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.RiverWare
{

    public partial class ImportRiverWare : Form
    {
        public enum ScenarioConvention { Default, ByYear };

        public ImportRiverWare()
        {
            InitializeComponent();
        }

        public ScenarioConvention NamingConvention
        {
            get
            {
                if (radioButtonDefault.Checked)
                    return ImportRiverWare.ScenarioConvention.Default;
                return ImportRiverWare.ScenarioConvention.ByYear;

            }
        }

        public int FirstYear
        {
            get
            {
                int yr = 1;
                int.TryParse(this.textBoxYear.Text, out yr);
                return yr;
            }
        }
    }
}
