using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Forms
{
    public partial class Options : Form
    {
        public Options()
        {
            InitializeComponent();
        }


        public bool HydrometWebOnly
        {
            get
            { return this.checkBoxHydrometWebOnly.Checked; }

            set { this.checkBoxHydrometWebOnly.Checked = value;
            Enabling();

            }
        }

        public string DecodesOutputDirectory
        {
            get { return this.textBoxDecodesPath.Text; }
            set { this.textBoxDecodesPath.Text = value; }
        }
        public bool HydrometUseWebCache
        {
            get
            { return this.checkBoxHydrometWebCache.Checked; }

            set { this.checkBoxHydrometWebCache.Checked = value; }
        }
        public bool HydrometAutoUpdate
        {
            get
            { return this.checkBoxHydrometAutoUpdate.Checked; }

            set { this.checkBoxHydrometAutoUpdate.Checked = value; }
        }

        public bool HydrometIncludeFlaggedData
        {
            get
            { return this.checkBoxHydrometIncludeFlaggedData.Checked; }

            set { this.checkBoxHydrometIncludeFlaggedData.Checked = value; }
        }

        public bool UsgsDailyAutoUpdate
        {
            get
            { 
                return this.checkBoxUsgsAutoUpdate.Checked; 
            }
            set {
                this.checkBoxUsgsAutoUpdate.Checked = value;
            }
        }
        public bool MultipleYAxis
        {
            get
            {
                return this.checkBoxMultiYAxis.Checked;
            }
            set
            {
                this.checkBoxMultiYAxis.Checked = value;
            }
        }


        public void Enabling()
        {
            checkBoxHydrometAutoUpdate.Enabled = !checkBoxHydrometWebOnly.Checked;
        }

        private void checkBoxHydrometWebOnly_CheckedChanged(object sender, EventArgs e)
        {
            Enabling();
        }


        public bool ModsimDisplayFlowInCfs
        {

            set
            {
                this.checkBoxModsimDisplayCfs.Checked = value;
            }
            get
            {
                return this.checkBoxModsimDisplayCfs.Checked;
            }
        }

        public bool ExcelAutoUpdate
        {
            get { return this.checkBoxExcelAutoUpdate.Checked; }
            set { this.checkBoxExcelAutoUpdate.Checked = value; }
        }

        public bool AutoRefresh
        {
            get { return this.checkBoxAutoRefresh.Checked; }
            set { this.checkBoxAutoRefresh.Checked = value; }
        }

        public bool HydrometVariableResolver
        {
            get { return checkBoxHydrometVariableResolver.Checked; }
            set { this.checkBoxHydrometVariableResolver.Checked = value; }
        }

        public bool VerboseLogging 
        {
            get { return checkBoxVerboseLogging.Checked; }
            set { this.checkBoxVerboseLogging.Checked = value; }
        }

    }
}
