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

    public class TsDataALC : TsData
    {
        [JsonProperty("Actual Flow (CFS)")] public override string QD { get; set; }
        [JsonProperty("Current Contents (AF)")] public override string AF { get; set; }
    }

}
