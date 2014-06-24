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

            this.label1.Text =Application.ProductName +  " " + Application.ProductVersion;
            button1.Select();
        }
    }
}