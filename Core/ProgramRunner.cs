using System;
using System.Collections;
using System.Threading;
using System.Diagnostics;
namespace Reclamation.Core
{
  /// <summary>
  /// ProgramRunner runs a program from the command line
  /// The OnOutput event fires for each line of output.
  /// 
  /// Based on vb code from:
  /// Herfried K. Wagner [MVP]
  /// http://dotnet.mvps.org
  /// </summary>
  public class ProgramRunner
  {
    private Thread m_ErrorThread;
    private Thread m_OutputThread;
    private Process m_Process;
    bool stdoutFinished=true;
    bool stderrorFinished=true;
    ArrayList outputList;

    public ProgramRunner()
    {
      outputList = new ArrayList();
    }

    public void Run(string programName, string args)
    {
      Run(programName,args,"");
    }

    public void SendCommand(string cmd)
    {
      this.m_Process.StandardInput.WriteLine(cmd);
    }
    public void Run(string programName, string args, string WorkingDirectory)
    {
      stdoutFinished = false;
      stderrorFinished = false;
      //SoiUtility.LogMessage("Preparing to start "+programName);
      this.m_Process = new Process();
      ProcessStartInfo info1 = this.m_Process.StartInfo;
      info1.WorkingDirectory = WorkingDirectory;
      info1.FileName = programName;
      info1.Arguments = args;
      info1.UseShellExecute = false;
      info1.CreateNoWindow = true;
      info1.RedirectStandardOutput = true;
      info1.RedirectStandardError = true;
      info1.RedirectStandardInput = true;
      info1 = null;
      this.m_Process.Start();
		
      ThreadStart start1 = new ThreadStart(this.StreamOutput);
      this.m_OutputThread = new Thread(start1);
      this.m_OutputThread.Start();
      ThreadStart start2 = new ThreadStart(this.StreamError);
      this.m_ErrorThread = new Thread(start2);
      this.m_ErrorThread.Start();
      //SoiUtility.LogMessage(programName +" has been launched");
    }

    public int WaitForExit()
    {
      while(IsRunning)
      {
        this.m_Process.WaitForExit();
      }
      return m_Process.ExitCode;

    }
    public bool IsRunning
    {
      get {return !this.m_Process.HasExited;}
    }

    private void StreamError()
    {
      string text1 = this.m_Process.StandardError.ReadLine();
      try
      {
        while (text1 != null && (text1.Length >= 0) && m_Process != null)
        {
        
          if (text1.Length > 0)
          {
            OnWriteLine(text1);
          }
          text1 = this.m_Process.StandardError.ReadLine();
        }
        return;
        
      }
      catch (Exception exception1)
      {
        Console.WriteLine(exception1.Message);
        OnWriteLine(exception1.Message);
        return;
      }
      finally
      {

        stderrorFinished = true;
        CheckIfFinished();
      }
    }
 

    private void StreamOutput()
    {
      string text1 = this.m_Process.StandardOutput.ReadLine();
      try
      {
        while (text1 != null && (text1.Length >= 0)&& m_Process != null)
        {
          if (text1.Length > 0)
          {
            OnWriteLine(text1);
          }
          text1 = this.m_Process.StandardOutput.ReadLine();
        }
        return;
      }
      catch (Exception exception1)
      {
		  
        OnWriteLine(exception1.Message);
	
        Console.WriteLine(exception1.Message);
        return;
      }
      finally
      {
        stdoutFinished=true;
        CheckIfFinished();
      }
    }
 
    public class ProgramEventArgs : EventArgs 
    {
      private readonly string message;
      public ProgramEventArgs(string message)
      {
        this.message=message;
      }
      public string Message
      {  
        get {return message;}
      }
    }

    public delegate void InfoEventHandler(object sender, ProgramEventArgs e);
    public delegate void ProgramFinishedEventHandler(object sender, ProgramEventArgs e);
  
    public event InfoEventHandler WriteLine;
    public event ProgramFinishedEventHandler ProgramFinished;

    /// <summary>
    /// Check if both standard error and standared output are finished.
    /// if finished the call the ProgramFinishedEvent
    /// </summary>
    private void CheckIfFinished()
    {
      if( this.stderrorFinished && this.stdoutFinished)
      {
        OnProgramFinished("done.");
      }
    }
    protected virtual void OnProgramFinished(string message)
    {
      if (ProgramFinished != null)
        ProgramFinished(this,new ProgramEventArgs("Finished"));
    }

    protected virtual void OnWriteLine(string message) 
    {
      if (WriteLine != null)
        WriteLine(this,new ProgramEventArgs(message));

      outputList.Add(message);
    }


    public string[] Output
    {
      get
      {
        string[] rval = new string[outputList.Count];
        outputList.CopyTo(rval,0);
        return rval;
      }
    }

    /// <summary>
    /// Runs executable and returns the output as an array of strings.
    /// </summary>
    /// <param name="exe"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static string[] RunExecutable(string exe, string args)
    {
      //SoiUtility.LogMessage("calling static RunExecutagle(" +exe+","+args+ " )");
      Process myProcess = new Process();
      myProcess.StartInfo.FileName = exe;
      myProcess.StartInfo.Arguments = args;
      myProcess.StartInfo.UseShellExecute = false;
      myProcess.StartInfo.CreateNoWindow = true;
      myProcess.StartInfo.RedirectStandardOutput = true;

      var started = myProcess.Start();
      if (!started)
          Logger.WriteLine("Error starting process " + exe);

      string s = myProcess.StandardOutput.ReadToEnd();
      string[] rval = s.Split(new char[] {'\n'});

      myProcess.WaitForExit();
      //SoiUtility.LogMessage("the command    RunExecutagle(" +exe+","+args+ " ) has returned");
      return rval;

    }



  }
}
