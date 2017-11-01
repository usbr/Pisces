using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PiscesAPI.Models
{
    public class SeriesModel
    {
        /// <summary>
        /// SITECATALOG table contents
        /// </summary>
        public class PiscesSeries
        {
            /// <summary>
            /// Unique series ID
            /// </summary>
            public int site_id { get; set; }

            /// <summary>
            /// Site description
            /// </summary>
            public int parentid { get; set; }

            /// <summary>
            /// U.S. State
            /// </summary>
            public int isfolder { get; set; }

            /// <summary>
            /// Site geographic latitude (if applicable)
            /// </summary>
            public int sortorder { get; set; }

            /// <summary>
            /// Site geographic longitude (if applicable)
            /// </summary>
            public string iconname { get; set; }

            /// <summary>
            /// Site geographic elevation (if applicable)
            /// </summary>
            public string name { get; set; }

            /// <summary>
            /// Site geographic time zone (if applicable)
            /// </summary>
            public int siteid { get; set; }

            /// <summary>
            /// Site installation date
            /// </summary>
            public string units { get; set; }

            /// <summary>
            /// Site horizontal spatial datum
            /// </summary>
            public string timeinterval { get; set; }

            /// <summary>
            /// Site vertical spatial datum
            /// </summary>
            public string parameter { get; set; }

            /// <summary>
            /// Site elevation accuracy
            /// </summary>
            public string tablename { get; set; }

            /// <summary>
            /// Site elevation derivation method
            /// </summary>
            public string provider { get; set; }

            /// <summary>
            /// Site time zone UTC offset
            /// </summary>
            public string connectionstring { get; set; }

            /// <summary>
            /// Site is active flag
            /// </summary>
            public string expression { get; set; }

            /// <summary>
            /// Site type
            /// </summary>
            public string notes { get; set; }

            /// <summary>
            /// Site owner
            /// </summary>
            public int enabled { get; set; }
        }

    }
}
