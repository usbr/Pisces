using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HydrometForecast
{
    public partial class ForecastList : UserControl
    {
        public ForecastList()
        {
            InitializeComponent();
        }

        internal void SetSheetNames(string[] sheetNames)
        {
            this.listBox1.Items.Clear();
            this.listBox1.Items.AddRange(sheetNames);
        }

        private void buttonSelectAll_Click(object sender, EventArgs e)
        {
            SelectAll(true);
        }

        private void SelectAll(bool sel)
        {
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                listBox1.SetSelected(i, sel);
            }
        }

        private void buttonClearAll_Click(object sender, EventArgs e)
        {
            SelectAll(false);
        }

        public string[] SelectedItems {
            get
            {
                var rval = new List<string>();
                for (int i = 0; i < listBox1.SelectedItems.Count; i++)
                {
                    rval.Add(listBox1.SelectedItems[i].ToString());
                }
                return rval.ToArray();
            }
        
        
        }
    }
}
