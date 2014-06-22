using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Reclamation.Core;

namespace Reclamation.TimeSeries.Usace
{
    /// <summary>
    /// United States Corps of Engineers (Usace)
    /// </summary>
    public class UsaceSeries : Series
    {
        //http://www.nwd-wc.usace.army.mil/perl/dataquery.pl?k=id%3ADWR%2Brecord%3A%2F%2FDWR%2FHF%2F%2FIR-MONTH%2FDRXZZAZD%2F%2Bpk%3Adworshak&sd=10&sm=OCT&sy=2007&ed=20&em=OCT&ey=2007&of=Text+Comma-Delimited&et=Screen&dc=One+Column&snpt=Daily&snph=00&snpw=15&f1m=OCT&f1d=20&f2sm=OCT&f2sd=20&f2em=OCT&f2ed=20&f3c=Less+Than+Or+Equal+To&f3t=


        string m_path; //   //CHJ/YT//IR-MONTH/IRVZZBZD/

        public UsaceSeries(TimeSeriesDatabase db, TimeSeriesDatabaseDataSet.SeriesCatalogRow sr)
            : base(db, sr)
        {
            m_path = ConnectionStringUtility.GetToken(sr.ConnectionString, "DssPath", "");
            ExternalDataSource = true;
            
        }

        
        public UsaceSeries(string path )
        {
            m_path = path;
            ExternalDataSource = true;
            var dsspath = new Hec.HecDssPath(path);
            DataQueryInfo info = new DataQueryInfo(dsspath);
            Name = info.Name;
            Parameter = info.Parameter;
            Units = info.Units;
            ConnectionString = "DssPath=" + m_path;
            base.Provider = "UsaceSeries";
            Source = "hecdss"; // for icon name
           
        }

       

        protected override void ReadCore()
        {
            Read(DateTime.Now.AddDays(-5), DateTime.Now);
        }


        
        protected override void ReadCore(DateTime t1, DateTime t2)
        {
            //  http://www.nwd-wc.usace.army.mil/perl/dataquery.pl?k=id%3ACHJ%2Brecord%3A%2F%2FCHJ%2FYT%2F%2FIR-MONTH%2FIRVZZBZD%2F%2Bpk%3Achj&sd=23&sm=MAY&sy=2011&ed=24&em=MAY&ey=2011&of=HTML&et=Screen&dc=One+Column&snpt=Daily&snph=00&snpw=15&f1m=MAY&f1d=24&f2=on&f2sm=MAY&f2sd=19&f2em=MAY&f2ed=24&f3c=Less+Than+Or+Equal+To&f3t=
            //http://www.nwd-wc.usace.army.mil/perl/dataquery.pl?k=id%3ACHJ%2Brecord%3A%2F%2FCHJ%2FYT%2F%2FIR-MONTH%2FIRVZZBZD%2F%2Bpk%3Aid.chj&sd=23&sm=MAY&sy=2011&ed=24&em=MAY&ey=2011&of=Text+Comma-Delimited&et=Screen&dc=One+Column&snpt=Daily&snph=00&snpw=15&f1m=MAY&f1d=24&f2=on&f2sm=MAY&f2sd=15&f2em=MAY&f2ed=24&f3c=Less+Than+Or+Equal+To&f3t=
            //record://los/hf//ir-month/drxzzazd/ time:-1m

            string url = "http://www.nwd-wc.usace.army.mil/perl/dataquery.pl?k=id:hydromet@usbr.gov+record:";
            url += m_path + "&sd=01&sm=JAN&sy=2007&ed=23&em=FEB&ey=2007&of=Text+Comma-Delimited";

            url = ApplyDatesToURL(t1, t2, url);

            string[] results = Reclamation.Core.Web.GetPage(url);

            TextFile tf = new TextFile(results);

            string msg = "<P>The web server is too loaded right now to service your request. Please wait a few minutes and try your request again. ";
            int idx = tf.IndexOf(msg);
            if (idx >= 0)
                throw new ApplicationException(msg);


            string pattern = @"(?<date>\d{1,2}[A-Za-z]{3}\d{4}),(?<hour>\d\d:\d\d),(?<value>[+-]?([0-9]*\.?[0-9]+|[0-9]+\.?[0-9]*)([eE][+-]?[0-9]+)?)";
            Regex re = new Regex(pattern, RegexOptions.Compiled);
            Console.WriteLine(pattern);
            for (int i = 0; i < results.Length; i++)
            {
                DateTime t = DateTime.MinValue;
                double val = Point.MissingValueFlag;
                Match m = re.Match(results[i]);
                if (m.Success) 
                {
                    GroupCollection g = m.Groups;

                    if (g["hour"].Value == "24:00")
                    {
                        if( DateTime.TryParse(g["date"].Value, out t)
                        && double.TryParse(g["value"].Value, out val))
                    {
                        t = t.AddDays(1);
                        AddVal(t, val);
                    }

                    }
                    else
                    if (DateTime.TryParse(g["date"].Value+" "+g["hour"].Value, out t)
                        && double.TryParse(g["value"].Value, out val))
                    {
                        AddVal(t, val);
                        
                    }
                    else
                    {
                        SkipMessage("skipping " + results[i]);
                    }
                }
                else
                {
                    SkipMessage("skipping " + results[i]);
                }
                 //   Console.WriteLine(results[i]);
            }
        }

        public void AddVal(DateTime t, double val)
        {
            if (val == -901.0 || val == -902.0)
                AddMissing(t);
            else
            {
                Add(t, val);
            }
        }
        
        private void SkipMessage(string msg)
        {
            Messages.Add(msg);
            Logger.WriteLine(msg);
        }

        //http://www.nwd-wc.usace.army.mil/perl/dataquery.pl?k=id:DWR+record://DWR//HF//IR-MONTH/DRXZZAZD//+pk+dworshak

        //k=id%3ADWR%2Brecord%3A%2F%2FDWR%2FHF%2F%2FIR-MONTH%2FDRXZZAZD%2F%2Bpk%3Adworshak
        //id:DWR+record://DWR/HF//IR-MONTH/DRXZZAZD/+pk:dworshak
        //k=id:DWR+record://DWR//HF//IR-MONTH/DRXZZAZD//+pk+dworshak

        //&sd=10      // start day
        //&sm=OCT     // start month
        //&sy=2007   // start year
        //&ed=20   // ending day
        //&em=OCT   // ending month
        //&ey=2007           // ending year 
        //&of=Text+Comma-Delimited  // format
        //&et=Screen   // ouput to screen
        //&dc=One+Column  // number of columns
        //--- try to ignore all below
        //&snpt=Daily    // snap to
        //&snph=00       // snap  base hour
        //&snpw=15       // snap window minutes
        //&f1m=OCT       // extract month
        //&f1d=20        // extract day
        //&f2sm=OCT      // extract range month
        //&f2sd=20       // extract range day
        //&f2em=OCT      // extract range month
        //&f2ed=20       // extract range day2
        //&f3c=Less+Than+Or+Equal+To //filter tpye
        //&f3t=    // filter value
        //

        private static string ApplyDatesToURL(DateTime t1, DateTime t2, string url)
        {

            //Console.WriteLine("\n" +m_path);
            string rval = Regex.Replace(url, @"sd=\d{1,2}", "sd=" + t1.Day.ToString());
            rval = Regex.Replace(rval, @"sm=[A-Za-z]{3}", "sm=" + t1.ToString("MMM"));
            rval = Regex.Replace(rval, @"sy=\d{4}", "sy=" + t1.Year.ToString());

            rval = Regex.Replace(rval, @"ed=\d{1,2}", "ed=" + t2.Day.ToString());
            rval = Regex.Replace(rval, @"em=[A-Za-z]{3}", "em=" + t2.ToString("MMM"));
            rval = Regex.Replace(rval, @"ey=\d{4}", "ey=" + t2.Year.ToString());

            return rval;

            //Console.WriteLine("\n" + m_path);
        }


        /*
         * 
DWR : Dworshak Reservoir Near Ahsahka
Forebay Elevation (FT), Daily, Manual Collection (HFDRXZZAZD)

Gage Zero: 1,445 FT Latitude: 46 degrees 31' 12" N Longitude: 116 degrees 16' 48" W 
USGS-ID: 13340950 
Country: USA 
----------------------------------------------------------------------
DISCLAIMER:This data has not been verified and may contain bad and/or missing data. These data are only provisional and subject to revision and significant change and are not citeable until reviewed and approved by the Agency responsible for collection. Revision of real-time streamflow and water quality data may occur after review because the stage-discharge relationship may have altered or the water quality probe calibrations indicated a correction. Missing data may be denoted by a -901.0 or -902.0 value.

Date,     Time, Data
10OCT2007,24:00,1518.240
11OCT2007,24:00,1518.180
12OCT2007,24:00,1518.110
13OCT2007,24:00,1518.020
14OCT2007,24:00,1517.940
15OCT2007,24:00,1517.800
16OCT2007,24:00,1517.650
17OCT2007,24:00,1517.570
18OCT2007,24:00,1517.510
19OCT2007,24:00,1517.500

----------------------------------------------------------------------

         */
    }
}
