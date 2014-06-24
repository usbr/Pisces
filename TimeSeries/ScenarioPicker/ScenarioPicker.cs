using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace Reclamation.TimeSeries.ScenarioPicker
{
    public partial class ScenarioPicker : Form
    {
        List<string> filenames;
        public ScenarioPicker()
        {
            InitializeComponent();
            filenames = new List<string>();
            buttonOK.Enabled = false;
        }

        public List<string> ScenarioFiles
        {
            get { return filenames; }
        }

        public bool ScenariosChecked
        {
            get { return checkBoxScenarios.Checked; }
        }

        public bool AddToTreeChecked
        {
            get { return checkBoxAddToTree.Checked; }
        }

        OpenFileDialog fileDialog1 = new OpenFileDialog();

        public OpenFileDialog Dialog
        {
            get { return fileDialog1; }
        }

        private void enabling()
        {
            if (ScenariosChecked == false && AddToTreeChecked == false || listBoxScenarios.Items.Count == 0)
                buttonOK.Enabled = false;
            else
                buttonOK.Enabled = true;
        }
        private void checkBoxScenarios_CheckedChanged(object sender, EventArgs e)
        {
            enabling();
        }

        private void checkBoxAddToTree_CheckedChanged(object sender, EventArgs e)
        {
            enabling();
        }
        
        private void buttonAddScenario_Click(object sender, EventArgs e)
        {
            fileDialog1.RestoreDirectory = true;
            fileDialog1.FileName = null;
            fileDialog1.Multiselect = true;

            if (fileDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] fn = fileDialog1.FileNames;
                for (int i = 0; i < fn.Length; i++)
                {
                    filenames.Add(fn[i]);
                    listBoxScenarios.Items.Add(Path.GetFileNameWithoutExtension(fn[i]));
                }
            }
            enabling();
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            int index = listBoxScenarios.SelectedIndex;
            if (index == -1)
            {
                //MessageBox.Show("Please select an item first.", "No item selected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                while (listBoxScenarios.SelectedItems.Count > 0)
                {
                    listBoxScenarios.Items.RemoveAt(index);
                    filenames.RemoveAt(index);
                }
            }
            enabling();
        }

        private void buttonMoveUp_Click(object sender, EventArgs e)
        {
            if (listBoxScenarios.SelectedItems.Count > 1)
            {
                MessageBox.Show("Please select only one item.", "Multiple item selection not supported", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (listBoxScenarios.Items.Count == 0)
            {
                return;
            }

            int index = listBoxScenarios.SelectedIndex;
            string swap = listBoxScenarios.SelectedItem.ToString();
            string swapPath = filenames[index].ToString();

            if (index != 0)
            {
                listBoxScenarios.Items.RemoveAt(index);
                listBoxScenarios.Items.Insert(index - 1, swap);
                listBoxScenarios.SelectedItem = swap;

                filenames.RemoveAt(index);
                filenames.Insert(index - 1, swapPath);
            }
        }

        private void buttonMoveDown_Click(object sender, EventArgs e)
        {
            if (listBoxScenarios.SelectedItems.Count > 1)
            {
                MessageBox.Show("Please select only one item.", "Multiple item selection not supported", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (listBoxScenarios.Items.Count == 0)
            {
                return;
            }

            int index = listBoxScenarios.SelectedIndex;
            string swap = listBoxScenarios.SelectedItem.ToString();
            string swapPath = filenames[index].ToString();

            if (index < listBoxScenarios.Items.Count - 1 && listBoxScenarios.Items.Count > 1)
            {
                listBoxScenarios.Items.RemoveAt(index);
                listBoxScenarios.Items.Insert(index + 1, swap);
                listBoxScenarios.SelectedItem = swap;

                filenames.RemoveAt(index);
                filenames.Insert(index + 1, swapPath);
            }
        }
    }
}