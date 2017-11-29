using Reclamation.Core;
using Reclamation.TimeSeries;
using System;
using System.Collections.Generic;

namespace PiscesWebServices.CGI
{
     static class IdwrCustom
    {

        /// <summary>
        /// returns series that have idwr_cbtt set in the siteproperites
        /// and have a idwr_shef set in the seriesproperties.
        /// Also sets the CustomNames property in the ShefFormatter
        /// example url : instant?custom_list=idwr&format=shefa
        /// </summary>
        /// <param name="svr"></param>
        /// <returns></returns>
        internal static List<TimeSeriesName> GetIDWRInstantList(BasicDBServer svr)
        {
            List<TimeSeriesName> rval = new List<TimeSeriesName>();
            var sql = @"
select s.tablename,p.value parameter,sitep.value siteid ,

case(c.timezone)
  when 'US/Mountain' then 'M'
  when 'US/Pacific' then 'P'
  end as timezone
from seriescatalog s 
    join seriesproperties p on s.id = p.seriesid 
    join siteproperties sitep on sitep.siteid =s.siteid
    join sitecatalog c on c.siteid = s.siteid 
where p.name ='idwr_shef' and sitep.name='idwr_cbtt'
and timeinterval = 'Irregular'
";
            var tbl = svr.Table("idwr", sql);

            ShefAFormatter.CustomNames = tbl;

            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                var tn = new TimeSeriesName(tbl.Rows[i]["tablename"].ToString());
                rval.Add(tn);
            }
            return rval;
        }

        /// <summary>
        /// Returns a list of IDWR series that have  set
        /// in the seriesproperties.
        /// </summary>
        /// <param name="svr"></param>
        /// <returns></returns>
        internal static List<TimeSeriesName> GetIDWRDailyList(BasicDBServer svr, string customList)
        {
            string[] allowed = { "wd01", "wd63", "wd65" };
            customList = customList.ToLower();
            if (Array.IndexOf(allowed, customList) < 0)
            {
                Logger.WriteLine("invalid water district:" + customList);
                return new List<TimeSeriesName>();
             }


            List<TimeSeriesName> rval = new List<TimeSeriesName>();
            var sql = @"
            select s.tablename, p.value siteid ,s.parameter
            from seriescatalog s
                join seriesproperties p on s.id = p.seriesid
                join sitecatalog c on c.siteid = s.siteid
            where timeinterval = 'Daily' ";
           sql += " and p.name ='" + customList + "' order by tablename";


            var tbl = svr.Table("idwr", sql);

            IdwrAccountingFormatter.CustomNames = tbl;

            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                var tn = new TimeSeriesName(tbl.Rows[i]["tablename"].ToString());
                rval.Add(tn);
            }
            return rval;
        }
    }
}
