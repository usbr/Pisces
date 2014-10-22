using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reclamation.TimeSeries.Parser;
using Reclamation.Core;
using System.Text.RegularExpressions;
using System.Data;

namespace Reclamation.TimeSeries
{

    
    
    /// <summary>
    /// A CalculationSeries extends Series to use the expression 
    /// property as a mathmatical equation.
    /// </summary>
    public class CalculationSeries: Series
    {
        public CalculationSeries(TimeSeriesDatabase db,TimeSeriesDatabaseDataSet.SeriesCatalogRow sr):base(db,sr)
        {
            m_parser = db.Parser;
            m_db = db;
            Init();
        }
        public CalculationSeries(TimeSeriesDatabase db)
        {
            m_parser = db.Parser;
            m_db = db;
            Init();
        }

        public CalculationSeries(string name = "")
        {
            Init();
            Name = name;
            Table.TableName = name;
        }
        private void Init()
        {
            Source = "sum";
            Provider = "CalculationSeries";
        }
        
       
        public void Calculate()
        {
            Calculate(TimeSeriesDatabase.MinDateTime, TimeSeriesDatabase.MaxDateTime);
        }

        ///// <summary>
        ///// Returns a list of TimeSeriesNames that this calculation
        ///// explicitly depends on (not looking recersive)
        ///// </summary>
        ///// <returns></returns>
        //public TimeSeriesName[] GetDependentTimeSeriesNames()
        //{
        //    List<TimeSeriesName> rval = new List<TimeSeriesName>();

        //    var vars = GetDependendVariables();
        //    foreach (var item in vars)
        //    {
        //        rval.Add(new TimeSeriesName(item));   
        //    }
        //    return rval.ToArray();
        //}

        internal string[] GetDependentVariables()
        {
            List<string> rval = new List<string>();
            string equation = ExpressionPreProcessor();

            foreach (var n in VariableParser.Default().GetAllVariables(equation))
            {
                if (this.Table.TableName == n)
                {
                    Logger.WriteLine("warning: possible recursive dependency: "+n);
                    continue;
                }

                if (rval.Contains(n))
                    continue;

                rval.Add(n);
            }

            return rval.ToArray();
        }



        public void Calculate(DateTime t1, DateTime t2)
        {

            var t1a = t1;
            var t2a = t2;

           

            Logger.OnLogEvent += Logger_OnLogEvent;

            var seriesBeforeCalc = this.Clone();
            //if( this.TimeInterval == TimeSeries.TimeInterval.Irregular)
               t2a = t2.AddDays(1); // we may need midnight value in the next day.

               // for example daily QU calculations default back 7 days (when running previous day)
               if (Properties!= null &&  Properties.Contains("DaysBack") ) // && t2.Date == DateTime.Now.AddDays(-1).Date)
               {
                   var daysBack = Convert.ToInt32(Properties.Get("DaysBack","0"));
                   t1a = t1a.AddDays(-daysBack);
               }
            
            
            Exception error = new Exception();

            if (Expression != null && Expression.Trim() != "")
            {
                string tmpExpression = Expression;

                Logger.WriteLine("begin Calculate()");

                
                Expression = ExpressionPreProcessor();

                ParserResult result = null;
                try
                {
                    result = Parser.Evaluate(this.Expression, t1a, t2a, this.TimeInterval);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Logger.WriteLine(e.Message, "ui");
                    Messages.Add(e.Message);
                    error = e;
                    result = null;
                }


                if (result != null &&  result.IsSeries)
                {
                    result.Series.Trim(t1, t2);
                    //var s = result.Series;
                    //result.Series = Math.Subset(s, t1, t2); // trim extra data used for calculations

                    Logger.WriteLine("Calculation result has " + result.Series.Count + " rows");
                    this.TimeInterval = result.Series.TimeInterval;
                    this.Units = result.Series.Units; 

                    this.Table = result.Series.Table;
                    this.Table.AcceptChanges();// prevents error releated to Deleted rows from Trim() above.
                    foreach (DataRow row in this.Table.Rows)
                    {
                        row.SetAdded(); // so database will insert these rows.
                    }
                    if (m_db != null) // database is not required for calculations.
                    {
                        m_db.SaveTimeSeriesTable(this.ID, this, DatabaseSaveOptions.UpdateExisting);
                        Expression = tmpExpression;

                        if (seriesBeforeCalc.TimeInterval != this.TimeInterval
                            || seriesBeforeCalc.Units != this.Units)
                        {
                            Logger.WriteLine("Warning Units or interval has changed.");
                            m_db.SaveProperties(this); // time interval, units, are dependent on calculation.
                        }
                    }
                   
                }
                else
                {
                    Clear();
                    Console.WriteLine(error.Message);
                    Logger.WriteLine(error.Message);
                }

                Logger.WriteLine("Calculate() completed");
                Logger.OnLogEvent -= Logger_OnLogEvent;
            }
        }

        void Logger_OnLogEvent(object sender, StatusEventArgs e)
        {
            Messages.Add(e.Message);
        }

        /// <summary>
        /// Expand %site% into specific SiteID
        /// Expand  siteid.columnName  table lookups
        /// such as  jck.elevation replaced with 6789.00
        /// </summary>
        private string ExpressionPreProcessor()
        {
            if (SiteID == "")
                return Expression;

            string rval = Expression.Replace("%site%", SiteID); // TO DO.. SiteID 

            // check for %site%.elevation or other lookups in sitecatalog table
            //           abei.latitude
            string pattern = "(?<!\")(?<siteid>"+ SiteID+")\\.(?<column>[a-zA-Z]+)";
            var m = Regex.Match(rval, pattern);
            while (m.Success && m_db != null)
            {
                var site = m_db.GetSiteRow(SiteID);

                string colName = m.Groups["column"].Value;
                if (site != null && site.Table.Columns.IndexOf(colName) >= 0)
                {
                    rval = rval.Replace(m.Groups[0].Value, site[colName].ToString());
                }
                else
                {
                //    Logger.WriteLine("Error (ExpressionPreprocessor) doing lookup on " + site + "." + colName);
                    break;
                }

                m = Regex.Match(rval, pattern);
            }


            //Logger.WriteLine("Expression after preprocessor:");
            //Logger.WriteLine(rval);
            return rval;
        }

        private SeriesExpressionParser m_parser;

        public SeriesExpressionParser Parser
        {
            get
            {
                if (m_parser == null)
                {
                    if (m_db != null)
                    {
                        //m_parser = m_db.Parser;
                        m_parser = new SeriesExpressionParser(m_db);
                        m_parser.VariableResolver.Add("this", this);
                    }
                    else
                    {
                        m_parser = new SeriesExpressionParser();
                        m_parser.VariableResolver.Add("this", this);
                    }
                }
                return m_parser;
            }
            //set
            //{
            //    m_parser = value;
            //}
        }


        public bool IsValidExpression(string expression, out string errorMessage)
        {
            return Parser.IsValidExpression( expression, out errorMessage);
        }

        public void Test()
        {
            CalculationSeries s = new CalculationSeries();
            SeriesExpressionParser p = s.Parser;
            VariableResolver vr = p.VariableResolver;

        }
    }
}
