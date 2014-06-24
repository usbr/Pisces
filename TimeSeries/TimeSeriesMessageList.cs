using System;
using System.Collections.Specialized;
namespace Reclamation.TimeSeries
{
	/// <summary>
	/// Summary description for TimeSeriesMessageList.
	/// </summary>
	public class TimeSeriesMessageList : StringCollection
	{
    /// <summary>
    /// Constructor.
    /// </summary>
		public TimeSeriesMessageList()
		{
		}


    /// <summary>
    /// summary of first few messages as a single string
    /// </summary>
    /// <returns></returns>
    public string ToString(int maxMessages=10)
    {
      string msg = "";
      for(int i=0; i < Count ; i++)
      {
        msg += this[i]+"\n";
        if (i >= maxMessages && Count > maxMessages+1)
        {
            int moreMessages = Count - (maxMessages+1);
          msg += "...("+moreMessages.ToString()+") more messages";
          break;
        }
      }
      return msg;
    }

	}
}
