using Reclamation.Core;
using Reclamation.TimeSeries.Usgs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace HydrometServer
{
    /// <summary>
    /// Download Rating Tables from the internet.
    /// </summary>
    class RatingTableDownload
    {

        /* NOTES: 
         * 1. Look for '[JR]' within this code file to see areas that need modification to put the code in production
         * 2. This method assumes that the USGS, OWRD, and Idaho Power rating table file formats do not change. Changes to file 
         *      lengths and contents may change but the string indicators for the station number, Breakpoints, Offsets, Stages, 
         *      Shifts, Skeleton Points, and other similar properties should not change.
         * 3. 8/8/2014: Tested and working for all 139 RTF stations in the repository. May need to modify the code in the future
         *      to capture other errors that are not manifested in the 8/8/2014 RTFs.
         * 4. 8/20/2014: Modified to account for lone Offsets with no matching Breakpoints. Tested and working 8/22/2014
         * 5. 9/25/2014: Modified to also check OWRD and Idaho Power rating tables. Also added some code to generate flow and 
         *      shift CSV files to be used by the FileRatingTable() method to interpolate flows given a gage height. Tested and 
         *      working 9/26/2014
         * 
         */

        private static string hydrometRTFs = Path.Combine(ConfigurationManager.AppSettings["LocalConfigurationDataPath"], "rating_tables");
        private static UsgsRatingTable usgsRatingTable;

        /// <summary>
        /// Checks Current Rating Table files against the USGS website.
        /// Copies new USGS Rating Table if an update was made and moves the old Rating Table into an '_Attic' in the current repository
        /// before regenerating new HJ and Q tables
        /// </summary>
        /// <param name="generateNewTables">Override to regenerate HJ and Q tables from current USGS website rating tables</param>
        public static void UpdateRatingTables(string configFile, bool generateNewTables = false)
        {
            Console.WriteLine("reading " + configFile);
            var inputText = new CsvFile(configFile);
            var stationUpdateList = new List<string>();
            var attachments = new List<string>();
            List<string> attachmentRecipientList = new List<string>(); // additional recipients for USGS attachemnts
            // Loop through each row in the input text file
            for (int k = 0; k < inputText.Rows.Count; k++)
            {
                try
                {
                    var dRow = inputText.Rows[k];
                    string attachmentRecipients = "";
                    UpdatesingleRatingTable(dRow, generateNewTables, inputText, stationUpdateList, attachments, out attachmentRecipients);
                    if (attachmentRecipients != "" && !attachmentRecipientList.Contains(attachmentRecipients) )
                        attachmentRecipientList.Add(attachmentRecipients);
                }
                catch (Exception e)
                {

                    Console.WriteLine("Error: processing table  "+e.Message);
                }
            }

            // Send out e-mail notifications
            string subject = "Daily Rating Table Update Results " + DateTime.Now.ToString("MM-dd-yyyy");
            if (stationUpdateList.Count > 0)
            {
                var emailMsg = "Daily Rating Table Update Results " + DateTime.Now.ToString("MM-dd-yyyy") + ": " + stationUpdateList.Count +
                     " rating tables were updated.<br>";
                foreach (var item in stationUpdateList)
                {
                    emailMsg += item + "<br>";
                }
                SendEmail(subject, emailMsg, attachments, attachmentRecipientList);
            }
        }

        private static void UpdatesingleRatingTable(DataRow dRow,bool generateNewTables, CsvFile inputText,
            List<string> stationUpdateList, List<string> attachments, out string attachmentRecipients)
        {
            string urlDownload = "";
            // Define parameters to be used for this checking iteration
            
            string cbtt = dRow["cbtt"].ToString().ToLower();
            attachmentRecipients = "";
            string stationID = dRow["site_id"].ToString();
            var email = dRow["email"].ToString();
            string agency = dRow["agency"].ToString();

            Console.Write(cbtt.PadRight(8, '.') + " " + agency.ToLower().PadLeft(5));
            // Check if the RDB is currently in the system
            string rdbFileName = Path.Combine(hydrometRTFs, cbtt + ".rdb");
            string shiftFileName = Path.Combine(Path.Combine(hydrometRTFs, "_hj_tables"), cbtt + "_hj.csv");
            string qFileName = Path.Combine(Path.Combine(hydrometRTFs, "_q_tables"), cbtt + "_q.csv");

            // Get full RatingTableFile
            DataTable fullRatingTable;
            TextFile webRdbTable;
            TextFile fileRdbTable;
            if (agency == "USGS")
            {
                usgsRatingTable = new Reclamation.TimeSeries.Usgs.UsgsRatingTable(stationID);
                usgsRatingTable.CreateShiftAndFlowTablesFromWeb();
                usgsRatingTable.CreateFullRatingTableFromWeb();
                fullRatingTable = usgsRatingTable.fullRatingTable;
                webRdbTable = usgsRatingTable.webRdbTable;
                urlDownload = usgsRatingTable.downloadURL;
            }
            else if (agency == "OWRD")
            {
                var ratingTable = new Reclamation.TimeSeries.Owrd.OwrdRatingTables(stationID);
                fullRatingTable = ratingTable.fullRatingTable;
                webRdbTable = ratingTable.rawTable;
                urlDownload = ratingTable.downloadURL;
            }
            else if (agency == "IDPWR")
            {
                var ratingTable = new Reclamation.TimeSeries.IdahoPower.IdahoPowerRatingTables(cbtt);
                ratingTable.CreateFullRatingTableFromWeb();
                fullRatingTable = ratingTable.fullRatingTable;
                webRdbTable = ratingTable.webRdbTable;
                urlDownload = ratingTable.downloadURL;
            }
            else
            { throw new Exception(cbtt.ToUpper() + "'s rating table from " + agency + " is not supported."); }

            // Create new RDB, files if the file does not currently exist
            if (!File.Exists(rdbFileName))
            {
                stationUpdateList.Add(@"<a href=""" + urlDownload + @""">" + cbtt + " (" + agency + " " + stationID + ")</a>  updated existing table");
                if (agency == "USGS")
                { WriteHjAndQTables(shiftFileName, qFileName, usgsRatingTable); }
                WriteCsvFiles(fullRatingTable, cbtt);
                Console.WriteLine("                     new table");
                webRdbTable.SaveAs(rdbFileName);
            }
            // Check the existing file for updates
            else
            {
                // Get old RTF currently on file and copy it into temp. This is done to enable overwriting if the web file has been updated.
                fileRdbTable = GetRDBTableFromFile(cbtt, hydrometRTFs);
                // Compare
                var fileDiff = TextFile.Compare(fileRdbTable, webRdbTable);
                // Save new RTF file to repository and generate new HJ and Q tables if the file was updated
                if (fileDiff.Count() != 0 || generateNewTables)
                {
                    stationUpdateList.Add(@"<a href=""" + urlDownload + @""">" + cbtt + " (" + agency + " " + stationID + ")</a>  updated existing table");
                    // Copy old RDB to _Attic and save new RDB to repository
                    if (!generateNewTables)
                    {
                        fileRdbTable.SaveAs(Path.Combine(Path.Combine(hydrometRTFs, "_attic"), cbtt + DateTime.Now.ToString("_yyyy-MM-dd") + ".rdb"));//[JR] relies on the existence of an '_Attic' folder in the repository
                        webRdbTable.SaveAs(rdbFileName);
                    }
                    if (agency == "USGS")
                    { WriteHjAndQTables(shiftFileName, qFileName, usgsRatingTable); }
                    WriteCsvFiles(fullRatingTable, cbtt);
                    // Define which attachments to add to the mail message if the 'email' field in the input file is not blank
                    if (email != "")
                    {
                        attachmentRecipients = email;
                        attachments.Add(shiftFileName);
                        attachments.Add(qFileName);
                    }
                    Console.WriteLine("             UPDATED");
                }
                else
                { Console.WriteLine("   current"); }
            }
            //return urlDownload;
        }

        private static void WriteHjAndQTables(string shiftFileName, string qFileName,
            UsgsRatingTable ratingTable)
        {
            CsvFile.WriteToCSV(ratingTable.hjTable, shiftFileName, false);
            CsvFile.WriteToCSV(ratingTable.qTable, qFileName, false);
        }

        /// <summary>
        /// Writes CSV files to be used by the FileRatingTable() method
        /// </summary>
        /// <param name="rdb"></param>
        /// <param name="cbtt"></param>
        private static void WriteCsvFiles(DataTable fullRatingTable, string cbtt)
        {
            // Build q container DataTable
            DataTable qRatingTable = new DataTable();
            qRatingTable.Columns.Add(new DataColumn("Stage", typeof(double)));
            qRatingTable.Columns.Add(new DataColumn("Flow", typeof(double)));
            // Build hj container DataTable
            DataTable hjRatingTable = new DataTable();
            hjRatingTable.Columns.Add(new DataColumn("Stage", typeof(double)));
            hjRatingTable.Columns.Add(new DataColumn("Shift", typeof(double)));

            foreach (DataRow row in fullRatingTable.Rows)
            {
                var qNewRow = qRatingTable.NewRow();
                qNewRow["Stage"] = Round(row["Stage"]);
                qNewRow["Flow"] = row["Flow"];
                qRatingTable.Rows.Add(qNewRow);

                var hjNewRow = hjRatingTable.NewRow();
                hjNewRow["Stage"] = Round(row["Stage"]);
                hjNewRow["Shift"] = row["Shift"];
                hjRatingTable.Rows.Add(hjNewRow);
            }
            CsvFile.WriteToCSV(hjRatingTable, Path.Combine(hydrometRTFs, cbtt + "_shift.csv"), false);
            CsvFile.WriteToCSV(qRatingTable, Path.Combine(hydrometRTFs, cbtt + ".csv"), false);
        }

        private static double Round(object p)
        {

            double rval  =Convert.ToDouble(p);

            rval = Math.Round(rval, 2);

            return rval;

        }

        private static TextFile GetRDBTableFromFile(string cbtt, string ratingTablePath)
        {
            string fileName = Path.Combine(ratingTablePath, cbtt + ".rdb");
            if (!File.Exists(fileName))
            {
                return null;
            }
            else
            {
                return new TextFile(fileName);
            }
        }

        private static void SendEmail(string subject, string body, List<string> attachments, List<string> attachmentRecipientList)
        {
            MailMessage msg = new MailMessage();
            msg.IsBodyHtml = true;
            msg.To.Add(ConfigurationManager.AppSettings["rating_email_to"]);
            if (attachmentRecipientList.Count > 0)
            {
                var x = String.Join(",", attachmentRecipientList);
                msg.To.Add(x);
            }
            msg.From = new MailAddress(ConfigurationManager.AppSettings["rating_email_reply"]);
            msg.Subject = subject;
            msg.Body = body;
            // Add attachments
            System.Net.Mail.Attachment attachment;
            foreach (var item in attachments)
            {
                attachment = new System.Net.Mail.Attachment(item);
                msg.Attachments.Add(attachment);
            }
            SmtpClient c = new System.Net.Mail.SmtpClient();
            c.Host = ConfigurationManager.AppSettings["smtp"];
            c.Send(msg);

            Logger.WriteLine("mail server " + c.Host);
            Logger.WriteLine("to : " + msg.To.ToString());
            Logger.WriteLine("from : " + msg.From.Address);
            Logger.WriteLine("message sent ");
            Logger.WriteLine(body);
        }

    }
}
