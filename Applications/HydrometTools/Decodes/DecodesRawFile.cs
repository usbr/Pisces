using HydrometTools.Decodes;
using Reclamation.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Reclamation.TimeSeries.Decodes
{
    /// <summary>
    /// Manage DECODES raw format output file
    /// Extracts POWER,and LENERR, TIMEERR, MSGLEN
    /// </summary>
    public class DecodesRawFile
    {

        public static bool IsValidFile(TextFile tf)
        {
            return tf.Length > 0 && tf[0].Length > 37
                && Regex.IsMatch(tf[0], "[A-F0-9]{8}");

        }

        TextFile tf;
        public DecodesRawFile(TextFile tf)
        {
            this.tf = tf;
        }

        Dictionary<string, Series> seriesDict = new Dictionary<string, Series>();

        public SeriesList ToSeries()
        {
            seriesDict.Clear();   
            

            for (int i = 0; i < tf.Length; i++)
            {
                var msg = GoesMessageDataSet.ParseMessage(tf[i]);
                if (msg != null)
                {
                   var cbttList =SiteLookupMultiple("NESSID", msg.nessid, "SITE");

                   if (cbttList.Length == 0)
                   {
                       Logger.WriteLine("Warning: no cbtt found for "+msg.nessid +"  skipping this messages");
                       continue;
                   }

                   for (int c = 0; c < cbttList.Length; c++)
                   {

                       string cbtt = cbttList[c];
                       int expectedLength = Convert.ToInt32(SiteLookup("NESSID", msg.nessid, "SMSGLEN"));

                       int lenerr = msg.length - expectedLength;

                       var reptime = Convert.ToInt32(SiteLookup("NESSID", msg.nessid, "REPTIM"));
                       var primaryChannel = SiteLookup("NESSID", msg.nessid, "PCHAN");


                       var expectedSeconds = msg.MST.Hour * 3600 + reptime;

                       //DateTime midnight = new DateTime(msg.MST.Year,msg.MST.Month,msg.MST.Day,0,0,0);
                       TimeSpan ts = new TimeSpan(msg.MST.Hour, msg.MST.Minute, msg.MST.Second);
                       var timeerr = ts.TotalSeconds - expectedSeconds;

                       int parity = 0;
                       if (msg.failure == "?")
                           parity = 1;
                       Add(cbtt, "PARITY", msg.MST, parity);
                       Add(cbtt, "POWER", msg.MST, Convert.ToDouble(msg.power));
                       Add(cbtt, "MSGLEN", msg.MST, msg.length);

                       if (msg.channel != primaryChannel)
                           continue;

                       Add(cbtt, "LENERR", msg.MST, lenerr);
                       Add(cbtt, "TIMEERR", msg.MST, timeerr);
                   }
                    
                }

            }
            SeriesList rval = new SeriesList();
            rval.AddRange(seriesDict.Values);
            return rval;
        }

        internal static string[] QualityParameters = new string[] { "PARITY","POWER","MSGLEN","LENERR","TIMEERR"};

        private static string SiteLookup(string keyColumnName, string key, string columnName)
        {
            var site = Hydromet.HydrometInfoUtility.Site;
            if (site.Rows.Count == 0)
                return "";

            DataRow[] rows = site.Select(keyColumnName+"='" + key + "'");
            if (rows.Length == 0)
                return "";
            return rows[0][columnName].ToString();
        }
        private static string[] SiteLookupMultiple(string keyColumnName, string key, string columnName)
        {
            var rval = new List<string>();
            var site = Hydromet.HydrometInfoUtility.Site;
            if (site.Rows.Count == 0)
                return rval.ToArray();

            DataRow[] rows = site.Select(keyColumnName + "='" + key + "'");
            if (rows.Length == 0)
                return rval.ToArray();
            foreach (var item in rows)
            {
                rval.Add(item[columnName].ToString());
            }
            return rval.ToArray();
        }
        
        private Series Add( string cbtt, string pcode, DateTime t, double val)
        {
            string name = "instant_" + cbtt + "_" + pcode;
            name = name.ToLower();

            Series s = null;

            if (this.seriesDict.ContainsKey(name))
            {
                s = seriesDict[name];
            }
            else
            {
                s = new Series();
                this.seriesDict.Add(name, s);
            }

            s.SiteID = cbtt;
            s.Parameter = pcode;
            s.Name = cbtt + "_" + pcode;
            s.Name = s.Name.ToLower();
            
            s.Table.TableName = name;
            s.Add(t, val);

            return s;
        }
        /* Example Message
0EE4B53015007141614G40+1HN066WDR00030`BST@AO@}d@AP@}e@AP@}g@AP@}he
0EE4B53015007131614G40+1HN066WDR00030`BST@AP@}j@AP@}k@AP@}m@AP@}ne
16DBB19415007131805G46-0NN094WFF00030bB1CAl]Al_Al_Al]Al_Al_Al^Al_N
16DBB19415007121805G47-0NN094WFF00030bB1CAl_Al_Al^Al_AlaAl]Al_Al`N
3420872C15007132733G52+0NN172WDR00018bB1L@Jx@Jy@Jx@JxI
3420872C15007122733G51+0NN172WDR00018bB1L@Jy@Jy@Jy@JxI
3443B95815007132225G47+0NN152WDR00078bB1GBXIBXHBXHBXH@Cq@Cq@Cq@Cq@Cb@Ca@C]@CY@FK@FK@FK@FK@EA@EA@EB@EA@Iv@Iv@Iv@IvG

*/
    }
}
