using System;
namespace Reclamation.TimeSeries.Alarms
{
}
namespace Reclamation.TimeSeries.Alarms {
    
    
    public partial class AlarmDataSet {


        private static void MakeTestData(AlarmDataSet.alarm_queueDataTable q)
        {

            //q.Addalarm_queueRow(1, "boii", "ob", 505.23, "confirmed", "Obi-wan", true, DateTime.Now.AddDays(-200));
            //q.Addalarm_queueRow(2, "boii", "ob", 405.23, "confirmed", "", false, DateTime.Now.AddDays(-100));
            q.Addalarm_queueRow(3, "boii", "ob", 305.23, "unconfirmed", "", false, DateTime.Now.AddDays(-10));
            q.Addalarm_queueRow(4, "boii", "ob", 105.23, "new", "", false, DateTime.Now);
        }
        /// <summary>
        /// Gets a list of alarms in priority order for processing
        /// only alarms with status (new, or unconfirmed)
        /// </summary>
        /// <returns></returns>
        public alarm_queueDataTable GetNewAlarms()
        {
            var rval = new alarm_queueDataTable();
            MakeTestData(rval);
            return rval;

        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}



