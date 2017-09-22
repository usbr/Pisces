using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace Reclamation.TimeSeries.Hydross
{
    public partial class HydrossScenarioPicker : Form
    {
        List<string> filenames;
        public HydrossScenarioPicker()
        {
            InitializeComponent();
            filenames = new List<string>();
        }


        public List<string> ScenarioFiles
        {
            get { return filenames; }
        }
        private void buttonAddScenario_Click(object sender, EventArgs e)
        {

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fn = openFileDialog1.FileName;
                string ext = Path.GetExtension(fn);
                fn = fn.Replace(ext, "");
                filenames.Add(fn);
                this.listBoxScenarios.Items.Add(Path.GetFileNameWithoutExtension(fn));

            }
        }
    }
}
