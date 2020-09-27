using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Reclamation.TimeSeries.IDWR
{
    public class RiverItems
    {
        public string River { get; set; }
        public string Name { get; set; }
        public string WD { get; set; }
    }

    public class RiverSite
    {
        public string SiteID { get; set; }
        public string SiteType { get; set; }
        public string SiteName { get; set; }
        public string HSTCount { get; set; }
        public string ALCCount { get; set; }
    }

    public class SiteInfo
    {
        public string SiteId { get; set; }
        public string SiteType { get; set; }
        public string StationName { get; set; }
        public string FullName { get; set; }
        public string River { get; set; }
        public string ALCSiteType { get; set; }
    }

    public class TsData
    {
        [JsonProperty("SiteID")] public string SiteID { get; set; }
        [JsonProperty("SiteType")] public string SiteType { get; set; }
        [JsonProperty("HSTDate")] public string Date { get; set; }

        // override properties
        [JsonProperty("Flow (CFS)")] public virtual string QD { get; set; }
        [JsonProperty("Gage Height (Feet)")] public virtual string GH { get; set; }
        [JsonProperty("Reservoir Contents (Acre Ft)")] public virtual string AF { get; set; }
        [JsonProperty("Surface Elevation (Feet)")] public virtual string FB { get; set; }
    }

    public class TsDataALC
    {
        [JsonProperty("SiteID")] public string SiteID { get; set; }
        [JsonProperty("SiteType")] public string SiteType { get; set; }
        [JsonProperty("Accounting Date")] public string Date { get; set; }
        [JsonProperty("HSTDate")] public string HistDate { get; set; }
        // F IDWR Data Type
        [JsonProperty("Natural Flow (CFS)")] public virtual string NATQ { get; set; }
        [JsonProperty("Actual Flow (CFS)")] public virtual string ACTQ { get; set; }
        [JsonProperty("Stored Flow (CFS)")] public virtual string STRQ { get; set; }
        [JsonProperty("Reach Gain (CFS)")] public virtual string GANQ { get; set; }
        // R IDWR Data Type
        [JsonProperty("Reservoir Evaporation (CFS)")] public virtual string EVAP { get; set; }
        [JsonProperty("Total Evaporation (AF)")] public virtual string TOTEVAP { get; set; }
        [JsonProperty("Accrued Storage (CFS)")] public virtual string STORACC { get; set; }
        [JsonProperty("Total Accrual (AF)")] public virtual string TOTACC { get; set; }
        [JsonProperty("Current Contents (AF)")] public virtual string CURSTOR { get; set; }
        // D & P IDWR Data Type
        [JsonProperty("Flow (CFS)")] public virtual string DIV { get; set; }
        [JsonProperty("Total Diversion to Date (AF)")] public virtual string TOTDIVVOL { get; set; }
        [JsonProperty("Daily Storage Diversion (CFS)")] public virtual string STORDIV { get; set; }
        [JsonProperty("Storage Diversion to Date (AF)")] public virtual string STORDIVVOL { get; set; }
        [JsonProperty("Remaining Storage (AF)")] public virtual string STORBAL { get; set; }     
    }

}
