using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Reclamation.TimeSeries.Forms;


namespace Reclamation.TimeSeries.Forms
{
    public partial class DisplayOptionsDialog : Form, IExplorerSettings
    {
        PiscesEngine explorer1;
        
       // List<Control> userCtrls;
       // List<IExplorerSettings> settingsViews;
        Control activeControl;
        Analysis.AnalysisCollection analysisList;

        public DisplayOptionsDialog(PiscesEngine explorer)
        {

            InitializeComponent();
            this.explorer1 = explorer;
            
            listBox1.Items.Clear();
            analysisList = explorer1.AnalysisList;

            for (int i = 0; i < analysisList.Count; i++)
            {
                listBox1.Items.Add(explorer1.AnalysisList[i].Name);
                
            }
            analysisList[AnalysisType.TimeSeries].UserInterface = new TimeSeriesOptions();
            analysisList[AnalysisType.Exceedance].UserInterface = new ExceedanceOptions();
            analysisList[AnalysisType.Probability].UserInterface = new ProbabilityOptions();
            analysisList[AnalysisType.WaterYears].UserInterface = new WaterYearOptions();
            analysisList[AnalysisType.SummaryHydrograph].UserInterface = new SummaryHydrographOptions();
            analysisList[AnalysisType.Correlation].UserInterface = new CorrelationOptions();
            analysisList[AnalysisType.MonthlySummary].UserInterface = new MonthlySummaryOptions();
            analysisList[AnalysisType.MovingAverage].UserInterface = new MovingAverageOptions();
            analysisList[AnalysisType.TraceAnalysis].UserInterface = new TraceOptions();


            listBox1.SelectedIndex = 0;

            ReadFromSettings(explorer);

        }


        public void WriteToSettings(PiscesEngine settings)
        {
            analysisList[this.listBox1.SelectedIndex].ExplorerSettings.WriteToSettings(settings);
        }

        public void ReadFromSettings(PiscesEngine settings)
        {
         analysisList[this.listBox1.SelectedIndex].ExplorerSettings.ReadFromSettings(settings);
         //explorer1.SaveSettings();
        }


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (activeControl != null)
            {
                activeControl.Visible = false;
            }

            activeControl = analysisList[listBox1.SelectedIndex].UserInterface;
            activeControl.Parent = this.groupBox1;
            activeControl.Left = 5;
            activeControl.Top = 15;
            activeControl.Visible = true;
            this.textBoxDescription.Lines = analysisList[listBox1.SelectedIndex].Description.Split('\n');
            analysisList[this.listBox1.SelectedIndex].ExplorerSettings.ReadFromSettings(explorer1);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            analysisList[this.listBox1.SelectedIndex].ExplorerSettings.WriteToSettings(explorer1);
            explorer1.SaveSettings();
        }

        private void linkLabelPropertyGrid_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SettingsPropertyGrid pg = new SettingsPropertyGrid();
            pg.ObjectToExplore = this.explorer1;
            pg.ShowDialog();
        }

   






       
    }
}