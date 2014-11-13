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
        string stationNumber;
        string ratingNumber;
        public string downloadURL;
        double recorderCorrectionValue;
        DateTime ratingBeginDate;
        DateTime recorderCorrectionDate;
        DateTime shiftEffectiveDate;
        public TextFile rawTable;
        public DataTable fullRatingTable;

        /// <summary>
        /// OwrdRatingTables constructor
        /// </summary>
        /// <param name="idNumber"></param>
        /// <param name="ratingTablePath"></param>
        public OwrdRatingTables(string idNumber )
        {
            this.stationNumber = idNumber;

            // Get and assign rating table file from the web
            string owrdURL = "http://apps.wrd.state.or.us/apps/sw/hydro_near_real_time/" +
                "hydro_download.aspx?dataset=RatingCurve&format=tsv&station_nbr=XXXXXXXX";
            downloadURL = owrdURL.Replace("XXXXXXXX", idNumber);
            var newData = Web.GetPage(owrdURL.Replace("XXXXXXXX", idNumber));
            if (newData.Count() == 0)
            { throw new Exception("OWRD data not found. Check inputs or retry later."); }

            rawTable = new TextFile(newData);
            rawTable.DeleteLine(rawTable.Length - 1); //last line from web is blank and the exisitng RDB does not have an empty last line

            ReadProperties();
            CreateFullRatingTable(this.rawTable);


        }

        private void ReadProperties()
        {
            this.ratingNumber = rawTable.ReadString("RATING_NBR");
            this.ratingBeginDate = rawTable.ReadDate("RATING_BEGIN_DATE");
            this.recorderCorrectionValue = rawTable.ReadSingle("RECORDER_CORRECTION_VALUE");
            this.recorderCorrectionDate = rawTable.ReadDate("RECORDER_CORRECTION_DATE");
            this.shiftEffectiveDate = rawTable.ReadDate("SHIFT_EFFECTIVE_DATE");
        }

        /// <summary>
        /// This method generates the full table from the RDB file
        /// </summary>
        /// <param name="rdbFile"></param>
        private void CreateFullRatingTable(TextFile rdbFile)
        {
            // Build full rating table
            var shiftTable = CreateShiftTable(rdbFile);
            DataTable fullRatingTable = new DataTable();
            fullRatingTable.Columns.Add(new DataColumn("Stage", typeof(double)));
            fullRatingTable.Columns.Add(new DataColumn("Shift", typeof(double)));
            fullRatingTable.Columns.Add(new DataColumn("Flow", typeof(double)));
            int idx1 = rdbFile.IndexOf("station_nbr") + 1;
            for (int i = idx1; i < rdbFile.Length; i++)
            {
                var row = rdbFile[i].Split('\t');
                if (row.Length < 5)
                    continue;
                var ratingStage = Convert.ToDouble(row[1]);
               // var ratingFlow = Convert.ToDouble(row[2]);
                var shiftedStage = Convert.ToDouble(row[3]);
                var shiftedFlow = Convert.ToDouble(row[4]);

                var newRow = fullRatingTable.NewRow();
                newRow["Stage"] = ratingStage - recorderCorrectionValue;
                newRow["Shift"] = shiftTable.Interpolate(ratingStage - recorderCorrectionValue);
                newRow["Flow"] = shiftedFlow;
                fullRatingTable.Rows.Add(newRow);
            }
            this.fullRatingTable = fullRatingTable;
        }

        /// <summary>
        /// This method creates the shift table based on the 'lower'-'mid'-'upper' values specified in the owrd RDB file
        /// </summary>
        /// <param name="rdbFile"></param>
        private TimeSeriesDatabaseDataSet.RatingTableDataTable CreateShiftTable(TextFile rdbFile)
        {
            double gh1 = rdbFile.ReadSingle("SHIFT_LOWER_STAGE");
            var shift1 = rdbFile.ReadSingle("SHIFT_LOWER_VALUE");
            double gh2 = rdbFile.ReadSingle("SHIFT_MID_STAGE");
            var shift2 = rdbFile.ReadSingle("SHIFT_MID_VALUE");
            double gh3 = rdbFile.ReadSingle("SHIFT_UPPER_STAGE");
            var shift3 = rdbFile.ReadSingle("SHIFT_UPPER_VALUE");

            if (gh1 == 0 && gh2 == 0 && gh3 == 0)
            {
                gh2 = 5.0;
                gh3 = 100;
            }

            // Build shift table
            var tbl = new TimeSeriesDatabaseDataSet.RatingTableDataTable();
            tbl.AddRatingTableRow(gh1, shift1);
            tbl.AddRatingTableRow(gh2, shift2);
            tbl.AddRatingTableRow(gh3, shift3);
            tbl.AddRatingTableRow(gh3*5, shift3); // extrapolate out
            return tbl;
        }
    
    
    
    }
}
