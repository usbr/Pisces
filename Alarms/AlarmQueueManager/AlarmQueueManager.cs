using System;
using Reclamation.TimeSeries.Alarms;
using Reclamation.Core;
using System.Configuration;
using System.IO;
using System.Diagnostics;
namespace AlarmQueueManager
{
    /// <summary>
    /// AlarmQueueManager watches a queue of alarms.
    /// the queue is a database table alarm_phone_queue
    /// runs every minute from a cron job
    /// processes alarms with status of 'new' or 'unconfirmed'
    /// 
    ///   0) verify asterisk is not busy with alarm_phone_queue.id 
    ///   1) update alarm_phone_queue.current_phone_index
    ///         current_phone_index is incremented +1
    ///         or back to zero (to start over)
    ///   2) create asterisk call file
    ///   3) copy call file to asterisk server
    /// 
    /// </summary>
    class AlarmQueueManager
    {
        static void Main(string[] args)
        {
            try
            {
                Logger.EnableLogger();
                Logger.WriteLine("Starting AlarmQueueManager");
                var userName = ConfigurationManager.AppSettings["PostgresUser"];

                var svr = PostgreSQL.GetPostgresServer(userName: userName);
                var aq = new AlarmQueueManager(svr);
                Logger.WriteLine("Processing Alarms");
                aq.ProcessAlarms();
            }
            catch (Exception e)  
            {
                Logger.WriteLine(e.Message);
            }
        }


        BasicDBServer m_server;
        AlarmDataSet alarmDS;
        public AlarmQueueManager(BasicDBServer server)
        {
            m_server = server;
            alarmDS = AlarmDataSet.CreateInstance(server);
        }


        public void ProcessAlarms()
        {

            var alarmQueue = alarmDS.GetNewAlarms();
        
            Logger.WriteLine("found "+alarmQueue.Rows.Count+" unconfirmed alarms in the queue");
            
            for (int i = 0; i < alarmQueue.Count; i++)
            {
                var alarm = alarmQueue[i];
                LogDetails(alarm);

                string[] numbers = alarmDS.GetPhoneNumbers(alarm.list);

                int minutesBeforeNextPhone = 5;
                if (alarmDS.CurrentActivity(alarm.id, minutesBeforeNextPhone)) // any activity in last x minutes.
                {
                    Logger.WriteLine("waiting on id = "+alarm.id+ " it has activity in the last "+minutesBeforeNextPhone+ " minutes");
                    continue;
                }

                alarm.current_list_index = UpdateCurrentPhoneIndex(alarm, numbers);

                var c = new AsteriskCallFile( numbers[alarm.current_list_index]);
                c.AddVariable("siteid", alarm.siteid);
                c.AddVariable("parameter", alarm.parameter);
                c.AddVariable("value", alarm.value.ToString());
                c.AddVariable("id", alarm.id.ToString());
                c.AddVariable("phone", numbers[alarm.current_list_index]);

                SendCallFile(c);
                alarmDS.SaveTable(alarmQueue);

            }
        }



        void SendCallFile(AsteriskCallFile c)
        {
          //C:\TEMP>pscp  -i c:\mykey.ppk -v temp3.call hydromet@pbx:/var/spool/asterisk/outgoing/

            var src = c.SaveToTempFile();
            var host = ConfigurationManager.AppSettings["pbx_server"];
            var user = ConfigurationManager.AppSettings["pbx_username"];
            var scp = ConfigurationManager.AppSettings["scp"];
            var key = ConfigurationManager.AppSettings["pbx_ssh_key"];
          

            var dest = user+"@" + host + ":/var/spool/asterisk/outgoing/";

            var args = "-i "+key +" "+ src + " " + dest;

           Process.Start(scp, args);
        }
        


        /// <summary>
        /// current_list_index is initilized to -1 in database.
        /// increment or rollback to zero 
        /// </summary>
        /// <param name="alarm"></param>
        /// <param name="numbers"></param>
        /// <returns></returns>
        private static int UpdateCurrentPhoneIndex(AlarmDataSet.alarm_phone_queueRow alarm, string[] numbers)
        {
            if (numbers.Length == 0)
                throw new Exception("Error: no phone numbers... list is empty");
            
            int rval = alarm.current_list_index + 1;

            if (alarm.current_list_index < 0
                || rval >= numbers.Length)
            {
                rval = 0; // start back at beginning.
            }

            return rval;
        }

        

        private void LogDetails(AlarmDataSet.alarm_phone_queueRow alarm)
        {
            var tbl = alarm.Table;
            for (int c = 0; c < tbl.Columns.Count; c++)
            {
                Logger.WriteLine(tbl.Columns[c].ColumnName + ": " + alarm[c].ToString());
            }
        }
    }
}
