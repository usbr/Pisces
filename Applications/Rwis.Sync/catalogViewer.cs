using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Rwis.Sync
{
    public partial class catalogViewer : Form
    {
        public catalogViewer(DataTable dt)
        {
            InitializeComponent();
            catalogDataGridView.DataSource = dt;
        }
    }
}
