using Reclamation.Core;
using System.Text.RegularExpressions;
namespace Reclamation.TimeSeries.Hydromet {
    
    
    public partial class McfDataSet {
        partial class pcodemcfDataTable
        {
        }
    
        partial class siteDataTable
        {
        }

        public partial class pcodemcfRow : global::System.Data.DataRow
        {
            public string ParameterName
            {
                get
                {
                    return PCODE.Substring(7).Trim(); 
                }
            }
        }

        public partial class sitemcfRow : global::System.Data.DataRow
        {

            public bool IsSnotel
            {
                get
                {
                    if (!this.IsALTIDNull())
                    {
                        return Regex.IsMatch(this.ALTID.Trim(), "[0-9]{2}[A-Z][0-9]{2}[A-Z]");
                    }
                    return false;
                }
            }




            public string TimeZone(string timeZoneNumber)
            {
                string timeZone = "";
                if (timeZoneNumber == "6" )
                {
                    timeZone = "US/Central";
                }
                if (timeZoneNumber == "7" )
                {
                    timeZone = "US/Mountain";
                }
                if (timeZoneNumber == "8")
                {
                    timeZone = "US/Pacific";
                }
                return timeZone;   
            }

        }
    }
}
