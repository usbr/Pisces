using System;

namespace Reclamation.TimeSeries
{
	/// <summary>
	/// Selection
	/// </summary>
	public struct Selection
	{
    public double ymin,ymax;
    public DateTime t1,t2;

    public Selection(DateTime t1, DateTime t2)
    {
        this.t1 = t1;
        this.t2 = t2;
        this.ymin = double.MinValue;
        this.ymax = double.MaxValue;
    }
		public Selection(DateTime t1,DateTime t2,double ymin, double ymax)
		{
      this.t1 =  t1;
      this.t2 = t2;
      this.ymin = ymin;
      this.ymax = ymax;
		}

        //public Selection(DateTime t1, DateTime t2)
        //{
        //    this.t1 = t1;
        //    this.t2 = t2;
        //    this.ymin = double.MinValue;
        //    this.ymax = double.MaxValue;
        //}

    /// <summary>
    /// 
    /// </summary>
    public void SnapToNearest(Series s)
    {
        int idx1 = s.LookupIndex(t1, true);
        int idx2 = s.LookupIndex(t2, true);

        if (idx1 < 0 || idx2 < 0)
        {
            throw new Exception("Error trying to snap to nearest point");
        }
        t1 = s[idx1].DateTime;
        t2 = s[idx2].DateTime;
    }


    public override string ToString()
    {
      string s = t1.ToString(Series.DateTimeFormatInstantaneous)+" "+t2.ToString(Series.DateTimeFormatInstantaneous)
        +" "+ymin.ToString("F3") + " " +ymax.ToString("F3");
      return s;
    }
   
	}
}
