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
    public partial class ScenarioReferenceControl : UserControl
    {
        public ScenarioReferenceControl()
        {
            InitializeComponent();
        }

        public bool SubtractFromBaseline
        {
            get { return checkBoxSubtractFromReference.Checked; }
        }
        public bool IncludeBaseline
        {
            get { return checkBoxIncludeReference.Checked; }
        }

        public bool IncludeSelected
        {
            get { return checkBoxIncludeSelected.Checked; }
        }
    }
}
