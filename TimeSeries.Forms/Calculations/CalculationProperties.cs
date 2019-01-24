using System;
using System.Windows.Forms;
using Aga.Controls.Tree;

namespace Reclamation.TimeSeries.Forms.Calculations
{
    public partial class CalculationProperties : Form
    {
        public CalculationProperties()
        {
            m_series = new CalculationSeries();
            InitializeComponent();
        }
        CalculationSeries m_series;

        PiscesTree tree1;

        TimeSeriesDatabase m_db;
        public CalculationProperties(CalculationSeries s, ITreeModel model, TimeSeriesDatabase db)
        {
            string[] DBunits = db.GetUniqueUnits();
            m_db = db;
            InitializeComponent();
            tree1 = new PiscesTree(model);
            tree1.ExpandRootNodes();

            tree1.AllowDrop = false;
            tree1.Parent = this.splitContainer1.Panel1;
            tree1.Dock = DockStyle.Fill;
            tree1.RemoveCommandLine();

            m_series = s;
            basicEquation1.SeriesExpression = m_series.Expression;
            if (string.IsNullOrEmpty(m_series.SiteID))
            {
                basicEquation1.SiteID = m_series.Name;
            }
            else
            {
                basicEquation1.SiteID = m_series.SiteID;
            }
            basicEquation1.Parameter = m_series.Parameter;
            basicEquation1.TimeInterval = m_series.TimeInterval;
        }

        public bool Calculate
        {
            get { return this.basicEquation1.Calculate; }
        }


        private void buttonOK_Click(object sender, EventArgs e)
        {
            string errorMessage = "";

            m_series.Expression = basicEquation1.SeriesExpression;
            var a = this.basicEquation1.SiteID;
            if (!string.IsNullOrEmpty(basicEquation1.Parameter))
                a += "_" + basicEquation1.Parameter;

            m_series.Name = a;
            string tn = basicEquation1.TimeInterval.ToString().ToLower() + "_" + TimeSeriesDatabase.SafeTableName(a);
            tn = tn.Replace("irregular", "instant");
            m_series.Table.TableName = tn;
            m_series.TimeInterval = basicEquation1.TimeInterval;

            CalculationSeries xcs;
            TimeSeriesName tsName;
            if (a.Contains("_"))
            {
                tsName = new TimeSeriesName(basicEquation1.SiteID, basicEquation1.Parameter, basicEquation1.TimeInterval);
                m_series.Parameter = tsName.pcode;
                m_series.SiteID = tsName.siteid;
                xcs = m_db.GetCalculationSeries(m_series.SiteID, m_series.Parameter, m_series.TimeInterval);
            }
            else
            {
                tsName = new TimeSeriesName(m_series.Name, m_series.TimeInterval);
                xcs = m_db.GetCalculationSeries(m_series.Name, m_series.TimeInterval);
            }

            if( xcs != null) 
            {
                errorMessage = "This calculation already exists.";
                MessageBox.Show("Error: "+errorMessage);
                DialogResult = DialogResult.None;
            }
            else
            if ( 
                m_series.TimeSeriesDatabase.Parser.VariableResolver is Parser.HydrometVariableResolver 
               || m_series.IsValidExpression(basicEquation1.SeriesExpression, out errorMessage) 
                )
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                
               var result = MessageBox.Show("Your equation may have an error. Click OK to proceed.\n" + errorMessage,"Use this Equation?", MessageBoxButtons.OKCancel);
                if( result == DialogResult.Cancel)
                  DialogResult = DialogResult.None;   
            }
        }
    }
}
