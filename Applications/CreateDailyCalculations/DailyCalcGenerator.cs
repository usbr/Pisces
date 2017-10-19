using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Hydromet;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydrometDailyToPisces
{
    class DailyCalcGenerator
    {

        TimeSeriesDatabase m_db;
        TimeSeriesDatabaseDataSet.sitecatalogDataTable m_sites;
        int newCalcCount = 0;
        int changeEqCount = 0;
        StreamWriter sw;

        public DailyCalcGenerator(TimeSeriesDatabase db, string outputFileName)
        {
            m_db = db;
            m_sites = m_db.GetSiteCatalog();
            sw = new StreamWriter(outputFileName);
        }


        /// <summary>
        /// For each instant Series in the database create an appropirate 
        /// Dailycalculation.
        /// </summary>
        public void AddDailyCalculations(DataTable pcodeLookup, bool dryRun, string[] sites)
        {
            var codes = new List<string>();
            for (int i = 0; i < pcodeLookup.Rows.Count; i++)
            {
                var pc = pcodeLookup.Rows[i]["instantpcode"].ToString();
                codes.Add(pc.ToLower());
            }

            var filter = "select * from seriescatalog a join sitecatalog b on a.siteid=b.siteid "
                  + "where timeinterval= 'Irregular' and b.type <> 'agrimet' ";

            filter += "and parameter in ( '" + String.Join("','", codes.ToArray()) + "')";

            var q = new TimeSeriesDatabaseDataSet.SeriesCatalogDataTable();

            m_db.Server.FillTable(q, filter);

            newCalcCount = 0;
            changeEqCount = 0;


            //foreach (var r in q)
            for (int j = 0; j < q.Rows.Count; j++)
            {
                var r = q[j];
                try
                {
                    if (sites.Length > 0 && Array.IndexOf(sites, r.siteid) == -1 )
                        continue;

                    if (HasCurrentData(r.TableName)) // don't add equations for 'old' data
                    {
                        var calcs = pcodeLookup.Select("InstantPcode ='" + r.Parameter + "'");

                        for (int i = 0; i < calcs.Length; i++)
                        {
                            var dailyPcode = calcs[i]["DailyPcode"].ToString().ToLower();
                            var siteFilter = calcs[i]["siteFilter"].ToString();
                            var expression = calcs[i]["Equation"].ToString();
                            expression = expression.Replace("%site%", r.siteid);
                            var name = r.siteid + "_" + dailyPcode;
                            TimeSeriesName tn = new TimeSeriesName(name, "daily");

                            if (m_sites.Select("siteid='" + r.siteid + "'").Length == 0)
                            {
                                Console.WriteLine("Warning: skipping site not cataloged: " + r.siteid);
                                continue;
                            }

                            // remove property--> program:hydromet (prevents data flow from pnhyd0)
                            RemoveProgramProperty(tn);

                            if (!FilterAllows(r.siteid, siteFilter)) // config spreasheet limits i.e. responsibility='IDWR'
                                continue;

                            if (m_db.TableNameInUse(tn.GetTableName()))
                            { // already defined in some manner.
                                ConvertToDailyEquation(tn, expression, dryRun);
                            }
                            else
                            {
                                CalculationSeries s = CreateCalcSeries(r, name, tn, expression);
                                newCalcCount++;
                                if (!dryRun)
                                    AddCalcSeries(s);
                            }

                            // check if this equation matches Hydromet Legacy.

                            if (!dryRun)
                                CompareWithVMS(tn);

                        }
                    }

                }
                catch (Exception exc)
                {
                    sw.WriteLine("Error:"+exc.Message);
                }
                if (j % 100 == 0)
                    Console.WriteLine(j + " of " + q.Count);
            }


            sw.WriteLine("New equations: " + newCalcCount);
            sw.WriteLine("modified to Equations :" + this.changeEqCount);
            sw.Close();
        }

        private void RemoveProgramProperty(TimeSeriesName tn)
        {
            var x = m_db.GetSeriesCatalog("tablename ='" + tn.GetTableName() + "'");
            if (x.Count == 0)
                return;

            var row = x[0];

            var props = m_db.GetSeriesProperties(true);

            props.Delete("program", row.id);
            props.Save();

        }

        private void CompareWithVMS(TimeSeriesName tn)
        {
            try
            {
                var cs = m_db.GetCalculationSeries(tn.siteid, tn.pcode, tn.GetTimeInterval());
                DateTime t = DateTime.Now.Date.AddDays(-1);
                cs.Calculate(t, t);

                HydrometDailySeries hs = new HydrometDailySeries(tn.siteid, tn.pcode);
                hs.Read(t, t);

                if (cs.Count == 0)
                {
                    sw.WriteLine("Error: no data computed " + tn.siteid + " " + tn.pcode);
                    return;
                }
                var diff = cs - hs;


                var x = Reclamation.TimeSeries.Math.Sum(diff);
                var percent = System.Math.Round(x, 2) / System.Math.Round(hs[0].Value, 2) * 100.0;
                double absDiff = System.Math.Abs(cs[0].Value - hs[0].Value);

                if (System.Math.Abs(percent) > 0.05)
                {
                    var msg = "Stats, " + tn.Name + ", " + percent + " , hyd0," + hs[0].Value + " , hyd1, " + cs[0].Value + ", " + absDiff;
                    sw.WriteLine(msg);
                    Console.WriteLine(msg);
                }
            }
            catch (Exception e)
            {
                sw.WriteLine("Error:"+e.Message);
                throw;
            }
        }

        /// <summary>
        /// Convert to equation 
        /// </summary>
        /// <param name="tn"></param>
        /// <param name="expression"></param>
        /// <param name="dryRun"></param>
        private void ConvertToDailyEquation(TimeSeriesName tn, string expression, bool dryRun)
        {
            Reclamation.TimeSeries.TimeSeriesDatabaseDataSet.SeriesCatalogDataTable x;
            x = m_db.GetSeriesCatalog("tablename ='" + tn.GetTableName() + "'");
            var row = x[0];

            if (row.Provider != "CalculationSeries")
            {
                //Console.WriteLine("change to equation: "+expression);
                row.Provider = "CalculationSeries";
                if (row.Expression.Trim() != "")
                    throw new Exception("ooops .. an equation is already in place.");
                row.Expression = expression;
                if (!dryRun)
                    m_db.Server.SaveTable(x);

                sw.WriteLine("Changing " + row.TableName + "  from Series to CalculationSeries   " + expression);
                changeEqCount++;
            }

        }

        private bool FilterAllows(string siteid, string siteFilter)
        {
            if (siteFilter.Trim() != "")
            {
                var sql = "siteid = '" + siteid + "' and " + siteFilter;
                var siteRows = m_sites.Select(sql);
                if (siteRows.Length == 0)
                    return false;
            }
            return true;
        }


        private void AddCalcSeries(CalculationSeries s)
        {
            m_db.AddSeries(s);
        }

        private  CalculationSeries CreateCalcSeries(TimeSeriesDatabaseDataSet.SeriesCatalogRow r, string name, TimeSeriesName tn, string expression)
        {
            var s = new CalculationSeries(name)
            {
                Parameter = tn.pcode,
                SiteID = r.siteid,
                Expression = expression
            };
            s.Table.TableName = tn.GetTableName();
            s.TimeInterval = tn.GetTimeInterval();

            sw.WriteLine("Created: " + tn.GetTableName() + " = " + expression);
            return s;
        }

        private bool HasCurrentData(string tableName)
        {
            if (m_db.Server.TableExists(tableName))
            {
                var sql = " select * from " + tableName + " order by datetime desc limit 1";
                var tbl = m_db.Server.Table(tableName, sql);
                if (tbl.Rows.Count == 1 && tbl.Rows[0][0] != DBNull.Value)
                {
                    DateTime t = (DateTime)tbl.Rows[0][0];

                    if (t.Year == DateTime.Now.Year)
                        return true;
                }
            }
            //SELECT * from pn_daily_andi_qd order by datetime desc limit 1
            return false;
        }

    }
}
