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

namespace HydrometTools.Reports
{
    public partial class ViewReportsForm : Form
    {
        public ViewReportsForm()
        {
            InitializeComponent();

             var tbl = Database.YakimaStatusReports();

            var lc = new DataGridViewLinkColumn();
            lc.DataPropertyName = tbl.Columns[0].ColumnName;
            this.dataGridView1.Columns.Add(lc);
            dataGridView1.DataSource = tbl;
            dataGridView1.CellClick += dataGridView1_CellClick;
        }

        void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 0 || e.RowIndex == -1)
                return;

            DataGridViewCell cell = (DataGridViewCell)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
            DateTime t = DateTime.Now.AddDays(-1).Date;

            if (DateTime.TryParse(cell.Value.ToString(), out t))
            {
                var s = Database.GetYakimaStatusReport(t);
                var fn = FileUtility.GetTempFileName(".txt");
                File.WriteAllText(fn, s);
                System.Diagnostics.Process.Start("notepad.exe","\""+ fn +"\"");
            }
        }
    }
}
