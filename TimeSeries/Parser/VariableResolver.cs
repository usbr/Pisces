using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reclamation.Core;
using System.Text.RegularExpressions;

namespace Reclamation.TimeSeries.Parser
{

     public enum  LookupOption { SeriesName, TableName };//, TableNameDaily, TableNameInstant};

    /// <summary>
    /// Basic variable resolver for 
    /// calculation expressions.
    /// This version requires user to add all variables in advance.
    /// </summary>
    public class VariableResolver
    {
         TimeSeriesDatabase db;

         LookupOption m_lookup;
         public LookupOption LookupOption
         {
             get { return m_lookup; }
             set { this.m_lookup = value; }
         }


        public VariableResolver(TimeSeriesDatabase db, LookupOption lookup) 
        {
            this.db = db;
            m_lookup = lookup;
            vars = new Dictionary<string, ParserResult>();
        }

        Dictionary<string, ParserResult> vars;
        public VariableResolver()
        {
            vars = new Dictionary<string, ParserResult>();
        }

        public void Add(string name, Series s)
        {
            vars.Add(name, new ParserResult(s));
        }
        public void Add(string name, ParserResult value)
        {
            vars.Add(name, value);
        }


        /// <summary>
        /// Finds the object or value assoicated with the named variable.
        /// The search matches either  (TableName or SeriesName) based on the LookupOption
        /// 
        /// SeriesName searches that include a name with a prefix  such as daily_ override the
        /// defaultInterval in the search.
        /// 
        /// </summary>
        /// <param name="name">variable name</param>
        /// <param name="defaultInterval">interval used when looking up variable name</param>
        /// <returns></returns>
        public virtual ParserResult Lookup(string name,TimeInterval defaultInterval)
        {

            if (name == "true") // This is not a variable... but a boolean... move this code to SeriesExpressionParser token type.?
                return new ParserResult(true);

            if (name == "false")
                return new ParserResult(false);

            if (vars.ContainsKey(name))
            {
                return vars[name];
            }
            if (db != null)
            {

                TimeSeriesName tn = new TimeSeriesName(name);
                string lookupInterval = "";

                if (tn.HasInterval)
                    lookupInterval = tn.interval;
                else
                    lookupInterval = defaultInterval.ToString().ToLower();

                Series s2 = null;
                if (m_lookup == LookupOption.SeriesName)
                { // drop the prefix (such as daily_) if it exists
                    var search = Regex.Replace(name,"^(daily_)|(monthly_)|(instant_)","");
                    s2 = db.GetSeriesFromName(search, ""); // ignore default interval when using nam..
                    if (s2 == null)
                    {
                        s2 = db.GetSeriesFromName(search, lookupInterval);
                    }
                }
                else
                {
                    if (m_lookup == LookupOption.TableName)
                        s2 = db.GetSeriesFromTableName(name, lookupInterval);
                }
                if (s2 != null)
                return new ParserResult(s2);
            }

            // throw an error....  
            throw new Exception("Error finding variable '" + name + "'");
            //return new ParserResult(name); // returns a string result. ... 

        }

      

    }
}
