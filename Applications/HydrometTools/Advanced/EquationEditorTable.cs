using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Forms.Calculations;
using Reclamation.TimeSeries.Forms;
using DgvFilterPopup;

namespace HydrometTools.Advanced
{
    public partial class EquationEditorTable : UserControl
    {
        DgvFilterManager filterManager;

        Reclamation.TimeSeries.TimeSeriesDatabaseDataSet.SeriesCatalogDataTable tbl;
        public EquationEditorTable()
        {
            InitializeComponent();
            LoadTable();
        }

        /// <summary>
        /// Loads table of equations from the database.
        /// </summary>
        private void LoadTable()
        {
             filterManager = new DgvFilterManager();
            tbl = Database.DB().GetSeriesCatalog("Provider ='CalculationSeries'");
            this.dataGridView1.DataSource = new DataTable();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            this.dataGridView1.DataSource = tbl;
            var cols = this.dataGridView1.Columns;
            var visible = new string[]{"Units","TimeInterval","Name","TableName","Expression"};
            for (int i = 0; i < cols.Count; i++)
			{
                cols[i].Visible = Array.IndexOf(visible, cols[i].Name) >= 0;
			}
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            filterManager.DataGridView = dataGridView1;
            
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var db = Database.DB();
            var s = new CalculationSeries(db);
            var m = new TimeSeriesTreeModel( db);
            var p = new CalculationProperties(s, m , db);
            

            if (p.ShowDialog() == DialogResult.OK)
            {
                db.SuspendTreeUpdates();
                db.AddSeries(s);
            }
            LoadTable();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            LoadTable();
        }
    }
}
