using System;
using Reclamation.Core;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Configuration;
using HydrometTools.ssh;
namespace Reclamation.TimeSeries.Hydromet
{
     static class HydrometEditsVMS
    {

        public static string HydrometHostAddress
            
        {
         get {
             HydrometHost svr = HydrometInfoUtility.HydrometServerFromPreferences();

             if (svr == HydrometHost.PN || svr == HydrometHost.PNLinux)
                 return ConfigurationManager.AppSettings["BoiseHydrometAddress"];
             else
                 if (svr == HydrometHost.Yakima)
                     return ConfigurationManager.AppSettings["YakimaHydrometAddress"];
                 else if (svr == HydrometHost.GreatPlains)
                     return ConfigurationManager.AppSettings["BillingsHydrometAddress"]; 

             throw new NotImplementedException(svr.ToString());
         }
        }



        /// <summary>
        /// Runs multiple Archive commands in a single batch
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="commands"></param>
        /// <returns></returns>
        public static string RunArchiveCommands(string user, string password, string[] commands)
        {
            //interpret/nodebug acm 2010Jun14 mck fb
            string rval = "";
             
            var tmpFile = FileUtility.GetTempFileName(".txt");
            TextFile tf = new TextFile(tmpFile);

            foreach (var item in commands)
            {
                tf.Add(item);
                rval += "\n" + item;
            }

            tf.SaveAsVms(tf.FileName);
            string t = DateTime.Now.ToString("MMMdyyyyHHmmss").ToLower();
            string remoteFile = "huser1:[edits]run_archiver_"+user+t+".com";
            string unixRemoteFile = VmsToUnixPath(remoteFile);
            rval += "\n" + SendFileAndRunCommand(user, password, tmpFile, unixRemoteFile, "@" + remoteFile);
            return rval;
        }

      


        public static string RunArchiver(string user, string password, string[] cbtt,
    string pcode, DateTime t1, DateTime t2,bool previewOnly)
        {
            string tmpFile = FileUtility.GetTempFileName(".com"); // create script.
            TextFile tf = new TextFile(tmpFile);

            for (int i = 0; i < cbtt.Length; i++)
            {

                if (cbtt[i].Trim() == "" || Regex.IsMatch(cbtt[i], "[^a-zA-z0-9]"))
                {
                    return "Error: invalid cbtt";
                }
                if (Regex.IsMatch(pcode, "[^a-zA-z0-9\\.]"))
                {
                    return "Error: invalid character in pcode";
                }

                if (pcode.Trim() == "")
                {
                    return "Error: empty pcode not allowed!  ";
                }


                if (t2 < t1)
                {
                    return "Error: Invalid Dates -- ending date is less than the beginning date";
                }


                tf.Add("$! -- Archiver script --- ");
                AddVmsScriptHeader(user, tf);
                /*
                 * interpret/nodebug acm 2010Jun14 mck fb
                 * interpret/nodebug acm 2010Jun15 mck fb
                 * 
                 * */
                DateTime t = t1.Date;
                if (pcode == "ALL")
                    pcode = "";

              
                

                while (t <= t2.Date)
                {
                    tf.Add(DayFiles.ArchiveCommand(cbtt[i], pcode, t));
                    t = t.AddDays(1);
                }
                //tf.Add("$ Mail/subject=\"Archiver data has run for cbtt=[" + cbtt + "]  \" run_archiver.com ktarbet");

            }

            if (previewOnly)
            {
                return String.Join("\n", tf.FileData);
            }
            else
            {
                tf.SaveAsVms(tf.FileName);
                string t = DateTime.Now.ToString("MMMdyyyyHHmmss").ToLower();
                string remoteFile = "huser1:[edits]run_archiver_" + user +t+ ".com";
                string unixRemoteFile = VmsToUnixPath(remoteFile);
                string rval = SendFileAndRunCommand(user, password, tmpFile, unixRemoteFile, "@" + remoteFile);
                return rval;
            }
        }


        public static string RunRawData(string user, string password, string cbtt, DateTime t1, DateTime t2)
        {
            if (cbtt.Trim() == "" || Regex.IsMatch(cbtt, "[^a-zA-z0-9]"))
            {
                return "Error: invalid cbtt";
            }

            if (t2 <= t1)
            {
                return "Error: Invalid Dates -- ending date must be greater than beginning";
            }

            string tmpFile = FileUtility.GetTempFileName(".com"); // create script.
            TextFile tf = new TextFile(tmpFile);
            CreateRawDataCommands(user, cbtt, t1, t2,tf);
            //PNHYD0$ mail/subject="karl test" get_all_gstable.com ktarbet

          //  tf.Add("$ Mail/subject=\"Raw data has run for cbtt=["+cbtt + "]  \" run_rawdata.com ktarbet");

            tf.SaveAsVms(tf.FileName);
            string t = DateTime.Now.ToString("MMMdyyyyHHmmss").ToLower();
            string remoteFile = "huser1:[edits]run_rawdata_"+user+t+".com";
            string unixRemoteFile = VmsToUnixPath(remoteFile);
            string rval = SendFileAndRunCommand(user, password, tmpFile, unixRemoteFile, "@" + remoteFile);
            return rval;
        }

        private static void CreateRawDataCommands(string user, string cbtt, DateTime t1,DateTime t2, TextFile tf)
        {
            /*
            day=2010Jun14
            g/00:00,23:59/route=huser1:[jdoty]route.dat/brief mck
            day=2010Jun15
            g/a/route=huser1:[jdoty]route.dat/brief mck
            day=2010Jun16
            */
            bool singleDay = t1.Date == t2.Date;

            tf.Add("$! -- rawdata script --- ");
            AddVmsScriptHeader(user, tf);
            
            tf.Add("$ rawdata");
            tf.Add("day=" + t1.ToString("yyyyMMMdd"));
            if( singleDay) 
            {
               tf.Add("g/" + t1.ToString("HH") + ":" + t1.ToString("mm") + ","
                        + t2.ToString("HH") + ":" + t2.ToString("mm") +"/route=huser1:[edits]route.dat/brief " + cbtt);
            }
            else // multi day
            {
             tf.Add("g/" + t1.ToString("HH") + ":" + t1.ToString("mm") + ",23:59/route=huser1:[edits]route.dat/brief " + cbtt);
             DateTime t = t1.Date.AddDays(1);
             while (t <= t2.Date)
             {
                 tf.Add("day=" + t.ToString("yyyyMMMdd"));

                 if (t.Date == t2.Date)
                 {// final value. look at hour and minute.
                     tf.Add("g/00:00," + t2.ToString("HH") + ":" + t2.ToString("mm") + "/route=huser1:[edits]route.dat/brief " + cbtt);
                 }
                 else
                 {
                     tf.Add("g/all/route=huser1:[edits]route.dat/brief " + cbtt);
                 }
                 t = t.Date.AddDays(1);
              
             }

            }

            tf.Add("$ Exit");
        }

        public static string RunRatingTableMath(string user, string password, string cbtt, string pcodeIn, 
            string pcodeOut, DateTime t1, DateTime t2, bool ace=false)
        {
            if (cbtt.Trim() == "" || Regex.IsMatch(cbtt, "[^a-zA-z0-9]")
                || pcodeIn.Trim() == "" || Regex.IsMatch(pcodeIn, "[^a-zA-z0-9]")
                || pcodeOut.Trim() == "" || Regex.IsMatch(pcodeOut, "[^a-zA-z0-9]"))
            {
                return "Error: invalid cbtt or pcode";
            }

            if (t2 <= t1)
            {
                return "Error: Invalid Dates (%rating) -- ending date must be greater than beginning";
            }

            string tmpFile_dfp = FileUtility.GetTempFileName(".dfp"); // create script.
            string tmpFile_com = FileUtility.GetTempFileName(".com");
            TextFile tf_dfp = new TextFile(tmpFile_dfp);
            TextFile tf_com = new TextFile(tmpFile_com);
            
            CreateRatingMathCommands(user, cbtt, pcodeIn, pcodeOut, t1, t2, tf_dfp,ace);

            string t = DateTime.Now.ToString("MMMdyyyyHHmmss").ToLower();
            string remoteFile_dfp = "huser1:[edits]math_cmds_" + user +t+ ".dfp";
            string remoteFile_com = "huser1:[edits]run_math_" + user +t+ ".com";
            string unixRemoteFile_dfp = VmsToUnixPath(remoteFile_dfp);
            string unixRemoteFile_com = VmsToUnixPath(remoteFile_com);

            tf_com.Add("$! -- Archiver script --- ");

            AddVmsScriptHeader(user, tf_com);
            tf_com.Add("$day");
            tf_com.Add("@" +remoteFile_dfp);
            
            tf_dfp.SaveAsVms(tf_dfp.FileName);
            tf_com.SaveAsVms(tf_com.FileName);



            if (!SendFile(user, password, tmpFile_dfp, unixRemoteFile_dfp))
            {
                return "Error sending file to server '"+remoteFile_com +"'";
            }

            string rval = SendFileAndRunCommand(user, password, tmpFile_com, unixRemoteFile_com, "@" + remoteFile_com);
            return rval;
        }

        private static void AddVmsScriptHeader(string user, TextFile tf)
        {
            string un = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
            tf.Add("$! generated by HydrometTools " + DateTime.Now.ToString());
            tf.Add("$! username: " + un + " " + user);
            tf.Add("$! ----------------------------------");
            tf.Add("$interpret:== $SUTRON$:[rtcm]rtcm");
         HydrometHost svr = HydrometInfoUtility.HydrometServerFromPreferences();
         if (svr == HydrometHost.PN || svr == HydrometHost.Yakima)
         {
          tf.Add("$set process/priv=syslck");
         }
            tf.Add("$DAY:== $SUTRON$:[DAYFILE]DAYFILE");
        }

        private static void CreateRatingMathCommands(string user, string cbtt, string pcodeIn, 
            string pcodeOut, DateTime t1, DateTime t2, TextFile tf, bool ace)
        {
            /*
            day= 2010Jun14
            g/00:00, 23:59/fb,af mck
            math/fb,af
            mck/af:=%rating(mck/fb)
            exit
            update
            clear

            day= 2010Jun15
            g/a/fb,af mck
            math/fb,af
            mck/af:=%rating(mck/fb)
            exit
            update
            clear
             * 
             * 
             * === GP Region has different column names (instead of AF using ACE in rating table)
            day= 2010Jun15
            g/a/fb,af gibr
            math/fb,af
            gibr/ace:=%rating(gibr/fb)
            gibr/af :=gibr/ace
            remove gibr/ace
            exit
            update
            clear
             * 
            */
            bool singleDay = t1.Date == t2.Date;

            
            tf.Add("day=" + t1.ToString("yyyyMMMdd"));
            if (singleDay)
            {
                tf.Add("g/" + t1.ToString("HH") + ":" + t1.ToString("mm") + ","
                         + t2.ToString("HH") + ":" + t2.ToString("mm") + "/" +pcodeIn+ "," +pcodeOut+ " " + cbtt);
                tf.Add("math/" + pcodeIn + "," + pcodeOut);
                RatingEquation(cbtt, pcodeIn, pcodeOut, tf,ace);

                tf.Add("exit");
                tf.Add("update");
                tf.Add("clear");
            }
            else // multi day
            {
                tf.Add("g/" + t1.ToString("HH") + ":" + t1.ToString("mm") + ",23:59/" + pcodeIn + "," + pcodeOut + " " + cbtt);
                tf.Add("math/" + pcodeIn + "," + pcodeOut);
                RatingEquation(cbtt, pcodeIn, pcodeOut, tf,ace);
                //tf.Add(cbtt + "/" + pcodeOut + ":=%rating(" + cbtt + "/" + pcodeIn + ")");
                tf.Add("exit");
                tf.Add("update");
                tf.Add("clear");
                DateTime t = t1.Date.AddDays(1);
                while (t <= t2.Date)
                {
                    tf.Add("day=" + t.ToString("yyyyMMMdd"));

                    if (t.Date == t2.Date)
                    {// final value. look at hour and minute.
                        tf.Add("g/00:00," + t2.ToString("HH") + ":" + t2.ToString("mm") + "/" + pcodeIn + "," + pcodeOut + " " + cbtt);
                    }
                    else
                    {
                        tf.Add("g/a/" + pcodeIn + "," + pcodeOut + " " + cbtt);
                    }
                    tf.Add("math/" + pcodeIn + "," + pcodeOut);
                    RatingEquation(cbtt, pcodeIn, pcodeOut, tf,ace);
                    tf.Add("exit");
                    tf.Add("update");
                    tf.Add("clear");
                    t = t.Date.AddDays(1);
                }
            }
        }

        private static void RatingEquation(string cbtt, string pcodeIn, string pcodeOut, TextFile tf, bool ace)
        {

            if (HydrometInfoUtility.HydrometServerFromPreferences() == HydrometHost.GreatPlains
                    && pcodeIn.ToLower().Trim() == "fb" && ace)
            {
                tf.Add(cbtt + "/" + "ace" + ":=%rating(" + cbtt + "/" + pcodeIn + ")");
                tf.Add(cbtt + "/" + pcodeOut + ":=" + cbtt + "/ace");
                tf.Add("remove " + cbtt + "/ace");
            }
            else
            {
                tf.Add(cbtt + "/" + pcodeOut + ":=%rating(" + cbtt + "/" + pcodeIn + ")");
            }
        }

        public static string SaveDailyData(string user, string password, string localFile,
            string remoteFile, bool overwrite = false, bool checkDayfileBuffer = true, bool interactive = true)
        {
            string rmtFile = VmsToUnixPath(remoteFile);
            if( overwrite)
                return SendFileAndRunCommand(user, password, localFile, rmtFile, "Arcimport " + remoteFile + " overwrite",checkDayfileBuffer,interactive);
            else
            return SendFileAndRunCommand(user, password, localFile, rmtFile, "Arcimport "+remoteFile,checkDayfileBuffer,interactive);
        }
        public static string SaveInstantData(string user, string password, string localFile, string remoteFile, bool interactive=true)
        {
            string rmtFile = VmsToUnixPath(remoteFile);
            return SendFileAndRunCommand(user, password, localFile, rmtFile, "dayflag "+remoteFile,true,interactive);
        }

        public static string RunMpollImport(string user, string password, string localFile, string remoteFile)
        {
            string rmtFile = VmsToUnixPath(remoteFile);
            return SendFileAndRunCommand(user, password, localFile, rmtFile, "mpimport " + remoteFile,false);
        }

        public static string SendFileAndRunCommand(string user, string password, string localFile,
            string remoteFile, string command = "", bool checkDayfileBuffer = true, bool interactive = true)
        {
            try
            {
                // wait for any pending procesing.
                bool ready = true; // no pending jobs

                if( checkDayfileBuffer)
                   ready = EmptyBuffer(user,password,interactive); 

               if (  ready && SendFile(user,password,localFile,remoteFile))
                {
                    Logger.WriteLine("file copy was sucessful ", "ui");
                    
                    if (command != "")
                    {
                        Logger.WriteLine("Running command '" + command + "'", "ui");
                        string rval = Utility.RunCommand(HydrometHostAddress, user, password, @"\$", command);
                        return rval;
                    }
                    Logger.WriteLine("done.","ui");
                    return "";
                }
                else
                {
                    return "Error copying file to server. Check permissions and path to server "+remoteFile;
                }
            }
            catch (Exception ex)
            {
                if( interactive)
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                Logger.WriteLine(ex.Message);
                return ex.Message;
            }
            //Logger.WriteLine("Completed ", "ui");
            //return "";
        }
        
        /// <summary>
        /// check the sutron buffer for pending jobs 
        /// that may interfere with work.  Wait a few seconds if needed
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private static bool EmptyBuffer(string user, string password, bool interactive = true)
        {

            string s = "";
          for (int i = 0; i < 500; i++)
          {
              Logger.WriteLine("Checking if server buffer is busy "+i.ToString());
              Application.DoEvents();
              s = Utility.RunCommand(HydrometHostAddress, user, password, @"\$", "dir DMSSYS:[DMS_V4.DMS402.mbxbuf]*.buf");
              if( s.IndexOf("%DIRECT-W-NOFILES, no files found") >=0)
                  return true;

              Logger.WriteLine("result of shobuf = '" + s + "'");
              Logger.WriteLine("Server is busy:  Waiting for 10 seconds");
              HydrometTools.ssh.Utility.Close(); // hope to fix false busy bug.. (lost connection)
              System.Threading.Thread.Sleep(10000);
              

              if (interactive)
              {
                  var result = MessageBox.Show("Click yes continue waiting?", "The server is too busy to perform the command", MessageBoxButtons.YesNo);
                  if (result == DialogResult.No)
                  {
                      //return false;
                      throw new Exception("Command canceled ");
                  }
              }
          }

           if( interactive)
                    System.Windows.Forms.MessageBox.Show("ERROR:  Buffer is busy please try later. "+s);

           Logger.WriteLine("Error: giving up because after 500 tries the server is still busy");
          return false;
        }

        private static bool SendFile(string user, string password, string localFile, string remoteFile)
        {
            try
            {
                Logger.WriteLine("copying " + localFile + " to " + remoteFile, "ui");
                SecureCopy.CopyTo(user, HydrometHostAddress, password, localFile, remoteFile);
                return true;
            }
            catch (Exception ex)
            {
                Logger.WriteLine(ex.Message, "ui");
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return false;
            }
        }

        public static string VmsToUnixPath(string remoteFile)
        {
            // convert huser2:[ktarbet.tmp]edits.txt
            // to /huser2/ktarbet/tmp/edits.txt

           var m = Regex.Match(remoteFile,@"(?<path>.+\:\[.{1,256}\])(?<filename>.{1,50})");
            if( m.Success)
            {
                string path = m.Groups["path"].Value;
                string filename = m.Groups["filename"].Value;

                path = "/"+path.Replace(":[", "/");
                path = path.Replace("]", "/");
                path = path.Replace(".", "/");

                
                return path + "" + filename;
            }

            return remoteFile;
        }



       

        /// <summary>
        /// Inserts a constant value many times into dayfiles
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="t1">starting date/time</param>
        /// <param name="t2">ending date/time</param>
        /// <param name="value">value to insert</param>
        /// <param name="increment">minutes between values</param>
        /// <returns></returns>
        public static string InsertDayFileValue(string user, string password, DateTime t1, 
            DateTime t2,string cbtt, string pcode, double value, int increment)
        {
            
            DateTime t = t1;
            if (t2 < t1)
                return "Error dates out of order";

            TimeSpan ts = new TimeSpan(t2.Ticks - t1.Ticks);

            if (ts.Days > 365)
            {

                if (System.Windows.Forms.MessageBox.Show("Warning.  A large amount of data will be inserted. Is this OK?", "Continue?", System.Windows.Forms.MessageBoxButtons.OKCancel)
                    != System.Windows.Forms.DialogResult.OK)
                {
                    return "inserting data was canceled";
                }

            }
            string fn = FileUtility.GetTempFileName(".txt");
            StreamWriter output = new StreamWriter(fn);

            output.WriteLine("yyyyMMMdd hhmm cbtt     PC        NewValue   OldValue   Flag User:"+user);
            do
            {
                string flagCode = "-03";// manually entered data
                double missing = 998877.0;
              string str = t.ToString("yyyyMMMdd HHmm").ToUpper()
                            + " " + cbtt.ToUpper().PadRight(8)
                            + " " + pcode.ToUpper().Trim().PadRight(9)
                            + " " + value.ToString("F2").PadRight(10)
                            + " " + missing.ToString("F2").PadRight(10)
                            + " " + flagCode.ToString().PadRight(3);
                        output.WriteLine(str);
                t = t.AddMinutes(increment);
            } while (t<=t2);


            output.Close();
            string remoteFilename = HydrometDataUtility.CreateRemoteFileName(user, TimeInterval.Irregular);
            return SaveInstantData(user, password, fn, remoteFilename);
        }


        public static string UpdateShift(string user, string password, string cbtt, 
            string pcode, double shift)
        {
            string command = "input_shift "+cbtt+" " +pcode + " " +shift.ToString("F2");
            Logger.WriteLine("Running command '" + command + "'", "ui");
            string rval = Utility.RunCommand(HydrometHostAddress, user, password, @"\$", command);
            return rval;

        }
          
    }
}
