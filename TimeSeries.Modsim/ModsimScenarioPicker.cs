using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace Reclamation.TimeSeries.Modsim
{
    public partial class ModsimScenarioPicker : Form
    {
        List<string> filenames;
        public ModsimScenarioPicker()
        {
            InitializeComponent();
            filenames = new List<string>();
        }

        public List<string> ScenarioFiles
        {
            get { return filenames; }
        }

        public bool ScenariosChecked
        {
            get
            {
                return checkBoxScenarios.Checked;
            }
        }

        public bool AddToTreeChecked
        {
            get
            {
                return checkBoxAddToTree.Checked;
            }
        }

        private void buttonAddScenario_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd = openFileDialog1;
            fd.DefaultExt = ".xy";
            fd.Filter = "Modsim File (*.xy)|*.xy|All Files|*.*";
            fd.RestoreDirectory = true;
            fd.Title = "Open Modsim XY File";
            fd.FileName = null;
            fd.Multiselect = true;

            if (fd.ShowDialog() == DialogResult.OK)
            {
                string[] fn = fd.FileNames;
                for (int i = 0; i < fn.Length; i++)
                {
                    filenames.Add(fn[i]);
                    listBoxScenarios.Items.Add(Path.GetFileNameWithoutExtension(fn[i]));
                }
            }
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