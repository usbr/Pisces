using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Reclamation.Core;
using System.Configuration;
using Reclamation.TimeSeries.Urgsim;

namespace Reclamation.TimeSeries.Forms
{
    public partial class ScenarioSelectorCCProjections : UserControl
    {
        private TimeSeriesDatabase m_db;
        private bool m_referenceBuilder = false;
        OperationsModelSelector opModelSelector;

        public ScenarioSelectorCCProjections(TimeSeriesDatabase db)
        {
            InitializeComponent();
            SetupOperationsModelSelector();
            m_db = db;
        }
       

        public ScenarioSelectorCCProjections()
        {
            InitializeComponent();
            m_referenceBuilder = true;
            SetupOperationsModelSelector();
        }

        private void SetupOperationsModelSelector()
        {
            opModelSelector = new OperationsModelSelector();
            opModelSelector.Parent = groupBoxMatrix;
            opModelSelector.Dock = DockStyle.Fill;
        }

        /// <summary>
        /// Check or uncheck a whole row/column of climate modeles using the 'all' or 'none' buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MakeCheckBoxSelection(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                var btn = sender as Button;
                Console.WriteLine(btn.Tag);

                var tokens = btn.Tag.ToString().Split(',');
                if (tokens.Length < 3)
                {
                    MessageBox.Show("Error:  This button does not have the tag defined");
                    return;
                }

                bool isModel = tokens[0].ToLower().Trim() == "model";
                bool checkState = tokens[1].ToLower().Trim() == "all";
                string modelName = "";
                string emissionName = "";
                if (isModel)
                {
                    modelName = tokens[2].Trim().ToLower();
                }
                else
                {
                    emissionName = tokens[2].Trim().ToLower();
                }

                GCM_CheckBox[] checkBox = GetCheckBoxes();
                foreach (GCM_CheckBox cb in checkBox)
                {
                    if (cb.GCM.ToLower() == modelName
                        || cb.EmissionScenario.ToLower() == emissionName)
                    {
                        cb.Checked = checkState;
                    }
                }
            }
        }

        private GCM_CheckBox[] GetCheckBoxes()
        {
            var rval = new List<GCM_CheckBox>();
            for (int i = 0; i < this.Controls.Count; i++)
            {
                if (Controls[i] is GCM_CheckBox)
                {
                    var checkBox = Controls[i] as GCM_CheckBox;

                    if (!checkBox.Enabled)
                        continue;

                    if (checkBox.GCM == null)
                    {
                        MessageBox.Show("Error: checkbox " + checkBox.Name + " doesn't have GCM defined");
                        break;
                    }

                    rval.Add(checkBox);
                }
            }
            return rval.ToArray();
        }

        public void SaveChanges()
        {
            var scenarioTable = GetScenarioTable();
            var checkBox = GetCheckBoxes();
            
            var som = opModelSelector.SelectedOperationModels();
            if (!ValidateModelSelection() )
            {
                return;
            }
            string[] opModels = UrgsimUtilitycs.OperationModels();

            foreach (GCM_CheckBox cb in checkBox)
            {
                foreach (string om in opModels)
                {
                    string scenarioName = om + "/" + GetClimateProjectionName(cb);
                    var row = GetScenarioTableRowsByName(scenarioTable, scenarioName);

                    bool ck = (cb.Checked && Array.IndexOf(som, om) >= 0);

                    row.Checked = ck;
                }
            }
            SaveScenarioTable(scenarioTable);
        }

        private void SaveScenarioTable(TimeSeriesDatabaseDataSet.ScenarioDataTable scenarioTable)
        {
            if (m_referenceBuilder)
                scenarioTable.WriteLocalXml();
            else
                m_db.Server.SaveTable(scenarioTable);
        }

        private TimeSeriesDatabaseDataSet.ScenarioDataTable GetScenarioTable()
        {
            if (m_referenceBuilder)
            {
                var rval = new TimeSeriesDatabaseDataSet.ScenarioDataTable();
                rval.ReadLocalXml();
                return rval;
            }
            else
            {
                var scenarioTable = m_db.GetScenarios();
                return scenarioTable;
            }
        }

        public void ScenarioSelectorCCProjections_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            ReadSelectionsFromScenarioTable();
        }

        private void ReadSelectionsFromScenarioTable()
        {
            var scenarioTable = GetScenarioTable();
            var checkBox = GetCheckBoxes();
            string[] opModels = UrgsimUtilitycs.OperationModels();
             
        
            foreach (GCM_CheckBox cb in checkBox)
            {
                cb.Checked = false;
             string projectionName = GetClimateProjectionName(cb);
   
                foreach (string om in opModels)
                {
                    string sn = om + "/" + projectionName;
                    var row = GetScenarioTableRowsByName(scenarioTable, sn);
                    if (row.Checked)
                        cb.Checked = true;
                }
            }

            // select operation models.
            var som = (from r in scenarioTable.AsEnumerable()
                       where r.Checked
                       && r.Name.IndexOf("/") > 0
                       select r.Name.Substring(0, r.Name.IndexOf("/"))).Distinct().ToArray();

            opModelSelector.SelectModels(som);

            
        }

        private bool ValidateModelSelection()
        {
            if (!m_referenceBuilder && opModelSelector.SelectedOperationModels().Count() ==0)
            {
                MessageBox.Show("Must select at least one operations model");
                return false;
            }
            return true;
        }

        private TimeSeriesDatabaseDataSet.ScenarioRow GetScenarioTableRowsByName(TimeSeriesDatabaseDataSet.ScenarioDataTable scenarioTable,
                                                     string scenarioName)
        {
            var rows = scenarioTable.Select("name='" + scenarioName + "'");
            if (rows.Length > 1)
            {
                throw new DuplicateNameException("Duplicate rows found for name '" + scenarioName + "'");
            }
            return (TimeSeriesDatabaseDataSet.ScenarioRow) rows[0];
        }


        //private string[] GetSelectedScenarioNames(GCM_CheckBox cb)
        //{
        //    var rval = new List<string>();
        //    string projectionName = GetClimateProjectionName(cb);
            
        //    foreach (var m in opModelSelector.SelectedOperationModels() )
        //    {
        //        rval.Add(m + "/" + projectionName);
        //    }
            
        //    return rval.ToArray();
        //}

        private static string GetClimateProjectionName(GCM_CheckBox cb)
        {
            string projectionName = string.Empty;
            projectionName = string.Concat(cb.GCM.ToLower(), ".", cb.Run, ".sres",
                                                  cb.EmissionScenario.ToLower());
            return projectionName;
        }
    }
}
