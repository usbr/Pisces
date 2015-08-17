using System;
using System.Diagnostics;

namespace Reclamation.Core
{
	/// <summary>
	/// Simple time keeper to
	/// search for bottlenecks in application
	/// Karl Tarbet
	/// </summary>
	public class Performance
  {
    /// <summary>
    /// Starting time of performance measurement
    /// </summary>
         Stopwatch stopwatch1;
    /// <summary>
    /// Number of seconds elapsed so far
    /// </summary>
		public double ElapsedSeconds
		{
			get
			{
                return stopwatch1.ElapsedMilliseconds * 1000.0;
			}
		}


    /// <summary>
    /// prints number of seconds elapsed
    /// </summary>
    public void Report()
    {
      Report("");
    }

    public void Report(string msg)
    {
        Report(msg, false);
    }
    /// <summary>
    /// prints number of seconds elapsed, and message
    /// </summary>
    /// <param name="msg"></param>
    public void Report(string msg, bool logger)
    {
      string log = this.ElapsedSeconds.ToString("F3")+" seconds elapsed. "+msg;
      if (logger)
          Logger.WriteLine(log);
      else
      Console.WriteLine(log);
      //Utility.LogMessage(log);
    }


    /// <summary>
    /// Constructor
    /// </summary>
		public Performance()
		{
            stopwatch1 = new Stopwatch();
		}

        public void Continue()
        {
            stopwatch1.Start();
        }

        public void Pause()
        {
            stopwatch1.Stop();
        }

    /// <summary>
    /// Destructor
    /// </summary>
    ~Performance()
    {
   // Console.WriteLine("~Performance() called");
    }
	}
}
