using System;

namespace Reclamation.TimeSeries
{

    public static class PointFlag
    {
        public const string Edited = "e"; // edit, math
        public const string None = "";
        public const string OK = " ";
        public const string Inserted = "I";
        public const string Locked = "L";
        public const string Computed = "C";
        public const string Estimated = "E";
        public const string Missing = "m";
        public const string Interpolated = "N";
        public const string QualityHigh = "+";
        public const string QualityLow = "-";
        public const string QualityRateOfChange = "^";

    }



    // Interpolated, Missing, Edited, Computed, Raw

  /// <summary>
  /// Data Flags used for TimeSeries data
  /// </summary>
  //public enum PointFlag 
  //{
  //  Raw=0,Good=1,Edited=2,Inserted=3,
  //  Bad=4,BadHigh=5,BadLow=6,Missing=7,Estimated=8,
  //  None=9,Computed=10,LessThan=11,GreaterThan=12,WriteProtected=13,
  //  Unknown = 14, Interpolated=15};
  /*
 e     Value has been edited or estimated by USGS personnel and is  write protected
 &     Value has computed from affected unit values
 <     Less than 
 >     Greater than
 1     Value is write protected without any remark code to be printed
      No remark (blank)

   */

  
	/// <summary>
	/// Point represents  a single point
	/// or measurement
	/// </summary>
	public struct Point
	{
    /// <summary>DateTime for point </summary>
    public DateTime DateTime;
    /// <summary>Value for point </summary>
    public double Value;
    /// <summary>Data Flag</summary>
    public string Flag;

   // public string Notes;
     /// <summary>
    /// Optional Weibul plotting position (can be used for exceedance percentage)
     /// </summary>
    public double Percent;

        /// <summary>
        /// NaN
        /// </summary>
    public static double MissingValueFlag = Double.NaN;

        public static bool IsMissingValue(double value)
        {
            return Double.IsNaN(value) || value == MissingValueFlag;
        }

        public static object DoubleOrNull(ref Point pt)
        {
            if (pt.IsMissing)
                return DBNull.Value;
            return pt.Value;
        }


    public static Point Missing
    {
        get { return new Point(DateTime.MinValue, Point.MissingValueFlag, PointFlag.Missing); }
    }


   public Point(DateTime date, double value)
    {
      this.DateTime  = date;
      this.Value = value;
      this.Flag = PointFlag.OK;
      Percent = 0;
      //Notes = "";
    }

        //public Point(DateTime date, double value, string flag)
        //{
        //    this.DateTime = date;
        //    this.Value = value;
        //    this.Flag = flag;
        //    Percent = 0;
        //    Notes = "";
        //}

    public Point(DateTime date, double value, string flag)
    {
      this.DateTime  = date;
      this.Value = value;
      this.Flag = flag;
      Percent = 0;
     // Notes = "";
    }

    public Point(DateTime date, double value, string flag, double percent)
    {
        this.DateTime = date;
        this.Value = value;
        this.Flag = flag;
        Percent = percent;
        //Notes = "";
    }


    public static Point operator *(Point pt, double value)
    {
        return new Point(pt.DateTime, pt.Value * value);
    }

    public bool IsMissing
    {
      get
      {
        if(Flag == PointFlag.Missing 
            || this.Value == MissingValueFlag
            || Double.IsNaN(this.Value))
        {
          return true;
        }
        return false;
      }
    }
    public bool BoundedBy(Selection select)
    {
      return BoundedBy(select.t1,select.t2,select.ymin,select.ymax);
    }
    public bool BoundedBy(DateTime t1, DateTime t2, double ymin, double ymax)
    {
      if( this.Value >= ymin && this.Value <= ymax 
        && this.DateTime >= t1 && this.DateTime <= t2)
      {
      return true;
      }
      return false;
    }

    public bool BoundedBy(DateTime t1, DateTime t2)
    {
      if(this.DateTime >= t1 && this.DateTime <= t2)
      {
        return true;
      }
      return false;
    }


        public string ToString(bool showFlag)
        {
            return ToString(showFlag, false);
        }
    public string ToString(bool showFlag, bool showPercentage)
    {
      string s = this.DateTime.ToString(Series.DateTimeFormatInstantaneous);
      s += "\t"+this.Value.ToString("F2");
      if( showFlag)
      {
        s+= "\t"+this.Flag.ToString();
      }
      if (showPercentage)
      {
          s += "\t" + this.Percent.ToString();
      }
      return s;
    }

    public override string ToString()
    {
      string s = this.DateTime.ToString(Series.DateTimeFormatInstantaneous);
      s += " "+this.Value.ToString("F2");
      return s;
    }
        public string ToString(string dateFormat)
        {
            string s = this.DateTime.ToString(dateFormat);
            s += " " + this.Value.ToString("F2");
            return s;
        }

        public bool FlaggedBad
        {
            get
            {
                if (Flag.IndexOf(PointFlag.QualityHigh) == 0
                    || Flag.IndexOf(PointFlag.QualityLow) == 0
                    || Flag.IndexOf(PointFlag.QualityRateOfChange) == 0
                    || Flag.IndexOf(PointFlag.Missing) == 0
                    )
                    return true;

                return false;
            }

        }
    }
}
