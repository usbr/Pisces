using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Forms.ImportForms
{
    public partial class ImportCr10x : Form
    {
        public ImportCr10x()
        {
            InitializeComponent();
            this.listBoxInterval.SelectedIndex = 3;
        }

        public int SelectedInterval
        {
            get
            {
                return Convert.ToInt32(listBoxInterval.SelectedItem);
            }
        }

        public int SelectedColumn
        {
            get { return (int)numericUpDown1.Value; } 
        }
    }
}
