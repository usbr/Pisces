﻿using Reclamation.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries
{
    /// <summary>
    /// TimeSeriesDependency determines order of calculations and 
    /// other dependencies for CalculationSeries
    /// </summary>
    public class TimeSeriesDependency
    {
        List<CalculationSeries> list;

        

        public TimeSeriesDependency(List<CalculationSeries> list)
        {
            this.list = list;
           
        }
        

        /// <summary>
        /// Find all CalculationSeries (Equations) that reference this Series.  Use the tablename
        /// of this series as the variable to look for in each equation.
        /// </summary>
        /// <param name="cs"></param>
        /// <returns></returns>
        private List<CalculationSeries> BuildDependencies(CalculationSeries cs)
        {
            var rval = new List<CalculationSeries>();

            var vars = cs.GetDependendVariables();
            string msg = cs.Table.TableName + " depends on :";

            TimeSeriesName cName = new TimeSeriesName(cs.Table.TableName);

            foreach (var vn in vars)
            {
                TimeSeriesName tn = new TimeSeriesName(vn,cName.interval);
             
                var dependents = list.FindAll(x => x.Table.TableName == tn.GetTableName() );

                foreach (var d in dependents)
                {
                    msg += d.Table.TableName + ",";
                }
                
                rval.AddRange(dependents);
            }

            Logger.WriteLine(msg);

            return rval;
        }

      


        /// <summary>
        /// Sort so the calculations are in the proper order.
        /// </summary>
        /// <returns></returns>
        public CalculationSeries[] Sort()
        {
            Logger.WriteLine("List before Sorting...");
            foreach (var item in list)
            {
                Logger.WriteLine(item.Table.TableName);
            }
            Logger.WriteLine("--end before sort --");

            
            var rval = TSort(list, BuildDependencies);

            Console.WriteLine("============  --- Begin Sorted  ---   ================");
            foreach (var item in rval)
            {
                Console.WriteLine(item.Name + " = '" + item.Expression + "'");
            }

            Console.WriteLine("============  ---  End - Sorted  ---   ================");

            return rval.ToArray();
        }

        /// <summary>
        /// Topological Sorting
        ///  http://stackoverflow.com/questions/4106862/how-to-sort-depended-objects-by-dependency
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="dependencies"></param>
        /// <returns></returns>
        private IEnumerable<T> TSort<T>(IEnumerable<T> source, Func<T, IEnumerable<T>> dependencies)
        {
            var sorted = new List<T>();
            var visited = new HashSet<T>();

            foreach (var item in source)
            {
                Console.WriteLine((item as CalculationSeries).Name);
                Visit(item, visited, sorted, dependencies);
            }

            return sorted;
        }

        private static void Visit<T>(T item, HashSet<T> visited, List<T> sorted, Func<T, IEnumerable<T>> dependencies)
        {
            if (!visited.Contains(item))
            {
                visited.Add(item);

                foreach (var dep in dependencies(item))
                    Visit(dep, visited, sorted, dependencies);

                sorted.Add(item);
            }
            else if (!sorted.Contains(item))
            {
                throw new Exception("Invalid dependency cycle! " + "missing item '" +( item  as CalculationSeries).Name+"'"); 
            
            }
        }


        /// <summary>
        /// inputDictionary contains CalculationSeries
        /// that use the input (key) tableName as input to the calculation.
        /// </summary>

        Dictionary<string, List<CalculationSeries>> inputDictionary;

        /// <summary>
        /// Return list of all calculations that may need to be performed
        /// based on this inputSeries
        /// </summary>
        /// <param name="inputSeries"></param>
        /// <returns></returns>
        public SeriesList LookupCalculations(Series inputSeries)
        {
            var rval = new SeriesList();

            if (inputDictionary == null)
            {
                inputDictionary = new Dictionary<string, List<CalculationSeries>>();
                foreach (CalculationSeries cs in list)
                {
                    var vars = cs.GetDependendVariables();
                    foreach (var varName in vars)
                    {
                        TimeSeriesName tn = new TimeSeriesName(varName, inputSeries.TimeInterval);
                        AddToDictionary(tn.GetTableName(), cs);    
                    }
                }
            }

            Logger.WriteLine("LookupCalculations(" + inputSeries.Name + ")");
            Logger.WriteLine("inputDictionary.Count = " + inputDictionary.Count);

            if (this.inputDictionary.ContainsKey(inputSeries.Table.TableName))
            {
                rval.AddRange(inputDictionary[inputSeries.Table.TableName].ToArray());
            }

            return rval;
        }
        private void AddToDictionary(string key, CalculationSeries cs)
        {
            if (inputDictionary.ContainsKey(key))
            {
                var items = inputDictionary[key];

                if (!items.Contains(cs))
                    items.Add(cs);
            }
            else
            {
                var items = new List<CalculationSeries>();
                items.Add(cs);
                inputDictionary.Add(key, items);
            }
        }

    }
}