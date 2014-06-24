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

        public int[] ExceedanceLevels
        {
            get
            {
                List<int> rval = new List<int>();
                foreach (int indexChecked in checkedListBox1.CheckedIndices)
                {
                    string s = checkedListBox1.Items[indexChecked].ToString();
                    s = s.Replace("%", "");
                    rval.Add(int.Parse(s));
                }

                return rval.ToArray();
            }
            set
            {
                // TODO allow setting saved exceedance levels
                ;
            }

        }
    }
}
