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
        /// <param name="inputSeriesList"></param>
        /// <param name="computeDependencies"></param>
        /// <param name="computeDailyEachMidnight"></param>
        public void Import(SeriesList inputSeriesList,
            bool computeDependencies = false,
            bool computeDailyEachMidnight = false,
            string importTag="data")
        {
            var calculationQueue = new SeriesList();
            var routingList = new SeriesList();

            foreach (var s in inputSeriesList)
            {
                // set flags.
                Logger.WriteLine("Checking Flags ");
                m_db.Quality.SetFlags(s); // to do, log/email flagged data
                // To Do.. check for alarms..
                
                m_db.ImportSeriesUsingTableName(s,  "");
                routingList.Add(s);

                if (computeDependencies)
                {
                    var z = ComputeDependenciesSameInterval(s);
                    routingList.AddRange(z);
                }
                if (computeDailyEachMidnight)
                {
                    var x = GetDailyCalculationsIfMidnight(s);
                    foreach (var item in x)
                    {
                        if (!calculationQueue.ContainsTableName(item))
                            calculationQueue.Add(item);
                    }
                }
            }

            if (calculationQueue.Count >0)
            {
                // do Actual Computations now. (in proper order...)
                var list = new List<CalculationSeries>();
                foreach (Series item in calculationQueue)
                {
                    list.Add(item as CalculationSeries);
                }
                TimeSeriesDependency td = new TimeSeriesDependency(list);
                var sortedCalculations = td.Sort();
                foreach (CalculationSeries cs in sortedCalculations)
                {
                    Console.Write(">>> " + cs.Table.TableName + ": " + cs.Expression);
                    //var cs = item as CalculationSeries;
                    var t1 = inputSeriesList.MinDateTime.Date;
                    var t2 = inputSeriesList.MaxDateTime;

                    if (t1.Date == t2.AddDays(-1).Date) // spans midnight, compute yesterday.
                    {
                        t1 = t1.Date;
                        t2 = t1.Date;
                    }

                    cs.Calculate(t1, t2);
                    if (cs.Count > 0)
                    {
                        routingList.Add(cs);
                        if( cs.CountMissing() >0)
                        
                            Console.WriteLine(" Missing "+cs.CountMissing()+" records");
                        else
                            Console.WriteLine(" OK");
                    }
                } 
            }

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
        private SeriesList ComputeDependenciesSameInterval(Series s)
        {
            SeriesList rval = new SeriesList();
            var calcList = GetDependentCalculations(s.Table.TableName, s.TimeInterval);
            if (calcList.Count > 0)
                Logger.WriteLine("Found " + calcList.Count + " " + s.TimeInterval + " calculations to update ");
            foreach (var item in calcList)
            {
                var cs = item as CalculationSeries;
                cs.Calculate(s.MinDateTime, s.MaxDateTime);
                if (cs.Count > 0)
                    rval.Add(cs);
            }

            return rval;
        }

        private SeriesList GetDailyCalculationsIfMidnight(Series s)
        {
            var calcList = new SeriesList();
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
                            if (!calcList.ContainsTableName(item))
                                calcList.AddRange(x);
                        }
                    }
                }
            }
            return calcList;
        }

        /// <summary>
        /// gets daily dependents for this series (tablename)
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        private SeriesList GetDailyDependents(string tableName)
        {
            SeriesList rval = new SeriesList();
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
         SeriesList GetDependentCalculations(string tableName, TimeSeries.TimeInterval timeInterval)
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
