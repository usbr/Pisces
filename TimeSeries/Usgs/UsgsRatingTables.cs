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
        public TextFile webRdbTable;
        public TextFile fileRdbTable;
        public DataTable hjTable;
        public DataTable qTable;
        public DataTable fullRatingTable;

        private string ratingTablePath;

        /// <summary>
        /// Main constructor for this class
        /// </summary>
        /// <param name="idNumber"></param>
        public UsgsRatingTable(string idNumber, string ratingTablePath)
        {
            this.idNumber = idNumber;
            this.ratingTablePath = ratingTablePath;

            // Get and assign RDB file from the web
            string nwisURL = "http://waterdata.usgs.gov/nwisweb/get_ratings?site_no=XXXXXXXX&file_type=exsa";
            var newData = Web.GetPage(nwisURL.Replace("XXXXXXXX", idNumber));
            if (newData.Count() == 0)
            { throw new Exception("NWIS data not found. Check inputs or retry later."); }
            TextFile newRDB = new TextFile();
            foreach (var item in newData)
            { newRDB.Add(item); }
            newRDB.DeleteLine(newRDB.Length - 1); //last line from web is blank and the exisitng RDB does not have an empty last line
            this.webRdbTable = newRDB;
            
            // Get and assign RDB file properties
            var tempFile = Path.GetTempFileName();
            this.webRdbTable.SaveAs(tempFile);
            var rdbFile = new TextFile(tempFile);
            var rdbFileString = rdbFile.FileContents;
            var rdbItems = rdbFileString.Split('"').ToList();//[JR] these separate the rdb file into searchable chunks
            var unitsIdx = Enumerable.Range(0, rdbItems.Count).Where(i => rdbItems[i].Contains("LABEL=")).ToList();
            var stationNameIdx = Enumerable.Range(0, rdbItems.Count).Where(i => rdbItems[i].Contains("STATION NAME=")).ToList();
            var timeZoneIdx = Enumerable.Range(0, rdbItems.Count).Where(i => rdbItems[i].Contains("TIME_ZONE=")).ToList();
            var versionIdx = Enumerable.Range(0, rdbItems.Count).Where(i => rdbItems[i].Contains("RATING ID=")).ToList();
            var commentsIdx = Enumerable.Range(0, rdbItems.Count).Where(i => rdbItems[i].Contains("COMMENT=")).ToList();
            var expansionIdx = Enumerable.Range(0, rdbItems.Count).Where(i => rdbItems[i].Contains("RATING EXPANSION=")).ToList();
            this.units = rdbItems[unitsIdx[0] + 1].ToString().Replace("\"", "");
            this.stationName = rdbItems[stationNameIdx[0] + 1].ToString().Replace("\"", "");
            this.timeZone = rdbItems[timeZoneIdx[0] + 1].ToString().Replace("\"", "");
            this.ratingTableVersion = rdbItems[versionIdx[0] + 1].ToString().Replace("\"", "");
            this.ratingTableComments = rdbItems[commentsIdx[0] + 1].ToString().Replace("\"", "");
            this.ratingTableExpansion = rdbItems[expansionIdx[0] + 1].ToString().Replace("\"", "");
        }

        

        public void CreateShiftAndFlowTablesFromWeb()
        { CreateShiftAndFlowTables(this.webRdbTable); }

        public void CreateShiftAndFlowTablesFromFile()
        { CreateShiftAndFlowTables(this.fileRdbTable); }

        /// <summary>
        /// This method generates the HJ and Q tables
        /// </summary>
        /// <param name="station"></param>
        private void CreateShiftAndFlowTables(TextFile rdbFile)
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
            Regex rShift = new Regex("SHIFT_PREV STAGE(.*?)SHIFT_PREV COMMENT", RegexOptions.Singleline);
            var shiftTextRow = rShift.Match(rdbFileString);
            var shifts = Regex.Matches(shiftTextRow.Value, "\"([^\"]*)\"");
            var hjTable = new DataTable();
            hjTable.Columns.Add(new DataColumn("stage", typeof(double)));
            hjTable.Columns.Add(new DataColumn("shift", typeof(double)));
            for (int i = 0; i < shifts.Count; i++)
            {
                var shiftRow = hjTable.NewRow();
                try//some stage-shoft pairs are nans
                {
                    shiftRow["stage"] = Convert.ToDouble(shifts[i].ToString().Replace("\"", ""));
                    shiftRow["shift"] = Convert.ToDouble(shifts[i + 1].ToString().Replace("\"", ""));
                }
                catch
                {
                    shiftRow["stage"] = double.NaN;
                    shiftRow["shift"] = double.NaN;
                }
                hjTable.Rows.Add(shiftRow);
                i++;
            }
            // Define Logarithmic coefficient pairs in a C# DataTable
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

        public void CreateFullRatingTableFromWeb()
        { CreateFullRatingTable(this.webRdbTable); }

        public void CreateFullRatingTableFromFile()
        { CreateFullRatingTable(this.fileRdbTable); }

        private void CreateFullRatingTable(TextFile rdbFile)
        {
            var tempFile = Path.GetTempFileName();
            rdbFile.SaveAs(tempFile);
            rdbFile = new TextFile(tempFile);
            // [JR] work in progress

        }


    }
}
