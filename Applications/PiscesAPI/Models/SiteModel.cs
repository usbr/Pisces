using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PiscesAPI.Models
{
    public class SiteModel
    {
        /// <summary>
        /// SITECATALOG table contents
        /// </summary>
        public class PiscesSite
        {
            /// <summary>
            /// Unique site ID
            /// </summary>
            [Key] public string siteid { get; set; }

            /// <summary>
            /// Site description
            /// </summary>
            public string description { get; set; }

            /// <summary>
            /// U.S. State
            /// </summary>
            public string state { get; set; }

            /// <summary>
            /// Site geographic latitude (if applicable)
            /// </summary>
            public float latitude { get; set; }

            /// <summary>
            /// Site geographic longitude (if applicable)
            /// </summary>
            public float longitude { get; set; }

            /// <summary>
            /// Site geographic elevation (if applicable)
            /// </summary>
            public string elevation { get; set; }

            /// <summary>
            /// Site geographic time zone (if applicable)
            /// </summary>
            public string timezone { get; set; }

            /// <summary>
            /// Site installation date
            /// </summary>
            public string install { get; set; }

            /// <summary>
            /// Site horizontal spatial datum
            /// </summary>
            public string horizontal_datum { get; set; }

            /// <summary>
            /// Site vertical spatial datum
            /// </summary>
            public string vertical_datum { get; set; }

            /// <summary>
            /// Site elevation accuracy
            /// </summary>
            public float vertical_accuracy { get; set; }

            /// <summary>
            /// Site elevation derivation method
            /// </summary>
            public string elevation_method { get; set; }

            /// <summary>
            /// Site time zone UTC offset
            /// </summary>
            public string tz_offset { get; set; }

            /// <summary>
            /// Site is active flag
            /// </summary>
            public string active_flag { get; set; }

            /// <summary>
            /// Site type
            /// </summary>
            public string type { get; set; }

            /// <summary>
            /// Site owner
            /// </summary>
            public string responsibility { get; set; }

            /// <summary>
            /// Site owner agency and/or region
            /// </summary>
            public string agency_region { get; set; }
        }

    }
}
