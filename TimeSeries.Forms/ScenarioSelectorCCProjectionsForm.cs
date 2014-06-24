using System;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Forms
{
    public partial class ScenarioSelectorCCProjectionsForm : Form, Reclamation.TimeSeries.IScenarioSelector
    {
        private TimeSeriesDatabase m_db;
        ScenarioSelectorCCProjections scenarioCCProjections1;
        ScenarioSelectorCCProjections scenarioCCReference1;

        public ScenarioSelectorCCProjectionsForm(TimeSeriesDatabase db)
        {
            InitializeComponent();
            m_db = db;
            scenarioCCProjections1 = new ScenarioSelectorCCProjections(m_db);
            scenarioCCProjections1.Parent = tabPageProjections;
            scenarioCCProjections1.Dock = DockStyle.Fill;

            scenarioCCReference1 = new ScenarioSelectorCCProjections();
            scenarioCCReference1.Parent = tabPageReference;
            scenarioCCReference1.Dock = DockStyle.Fill;
        }

        public bool MergeSelected
        {
            get { return false; }
        }
        public bool IncludeBaseline
        {
            get { return scenarioReferenceControl1.IncludeBaseline; }
        }

        public bool IncludeSelected
        {
            get { return scenarioReferenceControl1.IncludeSelected; }
        }

        public bool SubtractFromBaseline
        {
            get { return scenarioReferenceControl1.SubtractFromBaseline; }
        }

        public event EventHandler OnApply;
        public event EventHandler OnCancel;

        private void buttonOK_Click(object sender, EventArgs e)
        {
            scenarioCCProjections1.SaveChanges();
            scenarioCCReference1.SaveChanges();
            if (OnApply != null)
                OnApply(this, EventArgs.Empty);
            this.Visible = false;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            scenarioCCProjections1.ScenarioSelectorCCProjections_Load(this, EventArgs.Empty);
            scenarioCCReference1.ScenarioSelectorCCProjections_Load(this, EventArgs.Empty);
            if (OnCancel != null)
                OnCancel(this, EventArgs.Empty);
            Visible = false;
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            scenarioCCProjections1.SaveChanges();
            scenarioCCReference1.SaveChanges();
            if (OnApply != null)
                OnApply(this, EventArgs.Empty);
        }

        private void ScenarioSelectorCCProjectionsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Visible = false;
        }

        private void tabPage_Click(object sender, EventArgs e)
        {
            if (tabControlCCScenarios.SelectedTab.Equals(tabPageReference))
                labelReference.Visible = true;
            else
                labelReference.Visible = false;
        }

        private void ScenarioSelectorCCProjectionsForm_Load(object sender, EventArgs e)
        {
            labelReference.Visible = false;
        }
    }
}
