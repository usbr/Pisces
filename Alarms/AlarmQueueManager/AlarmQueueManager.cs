using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reclamation.TimeSeries.Alarms;
using Reclamation.TimeSeries;
using Reclamation.Core;
namespace AlarmQueueManager
{
    /// <summary>
    /// Manage a queue of alarms and when to originate a call
    /// runs every minute from a cron job
    /// </summary>
    class AlarmQueueManager
    {

        AlarmDataSet m_alarmDS;

        static void Main(string[] args)
        {

            if (InstanceUtility.IsAnotherInstanceRunning())
            {
                Console.WriteLine("Exiting: Another instance is running ");
                return;
            }

            try
            {
                Console.WriteLine("Starting ");
                InstanceUtility.CreateProcessIdFile();
                CheckForAlarms();
            }
            finally
            {
                InstanceUtility.DeleteProcessIdFile();
            }

        }

        private static void CheckForAlarms()
        {

           InstanceUtility.TouchProcessFile();

            Console.WriteLine("Done.");
            if (DateTime.Now.Day > 0)
                return;

            Logger.EnableLogger();

            Logger.WriteLine("Starting AlarmQueueManager");


            var aq = new AlarmQueueManager();
            if (Asterisk.IsBusy())
            {
                Logger.WriteLine("Asterisk system is busy " + Asterisk.GetAllVariables());
                return;
            }
            else
            {
                Logger.WriteLine("Processing Alarms");
                aq.ProcessAlarms();
            }
        }


        public AlarmQueueManager()
        {
        }


        public void ProcessAlarms()
        {

            var alarmQueue = DB.GetNewAlarms();

            Logger.WriteLine("found "+alarmQueue.Rows.Count+" alarms in the queue");
            for (int i = 0; i < alarmQueue.Count; i++)
            {
                var alarm = alarmQueue[i];
                Logger.WriteLine("alarm #" + i);
                for (int c = 0; c < alarmQueue.Columns.Count; c++)
                {
                    Logger.WriteLine(alarmQueue.Columns[c].ColumnName + ": " + alarmQueue[i][c].ToString());
                }

                if( alarm.status == "new")
                {
                    string[] numbers = null;
                    
                    Asterisk.Call(alarm.siteid, alarm.parameter, alarm.value.ToString("F2"), numbers);
                    DB.SaveTable(alarmQueue);

                    while(1==1)
                    {
                        InstanceUtility.TouchProcessFile();
                        System.Threading.Thread.Sleep(1000);
                    }
                    return;
                }

                if (alarm.status != Asterisk.Status) 
                {
                    alarm.status = Asterisk.Status;
                    DB.SaveTable(alarmQueue);
                }
            }
        }

        
        public AlarmDataSet DB
        {
            get
            {
                if (m_alarmDS == null)
                {
                    var db = TimeSeriesDatabase.InitDatabase(new Arguments(new string[] { }));
                    m_alarmDS = new AlarmDataSet();
                    m_alarmDS.Server = db.Server;
                }

                return m_alarmDS;
            }
        }

        
        
    }
}
