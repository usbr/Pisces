using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Reclamation.Core;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.RatingTables;
using Reclamation.TimeSeries.Hydromet;
#if SpreadsheetGear
using Reclamation.TimeSeries.Excel;
using SpreadsheetGear;
#endif 

namespace HydrometTools
{
    public partial class RatingTableTableViewer : UserControl
    {
        public RatingTableTableViewer()
        {
            InitializeComponent();
        }

        private TimeSeriesDatabaseDataSet.RatingTableDataTable m_ratingTable;

        public TimeSeriesDatabaseDataSet.RatingTableDataTable RatingTable
        {
            get {
                if (m_ratingTable == null)
                    m_ratingTable = new TimeSeriesDatabaseDataSet.RatingTableDataTable();
                return m_ratingTable;
            }

            set
            {
                m_ratingTable = value;
                if (checkBoxMultiColumn.Checked)
                    this.dataGridView1.DataSource = RatingTableUtility.ConvertToMultiColumn(value);
                else
                    this.dataGridView1.DataSource = value;
            
            }

        }
        

        private void buttonXls_Click(object sender, EventArgs e)
        {
            try
            {
#if SpreadsheetGear                
                string tmpFilename = FileUtility.GetTempFileName(".xls");
                string template = FileUtility.GetFileReference("RatingTableTemplate.xls");
                File.Copy(template, tmpFilename, true);
                ExportToExcel(tmpFilename, checkBoxMultiColumn.Checked, RatingTable);
#else

                string tmpFilename = FileUtility.GetTempFileName(".csv");
                DataTable table = RatingTable;
                if( checkBoxMultiColumn.Checked)
                   table = RatingTableUtility.ConvertToMultiColumn(RatingTable);
                CsvFile.WriteToCSV(table, tmpFilename, false, true);
#endif
                System.Diagnostics.Process.Start(tmpFilename);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private static void ExportToExcel(string tmpXlsName, bool multiColumn,
               Reclamation.TimeSeries.TimeSeriesDatabaseDataSet.RatingTableDataTable rt)
        {
#if SpreadsheetGear

            var xls = new SpreadsheetGearExcel(tmpXlsName);

            string sheetName = multiColumn ? "Standard" : "simple";
            IWorksheet sheet = (IWorksheet)xls.Workbook.Sheets[sheetName];

            xls.Workbook.Sheets[!multiColumn ? "Standard" : "simple"].Delete();

            IRange rng = sheet.Range[0, 0];

            DataTable table = rt;
            rng[0, 0].Value = rt.Name + " " + rt.EditDate;

            if (multiColumn)
            {
                table = RatingTableUtility.ConvertToMultiColumn(table);
                rng["A3"].CopyFromDataTable(table, SpreadsheetGear.Data.SetDataFlags.AllText);
                for (int c = 1; c <= 10; c++)
                {
                    rng[1, c].Value = rt.YUnits;
                }
            }
            else
            {
                rng["A3"].CopyFromDataTable(table, SpreadsheetGear.Data.SetDataFlags.None);
                rng[2, 0].Value = rt.XUnits;
                rng[2, 1].Value = rt.YUnits;
            }

            xls.Save();
#else




#endif
        }

        private void checkBoxMultiColumn_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxMultiColumn.Checked)
                this.dataGridView1.DataSource = RatingTableUtility.ConvertToMultiColumn(RatingTable); 
            else
                this.dataGridView1.DataSource = RatingTable; 
        }
    }
}
