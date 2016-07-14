using Reclamation.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries
{
    /// <summary>
    /// TimeSeriesImporter Manages importing data,
    /// determines if calculations need to be made and flags data
    /// based on quality limits
    /// </summary>
    public class TimeSeriesImporter
    {

        TimeSeriesDatabase m_db;
        RouteOptions m_routing;

        public TimeSeriesImporter(TimeSeriesDatabase db, RouteOptions routing=RouteOptions.None)
        {
            m_db = db;
            m_routing = routing;
        }

        /// <summary>
        /// Imports a single series (used for testing)
        /// </summary>
        /// <param name="s"></param>
        /// <param name="computeDependencies"></param>
        /// <param name="computeDailyEachMidnight"></param>
        internal void Import(Series s, bool computeDependencies = false,
            bool computeDailyEachMidnight = false)
        {
            var sl = new SeriesList();
            sl.Add(s);
            Import(sl, computeDependencies, computeDailyEachMidnight,"test");
        }


        /// <summary>
        /// Imports time series data,
        /// 1) set flags
        /// 2) active alarms (TO DO)
        /// 3) compute dependent data (same interval)
        /// 4) compute daily data when encountering midnight values
        /// </summary>
        /// <param name="importSeries"></param>
        /// <param name="computeDependencies"></param>
        /// <param name="computeDailyDependencies"></param>
        public void Import(SeriesList importSeries,
            bool computeDependencies = false,
            bool computeDailyDependencies = false,
            string importTag="data")
        {
            var calculationQueue = new List<CalculationSeries>();
            var routingList = new SeriesList();

            foreach (var s in importSeries)
            {
                m_db.Quality.SetFlags(s); // to do, log/email flagged data

                // m_db.Alarms.Check(s);To Do.. check for alarms..

                m_db.ImportSeriesUsingTableName(s,  "");
                routingList.Add(s);

                if (computeDependencies)
                {
                    var z = ComputeDependenciesSameInterval(s);
                    routingList.AddRange(z);
                }

                if (computeDailyDependencies)
                {  // daily calcs that depend on instant
                   // if( ! s.AllInToday() )
                    GetDailyDependentCalculations(s,calculationQueue); 
                }
            }

            if (calculationQueue.Count >0)
            {
                PerformDailyComputations(importSeries, calculationQueue, routingList); 
            }

            RouteData(importTag, routingList);


        }

        /// <summary>
        /// Routes data to incoming and/or outgoing directories
        /// </summary>
        /// <param name="importTag"></param>
        /// <param name="routingList"></param>
        private void RouteData(string importTag, SeriesList routingList)
        {
            SeriesList instantRoute = new SeriesList();
            SeriesList dailyRoute = new SeriesList();
            // route data to other locations.
            foreach (var item in routingList)
            {
                TimeSeriesName tn = new TimeSeriesName(item.Table.TableName);
                item.Parameter = tn.pcode;
                item.SiteID = tn.siteid;
                if (item.TimeInterval == TimeInterval.Irregular)
                    instantRoute.Add(item);
                if (item.TimeInterval == TimeInterval.Daily)
                    dailyRoute.Add(item);
            }
            Console.WriteLine("Routing data");
            TimeSeriesRouting.RouteInstant(instantRoute, importTag, m_routing);
            TimeSeriesRouting.RouteDaily(dailyRoute, importTag, m_routing);
        }

        private static void PerformDailyComputations(SeriesList importSeries,
            List<CalculationSeries> calculationQueue, SeriesList routingList)
        {
            // do Actual Computations now. (in proper order...)
            TimeSeriesDependency td = new TimeSeriesDependency(calculationQueue);
            var sortedCalculations = td.Sort();
            foreach (CalculationSeries cs in sortedCalculations)
            {
                Console.Write(">>> " + cs.Table.TableName + ": " + cs.Expression);

                
                //if (cs.MinDateTime == DateTime.Now.Date)
                  //  continue; // data is all in today or future... don't compute
 
                TimeRange tr = GetDailyCalculationTimeRange(importSeries); 
                


                cs.Calculate(tr.StartDate, tr.EndDate);
                if (cs.Count > 0)
                {
                    routingList.Add(cs);
                    if (cs.CountMissing() > 0)

                        Console.WriteLine(" Missing " + cs.CountMissing() + " records");
                    else
                        Console.WriteLine(" OK");
                }
            }
        }

        /// <summary>
        /// Return range of dates to compute daily data.
        /// </summary>
        /// <param name="inputSeriesList"></param>
        /// <returns></returns>
        private static TimeRange GetDailyCalculationTimeRange(SeriesList inputSeriesList)
        {
           var t1 = inputSeriesList.MinDateTime.Date;
           var t2 = inputSeriesList.MaxDateTime;

               
           // determine dates for calculation 
           // based on:
           //   * don't compute for today until we get to midnight
           //   * compute daily value for previous day
           if (t1.Date == t2.AddDays(-1).Date)    // spans midnight, compute previous day
           {
               t1 = t1.Date;
               t2 = t1.Date;
           }
           else if (t1.Date == t2.Date) //  not a whole day of data
           {
               t1 = t1.AddDays(-1);
               t2 = t1;
           }

           var t1a = t1;

           TimeRange rval = new TimeRange(t1a, t2);
           return rval;
        }
        private SeriesList ComputeDependenciesSameInterval(Series s)
        {
            SeriesList rval = new SeriesList();
            var calcList = GetDependentCalculations(s.Table.TableName, s.TimeInterval);
            if (calcList.Count > 0)
                Logger.WriteLine("Found " + calcList.Count + " " + s.TimeInterval + " calculations to update ");

            // TO DO:  sort calc list and perform in proper order.
            foreach (var item in calcList)
            {
                var cs = item as CalculationSeries;
                cs.Calculate(s.MinDateTime, s.MaxDateTime);
                if (cs.Count > 0)
                    rval.Add(cs);
            }

            return rval;
        }

        /// <summary>
        /// As 15 minute(instant) data is imported determine if we should 
        /// any compute a daily values with this criteria.
        /// 
        /// 1) a midnight value arrives
        /// 2) manual data edits (data would appear late compared to other data)
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private void GetDailyDependentCalculations(Series s,List<CalculationSeries> calculationQueue)
        {
            if (s.Count == 0)
                return;

            // check for midnight values, and initiate daily calculations.
            if (s.TimeInterval == TimeInterval.Irregular)
            {
                for (int i = 0; i < s.Count; i++)
                {
                    var pt = s[i];
                    if (pt.DateTime.IsMidnight())
                    {
                        var x = GetDailyDependents(s.Table.TableName);
                        foreach (var item in x)
                        {
                            if (!calculationQueue.Any(a => a.Table.TableName == item.Table.TableName))
                                calculationQueue.AddRange(x);
                        }
                        break; 
                    }
                }
            }
        }

        /// <summary>
        /// gets daily dependents for this series (tablename)
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        private List<CalculationSeries> GetDailyDependents(string tableName)
        {
            var rval = new List<CalculationSeries>();
            TimeSeriesName tn = new TimeSeriesName(tableName);
            var calcList = GetDependentCalculations(tableName, TimeInterval.Daily);
            //if (calcList.Count > 0)
            //  Logger.WriteLine("Found " + calcList.Count + " daily calculations to update ref:"+tableName);
            foreach (var item in calcList)
            {
                // prevent recursive?
                rval.Add(item);
                // check for daily that depends on daily.
                var x = GetDailyDependents(item.Table.TableName);
                rval.AddRange(x);
            }
            return rval;
        }

        TimeSeriesDependency m_instantDependencies;
        TimeSeriesDependency m_dailyDependencies;

        /// <summary>
        /// Find calculations that depend on this series (tableName) 
        /// These calculations will have the series as input
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="timeInterval"></param>
        /// <returns></returns>
        List<CalculationSeries> GetDependentCalculations(string tableName, TimeSeries.TimeInterval timeInterval)
        {
            // cache with s_instantDependencies speed up from 174 seconds to 28 seconds (agrimet test)
            if (timeInterval == TimeSeries.TimeInterval.Irregular)
            {
                if (m_instantDependencies == null)
                {
                    var rawCalcList = m_db.Factory.GetCalculationSeries(timeInterval, "", "");
                    Logger.WriteLine("Info: GetDependentCalculations, found " + rawCalcList.Count
                           + " caluclation series");
                    m_instantDependencies = new TimeSeriesDependency(rawCalcList);
                }
                return m_instantDependencies.LookupCalculations(tableName, timeInterval);
            }
            else if (timeInterval == TimeSeries.TimeInterval.Daily)
            {
                if (m_dailyDependencies == null)
                {
                    var rawCalcList = m_db.Factory.GetCalculationSeries(timeInterval, "", "");
                    m_dailyDependencies = new TimeSeriesDependency(rawCalcList);
                }
                return m_dailyDependencies.LookupCalculations(tableName, timeInterval);
            }

            throw new NotImplementedException("Error: GetDependentCalculations does not support " + timeInterval);

        }

    }
}
