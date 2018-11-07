using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HydrometTools.Reports
{
    public partial class Reports : UserControl
    {
        public Reports()
        {
            InitializeComponent();
            UpdateTabs();

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTabs();
        }

        private void UpdateTabs()
        {
            if (tabControl1.SelectedTab == tabPageYakima)
            {
                var yak = new YakimaStatus();
                yak.Parent = tabPageYakima;
                yak.Dock = DockStyle.Fill;
            }
        }
    }
}
