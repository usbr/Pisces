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
using Newtonsoft.Json;

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

        public IdahoPowerRatingTables(string cbtt, string stationId)
        {
            this.cbtt = cbtt;

            // Get and assign rating table file from the web
            string idprURL = "https://ps.idahopower.com/RatingsService/Index?id=S-QRiv.Rating@XXXX";
            downloadURL = idprURL.Replace("XXXX", stationId);
            if (stationId.IndexOf('@') > 0)
            {
                downloadURL = idprURL.Replace("@XXXX", " " + stationId);
            }
            var newData = Web.GetPage(downloadURL);
            if (newData.Count() == 0 )
            {
                throw new Exception("Idaho Power data not found. Check inputs or retry later."); 
            }

            //if( newData.Length <5 )
            //{
            //    var err = String.Join("\n", newData);
            //    throw new Exception("Idaho Power:" +err); 
            //}
            TextFile newRDB = new TextFile();
            foreach (var item in newData)
            { newRDB.Add(item); }
            this.webRdbTable = newRDB;

            // Get and assign RDB file properties
            //var properties = newData[1].Split(',').ToList();
            //this.stationNumber = properties[0].Split(' ')[0].ToString();
            //this.ratingBeginDate = DateTime.Parse(properties[4].ToString());
            //try
            //{ this.ratingEndDate = DateTime.Parse(properties[5].ToString()); }
            //catch
            //{ this.ratingEndDate = DateTime.MaxValue; }
            //this.expandedPoints = Convert.ToInt16(properties[6]);
            //this.originalPoints = Convert.ToInt16(properties[7]);
            //this.remarks = properties[8];
            //this.equation = properties[9];
        }
               
        public void LegacyIdahoPowerRatingTables(string cbtt)
        {
            this.cbtt = cbtt;

            // Get and assign rating table file from the web
            string idprURL = "https://ps.idahopower.com/RatingsService/Index?id=XXXX";
            downloadURL = idprURL.Replace("XXXX", cbtt);
            var newData = Web.GetPage(idprURL.Replace("XXXX", cbtt));
            if (newData.Count() == 0)
            {
                throw new Exception("Idaho Power data not found. Check inputs or retry later.");
            }

            if (newData.Length < 5)
            {
                var err = String.Join("\n", newData);
                throw new Exception("Idaho Power:" + err);
            }
            TextFile newRDB = new TextFile();
            foreach (var item in newData)
            { newRDB.Add(item); }
            this.webRdbTable = newRDB;

            // Get and assign RDB file properties
            var properties = newData[1].Split(',').ToList();
            this.stationNumber = properties[0].Split(' ')[0].ToString();
            this.ratingBeginDate = DateTime.Parse(properties[4].ToString());
            try
            { this.ratingEndDate = DateTime.Parse(properties[5].ToString()); }
            catch
            { this.ratingEndDate = DateTime.MaxValue; }
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
            /*
             * [
             *      {"InputValue":3.29,"OutputValue":0.6},
             *      {"InputValue":3.3,"OutputValue":1.3477448440833504},
             *      .
             *      .
             *      .
             *      {"InputValue":3.3499999999999996,"OutputValue":5.8185421582878085}
             * ]
             */

            var rdbItems = JsonConvert.DeserializeObject<List<RatingItem>>(rdbFileString);
            
            // Build full rating table
            DataTable fullRatingTable = new DataTable();
            fullRatingTable.Columns.Add(new DataColumn("Stage", typeof(double)));
            fullRatingTable.Columns.Add(new DataColumn("Shift", typeof(double)));
            fullRatingTable.Columns.Add(new DataColumn("Flow", typeof(double)));

            foreach (RatingItem item in rdbItems)
            {
                var ratingStage = ConvertToDouble(item.InputValue.ToString());
                var ratingFlow = ConvertToDouble(item.OutputValue.ToString());
                
                var newRow = fullRatingTable.NewRow();
                newRow["Stage"] = ratingStage;
                newRow["Shift"] = 0.0;
                newRow["Flow"] = ratingFlow;
                fullRatingTable.Rows.Add(newRow);
            }
            this.fullRatingTable = fullRatingTable;
        }

        private class RatingItem
        {
            public string InputValue;
            public string OutputValue;
        }

        private void LegacyCreateFullRatingTable(TextFile rdbFile)
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
                var row = rdbFile[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (row.Length < 4)
                {
                    Console.WriteLine("Warning skipping incomplete entry (expected four values: Stage,Discharge,ShiftStage,ShiftDischarge) ");
                    Console.WriteLine(rdbFile[i]);
                    continue;
                }
                var ratingStage = ConvertToDouble(row[0].ToString());
                var ratingFlow = ConvertToDouble(row[1].ToString());
                var shiftedStage = ConvertToDouble(row[2].ToString());
                var shiftedFlow = ConvertToDouble(row[3].ToString());

                var newRow = fullRatingTable.NewRow();
                newRow["Stage"] = ratingStage;
                newRow["Shift"] = ConvertToDouble((shiftedStage - ratingStage).ToString("F03"));
                newRow["Flow"] = shiftedFlow;
                fullRatingTable.Rows.Add(newRow);
            }
            this.fullRatingTable = fullRatingTable;
        }

        private static double ConvertToDouble(string s)
        {
            double rval=0;
            if( double.TryParse(s,out rval))
            {
                return rval;
            }
            else
            {
                Console.WriteLine("Error converting '"+s+"' to a number");
                return Convert.ToDouble(s);
            }

        }
    }
}
