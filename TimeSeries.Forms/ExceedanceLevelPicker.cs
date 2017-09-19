using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace Reclamation.TimeSeries.Forms
{
    public partial class ExceedanceLevelPicker : UserControl
    {
        public ExceedanceLevelPicker()
        {
            InitializeComponent();
            LoadExceedanceLevels();
        }

        private void LoadExceedanceLevels()
        {
            this.checkedListBox1.SetItemChecked(1, true); // 10%
            this.checkedListBox1.SetItemChecked(5, true);// 50%
            this.checkedListBox1.SetItemChecked(9, true);// 90%

            string exceedanceLevels = ConfigurationManager.AppSettings["exceedanceLevels"];
            if (exceedanceLevels != null && exceedanceLevels.Trim() != "")
            {
                checkedListBox1.Items.Clear();
                string[] levels = exceedanceLevels.Split(',');
                int i = 0;
                foreach (string level in levels)
                {
                    string s = level + "%";
                    bool isChecked = (
                           i == 0 ||
                        (i + 1) == levels.Length / 2
                     || (i + 1) == levels.Length);

                    this.checkedListBox1.Items.Add(s, isChecked);
                    i++;
                }
            }
        }

        private void ClearExceedanceLevels()
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                this.checkedListBox1.SetItemChecked(i, false);
            }
        }

        public int[] ExceedanceLevels
        {
            get
            {
                List<int> rval = new List<int>();
                if (this.checkBox1.Checked)
                {
                    string[] customLevels = this.textBox1.Text.Split(',');
                    foreach (var item in customLevels)
                    {
                        if (item != "")
                        {
                            var level = item.Replace("%", "");
                            rval.Add(int.Parse(level));
                        }
                    }
                }
                else
                {
                    foreach (int indexChecked in checkedListBox1.CheckedIndices)
                    {
                        string s = checkedListBox1.Items[indexChecked].ToString();
                        s = s.Replace("%", "");
                        rval.Add(int.Parse(s));
                    }
                }

                return rval.ToArray();
            }
            set
            {
                // TODO allow setting saved exceedance levels
                ;
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                this.checkedListBox1.Enabled = false;
                this.textBox1.Enabled = true;
                ClearExceedanceLevels();
            }
            else
            {
                this.checkedListBox1.Enabled = true;
                this.textBox1.Enabled = false;
            }
        }
    }
}
