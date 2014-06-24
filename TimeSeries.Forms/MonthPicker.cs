using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Forms
{
    [Obsolete()]
    public partial class MonthPicker : UserControl
    {
        public MonthPicker()
        {
            InitializeComponent();
            button1_Click(this, EventArgs.Empty);
        }

        public int[] SelectedMonths
        {
            get
            {
                List<int> selected = new List<int>();
                foreach (int indexChecked in checkedListBox1.CheckedIndices)
                {
                    selected.Add(indexChecked +1);
                }
                return selected.ToArray();
            }
        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                this.checkedListBox1.SetItemChecked(i, true);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (int indexChecked in checkedListBox1.CheckedIndices)
            {
                checkedListBox1.SetItemChecked(indexChecked, false);
            }
        }
    }
}
