using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using Reclamation.Core;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.TimeSeries;
using System.Globalization;

namespace USFOShifts
{
    class IdwrShifts
    {

        static void Main(string[] args)
        
        {
            if( args.Length != 2)
            {
                Console.WriteLine("usage: USFOShifts shift.csv oldshift.csv");
                return;
            }
            List<string> recipients = new List<string>();

            string idwrFile = "shifts.html";
            string cleanFile = args[0];
            string oldFile = args[1];

            string[] cbtt = File.ReadAllLines("site_list.txt");


            //would store the old csv file in the attic and check it against yesterdays shifts
            if (File.Exists(cleanFile))
            {
                var str = File.ReadAllText(cleanFile);
                File.WriteAllText(oldFile, str);
                File.Delete(cleanFile);
            }

            // This is for testing we would get a new html each time we check for a new shift
            //if ( !File.Exists(idwrFile))
             Web.GetFile("http://www.waterdistrict1.com/SHIFTS.htm", idwrFile);
            
            string html = File.ReadAllText(idwrFile);
            Console.WriteLine("input html is " + html.Length + " chars");
            html = Web.CleanHtml(html);
            File.WriteAllText("stage1.txt",html);
            html = ConvertHtmlTableToCsv(html);
            html = ConvertCSVToShiftFormat(html, cbtt);

            File.WriteAllText(cleanFile, html);
            Console.WriteLine("cleaned html is " + html.Length + " chars");
            Console.WriteLine(cleanFile);
            
            //Compare files and add shift into pisces
            var csvNew = new CsvFile(cleanFile, CsvFile.FieldTypes.AutoDetect);
            CsvFile csvOld;
            if (!File.Exists(oldFile))
            {
                var tmp = new List<string>();
                var x = File.ReadAllLines(cleanFile);
                tmp.Add(x[0]);
                File.WriteAllLines(oldFile, tmp.ToArray());
            }

            csvOld = new CsvFile(oldFile, CsvFile.FieldTypes.AutoDetect);

            string emailMsg = "Updates have been made to the following shifts: ";

            for (int i = 0; i < cbtt.Length; i++)
            {
                Console.WriteLine("cbtt='"+cbtt[i]+"'");
                var tblNew = DataTableUtility.Select(csvNew, "cbtt='" + cbtt[i] + "'", "date_measured");
                var tblOld = DataTableUtility.Select(csvOld, "cbtt='" + cbtt[i] + "'", "date_measured");
                if(tblNew.Rows.Count > 0)
                {
                    var shftNew = tblNew.Rows[tblNew.Rows.Count - 1]["shift"].ToString();
                    var dateMeasured = tblNew.Rows[tblNew.Rows.Count - 1]["date_measured"].ToString();
                    double? discharge = null;
                    var q = 0.0;
                    if (double.TryParse(tblNew.Rows[tblNew.Rows.Count - 1]["discharge"].ToString(), out q))
                        discharge = q;

                    var gh1 = 0.0;
                    double? gh = null;
                    if (double.TryParse(tblNew.Rows[tblNew.Rows.Count - 1]["stage"].ToString(), out gh1))
                        gh = gh1;

                    if (tblOld.Rows.Count > 0)
                    {
                        var shftOld = tblOld.Rows[tblOld.Rows.Count - 1]["shift"].ToString();
                        if (shftNew != shftOld && shftNew != "")
                        {
                            emailMsg = InsertShift(cbtt, emailMsg, i, shftNew, dateMeasured, discharge, gh);
                        }
                    }
                    else if (shftNew != "")
                    {
                        emailMsg = InsertShift(cbtt, emailMsg, i, shftNew, dateMeasured, discharge, gh);

                        //InsertShiftToPostgres(cbtt[i], "ch", Convert.ToDouble(shftNew), dateMeasured, discharge, gh);
                        //emailMsg = emailMsg + cbtt[i] + " applied a shift of " + shftNew + ", ";
                    }

                }
            }
            if (emailMsg.Contains("applied"))
            {
                // check who needs to be included on email
                if (emailMsg.Contains("MIII") || emailMsg.Contains("MLCI") || emailMsg.Contains("TCNI"))
                {
                    recipients.Add("MILNER@tfcanal.com");
                }

                if (emailMsg.Contains("NMCI"))
                {
                    recipients.Add("jbwatermaster@gmail.com");
                }

                if (emailMsg.Contains("SMCI"))
                {
                    recipients.Add("bidwater4@gmail.com");
                }

                Console.WriteLine("found shifts. Sending email ");
                SendEmail("IDWR Shift Update", emailMsg, recipients);
            }
            else
            {
                Console.WriteLine("No shift changes found");
            }
        }

        private static string InsertShift(string[] cbtt, string emailMsg, int i, string shftNew, string dateMeasured, double? discharge, double? gh)
        {
            double x = 0;
            //x = Convert.ToDouble(shftNew);
            if (double.TryParse(shftNew, out x))
            {
                InsertShiftToPostgres(cbtt[i], "ch", x, dateMeasured, discharge, gh);
                emailMsg = emailMsg + cbtt[i] + " applied a shift of " + shftNew + ", ";
            }
            else
            {
                emailMsg = emailMsg + cbtt[i] + " Error reading Shift " + shftNew + ", ";
            }

            return emailMsg;
        }

        private static string ConvertCSVToShiftFormat(string html, string[] cbtt)
        {



            /*
             *  DATE  ,    GAGE HT  ,    CFS  ,    SHIFT  , 
         4/18/2015  ,    1.04  ,    152.16  ,    +0.29  , 
         4/21/2015  ,    1.07  ,    151.5  ,    +0.27  , 
         5/15/2015  ,    1.56  ,    211.26  ,    +0.11  , 
         6/16/2015  ,    1.77  ,    118.36  ,    -0.64  , 
         7/25/2015  ,    1.67  ,    151.3  ,    -0.35  , 
       ,  ,  ,  , 
       ,  ,  ,  , 
             */
            string[] CRLF = { "\r\n" };
            string[] lines = html.Split(CRLF, StringSplitOptions.None);
            string cleanFile = "cbtt,pcode,date_measured,discharge,stage,shift\r\n";


            var tf = new TextFile(html.Split(new char[]{'\n','\r'}, StringSplitOptions.RemoveEmptyEntries));
            for (int i = 0; i < cbtt.Length; i++)
            {
                var idx = tf.IndexOf(cbtt[i]);
                
                if( idx >=0)
                {
                   var idxDate = tf.IndexOf("DATE", idx);
                    if( idxDate > idx+5)// date should be within 5 lines of cbtt
                    {
                        Console.WriteLine("Error: did not find DATE with cbtt ="+cbtt);
                        continue;
                    }
                    // now parse data until it runs out
                    for (int j = idxDate+1; j < tf.Length; j++)
                    {
                        DateTime t;
                        var tokens = tf[j].Split(',');
                        if( tokens.Length < 4)
                            break;
                        if( !DateTime.TryParseExact(tokens[0].Trim(), "M/d/yyyy", CultureInfo.InvariantCulture,
                       DateTimeStyles.None, out t) )
                            break;

                        var x = cbtt[i] + ",CH," + t.ToShortDateString() + "," + tokens[2].Trim() + "," + tokens[1].Trim() + "," + tokens[3].Trim();
                        cleanFile += x + "\r\n";
                        Console.WriteLine(x);

                    }
                }
                else
                {
                    Console.WriteLine("Error: did not find "+cbtt[i]);
                }

            }

            return cleanFile;

        }



        /// <summary>
        /// cleanup and make csv between <table> and </table> tags
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private static string ConvertHtmlTableToCsv(string html)
        {
            var mc = Regex.Matches(html, "<table(.*?)>.*?</table>", RegexOptions.Singleline);
            //
            foreach (Match m in mc)
            {// format table one line per table row <tr>
                string t = m.Value;
                t = t.Replace("\r\n", "");
                t = t.Replace("</tr>", "\r\n");
                t = t.Replace("<tr>", "");
                t = ReplaceWithExpression(t, @"</td>", ",");
                t = t.Replace("<p>", "");
                t = t.Replace("</p>", "");
                t = t.Replace("<b>", "");
                t = t.Replace("<td>", "");
                t = t.Replace("</b>", "");
                t = t.Replace("<table>", "\n");
                t = t.Replace("</table>", "\n");
                t = t.Replace("&nbsp;", "");

                html = html.Replace(m.Value, t);
            }
            return html;
        }

        private static string ReplaceWithExpression(string html, string s, string replace)
        {
            RegexOptions o = RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant;
            html = Regex.Replace(html, s, replace, o);
            return html;
        }

        //private static void EnterShiftToPisces(string cbtt,string pcode,double shift)
        //{
        //    var svr = PostgreSQL.GetPostgresServer();
        //    TimeSeriesName tn = new TimeSeriesName(cbtt.ToLower()+"_"+pcode.ToLower(),"instant");
        //    TimeSeriesDatabaseDataSet.seriespropertiesDataTable.Set("shift", shift.ToString(), tn, svr);
        //    return;
        //}

        private static void InsertShiftToPostgres(string cbtt, string pcode, double shift, string dateMeasured, 
            double? discharge, double? gh)
        {
            var svr = PostgreSQL.GetPostgresServer("timeseries");
            //enter shift to shift db

            //string sql = "insert into ";

            //ds.insertshift(cbtt.ToUpper().Trim(), pcode.ToUpper().Trim(),
            //    Convert.ToDateTime(dateMeasured), discharge,
            //    gh, shift, "Shift Entered by USFOShifts Program", DateTime.Now);

            //enter shift to pisces db
           
            TimeSeriesName tn = new TimeSeriesName(cbtt.ToLower() + "_" + pcode.ToLower(), "instant");
            TimeSeriesDatabaseDataSet.seriespropertiesDataTable.Set("shift", shift.ToString(), tn, svr);
            return;
        }

        private static void SendEmail(string subject, string body, List<string> attachmentRecipientList)
        {
            MailMessage msg = new MailMessage();
            msg.IsBodyHtml = true;
            msg.To.Add(ConfigurationManager.AppSettings["email_to"]);
            if (attachmentRecipientList.Count > 0)
            {
                var x = String.Join(",", attachmentRecipientList);
                msg.To.Add(x);
            }
            msg.From = new MailAddress(ConfigurationManager.AppSettings["email_reply"]);
            msg.Subject = subject;
            msg.Body = body;

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
