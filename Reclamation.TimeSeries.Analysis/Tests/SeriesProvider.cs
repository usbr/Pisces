using Reclamation.Core;
using Reclamation.TimeSeries.Hydromet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Reclamation.TimeSeries.Analysis.Tests
{
    class SeriesProvider
    {
        private MemoryCache cache;

        public SeriesProvider()
        {
            cache = new MemoryCache("Reclamation.TimeSeries.Analysis.SeriesProvider");
        }

        public Series FetchInstantFromAPI(string site, string pcode, DateTime start, DateTime end)
        {
            // TODO: Add caching
            Series s = new Series("", TimeInterval.Irregular);
            s.Table = HydrometInstantSeries.Read(site, pcode, start, end, HydrometHost.PNLinux).Table;
            s.Table.TableName = "instant_" + site + "_" + pcode;
            s.Name = s.Table.TableName;

            return s;
        }

        public Series FetchDailyFromAPI(string site, string pcode, DateTime start, DateTime end)
        {
            // TODO: Add caching
            Series s = new Series("", TimeInterval.Daily);
            s.Table = HydrometDailySeries.Read(site, pcode, start, end, HydrometHost.PNLinux).Table;
            s.Table.TableName = "daily_" + site + "_" + pcode;
            s.Name = s.Table.TableName;

            return s;
        }

        public Series FetchFromCSV(string csvUrl)
        {
            // TODO: Add caching
            var fn = FileUtility.GetTempFileName(".csv");
            Web.GetTextFile(csvUrl, fn, false);
            return new TextSeries(fn);
        }
    }
}
