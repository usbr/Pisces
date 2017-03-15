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

namespace Reclamation.TimeSeries.Usgs
{
    public class UsgsRatingTable
    {
        // Define Class properties
        string idNumber;
        string units;
        string stationName;
        string timeZone;
        string ratingTableVersion;
        string ratingTableComments;
        string ratingTableExpansion;
        public string downloadURL;
        public TextFile webRdbTable;
        public TextFile fileRdbTable;
        public DataTable hjTable;
        public DataTable qTable;
        public DataTable fullRatingTable;

        /// <summary>
        /// Main constructor for this class
        /// </summary>
        /// <param name="idNumber"></param>
        public UsgsRatingTable(string idNumber)
        {
            this.idNumber = idNumber;

            // Get and assign RDB file from the web
            string nwisURL = "http://waterdata.usgs.gov/nwisweb/get_ratings?site_no=XXXXXXXX&file_type=exsa";
            downloadURL = nwisURL.Replace("XXXXXXXX", idNumber);
            var newData = Web.GetPage(nwisURL.Replace("XXXXXXXX", idNumber));
            if (newData.Count() == 0)
            { throw new Exception("NWIS data not found. Check inputs or retry later."); }
            webRdbTable = new TextFile(newData);
            webRdbTable.DeleteLine(webRdbTable.Length - 1); //last line from web is blank and the exisitng RDB does not have an empty last line
            
            // Get and assign RDB file properties
            this.units = webRdbTable.ReadString("LABEL=").Replace("\"", "");
            this.stationName = webRdbTable.ReadString("STATION NAME=").Replace("\"", "");
            this.timeZone = webRdbTable.ReadString("TIME_ZONE=").Replace("\"", "");
            this.ratingTableVersion = webRdbTable.ReadString("RATING ID=").Replace("\"", "");
            this.ratingTableComments = webRdbTable.ReadString("COMMENT=").Replace("\"", "");
            this.ratingTableExpansion = webRdbTable.ReadString("RATING EXPANSION=").Replace("\"", "");
        }

        public void CreateShiftAndFlowTablesFromWeb()
        { CreateShiftAndFlowTables(this.webRdbTable); }

       // public void CreateShiftAndFlowTablesFromFile()
       // { CreateShiftAndFlowTables(this.fileRdbTable); }
    //
        /// <summary>
        /// This method generates the HJ and Q tables
        /// </summary>
        /// <param name="station"></param>
        internal void CreateShiftAndFlowTables(TextFile rdbFile)
        {
            var tempFile = Path.GetTempFileName();
            rdbFile.SaveAs(tempFile);
            rdbFile = new TextFile(tempFile);
            var rdbFileString = rdbFile.FileContents;
            var rdbItems = rdbFileString.Split('\t', '\n', '\r', ' ').ToList();//[JR] these separate the rdb file into chunks, make sure that new line characters are specified in this line
            var skelPtIdx = Enumerable.Range(0, rdbItems.Count).Where(i => rdbItems[i] == "*").ToList();//get skeletal points idx
            var breakptIdx = Enumerable.Range(0, rdbItems.Count).Where(i => rdbItems[i].Contains("BREAKPOINT")).ToList();//get breakpoint idx
            var offsetIdx = Enumerable.Range(0, rdbItems.Count).Where(i => rdbItems[i].Contains("OFFSET")).ToList();//get offset idx
            // Generate HJ Table by defining stage-shift pairs within a C# DataTable
            var hjTable = CreateHJTable(rdbFileString);
            // Define Logarithmic coefficient pairs in a C# DataTable
            var coeffTable = CoefficientTable(rdbItems, breakptIdx, offsetIdx);
            // Generate Q DataTable
            var qTable = new DataTable();
            qTable.Columns.Add(new DataColumn("Stage", typeof(double)));
            qTable.Columns.Add(new DataColumn("Flow", typeof(double)));
            qTable.Columns.Add(new DataColumn("A-Coeff", typeof(double)));
            qTable.Columns.Add(new DataColumn("B-Coeff", typeof(double)));
            foreach (var item in skelPtIdx)
            {
                var qRow = qTable.NewRow();
                double ghVal = Convert.ToDouble(rdbItems[item - 3]);
                double shiftVal = Convert.ToDouble(rdbItems[item - 2]);
                double stageVal = ghVal + shiftVal;
                qRow["Stage"] = stageVal;
                qRow["Flow"] = Convert.ToDouble(rdbItems[item - 1]);
                DataRow[] coeffMatch = coeffTable.Select("[breakpoint] <= '" + stageVal + "'");
                qRow["A-Coeff"] = coeffMatch[coeffMatch.Count() - 1][1];
                qRow["B-Coeff"] = 0.0;
                qTable.Rows.Add(qRow);
            }
            if (qTable.Rows.Count < 1)
            { throw new Exception("No skeletal points found for station: " + this.stationName); }
            this.hjTable = hjTable;
            this.qTable = qTable;
        }

        private static DataTable CoefficientTable(List<string> rdbItems, List<int> breakptIdx, List<int> offsetIdx)
        {
            var coeffTable = new DataTable();
            coeffTable.Columns.Add(new DataColumn("breakpoint", typeof(double)));
            coeffTable.Columns.Add(new DataColumn("offset", typeof(double)));
            if (offsetIdx.Count == 0) // no breakpoint and coefficient data
            {
                var coeffRow = coeffTable.NewRow();
                coeffRow["breakpoint"] = -999999999.99;
                coeffRow["offset"] = 0.0;
                coeffTable.Rows.Add(coeffRow);
            }
            else if (offsetIdx.Count == 1)
            {
                var coeffRow = coeffTable.NewRow();
                coeffRow["breakpoint"] = -999999999.99;
                coeffRow["offset"] = Convert.ToDouble(rdbItems[offsetIdx[0]].Split('=')[1].ToString().Replace("\"", ""));
                coeffTable.Rows.Add(coeffRow);
            }
            else
            {
                for (int i = 0; i < offsetIdx.Count; i++)
                {
                    var coeffRow = coeffTable.NewRow();
                    if (i == 0)
                    { coeffRow["breakpoint"] = -999999999.99; }
                    else
                    { coeffRow["breakpoint"] = Convert.ToDouble(rdbItems[breakptIdx[i - 1]].Split('=')[1].ToString().Replace("\"", "")); }
                    coeffRow["offset"] = Convert.ToDouble(rdbItems[offsetIdx[i]].Split('=')[1].ToString().Replace("\"", ""));
                    coeffTable.Rows.Add(coeffRow);
                }
            }
            return coeffTable;
        }

        private DataTable CreateHJTable(string rdbFileString)
        {
            Regex rShift = new Regex("SHIFT_PREV STAGE(.*?)SHIFT_PREV COMMENT", RegexOptions.Singleline);
            var shiftTextRow = rShift.Match(rdbFileString);
            var shifts = Regex.Matches(shiftTextRow.Value, "\"([^\"]*)\"");
            var hjTable = new DataTable();
            hjTable.Columns.Add(new DataColumn("stage", typeof(double)));
            hjTable.Columns.Add(new DataColumn("shift", typeof(double)));
            for (int i = 0; i < shifts.Count; i++)
            {
                var shiftRow = hjTable.NewRow();

                shiftRow["stage"] = ReadShift(shifts[i].Value);
                shiftRow["shift"] = ReadShift(shifts[i + 1].Value);

                hjTable.Rows.Add(shiftRow);
                i++;
            }
            

            return hjTable;
        }

        private double ReadShift(string shiftText)
        {
            shiftText = shiftText.Replace("\"", "");
            double rval = 0;
            double.TryParse(shiftText, out rval);

            return rval;
        }

        /// <summary>
        /// Compares data in two usgs rdb files, ignoring comments at the beginning
        /// </summary>
        /// <param name="tf1"></param>
        /// <param name="tf2"></param>
        /// <returns>true if there are differences in the data</returns>
        public static bool Diff(TextFile tf1, TextFile tf2)
        {

            var diff = System.Math.Abs(tf1.Length - tf2.Length);
            if ( diff > 5)
            {
                Logger.WriteLine("difference in length of files: " + diff);
                return true; //different number of lines in file
            }

            var idx1 = tf1.IndexOfRegex(@"^INDEP\s*SHIFT\s*DEP\s*STOR");
            var idx2 = tf2.IndexOfRegex(@"^INDEP\s*SHIFT\s*DEP\s*STOR");

            tf1.DeleteLines(0, idx1);
            tf2.DeleteLines(0, idx2);

            return TextFile.Compare(tf1, tf2).Length != 0;

        }

       

        public void CreateFullRatingTableFromWeb()
        { CreateFullRatingTable(this.webRdbTable); }

       // public void CreateFullRatingTableFromFile()
       // { CreateFullRatingTable(this.fileRdbTable); }

        /// <summary>
        /// This method generates the full table from the RDB file
        /// </summary>
        /// <param name="rdbFile"></param>
        private void CreateFullRatingTable(TextFile rdbFile)
        {
            // Build container DataTable
            DataTable ratingTable = new DataTable();
            ratingTable.Columns.Add(new DataColumn("Stage", typeof(double)));
            ratingTable.Columns.Add(new DataColumn("Shift", typeof(double)));
            ratingTable.Columns.Add(new DataColumn("Flow", typeof(double)));
            // Save RDB file
            var tempFile = Path.GetTempFileName();
            rdbFile.SaveAs(tempFile);
            rdbFile = new TextFile(tempFile);
            // Loop through each RDB file row to find the data
            int headerRow = rdbFile.IndexOf("INDEP	SHIFT	DEP	STOR") + 2;
            for (int i = headerRow; i < rdbFile.Length; i++)
            {
                var row = rdbFile[i];
                var newRow = ratingTable.NewRow();
                var rowVals = rdbFile[i].Split('\t');
                newRow["Stage"] = Convert.ToDouble(rowVals[0]);
                newRow["Shift"] = Convert.ToDouble(rowVals[1]);
                newRow["Flow"] = Convert.ToDouble(rowVals[2]);
                ratingTable.Rows.Add(newRow);
            }
            this.fullRatingTable = ratingTable;
        }
    }
}
