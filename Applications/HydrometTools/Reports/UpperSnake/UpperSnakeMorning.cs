using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Reclamation.Core;
using Reclamation.TimeSeries.Hydromet;

namespace HydrometTools.Reports.UpperSnake
{
    public partial class UpperSnakeMorning : UserControl
    {
        public UpperSnakeMorning()
        {
            InitializeComponent();
        }

        public void LoadData()
        {
  //This section loads the data from hydromet with Karl Tarbet libraries
                //This code was modified for performance do to the slow loading time to load
                //All data in at once in a group then in the loop extract out the individual cbtt's,pcode's
                Performance perf = new Performance();
                // 7.5 seconds (from home with cache)
                // 20.8 seconds (from home )
                String[] cbtt = { "jck q", "jck fb", "jck af", "flgy q", "pcky q", "bfky q", "jksy q", "alpy q", "grs fb", "grs af", "pal fb", "pal af", "pali q", "wtxi q", "rir fb", "rir af", "riri q", "isl fb", "isl af", "isli q", "hen fb", "hen af", "heni q", "amf fb", "amf af", "amfi q", "min fb", "min af", "mini q", "mil fb", "mili q", "mhpi q","nmci qc", "wod fb", "wod af", "wodi q","smci qc" };
                Dictionary<string, double> today = new Dictionary<string, double>();
                Dictionary<string, double> yesterday = new Dictionary<string, double>();

                string query = String.Join(",", cbtt);
                var cache = new HydrometDataCache();
                DateTime t = DateTime.Now;
                t = new DateTime(t.Year, t.Month, t.Day, 6, 0, 0);
                var t2 = t.AddDays(-1);

                var t3 = DateTime.Now.AddDays(-2);

                cache.Add(cbtt, t3, t, HydrometHost.PN, Reclamation.TimeSeries.TimeInterval.Irregular);
                HydrometInstantSeries.Cache = cache;

  
                for (int i = 0; i < cbtt.Length; i++)
                {
                    var c = cbtt[i].Split(' ')[0];
                    var p = cbtt[i].Split(' ')[1];

                    HydrometInstantSeries s = new HydrometInstantSeries(c, p);
                    s.Read(t3, DateTime.Now); // read three days

                    if (s.IndexOf(t) >= 0)
                        today.Add(cbtt[i], s[t].Value);
                    else
                        today.Add(cbtt[i], Reclamation.TimeSeries.Point.MissingValueFlag);
                    if( s.IndexOf(t2)>=0)
                      yesterday.Add(cbtt[i], s[t2].Value);
                    else
                        yesterday.Add(cbtt[i], Reclamation.TimeSeries.Point.MissingValueFlag);
                }
                //Load the data into the user controls only loading 6:00 AM data 
                //from today and yesterday
                jacksonuc.LoadData(today, yesterday);
                palisadesuc.LoadData(today, yesterday);
                ririeuc.LoadData(today, yesterday);
                islandparkuc.LoadData(today, yesterday);
                americanfallsuc.LoadData(today, yesterday);
                minidokauc.LoadData(today, yesterday);
                milneruc.LoadData(today, yesterday);
                littlewooduc.LoadData(today, yesterday);
                perf.Report();
               
            }
    
    }
}
