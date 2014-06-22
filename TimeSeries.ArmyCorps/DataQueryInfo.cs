using System.Text.RegularExpressions;
using Reclamation.TimeSeries.Hec;
using Reclamation.Core;

namespace Reclamation.TimeSeries.Usace
{
    class DataQueryInfo
    {

        string[] results;
        HecDssPath path;
        TextFile tf;
        string name = "";

        public string Name
        {
            get
            {
                return path.B + " " + path.B + " " + name + " " + Parameter; 
            }
        }
        string parameter = "";

        public string Parameter
        {
            get { return parameter; }
        }
        string units = "";

        public string Units
        {
            get { return units; }
        }
        public DataQueryInfo(HecDssPath path)
        {
            string url = "http://www.nwd-wc.usace.army.mil/perl/dataquery.pl?k=" + path.B;
            results = Reclamation.Core.Web.GetPage(url);
            tf = new TextFile(results);
            this.path = path;
            name = GetName();
            ParseAttributes();
        }


        string GetName()
        {
         
            //<ACRONYM TITLE='This is the station identifier.'>CHJ</ACRONYM></A> : Chief Joseph Dam & Rufus Woods Lake</STRONG>
            string pattern = @"station identifier\.\'>"+path.B+"</ACRONYM></A> : (?<name>.{1,50})</Strong>";
            Regex re = new Regex(pattern, RegexOptions.IgnoreCase);
            
            
            int idx = tf.IndexOfRegex(@"station identifier\.\'>"+path.B+"</ACRONYM></A>");

            if (idx >= 0)
            {
                Match m = re.Match(tf[idx]);
                if (m.Success)
                {
                    return m.Groups["name"].Value;
                }
            }

            return path.B;

        }

        void ParseAttributes()
        {

            int idx = tf.IndexOfRegex(@"<A\sHREF='/perl/dataquery\.pl\?.+" + path.CondensedName);

            if (idx >= 0)
            {
                string pattern = @"<A\sHREF='/perl/dataquery\.pl\?k=id\:"+path.B+@"\+record\:" + path.CondensedName + @"[a-z\:\+]+'>(?<parameter>.{1,50})</A>";
                Regex re = new Regex(pattern, RegexOptions.IgnoreCase);
                var m = re.Match(tf[idx]);
                if (m.Success)
                {
                    //<TR BGCOLOR='#C0FAC0'><TD ><A HREF='/perl/dataquery.pl?k=id:CHJ+record://CHJ/YT//IR-MONTH/IRVZZBZD/+psy:+psm:+psd:+pey:+pem:+ped:+pk:chj'>Total Dissolved Gas (mm of Hg)</A></TD><TD NOWRAP>1999-Present</TD><TD NOWRAP>Instantaneous</TD><TD NOWRAP>Manual/Visual</TD><TD NOWRAP></TD><TD NOWRAP>YTIRVZZBZD</TD></TR>
                    parameter = m.Groups["parameter"].Value;
                    
                    pattern = @"\((.+)\)";
                    var m2 = Regex.Match(parameter, pattern);
                    if (m2.Success)
                        units = m2.Groups[1].Value;

                }
            }

        }

    }
}
