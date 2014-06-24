using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Forms
{
    public partial class GCM_CheckBox : UserControl
    {
        public GCM_CheckBox()
        {
            InitializeComponent();
        }

        public bool Checked
        {
            get { return this.checkBox1.Checked; }
            set { this.checkBox1.Checked = value; }
        }

        [Category("Custom")]
        public string GCM { get; set; }
        [Category("Custom")]
        public string Run { get; set; }
        [Category("Custom")]
        public string EmissionScenario { get; set; }
    }
}
