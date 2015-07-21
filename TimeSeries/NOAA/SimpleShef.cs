using Reclamation.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Reclamation.TimeSeries.NOAA
{
    public class SimpleShef
    {

        /// <summary>
        /// Reads shef .A file and extracts all shef locations and codes that match
        /// simple, only reads single parameter per line
        /// </summary>
        /// <param name="filename">input file to read</param>
        /// <param name="shefloc">shef location to search for</param>
        /// <param name="shefCode">shefcode to search for</param>
        /// <returns></returns>
        public static Series ReadSimpleShefA(string filename, string shefloc, string shefCode)
        {
            //.A BRWLD 150309 PD DH2315 /HE1H .00
            //.A BRWLD 150309 PD DH2315 /HE2H .19

            var rval = new Series();
            TextFile tf = new TextFile(filename);

            for (int i = 0; i < tf.Length; i++)
            {
                var m = ShefASimpleRegex.Match(tf[i]);
                if (m.Success)
                {
                    var loc = m.Groups["shefloc"].Value;
                    var sc = m.Groups["shefcode"].Value;
                    var yy = m.Groups["year"].Value;
                    var mm = m.Groups["month"].Value;
                    var dd = m.Groups["day"].Value;
                    var hh = m.Groups["hour"].Value;
                    var min = m.Groups["min"].Value;
                    
                    var val = m.Groups["value"].Value;


                    if (shefCode == sc
                        && loc == shefloc)
                    {
                        int year, month, day, hour,minute;
                        double num = Point.MissingValueFlag; ;
                        if (int.TryParse(yy, out year)
                            && int.TryParse(mm, out month)
                            && int.TryParse(dd, out day)
                            && int.TryParse(hh, out hour)
                            && int.TryParse(min, out minute)
                            && double.TryParse(val,out num))
                        {
                            var t = new DateTime(year + 2000, month, day, hour, minute,0);
                            if( rval.IndexOf(t)>=0)
                            {
                                Console.WriteLine("Skipping duplicate "+t.ToString()+" :" +num);
                                continue;
                            }
                            rval.Add(t, num);
                        }
                    }
                }
            }

            return rval;
        }

        /// <summary>
        ///  Regular expression built for C# on: Tue, Mar 10, 2015, 03:28:27 PM
        ///  Using Expresso Version: 3.0.4750, http://www.ultrapico.com
        ///  
        ///  A description of the regular expression:
        ///  
        ///  \.A\s+
        ///      Literal .
        ///      A
        ///      Whitespace, one or more repetitions
        ///  [shefloc]: A named capture group. [\w+]
        ///      Alphanumeric, one or more repetitions
        ///  Whitespace, one or more repetitions
        ///  [year]: A named capture group. [\d{2}]
        ///      Any digit, exactly 2 repetitions
        ///  [month]: A named capture group. [\d{2}]
        ///      Any digit, exactly 2 repetitions
        ///  [day]: A named capture group. [\d{2}]
        ///      Any digit, exactly 2 repetitions
        ///  Whitespace, one or more repetitions
        ///  [timezone]: A named capture group. [\w+]
        ///      Alphanumeric, one or more repetitions
        ///  \s+DH
        ///      Whitespace, one or more repetitions
        ///      DH
        ///  [hour]: A named capture group. [\d{2}]
        ///      Any digit, exactly 2 repetitions
        ///  [min]: A named capture group. [\d{2}]
        ///      Any digit, exactly 2 repetitions
        ///  \s+/
        ///      Whitespace, one or more repetitions
        ///      /
        ///  [shefcode]: A named capture group. [\w+]
        ///      Alphanumeric, one or more repetitions
        ///  Whitespace, one or more repetitions
        ///  [value]: A named capture group. [[0-9\.\-\+]+]
        ///      Any character in this class: [0-9\.\-\+], one or more repetitions
        ///  
        ///
        /// </summary>
        public static Regex ShefASimpleRegex = new Regex(
              "\\.A\\s+(?<shefloc>\\w+)\\s+(?<year>\\d{2})(?<month>\\d{2})(" +
              "?<day>\\d{2})\\s+(?<timezone>\\w+)\\s+DH(?<hour>\\d{2})(?<mi" +
              "n>\\d{2})\\s+/(?<shefcode>\\w+)\\s+(?<value>[0-9\\.\\-\\+]+)",
            RegexOptions.Multiline
            | RegexOptions.CultureInvariant
            | RegexOptions.Compiled
            );

    }
}
