using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Configuration;
using Reclamation.Core;

namespace Reclamation.TimeSeries.Owrd
{
    /// <summary>
    /// Oregon Water Resources Department Rating Tables.
    /// http://apps.wrd.state.or.us/apps/sw/hydro_near_real_time/hydro_download.aspx?dataset=RatingCurve&format=tsv&station_nbr=14030000
    ///Format: TSV – tab delimited text
    ///                HTML – html preformatted text
    ///                XLS – Microsoft Excel
    /// </summary>
    public class OwrdRatingTables
    {
        // Define class properties
        public string stationNumber;
        string ratingNumber;
        public string downloadURL;
        public double recorderCorrectionValue;
        DateTime ratingBeginDate;
        DateTime recorderCorrectionDate;
        DateTime shiftEffectiveDate;
        public TextFile webRdbTable;
        public TextFile fileRdbTable;
        public DataTable shiftTable;
        public DataTable fullRatingTable;
        string ratingTablePath;


        /// <summary>
        /// Main constructor for this class
        /// </summary>
        /// <param name="idNumber"></param>
        /// <param name="ratingTablePath"></param>
        public OwrdRatingTables(string idNumber, string ratingTablePath)
        {
            this.stationNumber = idNumber;
            this.ratingTablePath = ratingTablePath;

            // Get and assign rating table file from the web
            string owrdURL = "http://apps.wrd.state.or.us/apps/sw/hydro_near_real_time/"+
                "hydro_download.aspx?dataset=RatingCurve&format=tsv&station_nbr=XXXXXXXX";
            downloadURL = owrdURL.Replace("XXXXXXXX", idNumber);
            var newData = Web.GetPage(owrdURL.Replace("XXXXXXXX", idNumber));
            if (newData.Count() == 0)
            { throw new Exception("OWRD data not found. Check inputs or retry later."); }

            webRdbTable = new TextFile(newData);
            webRdbTable.DeleteLine(webRdbTable.Length - 1); //last line from web is blank and the exisitng RDB does not have an empty last line

            // Get and assign RDB file properties
            var tempFile = Path.GetTempFileName();
            this.webRdbTable.SaveAs(tempFile);
            var rdbFile = new TextFile(tempFile);
            var rdbFileString = rdbFile.FileContents;
            var rdbItems = rdbFileString.Split('\t', '\r', '\n').ToList();//[JR] these separate the rdb file into searchable chunks
            var ratingNumberIdx = Enumerable.Range(0, rdbItems.Count).Where(i => rdbItems[i].Contains("RATING_NBR")).ToList();
            var ratingBeginDateIdx = Enumerable.Range(0, rdbItems.Count).Where(i => rdbItems[i].Contains("RATING_BEGIN_DATE")).ToList();
            var recorderCorrectionValueIdx = Enumerable.Range(0, rdbItems.Count).Where(i => rdbItems[i].Contains("RECORDER_CORRECTION_VALUE")).ToList();
            var recorderCorrectionDateIdx = Enumerable.Range(0, rdbItems.Count).Where(i => rdbItems[i].Contains("RECORDER_CORRECTION_DATE")).ToList();
            var shiftEffectiveDateIdx = Enumerable.Range(0, rdbItems.Count).Where(i => rdbItems[i].Contains("SHIFT_EFFECTIVE_DATE")).ToList();
            this.ratingNumber = rdbItems[ratingNumberIdx[0] + 1].ToString();
            this.ratingBeginDate = DateTime.Parse(rdbItems[ratingBeginDateIdx[0] + 1].ToString());
            this.recorderCorrectionValue = Convert.ToDouble(rdbItems[recorderCorrectionValueIdx[0] + 1].ToString());
            this.recorderCorrectionDate = DateTime.Parse(rdbItems[recorderCorrectionDateIdx[0] + 1].ToString());
            this.shiftEffectiveDate = DateTime.Parse(rdbItems[shiftEffectiveDateIdx[0] + 1].ToString());
        }


        public void CreateFullRatingTableFromWeb()
        { CreateFullRatingTable(this.webRdbTable); }

        public void CreateFullRatingTableFromFile()
        { CreateFullRatingTable(this.fileRdbTable); }

        /// <summary>
        /// This method generates the full table from the RDB file
        /// </summary>
        /// <param name="rdbFile"></param>
        private void CreateFullRatingTable(TextFile rdbFile)
        {
            var tempFile = Path.GetTempFileName();
            rdbFile.SaveAs(tempFile);
            rdbFile = new TextFile(tempFile);
            var rdbFileString = rdbFile.FileContents;
            var rdbItems = rdbFileString.Split('\t', '\r', '\n').ToList();//[JR] these separate the rdb file into searchable chunks
            var dataRowIdx = Enumerable.Range(0, rdbItems.Count).Where(i => rdbItems[i].Contains(stationNumber)).ToList();

            double recorderCorrection = this.recorderCorrectionValue;

            // Build full rating table
            DataTable fullRatingTable = new DataTable();
            fullRatingTable.Columns.Add(new DataColumn("Stage", typeof(double)));
            fullRatingTable.Columns.Add(new DataColumn("Shift", typeof(double)));
            fullRatingTable.Columns.Add(new DataColumn("Flow", typeof(double)));
            for (int i = 1; i < dataRowIdx.Count; i++)
            {
                var row = dataRowIdx[i];
                var ratingStage = Convert.ToDouble(rdbItems[row + 1].ToString());
                var ratingFlow = Convert.ToDouble(rdbItems[row + 2].ToString());
                var shiftedStage = Convert.ToDouble(rdbItems[row + 3].ToString());
                var shiftedFlow = Convert.ToDouble(rdbItems[row + 4].ToString());

                var newRow = fullRatingTable.NewRow();
                newRow["Stage"] = shiftedStage + recorderCorrection;
                newRow["Shift"] = shiftedStage - ratingStage;
                newRow["Flow"] = shiftedFlow;
                fullRatingTable.Rows.Add(newRow);
            }
            this.fullRatingTable = fullRatingTable;
        }

        public void CreateShiftTableFromWeb()
        { CreateShiftTable(this.webRdbTable); }

        public void CreateShiftTableFromFile()
        { CreateShiftTable(this.fileRdbTable); }

        /// <summary>
        /// This method creates the shift table based on the 'lower'-'mid'-'upper' values specified in the owrd RDB file
        /// </summary>
        /// <param name="rdbFile"></param>
        private void CreateShiftTable(TextFile rdbFile)
        {
            var rdbFileString = rdbFile.FileContents;
            var rdbItems = rdbFileString.Split('\t', '\r', '\n').ToList();//[JR] these separate the rdb file into searchable chunks
            var shiftStageLoIdx = Enumerable.Range(0, rdbItems.Count).Where(i => rdbItems[i].Contains("SHIFT_LOWER_STAGE")).ToList();
            var shiftStageMdIdx = Enumerable.Range(0, rdbItems.Count).Where(i => rdbItems[i].Contains("SHIFT_MID_STAGE")).ToList();
            var shiftStageHiIdx = Enumerable.Range(0, rdbItems.Count).Where(i => rdbItems[i].Contains("SHIFT_UPPER_STAGE")).ToList();
            var shiftValueLoIdx = Enumerable.Range(0, rdbItems.Count).Where(i => rdbItems[i].Contains("SHIFT_LOWER_VALUE")).ToList();
            var shiftValueMdIdx = Enumerable.Range(0, rdbItems.Count).Where(i => rdbItems[i].Contains("SHIFT_MID_VALUE")).ToList();
            var shiftValueHiIdx = Enumerable.Range(0, rdbItems.Count).Where(i => rdbItems[i].Contains("SHIFT_UPPER_VALUE")).ToList();

            // Build shift table
            DataTable shiftTable = new DataTable();
            shiftTable.Columns.Add(new DataColumn("Stage", typeof(double)));
            shiftTable.Columns.Add(new DataColumn("Shift", typeof(double)));
            shiftTable.Rows.Add(Convert.ToDouble(rdbItems[shiftStageLoIdx[0] + 1]), Convert.ToDouble(rdbItems[shiftValueLoIdx[0] + 1]));
            shiftTable.Rows.Add(Convert.ToDouble(rdbItems[shiftStageMdIdx[0] + 1]), Convert.ToDouble(rdbItems[shiftValueMdIdx[0] + 1]));
            shiftTable.Rows.Add(Convert.ToDouble(rdbItems[shiftStageHiIdx[0] + 1]), Convert.ToDouble(rdbItems[shiftValueHiIdx[0] + 1]));
            this.shiftTable = shiftTable;
        }
    
    
    
    }
}
