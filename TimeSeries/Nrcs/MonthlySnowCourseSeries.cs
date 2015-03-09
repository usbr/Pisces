using Reclamation.Core;
using Reclamation.TimeSeries.Nrcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries.Nrcs
{

    /// <summary>
    /// Monthly Series from reading NRCS daily snow data
    /// and stores in appropirate place in the monthly database
    /// 
    /// </summary>
    class MonthlySnowCourseSeries : Series
    {
        string m_triplet;
        SnotelParameterCodes m_parameter;

        public MonthlySnowCourseSeries(string triplet)
        {
            m_triplet = triplet;
            ConnectionString = "Triplet=" + triplet + ";SnotelParameter=WTEQ";
            m_parameter = SnotelParameterCodes.WTEQ;
            Init();
        }

        public MonthlySnowCourseSeries(TimeSeriesDatabase db,Reclamation.TimeSeries.TimeSeriesDatabaseDataSet.SeriesCatalogRow sr):base(db,sr)
        {
            m_triplet = ConnectionStringUtility.GetToken(ConnectionString, "Triplet", "");
            var p = ConnectionStringUtility.GetToken(ConnectionString, "SnotelParameter", "");
            m_parameter = (SnotelParameterCodes)Enum.Parse(typeof(SnotelParameterCodes), p);

            Init();
        }

        private void Init()
        {
            TimeInterval = TimeSeries.TimeInterval.Monthly;
            this.Name = "snow_" + m_triplet.Replace(":", "_");
        }

        protected override void ReadCore(DateTime t1, DateTime t2)
        {
            Clear();
            var s = new SnotelSeries(m_triplet, m_parameter);

            var t1a = t1.FirstOfMonth().AddMonths(-1);
            var t2a = t2.FirstOfMonth().AddMonths(1);
            s.Read(t1a, t2a);
            Console.WriteLine(" count = "+s.Count);
            s.WriteToConsole();
            for (int i = 0; i < s.Count; i++)
            {
                Point pt = SnapPointToMonth(s[i]);
                if (pt.IsMissing)
                    continue;

                if (IndexOf(pt.DateTime) >= 0)
                {
                    Console.WriteLine("duplicate...? "+pt.ToString());
                }
                else
                {
                    Add(pt);
                }

            }

            Trim(t1, t2);
        }

        /// <summary>
        /// readings in days > 20 moved to next month
        /// readings in days < 10 are moved to first of month
        /// readings in days 10 to 20 are considered mid-month
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private Point SnapPointToMonth(Point point)
        {
            if (point.DateTime.Day > 20)
                return new Point(point.DateTime.FirstOfMonth().AddMonths(1), point.Value, "");
            else
                if (point.DateTime.Day < 10)
                    return new Point(point.DateTime.FirstOfMonth(), point.Value, "");
                else
                    if (point.DateTime.Day >= 10 || point.DateTime.Day <= 20)
                        return new Point(point.DateTime.FirstOfMonth(), point.Value, "M"); // mid-month
                    else
                        throw new InvalidOperationException("point error in SnapPointToMonth " + point.ToString());
        }
    }
}
