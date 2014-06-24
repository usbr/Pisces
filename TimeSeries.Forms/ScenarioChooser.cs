using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Forms
{
    /// <summary>
    /// Form that allows user to selectively choose multiple scenarios
    /// </summary>
    public partial class ScenarioChooser : Form
    {


        public ScenarioChooser(string[] names)
        {
            InitializeComponent();

            this.checkedListBox1.Items.Clear();

            int i=1;
            foreach (string s in names)
            { // select first 1 secnarios by default.
                this.checkedListBox1.Items.Add(s, (i <2));
                i++;
            }
        }



        public ScenarioChooser()
        {
            InitializeComponent();
        }


        private void buttonSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                this.checkedListBox1.SetItemChecked(i, true);
            }
        }

        private void buttonClearAll_Click(object sender, EventArgs e)
        {
            foreach (int indexChecked in checkedListBox1.CheckedIndices)
            {
                checkedListBox1.SetItemChecked(indexChecked, false);
            }
        }

        public string[] Selected
        {
            get
            {
                List<string> selected = new List<string>();
                foreach (int indexChecked in checkedListBox1.CheckedIndices)
                {
                    selected.Add(checkedListBox1.Items[indexChecked].ToString());
                }
                return selected.ToArray();

            }
        }
    }
}