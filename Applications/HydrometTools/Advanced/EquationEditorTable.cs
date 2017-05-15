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

namespace HydrometTools.Advanced
{
    public partial class EquationEditorTable : UserControl
    {

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
            tbl = Database.DB().GetSeriesCatalog("Provider ='CalculationSeries'");
            this.dataGridView1.DataSource = tbl;
            var cols = this.dataGridView1.Columns;
            var visible = new string[]{"Units","TimeInterval","Name","TableName","Expression"};
            for (int i = 0; i < cols.Count; i++)
			{
                cols[i].Visible = Array.IndexOf(visible, cols[i].Name) >= 0;
			}
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            
            
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var db = Database.DB();
            var s = new CalculationSeries(db);
            var m = new TimeSeriesTreeModel( db);
            var p = new CalculationProperties(s, m , db.GetUniqueUnits());

            if (p.ShowDialog() == DialogResult.OK)
            {
                db.AddSeries(s);
            }
            LoadTable();
        }
    }
}
