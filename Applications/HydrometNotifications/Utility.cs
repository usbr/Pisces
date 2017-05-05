using Reclamation.TimeSeries.Hydromet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydrometNotifications
{
    public class Utility
    {

        public static HydrometHost GetHydrometServer()
        {

            string s = ConfigurationManager.AppSettings["HydrometServer"];

          var rval=  HydrometInfoUtility.HydrometServerFromString(s);

          return rval;

        }
    }
}
