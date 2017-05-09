using Reclamation.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries
{
    /// <summary>
    /// TimeSeriesDependency determines dependencies 
    /// and order of dependent calculations 
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

            var vars = cs.GetDependentVariables();
            string msg = cs.Table.TableName + " depends on :";

            TimeSeriesName cName = new TimeSeriesName(cs.Table.TableName);

            foreach (var vn in vars)
            {
                TimeSeriesName tn = new TimeSeriesName(vn,cName.interval);

                if (tn.GetTableName() == cs.Table.TableName)
                {
                    Logger.WriteLine(cs.Expression);
                    Logger.WriteLine("Warning: prevented recursive dependency "+tn.GetTableName());
                    continue;
                }
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
            if (list.Count == 0)
                return new CalculationSeries[] { };

            Logger.WriteLine("List before Sorting...");
            foreach (var item in list)
            {
                Logger.WriteLine(item.Table.TableName);
            }
            Logger.WriteLine("--end before sort --");

            
            var rval = TSort(list, BuildDependencies);

            Console.WriteLine("Sorted Calculations");
            int i = 1;
            foreach (var item in rval)
            {
                Console.WriteLine(i+": "+item.Name + " = '" + item.Expression + "'");
                i++;
            }


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
                //Console.WriteLine((item as CalculationSeries).Name);
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
        public List<CalculationSeries> LookupCalculations(string tableName, TimeInterval interval)
        {
            var rval = new List<CalculationSeries>();

            if (inputDictionary == null)
            {
                inputDictionary = new Dictionary<string, List<CalculationSeries>>();
                foreach (CalculationSeries cs in list)
                {
                    var vars = cs.GetDependentVariables();
                    foreach (var varName in vars)
                    {
                        TimeSeriesName tn = new TimeSeriesName(varName, interval);
                        if( !tn.Valid )
                            Console.WriteLine("Error: Skipped Invalid equation .... "+cs.Expression);
                        else
                        AddToDictionary(tn.GetTableName(), cs);    
                    }
                }
            }

            Logger.WriteLine("LookupCalculations(" + tableName + ")");
            Logger.WriteLine("inputDictionary.Count = " + inputDictionary.Count);

            TimeSeriesName n = new TimeSeriesName(tableName,interval);
            var key = n.GetTableName();
            if (this.inputDictionary.ContainsKey(key))
            {
                rval.AddRange(inputDictionary[key].ToArray());
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
