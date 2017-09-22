using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Usace
{
    public partial class ImportCorpsDataQuery : Form
    {
        public ImportCorpsDataQuery()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "http://www.nwd-wc.usace.army.mil/perl/dataquery.pl";
            System.Diagnostics.Process.Start(url);

        }

        public string DssPath
        {
            get { return this.textBoxPath.Text; }
        }
    }
}
