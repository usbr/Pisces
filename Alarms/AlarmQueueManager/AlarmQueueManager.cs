using System;
using System.Threading;
using Reclamation.TimeSeries.Alarms;
using Reclamation.TimeSeries;
using Reclamation.Core;
using System.Configuration;
namespace AlarmQueueManager
{
    /// <summary>
    /// Manage a queue of alarms and when to originate a call
    /// runs every minute from a cron job to check for alarms.
    /// 
    /// Overview
    /// 
    ///  check alarm_queue for status = 'new' or 'unconfirmed'
    ///  process 'new' alarms first by priority
    ///  
    ///   send the list of phone numbers to asterisk
    ///   monitor status
    ///      if confirmed
    ///         update status and confirmed_by
    ///         exit
    ///      if error 
    ///         send error email to administrator
    ///         exit
    ///      if unconfirmed for 15 minutes
    ///         exit
    /// 
    /// 
    /// </summary>
    class AlarmQueueManager
    {

        AlarmDataSet m_alarmDS;

        static void Main(string[] args)
        {
            Logger.EnableLogger();
            

            if (InstanceUtility.IsAnotherInstanceRunning())
            {
                Console.WriteLine("Exiting: Another instance is running ");
                return;
            }

            try
            {
                Logger.WriteLine("Starting AlarmQueueManager");
                InstanceUtility.CreateProcessIdFile();
                var aq = new AlarmQueueManager();
                Logger.WriteLine("Processing Alarms");
                aq.ProcessAlarms();

            }
            finally
            {
                InstanceUtility.DeleteProcessIdFile();
            }

        }


        public AlarmQueueManager()
        {
        }


        public void ProcessAlarms()
        {
            InstanceUtility.TouchProcessFile();

            var alarmQueue = DB.GetNewAlarms();
            string user = ConfigurationManager.AppSettings["pbx_username"];
            string pass = ConfigurationManager.AppSettings["pbx_password"];

            Logger.WriteLine("found "+alarmQueue.Rows.Count+" new alarms in the queue");
            
            for (int i = 0; i < alarmQueue.Count; i++)
            {
                var alarm = alarmQueue[i];
                LogDetails(alarm);

                   string[] numbers = DB.GetPhoneNumbers(alarm.list);
                    //string[] numbers = new string[] { "5272", "5272" };
                    Asterisk.Call(alarm.siteid, alarm.parameter, alarm.value.ToString("F2"),
                        numbers,user,pass);
                    Thread.Sleep(2000);
                    string prevLog = "";
                    do
                    {
                        InstanceUtility.TouchProcessFile();
                        Thread.Sleep(2000);
                        UpdatePiscesStatus(alarm);

                        if( Asterisk.Log != prevLog)
                        {
                            prevLog = Asterisk.Log;
                            Logger.WriteLine(" Asterisk: " + Asterisk.LogTime + " : " + Asterisk.Log);
                        }

                        DB.SaveTable(alarmQueue);

                        if (Asterisk.ActiveChannels == 0)
                        {
                            UpdatePiscesStatus(alarm);
                            break; // someone hungup or other loss of connection
                        }

                        if( Asterisk.MinutesElapsed >=15)
                        {
                            UpdatePiscesStatus(alarm);
                            break;
                        }
                        
                    } while (Asterisk.Status == "unconfirmed");
            }
        }

        private static void UpdatePiscesStatus(AlarmDataSet.alarm_phone_queueRow alarm)
        {

            alarm.status = Asterisk.Status;
            alarm.status_time = Asterisk.StatusTime;
            alarm.confirmed_by = Asterisk.ConfirmedBy;
        }

        private void LogDetails(AlarmDataSet.alarm_phone_queueRow alarm)
        {
            var tbl = alarm.Table;
            for (int c = 0; c < tbl.Columns.Count; c++)
            {
                Logger.WriteLine(tbl.Columns[c].ColumnName + ": " + alarm[c].ToString());
            }

        }

        
        public AlarmDataSet DB
        {
            get
            {
                return AlarmDataSet.CreateInstance();
            }
        }

        
        
    }
}
