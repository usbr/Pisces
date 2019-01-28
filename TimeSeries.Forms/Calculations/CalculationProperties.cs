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
            basicEquation1.SiteID = m_series.SiteID;
            basicEquation1.Parameter = m_series.Parameter;
            basicEquation1.TimeInterval = m_series.TimeInterval;
        }

        public bool Calculate
        {
            get { return this.basicEquation1.Calculate; }
        }


        private void buttonOK_Click(object sender, EventArgs e)
        {
            m_series.Expression = basicEquation1.SeriesExpression;
            var a = this.basicEquation1.SiteID;
            if (a != "")
            {
                if (string.IsNullOrEmpty(basicEquation1.Parameter))
                {
                    m_series.Name = a;
                    a += "_c" + DateTime.Now.ToString("yyyyMMMddHHmmssfff").ToLower();
                }
                else
                {
                    a += "_" + basicEquation1.Parameter;
                    m_series.Name = a;
                }

                string tn = basicEquation1.TimeInterval.ToString().ToLower() + "_" + TimeSeriesDatabase.SafeTableName(a);
                tn = tn.Replace("irregular", "instant");
                m_series.Table.TableName = tn;

                TimeSeriesName x = new TimeSeriesName(a, basicEquation1.TimeInterval);
                m_series.Parameter = x.pcode;
                m_series.SiteID = x.siteid;
            }

            string errorMessage = "";
            m_series.TimeInterval = basicEquation1.TimeInterval;

            var xcs = m_db.GetCalculationSeries(m_series.SiteID, m_series.Parameter, m_series.TimeInterval);

            if ( xcs != null) 
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
