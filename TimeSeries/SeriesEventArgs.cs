using System;

namespace Reclamation.TimeSeries
{
  public class SeriesEventArgs: EventArgs
  {
    private Series s;
    public SeriesEventArgs(Series s)
    {
      this.s = s;
    }
    public Series Series{ get { return s; }  }
	}
}
