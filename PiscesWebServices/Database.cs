using Reclamation.Core;
using Reclamation.TimeSeries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PiscesWebServices
{
    /// <summary>
    /// Manage Connection to Pisces Database for webservices
    /// </summary>
    static class Database
    {
        private static TimeSeriesDatabase s_db;
         public static void InitDB(string[] args)
        {
            s_db = TimeSeriesDatabase.InitDatabase(new Arguments(args));
        }

        public static TimeSeriesDatabase DB()
        {
            return s_db;
        }

    }
}
