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
            if( m_db != null)
                m_db.Truncate(this.ID);

            Calculate(TimeSeriesDatabase.MinDateTime, TimeSeriesDatabase.MaxDateTime);
        }

        internal List<string> GetDependentVariables()
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

            return rval;
        }



        public virtual void Calculate(DateTime t1, DateTime t2)
        {

            var t1a = t1;
            var t2a = t2;

            Logger.OnLogEvent += Logger_OnLogEvent;

            var seriesBeforeCalc = this.Clone();
            //if( this.TimeInterval == TimeSeries.TimeInterval.Irregular)
               t2a = t2.AddDays(1); // we may need midnight value in the next day.

               if (this.TimeInterval == TimeSeries.TimeInterval.Daily)
               {
                   // daily_wrdo_pu needs AdjustStartingDate
                   t1a = this.AdjustStartingDateFromProperties(t1, t2a);// DO DO??? needed??
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

                if (result != null && ( result.IsDouble || result.IsInteger ))
                { // constant expression, need to convert to a Series.
                    Series constant = new Series();
                    constant.TimeInterval = this.TimeInterval;
                    constant = Math.FillMissingWithZero(constant, t1, t2);
                    if( result.IsDouble)
                      constant = Math.Add(constant, result.Double);
                    else
                      constant = Math.Add(constant, result.Integer);
                    result = new ParserResult(constant);
                }

                if (result != null &&  result.IsSeries)
                {
                    result.Series.Trim(t1, t2);
                    //var s = result.Series;
                    //result.Series = Math.Subset(s, t1, t2); // trim extra data used for calculations

                    Logger.WriteLine("Calculation result has " + result.Series.Count + " rows");
                    this.TimeInterval = result.Series.TimeInterval;
                    string tableName = this.Table.TableName;
                    
                    this.Table = result.Series.Table;
                    this.Table.TableName = tableName;
                    if (m_db != null)
                    {
                       // Logger.WriteLine("Setting Flags");
                        m_db.Quality.SetFlags(this);
                    }

                    this.Table.AcceptChanges();// prevents error releated to Deleted rows from Trim() above.
                    foreach (DataRow row in this.Table.Rows)
                    {
                        row.SetAdded(); // so database will insert these rows.
                    }
                    if (m_db != null) // database is not required for calculations.
                    {

                        //bool canSave = m_db.Server.HasSavePrivilge(this.Table.TableName);
                      //  if(canSave)
                        m_db.SaveTimeSeriesTable(this.ID, this, DatabaseSaveOptions.UpdateExisting);
                        Expression = tmpExpression;

                        if (seriesBeforeCalc.TimeInterval != this.TimeInterval
                            || seriesBeforeCalc.Units != this.Units)
                        {
                            Logger.WriteLine("Warning Units or interval has changed.");
                            //if(canSave)
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
        /// siteid.columnName becomes table lookup in sitecatalog
        /// %property%.name  is a table lookups in seriesproperties
        /// such as  jck.elevation replaced with 6789.00
        /// %property%.shift looks up the shift value 
        /// </summary>
        private string ExpressionPreProcessor()
        {
            var rval = Expression;
            rval = AddSiteData(rval);
            rval = AddSeriesPropertyData(rval);

            //Logger.WriteLine("Expression after preprocessor:");
            //Logger.WriteLine(rval);
            return rval;
        }


        /// <summary>
        /// Replaces items like %property%.shift
        /// with value in seriesproperty table, using name column for value
        /// and  id for seriesid.
        /// </summary>
        /// <param name="rval"></param>
        /// <returns></returns>
        private string AddSeriesPropertyData(string rval)
        {
            string pattern = "(?<!\")(?<prop>%property%)\\.(?<name>[a-zA-Z]+[0-9]*)";

            var m = Regex.Match(rval, pattern);
            while (m.Success )
            {
                string name = m.Groups["name"].Value;
                if (this.Properties.Contains(name))
                {
                    var val = Properties.Get(name);
                    if( val != "")
                     rval = rval.Replace(m.Groups[0].Value,val );
                }
                
                m = Regex.Match(rval, pattern);
            }

            return rval;
        }

        static TimeSeriesDatabaseDataSet.sitecatalogRow s_prevSiteRow = null;

        private string AddSiteData(string rval)
        {
            // check for %site%.elevation or other lookups in sitecatalog table
            //           abei.latitude

            if (SiteID == "")
                return rval;

            rval = rval.Replace("%site%", SiteID); // TO DO.. SiteID 


            string pattern = "(?<!\")(?<siteid>" + SiteID + ")\\.(?<column>[a-zA-Z]+)";
            var m = Regex.Match(rval, pattern);
            while (m.Success && m_db != null)
            {
                TimeSeriesDatabaseDataSet.sitecatalogRow site;
                if (s_prevSiteRow != null && s_prevSiteRow.siteid == SiteID)
                    site = s_prevSiteRow; // save DB call
                else
                {
                    site = m_db.GetSiteRow(SiteID);
                    s_prevSiteRow = site;
                }

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
