using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Specialized;
using HydrometTools.Properties;

namespace HydrometTools.Graphing
{
    public partial class GraphProperties : UserControl
    {
        public GraphProperties()
        {
            InitializeComponent();
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            EditStringCollection es = new EditStringCollection();
            StringCollection sc = Settings.Default.DailyGraphProperties;
            string[] items = new string[sc.Count];

            Settings.Default.DailyGraphProperties.CopyTo(items, 0);

            es.Items = items;
            if (es.ShowDialog() == DialogResult.OK)
            {
                sc.Clear();
                sc.AddRange(es.Items);
            }
        }
    }
}
