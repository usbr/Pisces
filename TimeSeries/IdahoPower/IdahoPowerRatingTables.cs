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

namespace Reclamation.TimeSeries.IdahoPower
{
    public class IdahoPowerRatingTables
    {
        // Define class properties
        public string stationNumber;
        public string cbtt;
        public string downloadURL;
        int expandedPoints;
        int originalPoints;
        public string remarks;
        string equation;
        DateTime ratingBeginDate;
        DateTime ratingEndDate;
        public TextFile webRdbTable;
        public TextFile fileRdbTable;
        public DataTable fullRatingTable;
        string ratingTablePath;

        public IdahoPowerRatingTables(string cbtt, string ratingTablePath)
        {
            this.cbtt = cbtt;
            this.ratingTablePath = ratingTablePath;

            // Get and assign rating table file from the web
            string idprURL = "https://ps.idahopower.com/RatingsService/Index?id=XXXX";
            downloadURL = idprURL.Replace("XXXX", cbtt);
            var newData = Web.GetPage(idprURL.Replace("XXXX", cbtt));
            if (newData.Count() == 0)
            { throw new Exception("OWRD data not found. Check inputs or retry later."); }
            TextFile newRDB = new TextFile();
            foreach (var item in newData)
            { newRDB.Add(item); }
            this.webRdbTable = newRDB;

            // Get and assign RDB file properties
            var properties = newData[1].Split(',').ToList();
            this.stationNumber = properties[0].Split(' ')[0].ToString();
            this.ratingBeginDate = DateTime.Parse(properties[4].ToString());
            this.ratingEndDate = DateTime.Parse(properties[5].ToString());
            this.expandedPoints = Convert.ToInt16(properties[6]);
            this.originalPoints = Convert.ToInt16(properties[7]);
            this.remarks = properties[8];
            this.equation = properties[9];
        }

        public void CreateFullRatingTableFromWeb()
        { CreateFullRatingTable(this.webRdbTable); }

        public void CreateFullRatingTableFromFile()
        { CreateFullRatingTable(this.fileRdbTable); }

        private void CreateFullRatingTable(TextFile rdbFile)
        {
            int ratingRows = this.expandedPoints;
            var tempFile = Path.GetTempFileName();
            rdbFile.SaveAs(tempFile);
            rdbFile = new TextFile(tempFile);
            var rdbFileString = rdbFile.FileContents;
            var rdbItems = rdbFileString.Split('\n').ToList();//[JR] these separate the rdb file into searchable chunks
            var dataRow = Enumerable.Range(0, rdbItems.Count).Where(i => rdbItems[i].Contains("Stage,Discharge,ShiftStage,ShiftDischarge")).ToList()[0] + 1;
            
            // Build full rating table
            DataTable fullRatingTable = new DataTable();
            fullRatingTable.Columns.Add(new DataColumn("Stage", typeof(double)));
            fullRatingTable.Columns.Add(new DataColumn("Shift", typeof(double)));
            fullRatingTable.Columns.Add(new DataColumn("Flow", typeof(double)));
            for (int i = dataRow; i < ratingRows + dataRow; i++)
            {
                var row = rdbFile[i].Split(',');
                var ratingStage = Convert.ToDouble(row[0].ToString());
                var ratingFlow = Convert.ToDouble(row[1].ToString());
                var shiftedStage = Convert.ToDouble(row[2].ToString());
                var shiftedFlow = Convert.ToDouble(row[3].ToString());

                var newRow = fullRatingTable.NewRow();
                newRow["Stage"] = ratingStage;
                newRow["Shift"] = Convert.ToDouble((shiftedStage - ratingStage).ToString("F03"));
                newRow["Flow"] = shiftedFlow;
                fullRatingTable.Rows.Add(newRow);
            }
            this.fullRatingTable = fullRatingTable;
        }
    }
}
