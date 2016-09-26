using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Forms
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();

            var version = new Version(Application.ProductVersion);
            label1.Text = Application.ProductName + " " + version.ToString(3);
            
            button1.Select();
        }
    }
}