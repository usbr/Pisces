using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PiscesAPI.Models
{
    public class ParameterModel
    {
        /// <summary>
        /// PARAMETERCATALOG table contents
        /// </summary>
        public class PiscesParameter
        {
            /// <summary>
            /// Unique parameter ID
            /// </summary>
            public string id { get; set; }

            /// <summary>
            /// Parameter name
            /// </summary>
            public string name { get; set; }

            /// <summary>
            /// Applicable time series intervals
            /// </summary>
            public string timeinterval { get; set; }

            /// <summary>
            /// Physical units
            /// </summary>
            public string units { get; set; }

            /// <summary>
            /// Parameter statistic
            /// </summary>
            public string statistic { get; set; }

            /// <summary>
            /// Parameter units description
            /// </summary>
            public string unitstext { get; set; }
        }

    }
}
