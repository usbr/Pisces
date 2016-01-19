using System.Windows.Forms;
using System.IO;
using Reclamation.Core;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Hydromet;
using System;

namespace HydrometNotifications
{
   

    class RogueMinimumFlow:Alarm
    {

        internal enum SystemState { Wet=1, Median=2, Dry=3, Unknown=99 };

        public static string Expression = @"\s*RogueMinimumFlow";

        bool m_clearCondition = false;

        public RogueMinimumFlow(AlarmDataSet.alarm_definitionRow row, bool clearCondition)
            : base(row)
        {
            Details = "";
            m_clearCondition = clearCondition;
        }


        /// <summary>
        /// Checks minimum flows for Sites in the Rogue
        /// The minumun flows are based on:
        ///     1) month of year
        ///     2) state of system (how much water in three reservoirs)
        ///     3) interval (instant or 7 day moving average)
        ///     this criteria is stored in a file named after the site and parameter
        ///     for example:   emi_q.csv
        /// </summary>
        /// <returns></returns>
        public override bool Check(DateTime t)
        {
            this.t = t;
            string baseFileName = m_row.cbtt + "_" + m_row.pcode;

            var dir = GetPathToMinimumFlowFiles();
            var minFlowFileName = Path.Combine(dir, baseFileName + ".csv");
            string canalFileName = Path.Combine(dir, baseFileName + ".canal");



            if (File.Exists(canalFileName)) 
            {
                bool canalOn = IsCanalOn(canalFileName,t);
                if (!canalOn)
                {// if canal is dry minium flows don't apply
                    Console.WriteLine("canal is dry minium flows don't apply at "+m_row.cbtt+ " "+m_row.pcode);
                    return m_clearCondition;
                }
            }

            CsvFile csv = new CsvFile(minFlowFileName);
            GetSeriesWithData(false);
            m_series.RemoveMissing();
            string month = t.ToString("MMMM");
            var state = DetermineSystemState(t);
            string msg = "State  (Wet, Median, or Dry) of the Rogue system is : " + state.ToString();
            customMessageVariables.Add("%system_state", state.ToString());
            Details += "\n"+msg;
            Details += "\nThe date is : " + t.ToLongDateString();
            var rows = csv.Select("Month = '" + month + "'");

            CheckForErrors(t, minFlowFileName, csv, month, state, rows);

            object o = rows[0][state.ToString()];
            if (o == DBNull.Value) // no criteria this month
            {
                Details += "\nNo alert criteria this month";
                return m_clearCondition; // this can clear an alarm, but don't create an alarm
            }

            double limit = Convert.ToDouble(o);
            customMessageVariables.Add("%flow_target", o.ToString());

            Details += "\nThe alert criteria is value < " + limit.ToString("F0");

            foreach (var pt in m_series)
            {
                event_point = pt;
                if (pt.DateTime.ToString("MMMM") != month)
                    continue; // only check in current month since rates may change

                if (m_clearCondition)
                {
                    if (pt.Value > limit)
                    {
                        Details += "\nAlert clear condition found at :" + pt.ToString(true);
                        return true;  // clear
                    }
                }
                else
                {
                    if (pt.Value < limit)
                    {
                        Details += "\nAlert condition found at :" + pt.ToString(true);
                        return true; // alert
                    }
                }
            }

            return false;
        }
     

        private static string GetPathToMinimumFlowFiles()
        {
            var dir = Path.GetDirectoryName(Application.ExecutablePath);
            if (dir.IndexOf("NUnit-") > 0)
            {// using test framework...
                dir = @"C:\Users\KTarbet\Documents\project\Hydromet\HydrometNotifications\bin\Debug";
            }
            return dir;
        }

        /// <summary>
        /// Simple sanity checks
        /// </summary>
        private static void CheckForErrors(DateTime t, string filename, CsvFile csv, string month, SystemState state, System.Data.DataRow[] rows)
        {
            if (state == SystemState.Unknown)
                throw new Exception("Error with system state: talsys_afavg.csv date=" + t.ToString());

            if (rows.Length != 1)
            {
                throw new Exception("Error with " + month + " in " + filename);
            }

            if (csv.Columns.IndexOf(state.ToString()) < 0)
                throw new Exception("Can't find column " + state + " in " + filename);
        }

        internal static SystemState DetermineSystemState( DateTime t)
        {
            if (t.Date == DateTime.Now.Date)
                t = t.Date.AddDays(-1); // we dont' have daily  value yet for today (use yesterday)

            t = t.Date; // state is daily but we could be running previous day at 9:55 am 

            var dir = GetPathToMinimumFlowFiles();
            // read avearge contents for three reservoirs 
            var avg = new TextSeries(Path.Combine(dir, "talsys_afavg.csv"));
            avg.Read();
            var talsys_avg = new PeriodicSeries(avg.Table);

            var t1 = t.Date.AddDays(-1);

            // current last two days system contents 
            var hmet = new HydrometDailySeries("talsys", "af");
            hmet.Read(t1, t);
            // determine state.

            Point talsys = hmet[t];
            if (talsys.IsMissing)
                talsys = hmet[t.AddDays(-1).Date]; // try back one day 

            if (talsys.IsMissing)
                return SystemState.Unknown;

            if (t.Month == 2 && t.Day == 29)// don't lookup 29th in periodic table
                t = t.AddDays(-1);

            double avg_af = talsys_avg.Interpolate(t);

            if (talsys.Value >= avg_af + 15000)
                return SystemState.Wet;
            if (talsys.Value <= avg_af - 15000)
                return SystemState.Dry;

            return SystemState.Median;
        }

        /// <summary>
        /// determines if a canal is running water.
        /// first line of file contains a 'cbtt pcode'
        /// for example  :talo qj
        /// 
        /// Assume canal is on unless data supporst that it is off.
        /// For errors or missing data return TRUE
        /// </summary>
        /// <param name="canalFileName"></param>
        /// <returns></returns>
        private bool IsCanalOn(string canalFileName,DateTime t)
        {
            string firstLine = File.ReadAllLines(canalFileName)[0];
            var tokens = firstLine.Trim().Split(' ');

            Console.WriteLine("Checking canal flow @ " + firstLine);

            if (t.Date == DateTime.Now.Date)
                t = t.Date.AddDays(-1); // we dont' have daily value yet for today (use yesterday)

            t = t.Date; // state is daily but we could be running previous day at 9:55 am 
            var t1 = t.Date.AddDays(-1);

            // current last two days system contents 
            var hmet = new HydrometDailySeries(tokens[0], tokens[1]);
            hmet.Read(t1, t);

            if (hmet.Count == 0)
                return true;

            Point canal = hmet[t];
            if (canal.IsMissing)
                canal = hmet[t.AddDays(-1).Date]; // try back one day 


            if (canal.IsMissing)
                return true;

            return canal.Value > 5;

        }
    }
}
