using System;

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
		 DateTime m_start; 

    /// <summary>
    /// Number of seconds elapsed so far
    /// </summary>
		public double ElapsedSeconds
		{
			get
			{
                if (m_paused)
                    return m_elapsed;

				DateTime t = DateTime.Now;
				TimeSpan diff = t.Subtract(this.m_start);
                return diff.TotalSeconds + m_elapsed;
			}
		}

        double m_elapsed = 0;

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
          this.m_start = DateTime.Now;
          m_elapsed = 0;
		}

        public void Continue()
        {
            this.m_start = DateTime.Now;
            m_paused = false;
        }

        bool m_paused = false;
        public void Pause()
        {
            m_elapsed = ElapsedSeconds;
            m_paused = true;
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
