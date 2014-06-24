using System;
using System.IO;
using System.Data;
using NUnit.Framework;
using Reclamation.Core;
using Reclamation.TimeSeries;
using System.Configuration;


namespace Reclamation.TimeSeries
{
	/// <summary>
	/// Summary description for TestData.
	/// </summary>
	public class TestData
	{
        

        public static Series SpecificationMonthValue
        {
            get
            {
                TextSeries s = new TextSeries(DataPath + @"\SpecificationTestData.csv");
                    //"Sheet1", "Date", "MonthValue");
                return s;
            }
        }  


 
    public static string DataPath
    {
      get 
      {
          return Globals.TestDataPath;
      }
    }

        private static string rootDir;
    public static string GetRootDir
    {
        get
        {
            if (rootDir == null)
            {
                rootDir = Path.Combine(Path.Combine(rootDir, ".."), "..");
                rootDir = Path.Combine(rootDir, "tests");
            }

            return rootDir;
        }
    }

        public static Series Empty
        {
            get
            {
                return new Series();
            }
        }

       
        public static Series SouthForkBoise
        {
            get
            {
                Series s = new TextSeries(DataPath + "\\" + "SouthForkOfBoiseNearFeatherville.txt");
                s.Read();
                s.Units = "cfs";
                s.Name = "BRFI_QD";
                s.TimeInterval = TimeInterval.Daily;
                return s;
            }
        }
    public static Series Site68OneFullDayInstantaneous
    {
      get 
      {
        Series s = new TextSeries(DataPath+"\\"+"site68_onefullday.txt");
        s.Read();
        s.Units = "feet";
        s.Name="site68uslev";
        s.TimeInterval = TimeInterval.Irregular;
        return s;
      }
    }
    public static Series LindCouleeWW1DailyAverageStage2004
    {
      get 
      {
        Series s = new TextSeries(DataPath+"\\"+"site68_LindCouleeWasteWay1DailyAverage.csv");
        s.Read();
        s.Units = "feet";
        s.Name="site68uslev";
        s.TimeInterval = TimeInterval.Daily;
        return s;
      }
    }

    public static Series LindCouleeWW1InstantanousStage2004
    {
      get 
      {
        Series s = new TextSeries(DataPath+"\\"+"site68_LindCouleeWasteWay1Stage.csv");
        s.Read();
        s.Units = "feet";
        s.Name="site68uslev";
        s.TimeInterval = TimeInterval.Irregular;
        return s;
      }
    }
    public static Series EL68dDailyAverageStage2004
    {
      get 
      {
       Series s = new TextSeries(DataPath+"\\"+"El686_2004DailyAverageStage.csv");
        s.Read();
        s.Units = "feet";
        s.Name="EL68D ";
        s.TimeInterval = TimeInterval.Daily;
        return s;
      }
    }
    public static Series Banks
    {
      /*
             cbp.Sybase sb = new cbp.Sybase("ktarbet2.pn.usbr.gov","ktarbet","sqlexpress","sb7srv1","cbpscada",false,"");
     string sql =" select tmstp,banks from cbp.site01  where tmstp >='2005-05-16 23:50' "
            +"  and tmstp <= '2005-05-25 01:00' order by tmstp ";
      DataTable t = sb.Table("banks",sql);
      DataSet ds = new DataSet("cbp");
      ds.Tables.Add(t);
        t.DataSet.WriteXml(@"C:\karl\project\Ephrata\TimeSeries\data\banks.xml",XmlWriteMode.WriteSchema);

       * */
      get 
      { 
          DataSet ds = new DataSet();
        
        ds.ReadXml(DataPath+"\\banks.xml");
        DataTable tbl = ds.Tables[0];
         return new Series(tbl,"feet",TimeInterval.Irregular);
      }
    }
		public static Series Simple1Day
		{
			get
			{
				DataTable  input = new DataTable("simple");
				input.Columns.Add("tmstp",typeof(DateTime));
				input.Columns.Add("value",typeof(double));
				input.Rows.Add(new object[]{Convert.ToDateTime("2004-12-02 00:00"),50});
				input.Rows.Add(new object[]{Convert.ToDateTime("2004-12-02 11:00 "),75});
				input.Rows.Add(new object[]{Convert.ToDateTime("2004-12-02 12:00 PM"),10});
				input.Rows.Add(new object[]{Convert.ToDateTime("2004-12-02 13:00"),75});
				input.Rows.Add(new object[]{Convert.ToDateTime("2004-12-03 00:00"),100});
				return new Series(input,"cfs",TimeInterval.Irregular);
			}
		}

    public static Series SimpleEnding2300
    {
      get
      {
        DataTable  input = new DataTable("simple");
        input.Columns.Add("tmstp",typeof(DateTime));
        input.Columns.Add("value",typeof(double));
        input.Rows.Add(new object[]{Convert.ToDateTime("2004-12-02 00:00"),50});
        input.Rows.Add(new object[]{Convert.ToDateTime("2004-12-02 11:00"),50});
        input.Rows.Add(new object[]{Convert.ToDateTime("2004-12-02 12:00 PM"),50});
        input.Rows.Add(new object[]{Convert.ToDateTime("2004-12-02 12:01"),100});
        input.Rows.Add(new object[]{Convert.ToDateTime("2004-12-02 13:00"),100});
        input.Rows.Add(new object[]{Convert.ToDateTime("2004-12-03 23:00"),100});
        return new Series(input,"cfs",TimeInterval.Irregular);
      }
    }
    public static Series Simple2Day
    {
      get
      {
        DataTable  input = new DataTable("simple");
        input.Columns.Add("tmstp",typeof(DateTime));
        input.Columns.Add("value",typeof(double));
        input.Rows.Add(new object[]{Convert.ToDateTime("2004-12-02 00:00"),50});
        input.Rows.Add(new object[]{Convert.ToDateTime("2004-12-02 11:00"),50});
        input.Rows.Add(new object[]{Convert.ToDateTime("2004-12-02 12:00 PM"),50});
        input.Rows.Add(new object[]{Convert.ToDateTime("2004-12-02 12:01"),100});
        input.Rows.Add(new object[]{Convert.ToDateTime("2004-12-02 13:00"),100});
        input.Rows.Add(new object[]{Convert.ToDateTime("2004-12-03 00:00"),100});
        return new Series(input,"cfs",TimeInterval.Irregular);
      }
    }
    /// <summary>
    /// 17 days of data with one missing point.
    /// </summary>
    public static Series MissingData
    {
      get
      {
        Series ts = new Series("cfs",TimeInterval.Irregular);
        DateTime d = new DateTime(1965,11,1);
        Point pt;
        for(int i=1; i<=17; i++)
        {
          if( i==15)
          {
            pt = new Point(d,-999,PointFlag.Missing);
          }
          else
          {
            pt = new Point(d,-999,PointFlag.None);
          }
          d = d.AddDays(1);

        }
        return ts;
      }
    }

        public static Series EntiatRiver
        {
            get
            {
                Series s = new TextSeries(DataPath + "\\" + "EntiatRiver.txt");
                s.Read();
                s.Units = "cfs";
                s.Name = "Entiat River near Entiat WA";
                s.TimeInterval = TimeInterval.Daily;
                return s;
            }

        }

        public static Series EntiatRiver24Points
        {
            get
            {
                Series s = new TextSeries(DataPath + "\\" + "EntiatRiver24points.txt");
                s.Read();
                s.Units = "cfs";
                s.Name = "Entiat River near Entiat WA";
                s.TimeInterval = TimeInterval.Daily;
                return s;
            }

        }

        /// <summary>
        /// Creates a test series with the values being the same as the month
        /// </summary>
        /// <param name="startingDate"></param>
        /// <param name="count"></param>
        /// <param name="months"></param>
        /// <returns></returns>
        public static Series SimpleDailyData(DateTime startingDate, int count, int[] months)
        {
                Series s = new Series();
                DateTime t = startingDate;

                for (int i = 0; i < count; i++)
                {
                    if (Array.IndexOf(months, t.Month) >= 0)
                    {
                        s.Add(t, t.Month);
                    }
                    t = t.AddDays(1);
                }
                return s;
        }
        
       
    }
}
