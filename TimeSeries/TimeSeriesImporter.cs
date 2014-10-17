using Reclamation.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries
{
    /// <summary>
    /// TimeSeriesImporter Manages importing data
    
    /// </summary>
    public class TimeSeriesImporter
    {

        TimeSeriesDatabase m_db;
        RouteOptions m_routing;
        Quality m_quality;

        public TimeSeriesImporter(TimeSeriesDatabase db, RouteOptions routing=RouteOptions.None)
        {
            m_db = db;
            m_routing = routing;
            m_quality = new Quality(m_db);
        }

        public void Import(Series s)
        {
            var sl = new SeriesList();
            sl.Add(s);
            Import(sl, false, false);

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
            bool computeDailyEachMidnight = false)
        {
            var calculationQueue = new SeriesList();
            var routingList = new SeriesList();

            foreach (var s in inputSeriesList)
            {
                // set flags.
                Logger.WriteLine("Checking Flags ");
                m_quality.SetFlags(s);
                // To Do.. check for alarms..

                m_db.ImportSeriesUsingTableName(s, true, "");

                if (computeDependencies)
                {
                    routingList = ComputeDependenciesSameInterval(s);
                }
                if (computeDailyEachMidnight)
                {
                    var x = ComputeDailyOnMidnight(s);
                    foreach (var item in x)
                    {
                        if (!calculationQueue.ContainsTableName(item))
                            calculationQueue.Add(item);
                    }
                }
            }

            // do Actual Computations now. (in proper order...)
            
            foreach (Series item in calculationQueue)
            {
                Console.WriteLine(">>> " + item.Table.TableName + ": " + item.Expression);
                var cs = item as CalculationSeries;
                cs.Calculate(inputSeriesList.MinDateTime.Date, inputSeriesList.MaxDateTime);
                if (cs.Count > 0)
                    routingList.Add(cs);
            }

            // route data to other locations.
            foreach (var item in routingList)
            	{
                TimeSeriesName tn = new TimeSeriesName(item.Table.TableName);
                if (item.TimeInterval == TimeInterval.Irregular)
                    TimeSeriesRouting.RouteInstant(item, tn.siteid, tn.pcode, m_routing);
                if (item.TimeInterval == TimeInterval.Daily)
                    TimeSeriesRouting.RouteDaily(item, tn.siteid, tn.pcode, m_routing);
            }


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
                // TO DO.. some calcs should go back 1 weeek. i.e.  QU
                // this is currently being done in TimeSeriesCalculator
                // for daily data.
                cs.Calculate(s.MinDateTime, s.MaxDateTime);
                if (cs.Count > 0)
                    rval.Add(cs);
            }

            return rval;
        }

        private SeriesList ComputeDailyOnMidnight(Series s)
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
