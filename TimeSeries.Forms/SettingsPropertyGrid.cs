using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Forms
{
    public partial class SettingsPropertyGrid : Form
    {
        public SettingsPropertyGrid()
        {
            InitializeComponent();
        }

        private object _settings;

        public object ObjectToExplore
        {
            get { return _settings; }
            set { _settings = value;
            this.propertyGrid1.SelectedObject = _settings;
            }
        }
	
    }
}