using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reclamation.TimeSeries.Alarms;
namespace AlarmQueueManager
{
    //public enum AlarmStatus
    //{
    //    /// <summary>
    //    /// new alarm that has not been processed
    //    /// </summary>
    //    New, 
    //    /// <summary>
    //    /// alarm is in asterisk and is being processed
    //    /// </summary>
    //    Busy, 
    //    /// <summary>
    //    /// alarm is confirmed
    //    /// </summary>
    //    Confirmed, 
    //    /// <summary>
    //    /// nobody confirmed alarm and asterisk is done
    //    /// </summary>
    //    Unconfirmed
    //}
    /// <summary>
    /// Manage a queue of alarms and when to originate a call
    /// runs every minute from a cron job
    /// </summary>
    class AlarmQueueManager
    {

        AlarmDataSet m_alarmDS;

        static void Main(string[] args)
        {
            var aq = new AlarmQueueManager();
            if( Asterisk.IsBusy())
            {
                Console.WriteLine("Asterisk system is busy "+ Asterisk.GetAllVariable());
                return;
            }
            aq.ProcessAlarms();

        }


        public void ProcessAlarms()
        {

            var alarmTable = DB().GetNewAlarms();

            for (int i = 0; i < alarmTable.Count; i++)
            {
                var alarm = alarmTable[i];

                if( alarm.status == "new")
                {
                  var asterisk = new Asterisk("boia_emm", alarm.siteid + "_" + alarm.parameter, alarm.value, alarm.event_time);
                    asterisk.Call();
                    alarm.status = "busy";
                    DB().Save();
                    return;
                }

                if (alarm.status != Asterisk.Status) 
                {
                    alarm.status = Asterisk.Status;
                    DB().Save();
                }
            }
        }
        public AlarmDataSet DB()
        {
            m_alarmDS = new AlarmDataSet();
            return m_alarmDS;
        }

        public AlarmQueueManager()
        {
            var q = m_alarmDS.alarm_queue;
        }
        
    }
}
