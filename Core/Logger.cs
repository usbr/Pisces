using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Reclamation.Core
{
    public static class Logger
    {
        /// <summary>
        /// in memory list of commands send to log trace
        /// </summary>
        public static List<string> LogHistory = new List<string>();


        public static event StatusEventHandler OnLogEvent;

        static void FireOnLogEvent(string message,string tag)
        {
            if (OnLogEvent != null)
            {
                OnLogEvent(null, new StatusEventArgs(message,tag));
            }
        }
        /// <summary>
        /// Write Exception and message to screen and to logging 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void WriteLine(string message, Exception exception)
        {
            //Console.WriteLine(exception.Message);
            WriteLine(message);
            WriteLine(exception.Message);
            WriteLine(exception.StackTrace);
        }


        public static void WriteLine(string message)
        {
            WriteLine(message, "");
        }

        static bool s_keepHistoryInMemory = false;
        public static void EnableLogger(bool keepHistoryInMemory=false)
        {
            s_keepHistoryInMemory = keepHistoryInMemory;
            InitTracing();
            if (_traceSwitch.Level == TraceLevel.Off)
                _traceSwitch.Level = TraceLevel.Verbose;
        }
        /// <summary>
        /// Writes message to logging trace
        /// </summary>
        /// <param name="message"></param>
        public static void WriteLine(string message, string tag)
        {
            
            if (_traceSwitch == null)
            {
                InitTracing();
                LogHistory = new List<string>();
            }
            if (_traceSwitch.Level != TraceLevel.Off)
                Console.WriteLine(DateTime.Now.ToString() + ": " + message);

            Trace.WriteLine(DateTime.Now.ToString() + ": " + message);
            if (LogHistory != null && s_keepHistoryInMemory && tag != "ui")
            {
                LogHistory.Add(DateTime.Now.ToString() + ": " + message);
            }
           FireOnLogEvent(message,tag);
        }

        private static TraceSwitch _traceSwitch = null;
        static string logName;

        /// <summary>
        /// View Log by opening a text file.
        /// </summary>
        public static void ViewLog()
        {
            var tmp = FileUtility.GetTempFileName(".txt");
            File.Copy(logName, tmp,true);
            System.Diagnostics.Process.Start(tmp);   
        }
        /// <summary>
        /// Begin Tracing to log file based on application name.
        /// </summary>
        public static void InitTracing()
        {
            if (_traceSwitch == null)
            {
                _traceSwitch = new TraceSwitch("LoggingLevel", "Controls level of Trace logging output");
                if (_traceSwitch.Level != TraceLevel.Off)
                { // if any tracing.. save to log.
                    //string logName = Path.GetFileNameWithoutExtension(Application.ExecutablePath) + ".log";
                    logName = Path.Combine(FileUtility.GetLocalApplicationPath(), Path.GetFileNameWithoutExtension(Application.ExecutablePath));
                    logName = logName + ".log";
                    FileStream fs = new FileStream(logName, FileMode.Append);
                    TextWriterTraceListener listener = new TextWriterTraceListener(fs);
                    Trace.Listeners.Add(listener);
                }
                Trace.AutoFlush = true;
            }
        }
    }
}
