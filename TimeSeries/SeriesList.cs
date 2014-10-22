using System;
using System.Data;
using System.Collections.Generic;
using Reclamation.Core;

namespace Reclamation.TimeSeries
{

    public enum SeriesListType
	{
	  Standard,
      WaterYears,
      Sorted
	}
  
  /// <summary>
    /// SeriesList contains a list of Series
  /// </summary>
  public class SeriesList : List<TimeSeries.Series>
  {
      public SeriesListType Type;
      public string DateFormat = "";

    public SeriesList()
    {
        Type = SeriesListType.Standard;
    }


    ///// <summary>
    ///// Subtracts Series within a list from series in another list
    ///// </summary>
    ///// <param name="a"></param>
    ///// <param name="b"></param>
    ///// <returns></returns>
    //public static SeriesList operator -(SeriesList a, SeriesList b)
    //{
    //    if (a.Count != b.Count)
    //        throw new ArgumentException("Error: number of series is not the same in the two lists");

    //    var rval = new SeriesList();
    //    for (int i = 0; i < a.Count; i++)
    //    {
    //        var sa = a[i];
    //        var sb = b[i];
    //        rval.Add(Math.Subtract(sa, sb));
    //    }

    //    return rval;
    //}
    ///// <summary>
    ///// Adds Series within a list to series in another list
    ///// </summary>
    ///// <param name="a"></param>
    ///// <param name="b"></param>
    ///// <returns></returns>
    //public static SeriesList operator +(SeriesList a, SeriesList b)
    //{
    //    if (a.Count != b.Count)
    //        throw new ArgumentException("Error: number of series is not the same in the two lists");

    //    var rval = new SeriesList();
    //    for (int i = 0; i < a.Count; i++)
    //    {
    //        var sa = a[i];
    //        var sb = b[i];
    //        rval.Add(Math.Add(sa, sb));
    //    }

    //    return rval;
    //}

      /// <summary>
      /// Creates a new SeriesList Filtered by interval
      /// </summary>
      /// <param name="interval"></param>
      /// <returns></returns>
    public SeriesList FilteredList(TimeInterval interval)
    {
        var rval = new SeriesList();
        foreach (var item in this)
        {
            if (item.TimeInterval == interval)
                rval.Add(item);
        }

        return rval;
    }

      public void Add(SeriesList items)
      {
          foreach (Series s in items)
          {
              Add(s);
          }
      }


      public string MissingRecordsMessage
      {
          get
          {
              string rval = "";
              foreach (Series s in this)
              {
                  if (s.CountMissing() > 0)
                  {
                      rval += s.Name + ": missing " + s.CountMissing() + " records";
                  }
              }
              return rval;
          }
      }

      public bool HasMultipleSites
      {
          get { return Text.UniqueSites.Length > 1; }
      }
      public SeriesListText Text
      {
          get { return new SeriesListText(this); }
      }

      
      /// <summary>
      /// sum list into one time series
      /// </summary>
      /// <param name="sl list of series"></param>
      /// <returns>series that is sum of all series in list</returns>
      public Series Sum()
      {
          if (this.Count == 1) return this[0];
          Series s = new Series();
          s = this[0].Clone();
          s.Table = this[0].Table.Copy();
          for (int i = 1; i < this.Count; i++)
          {
              s = s + this[i];
          }
          return s;
      }

      /// <summary>
      /// median list into one time series
      /// </summary>
      /// <returns>series that is median of all series in list</returns>
      public Series Median()
      {
          if (this.Count == 1) return this[0];
          DataTable dt = this.ToDataTable(true);
          Series s = new Series();
          Point pt = new Point();
          foreach (DataRow row in dt.Rows)
          {
              List<double> values = new List<double>();
              foreach (object item in row.ItemArray)
              {
                  if (item is DateTime)
                      pt.DateTime = DateTime.Parse(item.ToString());
                  else if (item is DBNull)
                      break;
                  else
                      values.Add(double.Parse(item.ToString()));
              }
              if (values.Count != this.Count)
              {
                  s.AddMissing(pt.DateTime);
              }
              else
              {
                  values.Sort();
                  int size = values.Count;
                  int mid = size / 2;
                  double median = (size % 2 != 0) ? values[mid] : (values[mid] + values[mid - 1]) / 2;
                  s.Add(pt.DateTime, median);
              }
          }
          return s;
      }

      public SeriesList Subset(DateTime d1, DateTime d2)
      {
          string datecol = this[0].Table.Columns[0].ColumnName;
          SeriesList rval = new SeriesList();
          String sql = "[" + datecol + "] >= '" + d1.ToShortDateString() + 
              "' AND [" + datecol + "] <= '" + d2.ToShortDateString() + "'";
          foreach (Series s in this)
          {
              Series ss = s.Subset(sql);
              rval.Add(ss);
          }
          return rval;
      }
      public Series Sum(DateTime t1, DateTime t2)
      {
          SeriesList sl = new SeriesList();
          sl = this.Subset(t1, t2);
          return sl.Sum();
      }

      //public string Title="";
      //public string SubTitle = "";
      
      public void Read()
      {
          foreach (Series s in this)
          {
              s.Read();
          }
      }

      public void Read(int year, int month)
      {
          foreach (Series s in this)
          {
              s.Read(year,month);
          }
      }
      public void Read(DateTime t1, DateTime t2)
      {
          foreach (Series s in this)
          {
              s.Read(t1, t2);
          }
      }

      public void Save()
      {
          foreach (Series s in this)
          {
              s.Save();
          }
      }

      public bool ReadOnly
      {
          get
          {
              bool rval = false;
              foreach (Series s in this)
              {
                  if (s.ReadOnly)
                      rval = true;
              }

              return rval;
          }
      }

    /// <summary>
    /// returns number of x,y pairs in longest series
    /// </summary>
    public int MaxLength
    {
      get
      {
        int rval = 0;
        for(int i=0; i<Count; i++)
        {
          if( this[i].Count >rval)
          {
            rval = this[i].Count;
          }
        }
        return rval;
      }
    }

      /// <summary>
      /// Gets the period of record for a series.
      /// </summary>
      /// <returns></returns>
    public PeriodOfRecord PeriodOfRecord()
    {
        DateTime t1 = TimeSeriesDatabase.MaxDateTime; // note reversal t1!= min
        DateTime t2 = TimeSeriesDatabase.MinDateTime;
        int sz = 0;
        for (int i = 0; i < Count; i++)
        {
            var por = this[i].GetPeriodOfRecord();
            if (por.T1 < t1)
                t1 = por.T1;
            if (por.T2 > t2)
                t2 = por.T2;
            sz += por.Count;
        }

        if (Count > 0)
            sz = sz / Count;

        return new PeriodOfRecord(t1, t2, sz);
    }

    public DateTime MaxDateTime
    {
      get
      {
        DateTime rval = DateTime.MinValue;
        for(int i=0; i<Count; i++)
        {
          if( this[i].MaxDateTime > rval )
          {
            rval = this[i].MaxDateTime;
          }
        }
        return rval;
      
      }
    }

    public DateTime MinDateTime
    {
      get
      {
        DateTime rval = DateTime.Now;
        for(int i=0; i<Count; i++)
        {
          if( this[i].MinDateTime <rval )
          {
            rval = this[i].MinDateTime;
          }
        }
        return rval;
      
      }
    }


    public void WriteToConsole()
    {
      DataTable tbl = this.ToDataTable(true);
      for(int i=0; i<tbl.Rows.Count; i++)
      {
        Console.Write(Convert.ToDateTime(tbl.Rows[i][0]).ToString("yyyy-MM-dd HH:mm:ss.ffff")+" ");
        //Console.Write(Convert.ToDateTime(tbl.Rows[i][0]).Ticks+" ");
        for(int c = 1; c < tbl.Columns.Count; c ++)
        { 
            if( tbl.Rows[i][c] == DBNull.Value)
                Console.Write("null ");
            else
          Console.Write(Convert.ToDouble(tbl.Rows[i][c]).ToString("F4")+" ");
        }
          Console.WriteLine();
      }
    }



    /// <summary>
    /// Combines multiple time series data into a single table
    /// for viewing and editing.
    /// </summary>
    public DataTable ToDataTable(bool removeFlagColumn)
    {
      
          DataTable table = new DataTable();
          if (this.Type == SeriesListType.Sorted)
          {
              table = CreateExceedanceCompositeTable();
          }
          else
          {
              table = CreateTimeSeriesCompositeTable();
          }

         if (table.Columns.Contains("flag") && removeFlagColumn  )
             //this.Count >1 )
         {
             table.Columns.Remove("flag"); //Date,Value,Percent
         }
          return table;
      
    }

       private DataTable CreateExceedanceCompositeTable()
      {
          if (Count == 0)
          {
              return new Series().Table.Copy();
          }
          if (Count == 1)
          {
              return this[0].Table;
          }
          // following is for multiple exceedances in one table --Leslie
          DataTable table = CreateCompositeExceedanceSchema();
                    
          for (int seriesIndex = 0; seriesIndex < Count; seriesIndex++)
          {
              Series s = this[seriesIndex];
              DataRow row = null;
              //int columnOffset = seriesIndex * 2; // with flag
              int columnOffset = seriesIndex * 3;
              
              for (int i = 0; i < s.Count; i++)
              {
                  Point pt = s[i];
                  if (i >= table.Rows.Count)
                  {
                      row = table.NewRow();

                      row[0 + columnOffset] = pt.DateTime; //first field in row
                      row[1 + columnOffset] = pt.Value;
                      row[2 + columnOffset] = pt.Percent;
                                      
                      if( row["datetime"] == DBNull.Value)
                          Console.WriteLine();
                      table.Rows.Add(row);
                  }
                  else
                  {
                      row = table.Rows[i];
                      row[0 + columnOffset] = pt.DateTime; //first field in row
                      row[1 + columnOffset] = pt.Value;
                      row[2 + columnOffset] = pt.Percent;
                  }                  
               
              }//endfor int i
          }//endfor int seriesIndex
          return table;
      }

      // <summary>
      /// Creates table and unique column names.
      /// Used to create a composite of 
      /// multiple Exceedance Time Series DataTables --Leslie
      /// </summary>
      /// <returns></returns>
      private DataTable CreateCompositeExceedanceSchema()
      {

          DataTable table = new DataTable("CompositeTable");
          if (this.Count == 0)
          {// empty table
              return table;
          }

          table = this[0].DataView.Table.Clone();  //Date,Value,Flag,Percent
          table.Columns[1].DefaultValue = DBNull.Value;// Point.MissingValueFlag;
          // don't require non-null datetime because multiple series my have different length.
          table.Columns[0].AllowDBNull = true;

          string colName = this.Text.Text[0];
          colName = UniqueColumnName(table, colName, 0);
          table.Columns[1].ColumnName = colName; //Date,Scenario1,Percent
          //table.Columns[1].ExtendedProperties.Add("series", this[0]);

          for (int i = 1; i < Count; i++)
          {
              colName = "Date";
              colName = UniqueColumnName(table, colName, i);
              DataColumn col = new DataColumn(colName, typeof(DateTime));
              //col.ExtendedProperties.Add("date", this[i]);
              //col.DefaultValue = Point.MissingValueFlag;
              table.Columns.Add(col); //Date Scenario1 Percent Date1

              colName = this.Text.Text[i];
              colName = UniqueColumnName(table, colName, i);
              DataColumn col2 = new DataColumn(colName, typeof(double));
              //col.ExtendedProperties.Add("series", this[i]);
              col2.DefaultValue = Point.MissingValueFlag;
              table.Columns.Add(col2); //Date Scenario1 Percent Date1 Scenario2

              colName = "Percent";
              colName = UniqueColumnName(table, colName, i);
              DataColumn col3 = new DataColumn(colName, typeof(double));
              col3.DefaultValue = Point.MissingValueFlag;
              table.Columns.Add(col3);

             
          }
          if (table.Columns.Contains("flag"))
          {
              table.Columns.Remove("flag"); //Date,Value,Percent
          }
          return table;
      }

     
      private DataTable CreateTimeSeriesCompositeTable()
      {
      /*
       * Example:   series with 2 tables.
       * 
       * table1:  Date,uslev,flag
       * table2:  Date,uslev,flag
       * 
       * Output:  Date,uslev,flag1,uslev2,flag2
       **/
          if (Count == 0)
          {
              return new Series().Table.Copy();
          }
          if (Count == 1)
          {
              // if data is read-write original reference is needed in table
              return this[0].Table;
          }
          DataTable table = CreateCompositeSchema();

          Series tempSeries = new Series(table, "", TimeInterval.Irregular);

          // tempSeries is not a "valid" series.  just taking advantage
          // of insert and lookup functions

          for (int seriesIndex = 0; seriesIndex < Count; seriesIndex++)
          {
              Series s = this[seriesIndex];
              DataRow row = null;

              for (int i = 0; i < s.Count; i++)
              {
                  Point pt = s[i];
                  int idx = tempSeries.IndexOf(pt.DateTime);
                  if (idx < 0)
                  {// need new row..
                      row = table.NewRow();
                      row[0] = pt.DateTime;
                      row[1 + seriesIndex] = Point.DoubleOrNull(ref pt);
                  }
                  else
                  { // using existing row
                      row = table.Rows[idx];
                      row[1 + seriesIndex] = Point.DoubleOrNull(ref pt);
                      continue;
                  }

                  // find spot to insert 
                  int sz = tempSeries.Count;
                  if (sz == 0 || pt.DateTime > tempSeries.MaxDateTime)
                  {   // append
                      table.Rows.Add(row);
                      continue;
                  }

                  int j = tempSeries.LookupIndex(pt.DateTime);
                  table.Rows.InsertAt(row, j);
              }
          }



          return table;
      }

    
    /// <summary>
    /// Creates table and unique column names.
    /// Useed to create a composite of 
    /// multiple time Series DataTables
    /// 
    /// TO DO:  make this simpler and better.
    /// such that.
    /// 
    /// consider scenario, wateryear, min,max
    /// </summary>
    /// <returns></returns>
    private DataTable CreateCompositeSchema()
    {
        
      DataTable table = new DataTable("CompositeTable");
      if(this.Count ==0)
      {// empty table
        return table;
      }

      table = this[0].DataView.Table.Clone();
      table.Columns[1].DefaultValue =  DBNull.Value; ///Point.MissingValueFlag;

        if( table.Columns.Contains("flag") )
        {
         table.Columns.Remove("flag");
        }
         
        string colName = this.Text.Text[0];
        if (colName.Trim() == "")
            colName = "column0";
        //colName = UniqueColumnName(table, colName, 0);
        table.Columns[1].ColumnName = colName;
        table.Columns[1].ExtendedProperties.Add("series", this[0]);


      for(int i=1; i<Count; i++)
      {
        colName = this.Text.Text[i];
        
        colName = UniqueColumnName(table, colName, i);

        DataColumn col = new DataColumn(colName,typeof(double));
        col.ExtendedProperties.Add("series", this[i]);
        col.DefaultValue = DBNull.Value;  //Point.MissingValueFlag;
        table.Columns.Add(col);
       
      }
      return table;
    }

    /// <summary>
    /// creates unique column name 
    /// first try to use the seedName.
    /// If seedName is not allready unique
    /// try appending a number to the end of
    /// the seedName
    /// </summary>
    /// <returns></returns>
    private string UniqueColumnName(DataTable table,string seedName, int seedNumber)
    {
      if(! table.Columns.Contains(seedName)  && seedName != "")
      {
      return seedName;
      }
      
      for(int i=seedNumber; i<1000; i++)
      {
        string cName = seedName +i;
        if( !table.Columns.Contains(cName))
        {
        return cName;
        }
      }

      return seedName+Guid.NewGuid().ToString();
  }



      public SeriesList Subset(int[] months)
      {
          SeriesList rval = new SeriesList();
          foreach (Series s in this)
          {
              rval.Add(TimeSeries.Math.Subset(s, months));
          }
          return rval;
      }

      public SeriesList Subset(MonthDayRange range)
      {
          SeriesList rval = new SeriesList();
          foreach (Series s in this)
          {
              rval.Add(TimeSeries.Math.Subset(s,range));
          }
          return rval;
      }
  #region /*  Aggregate Math operations */




      public SeriesList AggregateAndSubset(StatisticalMethods aggregateType, MonthDayRange monthDayRange, int beginningMonth)
      {
      
          SeriesList rval = new SeriesList();
          foreach (Series s in this)
          {
              rval.Add(TimeSeries.Math.AggregateAndSubset(aggregateType, s, monthDayRange, beginningMonth));
          }
          return rval;
      }

      public SeriesList SummaryHydrograph(int[] exceedanceLevels, DateTime beginningDate, bool max, bool min, bool avg, bool removeEmptySeries)//, bool removeFeb29)
      {
          SeriesList rval = new SeriesList();
          foreach (Series s in this)
          {
              SeriesList list = Math.SummaryHydrograph(s, exceedanceLevels, beginningDate, max, min, avg, removeEmptySeries);//,removeFeb29);
              rval.AddRange(list);
          }

          return rval;
      }

  #endregion

      /// <summary>
      /// removes missing values
      /// </summary>
      /// <returns></returns>
      public void RemoveMissing()
      {
          foreach (Series s in this)
          {
              s.RemoveMissing();
          }
        
      }

      /// <summary>
      /// combines this list of series into a single series
      /// can be used to merge multiple aggregate series into a single
      /// series.  
      /// </summary>
      /// <returns></returns>
      internal Series MergeYearlyScenarios()
      {
         
          var rval = new Series();

          if (HasMultipleSites)
              throw new Exception("Error: Merge is only supported for a single site location");

          for (int i = 0; i < this.Count; i++)
          {
              var s = this[i];
              if (s.ScenarioName == "")
                  continue;

              if (i == 0)
              {
                  rval.Name = "Merged "+this.Count +" items "+s.Name;
                  rval.SiteName = s.SiteName;
                  rval.TimeInterval = s.TimeInterval;
              }
              
              //shift to scenairo year.
              int yr;
              if (int.TryParse(s.ScenarioName ,out yr))
              {
                  var shifted = Math.ShiftToYear(s, yr);
              rval.Add(shifted);
              }
              else
              {
                  Logger.WriteLine("Error: Can't convert '" + s.ScenarioName + "' to an integer year");
                 // throw new Exception("Error: Can't convert '"+s.ScenarioName+"' to an integer year");
              }
          }
          
          return rval;
      }

      /*
  
 
       PAL          - PALISADES                                          - RESERVOIR OUTFLOW                        - 1000 AF
 
       YEAR         OCT      NOV      DEC      JAN      FEB      MAR      APR      MAY      JUN      JUL      AUG      SEP       TOTAL
 
       2001       240.09V   67.58V   67.28V   67.68V   61.30V   67.86V  112.61V  615.40V  645.31V  624.34V  401.14V     -           -
       2002       140.34V   63.33V   65.25V   65.27V   59.06V   65.31V   66.87V  412.66V  593.57V  764.51V  548.31V  390.29V    3234.75
       2003       123.01V   63.68V   65.06V   64.95V   59.14V   65.59V  122.29V  506.73V  774.59V  838.83V  563.44V     -           -
       2004       207.41V   59.63V   58.08V   58.24V   54.47V   58.55V  164.37V  797.40V  571.19V  622.57V  484.13V  373.33V    3509.37
       2005       205.79V   53.86V   55.17V   55.15V   49.76V   55.27V  111.60V  375.43V  640.24V  751.06V  551.93V  410.36V    3315.63
       2006       188.58V   55.62V   55.72V   67.93V  127.29V  154.23V  588.26V  646.04V  692.95V  716.70V  568.28V  349.01V    4210.62
       2007       173.33V  105.66V  110.13V   99.02V   66.45V   71.86V  134.77V  770.75V  782.83V  719.02V     -        -           -
       2008       172.87V   51.64V   49.19V   49.19V   46.13V   49.29V   81.39V  597.52V  774.86V  764.38V  762.57V  564.04V    3963.07
       2009       259.38V   61.45V   55.24V   55.83V   50.34V   55.99V  665.62V  865.79V  922.37V  916.51V  564.28V  461.32V    4934.12
       2010       204.60V  109.65V  111.82V  109.16V   70.93V   60.40V   84.67V  615.02V  885.03V  803.22V  583.98V  446.72V    4085.21
       2011       294.76V   69.48V   68.19V   93.97V  122.68V  200.70V  877.65V 1146.46V 1045.23V 1222.84V  605.88V  523.93V    6271.77
       2012       317.59V     -     197.17V  190.26V  178.48V  306.68V  508.65V  628.92V  641.29V  828.27V  727.57V  449.71V        -
       2013       227.12V   59.70V   54.93V     -        -        -        -        -        -        -        -        -           -
 
      AVERAGE     202.45   120.05   125.19   130.94   127.66   203.23   359.45   738.66   876.39   804.28   557.98   397.58     4690.95
      MAXIMUM     474.50   295.00   337.27   345.56   560.31   801.54   937.80  1263.07  1753.72  1222.84   762.70   573.31     7741.55
      MINIMUM      72.42    47.39    43.82    43.15    39.73    37.31    60.17   181.13   571.19   538.45   401.14   204.61     3189.96
 
      SUMS OF AVERAGES:
        MON-JUN  2884.02  2681.57  2561.53  2436.33  2305.39  2177.73  1974.51  1615.05   876.39
        MON-JUL  3688.30  3485.85  3365.80  3240.61  3109.67  2982.01  2778.78  2419.33  1680.67   804.28
        MON-AUG  4246.28  4043.83  3923.79  3798.60  3667.65  3539.99  3336.77  2977.31  2238.66  1362.26   557.98
        MON-SEP  4643.86  4441.42  4321.37  4196.18  4065.24  3937.58  3734.35  3374.90  2636.24  1759.84   955.57   397.58
        OCT-MON   202.45   322.49   447.68   578.63   706.29   909.51  1268.97  2007.63  2884.02  3688.30  4246.28  4643.86
 
      ------
 
      G - Published by USGS
      V - Loaded directly from ARCHIVES
      $
       */

      public int IndexOfTableName(string name)
      {

          for (int i = 0; i < this.Count; i++)
          {
              if (this[i].Table.TableName == name)
                  return i;
          }

          return -1;
      }

      public bool ContainsTableName(Series s)
      {
          return IndexOfTableName(s.Table.TableName) >= 0;
      }

      /// <summary>
      /// Creates a USGS table from monthly data
      /// </summary>
      /// <returns></returns>
      public DataTable ToUsgsMonthlyTable()
      {
          var rval = new DataTable();
          throw new NotImplementedException();

      }
  }
}
