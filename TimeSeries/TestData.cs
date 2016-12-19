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
	/// TestData
	/// </summary>
	public class TestData
	{
        

        public static Series SpecificationMonthValue
        {
            get
            {
                TextSeries s = new TextSeries(Path.Combine(DataPath, "SpecificationTestData.csv"));
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
                rootDir = Path.Combine(Path.Combine(rootDir, ".."), "..", "tests");
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
                Series s = new TextSeries(Path.Combine(DataPath, "SouthForkOfBoiseNearFeatherville.txt"));
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
          Series s = new TextSeries(Path.Combine(DataPath, "site68_onefullday.txt"));
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
          Series s = new TextSeries(Path.Combine(DataPath, "site68_LindCouleeWasteWay1DailyAverage.csv"));
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
          Series s = new TextSeries(Path.Combine(DataPath, "site68_LindCouleeWasteWay1Stage.csv"));
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
          Series s = new TextSeries(Path.Combine(DataPath, "El686_2004DailyAverageStage.csv"));
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
          var fn = Path.Combine(DataPath, "banks.csv");
          TextSeries s = new TextSeries(fn);
          s.Read();
          Console.WriteLine(s.Count);
          return s;
          //DataSet ds = new DataSet();
          
        //  Console.WriteLine(fn);
        //ds.ReadXml(fn);
        //DataTable tbl = ds.Tables[0];
        //Console.WriteLine(tbl.Rows.Count+" rows in table");
        // return new Series(tbl,"feet",TimeInterval.Irregular);
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
                Series s = new TextSeries(Path.Combine(DataPath, "EntiatRiver.txt"));
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
                Series s = new TextSeries(Path.Combine(DataPath, "EntiatRiver24points.txt"));
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

        public static DataTable RirieQ()
        {
            string[] data = {
                "16,-0.1, 0.0, 0.5, 1.0, 1.5, 2.0, 2.5, 3.0, 3.5, 4.0, 4.5, 5.0, 5.5, 6.0, 6.5, 7.0",
                "5023, 0, 0, 55, 109, 167, 225, 281, 338, 395, 453, 513, 574, 643, 713, 818, 924",
                "5028, 0, 0, 59, 118, 181, 245, 306, 368, 432, 496, 562, 629, 707, 785, 905, 1026",
                "5033, 0, 0, 64, 128, 196, 264, 332, 399, 469, 539, 611, 684, 770, 857, 992, 1128",
                "5038, 0, 0, 68, 137, 210, 284, 357, 430, 506, 582, 661, 739, 834, 929, 1080, 1231",
                "5043, 0, 0, 73, 145, 224, 302, 380, 458, 539, 621, 705, 789, 891, 993, 1157, 1320",
                "5048, 0, 0, 76, 153, 235, 317, 399, 482, 567, 653, 742, 831, 939, 1047, 1218, 1389",
                "5053, 0, 0, 80, 160, 246, 332, 419, 506, 595, 685, 779, 874, 987, 1100, 1279, 1459",
                "5058, 0, 0, 84, 167, 257, 348, 438, 529, 623, 716, 816, 916, 1035, 1153, 1341, 1528",
                "5063, 0, 0, 87, 175, 269, 363, 458, 553, 651, 748, 854, 959, 1083, 1207, 1402, 1597",
                "5068, 0, 0, 91, 181, 279, 377, 475, 574, 676, 777, 887, 997, 1126, 1254, 1456, 1657",
                "5073, 0, 0, 94, 187, 288, 389, 492, 594, 699, 804, 918, 1031, 1165, 1299, 1504, 1710",
                "5078, 0, 0, 97, 193, 298, 402, 508, 614, 722, 831, 948, 1066, 1204, 1343, 1553, 1763",
                "5083, 0, 0, 100, 199, 307, 415, 524, 633, 746, 858, 979, 1101, 1244, 1387, 1601, 1816",
                "5088, 0, 0, 103, 206, 317, 428, 540, 653, 769, 885, 1010, 1135, 1283, 1431, 1650, 1869",
                "5093, 0, 0, 106, 211, 325, 440, 555, 671, 790, 909, 1038, 1167, 1319, 1471, 1693, 1915",
                "5098, 0, 0, 108, 216, 333, 451, 570, 689, 811, 932, 1065, 1198, 1354, 1509, 1732, 1955",
                "5103, 0, 0, 111, 221, 341, 462, 584, 706, 831, 956, 1092, 1228, 1388, 1548, 1772, 1996",
                "5108, 0, 0, 113, 226, 350, 473, 598, 723, 851, 979, 1119, 1259, 1422, 1586, 1811, 2036",
                "5113, 0, 0, 116, 231, 358, 484, 612, 740, 871, 1002, 1146, 1289, 1457, 1624, 1850, 2076",
                "5118, 0, 0, 118, 236, 366, 495, 626, 758, 891, 1025, 1172, 1320, 1491, 1662, 1889, 2117",
                "5119, 0, 0, 119, 238, 367, 497, 629, 761, 895, 1030, 1178, 1326, 1498, 1670, 1897, 2125"};

            DataTable tbl = new DataTable();

            tbl.BeginLoadData();

            for (int i = 0; i < data.Length; i++)
            {
                object[] rowData = data[i].Split(',');

                if (tbl.Columns.Count == 0)
                {
                    for (int j = 0; j < rowData.Length; j++)
                    {
                        tbl.Columns.Add(rowData[j].ToString(), typeof(double));
                    }
                }
                else
                {
                    tbl.LoadDataRow(rowData, true);
                }
            }

            tbl.EndLoadData();

            return tbl;
        }
       
    }
}
