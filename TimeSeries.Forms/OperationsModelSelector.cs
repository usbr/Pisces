using System.Collections.Generic;
using System.Windows.Forms;
using Reclamation.TimeSeries.Urgsim;
using System.Data;
using Reclamation.Core;
using System.IO;
using System.Configuration;
using System;
namespace Reclamation.TimeSeries.Forms
{
    /// <summary>
    /// OperationsModelSelector manages a matrix of operation model runs
    /// This matrix is defined in a *.csv file 
    /// The first column is for Scenairo Names.  The other columns contain
    /// paths to the model data.  The first row is a header
    /// </summary>
    public partial class OperationsModelSelector : UserControl
    {

        public OperationsModelSelector()
        {
            InitializeComponent();
            dataGridView1.DataSource = Urgsim.UrgsimUtilitycs.GetOperationsModelMatrix();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            
            dataGridView1.ReadOnly = true;
        }

       


        

        public string[] SelectedOperationModels()
        {
            var rval = new List<string>();

            foreach (DataGridViewCell cell in dataGridView1.SelectedCells)
            {
                if (cell.ColumnIndex !=0 &&
                    cell.Value !=null && cell.Value.ToString() != "")
                    rval.Add(cell.Value.ToString());
            }
            

            return rval.ToArray();
        }


        /// <summary>
        /// mark cells as selected.
        /// </summary>
        /// <param name="som"></param>
        internal void SelectModels(string[] som)
        {
            dataGridView1.ClearSelection();

            for (int r = 0; r < dataGridView1.RowCount; r++)
            {
                for (int c = 1; c < dataGridView1.ColumnCount; c++)
                {
                    var cell = dataGridView1[c, r];
                    if (cell.Value != null &&  Array.IndexOf(som, cell.Value.ToString()) >= 0)
                        cell.Selected = true;
                }
            }
        }
    }
}
