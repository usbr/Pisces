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

            public string GetAgencyName()
            {
                if (this.GRP == 10)
                {
                    return "USGS";
                }

                if (this.GRP == 9)
                {
                    return "OWRD";
                }
                if (this.GRP == 8)
                {
                    return "IDWR";
                }

                return "USBR-PN";
            }
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

            TextFile m_usgs_sites;

            public bool IsUsgs
            {
                get
                {
                    if (m_usgs_sites == null)
                        m_usgs_sites = new TextFile(FileUtility.GetFileReference("usgs_sites.txt"));

                    return m_usgs_sites.IndexOfRegex(this.SITE.Trim() + "$") >= 0;
                }
            }

            TextFile m_owrd_sites;
            public bool IsOwrd
            {
                get
                {
                    if (m_owrd_sites == null)
                        m_owrd_sites = new TextFile(FileUtility.GetFileReference("owrd_sites.txt"));

                    return m_owrd_sites.IndexOfRegex(this.SITE.Trim() + "$") >= 0;
                }
            }

            TextFile m_idahopower_sites;
            public bool IsIdahoPower
            {
                get
                {
                    if( m_idahopower_sites == null)
                        m_idahopower_sites = new TextFile(FileUtility.GetFileReference("idahopower_sites.txt"));


                    return m_idahopower_sites.IndexOfRegex(this.SITE.Trim()+"$") >= 0;
                    
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
