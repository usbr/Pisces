using Reclamation.Core;
using Reclamation.TimeSeries;
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
        TimeInterval m_interval = TimeInterval.Daily;
        public SaveOptions()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            SaveToVMS = UserPreference.Lookup("SaveToVMS") == "True";
            ComputeDependencies = UserPreference.Lookup("ComputeDependencies") == "True";
            if (m_interval == TimeInterval.Monthly)
            {
                checkBoxDependencies.Enabled = false;
                checkBoxDependencies.Checked = false;

            }
        }

        public SaveOptions(TimeInterval interval)
        {
            InitializeComponent();
            m_interval = interval;
            Init();
           
         
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
            if( checkBoxDependencies.Enabled)
             UserPreference.Save("ComputeDependencies", ComputeDependencies.ToString());


        }
    }
}
