using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Depth
{
    public partial class FormMain : Form
    {
        RatingTableChart chart;
        public FormMain()
        {
            InitializeComponent();
            chart = new RatingTableChart();
            chart.Parent = this.panelChart;
            chart.Dock = DockStyle.Fill;
        }

        private void buttonPlot_Click(object sender, EventArgs e)
        {

            chart.Draw(ratingInputs1.RatingTable);
        }

        private string filename = "";
        private void openMenu_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "*.txt|*.txt";

            if( dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filename = dlg.FileName;
                ratingInputs1.Lines = File.ReadAllLines(dlg.FileName);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (File.Exists(filename))
            {
                File.WriteAllLines(filename, ratingInputs1.Lines);
            }
            else
            {
                Save();
            }

        }

        private void Save()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "*.txt|*.txt";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                filename = dlg.FileName;
                File.WriteAllLines(filename, ratingInputs1.Lines);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }
    }
}
