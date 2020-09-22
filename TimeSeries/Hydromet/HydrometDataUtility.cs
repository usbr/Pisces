using Reclamation.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
namespace Reclamation.TimeSeries.Hydromet
{



    /// <summary>
    /// HydrometDataUtility provides queries for multiple sites and parameters.
    /// used by HydrometTools.exe
    /// </summary>
    public class HydrometDataUtility
	{
        public static int yearNow = DateTime.Now.Year - 1;
        /// <summary>
        /// Start of 30 year averaging period Ex:1990-10-1 (increments on 1/1/20X1)
        /// </summary>
        public static DateTime T1Thirty = new DateTime((yearNow - 30 - (yearNow - 30) % 10) + 1, 10, 1);

        /// <summary>
        /// End of 30 year averaging period Ex:2020-9-30 (increments on 1/1/20X1)
        /// </summary>
        public static DateTime T2Thirty = new DateTime(yearNow - yearNow % 10, 9, 30);

        public static DataTable DayFilesTable(HydrometHost svr, string query, DateTime t1, DateTime t2, int back = 0, int interval = 0)
        {
            query = HydrometInfoUtility.ExpandQuery(query, TimeInterval.Irregular);

            string cgiUrl = ReclamationURL.GetUrlToDataCgi(svr, TimeInterval.Irregular);

            var rval = Table(cgiUrl, query, t1, t2, back);

            if (interval != 0 && rval.Rows.Count > 0)
            {// put nulls in table as needed.
                rval.PrimaryKey = new DataColumn[] { rval.Columns[0] };
                DateTime next = Convert.ToDateTime(rval.Rows[0][0]).AddMinutes(interval); ;
               
                for (int i = 1; i < rval.Rows.Count; i++)
                {
                    DateTime t = Convert.ToDateTime(rval.Rows[i][0]);

                    if (t > next)
                    {// insert new row
                        var row = rval.NewRow();
                        row[0] = next;
                        next = next.AddMinutes(interval);
                        rval.Rows.InsertAt(row, i);
                    }
                    else
                    {
                        next = t.AddMinutes(interval);
                    }
                }
            }
            rval.PrimaryKey = null;
            return rval;
        }


        public static DataTable ArchiveTable(HydrometHost server, string query, DateTime t1, DateTime t2,int back=0)
        {
            
            query =  HydrometInfoUtility.ExpandQuery(query, TimeInterval.Daily);

            string cgiUrl = ReclamationURL.GetUrlToDataCgi(server, TimeInterval.Daily);

            return Table(cgiUrl, query, t1, t2, back);

        }

        public static DataTable MPollTable(HydrometHost server, string query, DateTime t1, DateTime t2)
        {
            
            query = HydrometInfoUtility.ExpandQuery(query, TimeInterval.Monthly);
            string cgiUrl = ReclamationURL.GetUrlToDataCgi(server, TimeInterval.Monthly);
            Logger.WriteLine("url:" + cgiUrl);
            return Table(cgiUrl, query, t1, t2, endOfMonth: true);
        }

        /// <summary>
        /// Gets a table of hydromet data from the web.  The table has a column for each parameter.  I the table is 
        /// instant data a flag column is included to the right of each data column
        /// </summary>
        /// <param name="cgi"></param>
        /// <param name="query"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="back">when greater than zero used instead of t1,t2 (hours (instant) or days(daily) back)</param>
        /// <param name="endOfMonth"></param>
        /// <returns></returns>
        static DataTable Table(string cgi, string query, DateTime t1, DateTime t2, int back = 0, bool endOfMonth = false) 
		{
            //if (t2 > DateTime.Now ) can't do this for 6190 (special 30 yr average year)
              //  t2 = DateTime.Now;

			int colIndex =1;
			string flag = " ";
			string payload = "parameter="+query
				+"&syer="+t1.Year.ToString()
				+"&smnth="+t1.Month.ToString()
				+"&sdy="+t1.Day.ToString()
				+"&eyer="+t2.Year.ToString()
				+"&emnth="+t2.Month.ToString()
				+"&edy="+t2.Day.ToString()
				+"&format=2";

            if (back > 0)
            {
                 payload = "parameter=" + query
                + "&back="+back
                + "&format=2";
            }



			string[] data = Web.GetPage(cgi,payload,HydrometInfoUtility.WebCaching);

            bool hasFlag = cgi.ToLower().IndexOf("webdaycsv") >= 0
                           || cgi.IndexOf("webmpollcsv") >= 0
                           || cgi.IndexOf("monthly") >= 0
                           || cgi.IndexOf("instant") >= 0;


            TextFile tf = new TextFile(data);
            int idx1 = tf.IndexOf("BEGIN DATA");
			int idx2 = tf.IndexOf("END DATA");
            int idxDates = tf.IndexOfRegex(@"(\d{1,2}/\d{1,2}/\d{1,2})|(\s[A-Z]{3}\s+\d{4})"); // find first date

			if( idx1 <0 || idx2 <0 || idx2 == idx1+1 || idxDates <0) 
				return new DataTable("No_data");

            

            string strTitles = "";
            for (int i = idx1+1; i < idxDates; i++)
            {
                strTitles += data[i]; // put wrapped titles back in single line.
            }

			string[] columnNames = strTitles.Split(',');
			
			DataTable tbl = new DataTable("hydromet");

            CreateColumnNames(hasFlag, columnNames, tbl);

            for (int i = idxDates; i < idx2; i++)
            {


                JoinWrappedLines(data, i);

                string line = data[i];

                if (line == "")
                    continue;

                string[] parts = line.Trim().Split(',');
                if (parts.Length == 0)
                    continue;
                DataRow row = tbl.NewRow();

                DateTime date = DateTime.Now;


                //if (!DateTime.TryParse(parts[0], out date))
                if (!LinuxUtility.TryParseDateTime(parts[0], out date))
                {
                    Logger.WriteLine("Error parsing date " + parts[0]);
                    break;
                }

                if (endOfMonth)
                    date = date.Date.EndOfMonth();

                row[0] = date;
                for (int j = 1; j < parts.Length; j++)
                {
                    string strValue = parts[j];

                    if (strValue.IndexOf("NO RECORD") >= 0
                        || strValue.IndexOf("MISSING") >= 0
                        || strValue.IndexOf("998877.00") >= 0
                        || strValue.Trim() == ""
                        || strValue.Trim() == PointFlag.Missing
                        )
                    {
                        row[colIndex] = System.DBNull.Value;
                        if (hasFlag)
                        {
                            row[colIndex + 1] = "";
                            colIndex++;
                        }
                    }
					else
					{
						//Console.WriteLine("str = "+strValue);
                        double val = -999;
						
							int index = strValue.IndexOf(".");
                            string str = strValue;
							
							if(hasFlag && index >=0){
							    str = strValue.Substring(0,index+3);
								if(strValue.Length>str.Length)
									flag = strValue.Substring(str.Length,1);
								else
									flag = " ";
							}
                            
                            if (!double.TryParse(str, out val))
                            {
                                Logger.WriteLine("Error converting " + str + " to a number ");
                            }

                            try
                            {
                                row[colIndex] = val;
                                if (hasFlag)
                                {
                                    row[colIndex + 1] = flag;
                                    colIndex++;
                                }
                            }
                            catch (Exception ex)
                            {

                                Logger.WriteLine(ex.Message);
                            }
						
					}
					colIndex++;
				}
				tbl.Rows.Add(row);
				colIndex=1;
			}

            //DataSet ds = new DataSet();
            //ds.Tables.Add(tbl);
            return tbl;

		}

        private static void CreateColumnNames(bool hasFlag, string[] columnNames, DataTable tbl)
        {
            for (int i = 0; i < columnNames.Length; i++)
            {
                columnNames[i] = columnNames[i].Trim();
                System.Type type = System.Type.GetType("System.DateTime");
                if (i == 0)
                { // DateTime
                    tbl.Columns.Add(columnNames[i], type);
                }
                else
                {
                    type = System.Type.GetType("System.Double");
                    var cn = columnNames[i].Trim();
                    var tokens = cn.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (tokens.Length == 2)
                    {// trim cbtt/pcode pair
                        cn = tokens[0].Trim() + " " + tokens[1].Trim();
                    }

                    tbl.Columns.Add(cn, type);


                    if (hasFlag)
                    {
                        tbl.Columns.Add("Flag" + i, System.Type.GetType("System.String"));
                    }
                }

            }
        }

        /// <summary>
        ///check if we have data wrapped from CGI/server(limit of 1024 record length) 
        ///wrapped data will not have a DateTime in the first column
        /// </summary>
        /// <param name="data"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        internal static void JoinWrappedLines(string[] data, int startIndex)
        {
            //DateTime date;
            // if we have a long line and next line doesn't start with a DateTime
            // we need to combine the lines.

            for (int i = startIndex; i < data.Length-1; i++)
            {
                if (data[startIndex].Length >= 1024 &&
                    !System.Text.RegularExpressions.Regex.IsMatch(data[i+1],@"^\d{1,2}/\d{1,2}/\d{4}") // daily, instant
                    //|| !System.Text.RegularExpressions.Regex.IsMatch(data[i+1],@"^\s[A-Z]{3}\s{2}\d{4}\s{3}\,") 
                    )
                   //!DateTime.TryParse(data[i + 1].Split(',')[0], out date))
                {
                    data[startIndex] = data[startIndex] + data[i + 1];
                    data[i + 1] = "";
                }
                else
                {
                    return;
                }
            }
        }



        public static int WriteArchiveUpdateFile(DataTable tblNew, DataTable tblOld, string outputFilename, out string[] modifiedCbtt, out string[] modifiedPcodes,out DateRange range)
		{

            range = new DateRange();
            DateTime mint =DateTime.MaxValue;
            DateTime maxt = DateTime.MinValue;

            modifiedCbtt = new string[] { };
            modifiedPcodes = new string[] { };
            List<string> cbttList = new List<string>();
            List<string> pcodeList = new List<string>();
			if(tblNew.Rows.Count != tblOld.Rows.Count)
			{
				Logger.WriteLine("Error:  the number of rows in the data has changed. no update file will be written");
				return 0;
			}
			if(tblNew.Columns.Count != tblOld.Columns.Count)
			{
				Logger.WriteLine("Error:  the number of columns in the data has changed. no update file will be written");
				return 0;
			}
			StreamWriter output = new StreamWriter(outputFilename);

			int modifiedCounter = 0;
			// first column is date, other columns are values
			output.WriteLine("MM/dd/yyyy cbtt         PC        NewValue      OldValue ");

			for(int c=1; c<tblOld.Columns.Count; c++)
			{
				string columnName = tblOld.Columns[c].ColumnName;
				string stationName = columnName.Split(' ')[0];
				string pcode = columnName.Split(' ')[1];
               
				
				for(int r=0; r<tblOld.Rows.Count; r++)
				{
					double valNew,valOld;
					if( tblNew.Rows[r][c] != System.DBNull.Value)
						valNew = (double)tblNew.Rows[r][c];
					else
						valNew = 998877;
					if( tblOld.Rows[r][c] != System.DBNull.Value)
						valOld = (double)tblOld.Rows[r][c];
					else
						valOld = 998877;

					DateTime date = (DateTime)tblNew.Rows[r][0];
					
					if( valNew != valOld)
					{
                        if (date < mint)
                            mint = date;

                        if (date > maxt)
                            maxt = date;

                        if (cbttList.IndexOf(stationName) < 0)
                            cbttList.Add(stationName);

                        if (pcodeList.IndexOf(pcode) <0 )
                            pcodeList.Add(pcode);

						System.Globalization.NumberFormatInfo nf = new System.Globalization.NumberFormatInfo();
						nf.NumberDecimalDigits =2;
						output.WriteLine(date.ToString("MM/dd/yyyy")
							+" "+stationName.PadRight(12)
							+" "+pcode.PadRight(9)
							+" "+valNew.ToString("F2").PadRight(13)
							+" "+valOld.ToString("F2").PadRight(13));
						modifiedCounter++;
					}
				}
			}
			output.Close();
            modifiedCbtt = cbttList.ToArray();
            modifiedPcodes = pcodeList.ToArray();
            range = new DateRange(mint, maxt);
			return modifiedCounter;
		}

        

     
        public static int WriteMPollUpdateFile(DataTable tblNew, DataTable tblOld, string outputFilename, out bool mpollPermanentMarkChanged)
        {
            mpollPermanentMarkChanged = false;
            if (tblNew.Rows.Count != tblOld.Rows.Count)
            {
                Logger.WriteLine("Error:  the number of rows in the data has changed. no update file will be written");
                return 0;
            }
            if (tblNew.Columns.Count != tblOld.Columns.Count)
            {
                Logger.WriteLine("Error:  the number of columns in the data has changed. no update file will be written");
                return 0;
            }
            StreamWriter output = new StreamWriter(outputFilename);

            int modifiedCounter = 0;
            // first column is date, other columns are values
            output.WriteLine("cbtt,pc,Year,month,value,flag,oldValue,oldFlag");
            for (int c = 1; c < tblOld.Columns.Count; c += 2)
            {
                string columnName = tblOld.Columns[c].ColumnName;
                var parts = columnName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string stationName = parts[0];
                string pcode = parts[1];

                for (int r = 0; r < tblOld.Rows.Count; r++)
                {
                    double valNew = ReadDouble(tblNew.Rows[r][c]);
                    double valOld = ReadDouble(tblOld.Rows[r][c]);

                    string newFlag = ReadFlag(tblNew.Rows[r][c + 1]);
                    string oldFlag = ReadFlag(tblOld.Rows[r][c + 1]);


                    if (valNew != valOld || newFlag != oldFlag)
                    { // something changed.  write to script.

                        if (HydrometMonthlySeries.IsPermanentMark(oldFlag))
                            mpollPermanentMarkChanged = true;

                        DateTime date = (DateTime)tblNew.Rows[r][0];

                        System.Globalization.NumberFormatInfo nf = new System.Globalization.NumberFormatInfo();
                        nf.NumberDecimalDigits = 2;
                        string str = stationName.Trim() + ","
                       + pcode.Trim() + ","
                       + date.Year.ToString() + ","
                       + date.ToString("MMM").ToUpper() + ","
                       + valNew.ToString("F2") + ","
                       + newFlag+","
                       + valOld +","
                       +oldFlag ;
                        output.WriteLine(str);
                        modifiedCounter++;
                    }

                }
            }
            output.Close();
            return modifiedCounter;
        }

        public static string ReadFlag(object o)
        {
            if (o != System.DBNull.Value)
            {
                return Convert.ToString(o);
            }
            else
            {
                return "";
            }

        }
        public static double ReadDouble(object o)
        {
            double rval = 998877; // missing
            if (o != System.DBNull.Value)
            {
                rval = Convert.ToDouble(o);
            }

            return rval;
        }


        [Obsolete()]
		public static bool AnyChange(DataTable tblNew,DataTable tblOld, int Row, int Column)
		{
			bool change = false;
			string valNew,valOld;
			int c = Column+1;
			for(int Col=Column;Col<=c; Col++) // check value and flag columns
			{
				valNew = tblNew.Rows[Row][Col].ToString();
				valOld = tblOld.Rows[Row][Col].ToString();
				if(valOld=="")
					valOld=" ";
				if(valOld==null)
					valOld="998877";
				if(valNew!=valOld)
				{
					change=true;
					break;
				}
				else if(valNew==valOld)
				{
					change=false;
				}
			}
			return change;
		}

        /// <summary>
        /// Creates remote filename on vms server
        /// </summary>
        /// <param name="user"></param>
        /// <param name="db"></param>
        /// <param name="autoImport">use path to automated importing</param>
        /// <returns></returns>
        public static string CreateRemoteFileName(string user, TimeInterval db,bool autoImport=false)
        {
            string prefix = "huser1:[edits]";

            if( autoImport)
                prefix = "huser1:[edits.import]";

            string t = DateTime.Now.ToString("MMMdyyyyHHmmss");
            string remoteFilename = prefix+"edits_" + user +t+ ".txt";
            if (db == TimeInterval.Monthly)
                remoteFilename = prefix + "month_" + user + t + ".txt";
            if (db == TimeInterval.Irregular)
                remoteFilename = prefix + "instant_" + user + t + ".txt";
            if (db == TimeInterval.Daily)
                remoteFilename = prefix + "daily_" + user + t + ".txt";
            return remoteFilename.ToLower();
        }



	}
}
