using System.Data;
using System.Windows.Forms;
using System;
using System.Drawing;

namespace HydrometForecast
{
    public partial class CompareTerms : Form
    {
        public CompareTerms()
        {
            InitializeComponent();
        }

        public void LoadTables(DataTable terms, DataTable history, DataTable diff)
        {

            this.dataGridViewTerms.DataSource = terms;
            this.dataGridViewHistory.DataSource = history;
            this.dataGridDiff.DataSource = diff;


            dataGridDiff.DefaultCellStyle.Format = "F2";
            dataGridViewTerms.DefaultCellStyle.Format = "F2";

            dataGridDiff.Columns[0].DefaultCellStyle.Format = "F0";
            dataGridViewTerms.Columns[0].DefaultCellStyle.Format = "F0";


            dataGridDiff.CellFormatting += new DataGridViewCellFormattingEventHandler(dataGridViewDifference_CellFormatting);
        }

        void dataGridViewDifference_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 0)
                return;

            object o = dataGridDiff[e.ColumnIndex, e.RowIndex].Value; 
            if (o != null)
            {

                double d = Convert.ToDouble(o);

                if (Math.Abs(d) > 0.015)
                {
                    DataGridViewCellStyle s = e.CellStyle;
                    Font f = s.Font;
                    Font f2 = new Font(f, FontStyle.Bold);
                    e.CellStyle.Font = f2;
                    s.BackColor = Color.Red;
                }
            }
        }
    }
}
