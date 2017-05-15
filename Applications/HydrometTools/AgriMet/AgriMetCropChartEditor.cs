using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Reclamation.Core;
using System.Configuration;
using System.IO;
using Reclamation.TimeSeries.AgriMet;

namespace Reclamation.AgriMet.UI
{
    public partial class AgriMetCropChartEditor : UserControl
    {
        public AgriMetCropChartEditor()
        {
            InitializeComponent();
            this.textBoxYear.Text = DateTime.Now.Year.ToString();
            CropDatesDataSet.DB = HydrometTools.Database.GetServer("agrimet");
        }


        CropDatesDataSet.CropDatesDataTable tbl;
        private void buttonRead_Click(object sender, EventArgs e)
        {

            bool hasYear = textBoxYear.Text.Trim() != "";
            bool hasGroup = textBoxGroup.Text.Trim() != "";

            int? yr=null;
            if (hasYear)
                yr = Convert.ToInt32(this.textBoxYear.Text);

            int? group = null;
            if (hasGroup)
                group = Convert.ToInt32(textBoxGroup.Text);

            tbl = CropDatesDataSet.GetCropDataTable(yr, textBoxCbtt.Text.ToUpper(), group);


           this.spreadsheetControl1.SetDataTable(tbl);
        }


         private void buttonSave_Click(object sender, EventArgs e)
         {
             //Performance perf = new Performance();
             try
             {
                 if (tbl == null)
                     return;

                 Logger.WriteLine("Saving cropdates table");
                 int recordsModified = CropDatesDataSet.SaveTable(tbl);
                 MessageBox.Show("Saved "+recordsModified+" records ");


                 Logger.WriteLine("checking crop dates (test crop generation)");
                 // Create crop charts in a temporary directory to pre-check for errors.
                 int year = Convert.ToInt32(textBoxYear.Text);
                 CropChartGenerator.CreateCropReports(year,FileUtility.GetTempPath());
                 

             }
             catch (Exception ex)
             {
                 MessageBox.Show(ex.Message+" "+ex.StackTrace);
             }
             //perf.Report("done. with crops");
         }

        

        
         private void buttonCopyLastYear_Click(object sender, EventArgs e)
         {
             CropDatesDataSet.InitializeYear(Convert.ToInt32(this.textBoxYear.Text));
             buttonRead_Click(sender, e);
         }

         private void checkBox1_CheckedChanged(object sender, EventArgs e)
         {

         }

    }
}
