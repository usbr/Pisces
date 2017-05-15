using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HydrometTools.SnowGG
{
    public partial class SelectBasin : Form
    {

        public SelectBasin(string[] items)
        {
            InitializeComponent();

            foreach (var item in items)
            {
                var a = new ListViewItem(item);
                a.ToolTipText = item;
                listView1.Items.Add(a);
            }
            
        }

        public SelectBasin()
        {
            InitializeComponent();
        }

        internal string SelectedGroup
        {
            get
            {
                if (listView1.SelectedItems.Count == 0)
                    return "";
                return listView1.SelectedItems[0].Text;
            }
        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView1_ItemActivate(object sender, EventArgs e)
        {
            Visible = false;
        }
    }
}
