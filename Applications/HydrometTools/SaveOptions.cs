using Reclamation.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HydrometTools
{
    public partial class SaveOptions : Form
    {
        public SaveOptions()
        {
            InitializeComponent();

            SaveToVMS = UserPreference.Lookup("SaveToVMS") == "True";
            ComputeDependencies = UserPreference.Lookup("ComputeDependencies") == "True";
        }

        public bool SaveToVMS
        {
            get { return this.checkBoxSaveVMS.Checked; }
            set { this.checkBoxSaveVMS.Checked = value; }
        }

        public bool ComputeDependencies
        {
            get
            {
                return this.checkBoxDependencies.Checked;
            }
            set { this.checkBoxDependencies.Checked = value; }

        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            UserPreference.Save("SaveToVMS", SaveToVMS.ToString());
            UserPreference.Save("ComputeDependencies", ComputeDependencies.ToString());


        }
    }
}
