using System;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;
using Reclamation.Core;
namespace Reclamation.TimeSeries
{
  /// <summary>
  /// TextSeries represents a text file containing time series
  /// data such as comma seperated (*.csv,*.txt)
  /// </summary>
  public class TextSeries : Series
  {
    private string _filename;
    private string[] file;
    DateTime m_t1 = DateTime.MinValue;
    DateTime m_t2 = DateTime.MaxValue;

   // internal static readonly string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

    //public TextSeries(string fileName, string filterColumnName,
    //     string filterValue, string dateColumnName, string valueColumnName )
    //{

    //}

    /// <summary>
    /// Creates new TextSeries
    /// </summary>
    /// <param name="filename">filename containing timeseries data</param>
    public TextSeries(string filename)
    {
        if( !File.Exists(filename))
        {
            throw new FileNotFoundException(filename);
        }
      _filename = filename;

      this.Source = "TextFile";
      this.Provider = "TextSeries";
      FileInfo fi = new FileInfo(filename);
      this.ConnectionString = "FileName=" + Path.GetFullPath(filename) + ";LastWriteTime=" + fi.LastWriteTime.ToString(DateTimeFormatInstantaneous);
      Name = Path.GetFileName(filename);
      Table.TableName = Path.GetFileName(filename);
    }

    public TextSeries(TimeSeriesDatabase db, Reclamation.TimeSeries.TimeSeriesDatabaseDataSet.SeriesCatalogRow sr):base(db,sr)
    {
    }
    /// <summary>
    /// Updates database if the original source text file still exists and has 
    /// been modified. All series data is replaced with the source file contents
    /// </summary>
    private void Update()
    {
        if (m_db == null)
            return;
        Logger.WriteLine("Checking series " + Name + " (" + ID + ") for updates");
        string dir = Path.GetDirectoryName(m_db.DataSource);
        bool canUpdate= CanUpdateFromFile(ConnectionString, dir );
        if (canUpdate)
        {
            string fileName = ConnectionStringUtility.GetToken(ConnectionString, "FileName", "");
            if (!Path.IsPathRooted(fileName))
            {
                fileName = Path.Combine(dir, fileName);
            }
            Logger.WriteLine("Updating: File has changed " + fileName);
            TextSeries ts = TextSeries.ReadFromFile(fileName);

            ConnectionString = ts.ConnectionString;
            ConnectionString = ConnectionStringUtility.MakeFileNameRelative(ConnectionString, m_db.DataSource);
            m_db.SaveProperties(this); // last write time needs to be updated.
            m_db.SaveTimeSeriesTable(ID, ts, DatabaseSaveOptions.DeleteAllExisting);

        }
    }


    /// <summary>
    /// Reads time series from filename
    /// </summary>
    protected override void ReadCore()
    {

        if (m_db != null)
        {
            Update();
            base.ReadCore(); // read from database
        }
        else
        {
            m_t1 = DateTime.MinValue;
            m_t2 = DateTime.MaxValue;
            file = System.IO.File.ReadAllLines(_filename);
            Parse();
        }
    }

    protected override void ReadCore(DateTime t1, DateTime t2)
      {

          if (m_db != null)// using local database
          {
              Update();
              base.ReadCore(t1,t2);
          }
          else
          {
              m_t1 = t1;
              m_t2 = t2;
              file = System.IO.File.ReadAllLines(_filename);
              Parse();

              //Selection s = new Selection(DateTime.MinValue, t1.AddSeconds(-1),
              //    Math.Min(this).Value - 1, Math.Max(this).Value + 1);
              //this.Delete(s);

          }

      }
    /// <summary>
    /// Reads text file with time series data.
    /// Usgs formats
    /// ------
    /// YYYY-MM-DD
    /// MM/DD/YYYY
    /// YYYY.MM.DD
    /// YYYYMMDD
    /// ---- other formats..
    ///  shef...
    /// 
    /// </summary>
    private void Parse()
    {
        int errorCount = 0; // to improve performance by limiting error messages
      //InitTimeSeries(null,"",TimeInterval.Irregular,
       // false);
      TimeInterval = TimeInterval.Irregular;

      Regex re = FindRegularExpression(file);
       
      if( re == null)
      {
          string msg = "could not determine format of file!";
          Logger.WriteLine(msg);
          Messages.Add(msg);
        return;
      }
      Logger.WriteLine(re.ToString());
      string regexName = re.GroupNameFromNumber(1);

      bool isStevens = (regexName == "stevens");

      Logger.WriteLine(regexName);

      for(int i=0; i<file.Length; i++)
      {
        Match m = re.Match(file[i]);
        
        if( m.Success)
        {
          GroupCollection g = m.Groups;
            DateTime date;
            if (isStevens)
            {
                date = BuildStevensDate(g);
            }
            else
            {
                date = Convert.ToDateTime(g["date"].Value);
            }

            if (date < m_t1 || date > m_t2)
            {
                continue;
            }

          if (IndexOf(date) >= 0)
          {
              if (errorCount < 50)
              {
                  Logger.WriteLine("skipped  duplicate on line index " + i + " '" + file[i] + "' Date = " + date.ToString());
                  Logger.WriteLine("g[\"value\"] =" + g["value"].Value);
                  Messages.Add("skipped  duplicate date '" + file[i] + "'");
              }
              errorCount++;
          }
          else
          {
              double val = Convert.ToDouble(g["value"].Value);
              Add(date, val, "");
          }
        }
        else
        {
            if (errorCount < 50)
            {
                Logger.WriteLine("skipped '" + file[i] + "'");
                Messages.Add("skipped '" + file[i] + "'");
            }
            errorCount++;
        }
      }
      this.TimeInterval = Series.EstimateInterval(this);
        if( errorCount >50)
         Logger.WriteLine("skipped "+ (errorCount -50)+ " messages");
      Logger.WriteLine("Read " + Count + " valid records from " + _filename);
    }

    /// <summary>
    /// Determine File format and return Regular expression for parsing
    /// it. Return null the file format is unknown
    /// </summary>
    /// <returns></returns>
    private Regex FindRegularExpression(string[] tf)
    {
      Regex re = null;
      for(int e = 0; e < expressionList.Length; e++)
      {
        re = new Regex(expressionList[e],RegexOptions.Compiled|RegexOptions.IgnoreCase);
        for(int i=0; i<tf.Length; i++)
        {
          if( re.IsMatch(tf[i]))
          {
              Logger.WriteLine("found file format match");
              Logger.WriteLine(tf[i]);
            Logger.WriteLine("using "+expressionList[e]);
            return re;
          }
        }
      }
      return null;
    }

    private string[] expressionList 
      ={ 
          //#matches  10/1/1927 12:00:00 AM,4800  
         @"^\s*(?<date>\d\d?[/-]\d\d?[/-]\d\d\d\d(?<time>[\s]\d\d?:\d\d?(:\d\d?(\.\d*)?))?[\s](AM|PM))\s*[,\s]\s*\s*(?<value>[+-]?([0-9]*\.?[0-9]+|[0-9]+\.?[0-9]*)([eE][+-]?[0-9]+)?)", 
         //24 hour time
         @"^\s*(?<date>\d\d?[/-]\d\d?[/-]\d\d\d\d(?<time>[\s]\d\d?:\d\d?(:\d\d?(\.\d*)?)?))\s*[,\s]\s*\s*(?<value>[+-]?([0-9]*\.?[0-9]+|[0-9]+\.?[0-9]*)([eE][+-]?[0-9]+)?)" , 

         @"^\s*(?<date>\d\d?[/-]\d\d?[/-]\d\d\d\d"
         +@"(?<time>[\s]\d\d?:\d\d?(:\d\d?(\.\d*)?)?)?)" // TO DO.. 12 hr.. am/pm.
         //+"# comma or space delimited "
         +@"\s*[,\s]\s*"
         // +"# decimal number (from regexlib.com)"
         +@"\s*(?<value>[+-]?([0-9]*\.?[0-9]+|[0-9]+\.?[0-9]*)([eE][+-]?[0-9]+)?)"
         , 

         // matches  yyyy/mm/dd  and 24 hour time
         @"^\s*(?<date>\d\d\d\d[/-]\d\d?[/-]\d\d?"
         +@"(?<time>[\s]\d\d?:\d\d?(:\d\d?(\.\d*)?))?)" // TO DO.. 12 hr.. am/pm.
         //   +"# comma or space delimited "
         +@"\s*[,\s]\s*"
         //   +"# decimal number (from regexlib.com)"
         +@"\s*(?<value>[+-]?([0-9]*\.?[0-9]+|[0-9]+\.?[0-9]*)([eE][+-]?[0-9]+)?)"
         ,
         // matches  MM/dd/yy  optional 24 hour time
         @"^\s*(?<date>\d\d?[/-]\d\d?[/-]\d\d"
         +@"(?<time>[\s]\d\d?:\d\d?(:\d\d?(\.\d*)?)?)?)" // TO DO.. 12 hr.. am/pm.
         //+"# comma or space delimited "
         +@"\s*[,\s]\s*"
         // +"# decimal number (from regexlib.com)"
         +@"\s*(?<value>[+-]?([0-9]*\.?[0-9]+|[0-9]+\.?[0-9]*)([eE][+-]?[0-9]+)?)"

         ,
         @"(?<stevens>^.{29,29}\s*(?<yy>\d{2})(?<month>\d{2})(?<day>\d{2})\s+(?<hour>\d{2})(?<minute>\d{2})\s+(?<value>\d+\.\d{2}))"


			   

       };

    private DateTime BuildStevensDate(GroupCollection g)
    {

        int year = Convert.ToInt32(g["yy"].Value);
        int month = Convert.ToInt32(g["month"].Value);
        int day = Convert.ToInt32(g["day"].Value);
        int hour = Convert.ToInt32(g["hour"].Value);
        int minute = Convert.ToInt32(g["minute"].Value);

        // year is 2 digit.. i.e  95 for 1995 or  05 for 2005.
        // if year < 50  then it is after year 2000 
        // if year >=50  then it is after 1900.
        if (year >= 50)
        {
            year = 1900 + year;
        }
        else
        {
            year = 2000 + year;
        }
        DateTime tmstp = new DateTime(year, month, day, hour, minute, 0);
        return tmstp;

    }


    public static TextSeries ReadFromFile(string fileName)
    {
       TextSeries  s = new TextSeries(fileName);
        s.Read();
        return s;
    }

    /// <summary>
    /// checks LastWriteTime in connection string and compares to 
    /// the FileName in the connection string to see if a update is possible and is needed
    /// </summary>
    /// <param name="s"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static bool CanUpdateFromFile(string connectionString, string dataPath)
    {


        string d = ConnectionStringUtility.GetToken(connectionString, "LastWriteTime","");
        var fileName = ConnectionStringUtility.GetToken(connectionString, "FileName","");

        if( !Path.IsPathRooted(fileName))
        {
            fileName = Path.Combine(dataPath, fileName);
        }

        DateTime t = DateTime.MinValue;

        if (!DateTime.TryParse(d, out t))
        {
            Logger.WriteLine("Update Error: Could not parse LastWriteTime in '" + connectionString + "'");
            return false;
        }



        if (!File.Exists(fileName))
        {
            Logger.WriteLine("Update Error: File does not exist '" + fileName + "'");
            return false;
        }

        DateTime fileTime = File.GetLastWriteTime(fileName);
        fileTime = Convert.ToDateTime(fileTime.ToString(Series.DateTimeFormatInstantaneous));

        bool rval = fileTime != t;
        if (!rval)
        {
            Logger.WriteLine("No update required");
        }

        return rval;
    }


  }
}
