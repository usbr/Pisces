using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PiscesAPI.Models
{
    public class SeriesDataModel
    {
        /// <summary>
        /// Timeseries data model
        /// </summary>
        public class TimeSeriesData
        {
            /// <summary>
            /// Series Information
            /// </summary>
            public SeriesModel.Series series { get; set; }

            /// <summary>
            /// Site Information
            /// </summary>
            public SiteModel.PiscesSite  site { get; set; }

            /// <summary>
            /// Parameter Information
            /// </summary>
            public ParameterModel.PiscesParameter parameter { get; set; }

            /// <summary>
            /// Timeseries Data
            /// </summary>
            public List<Point> data { get; set; }
        }


        /// <summary>
        /// Timeseries data table contents
        /// </summary>
        public class Point
        {
            /// <summary>
            /// Data Timestamp
            /// </summary>
            public DateTime datetime { get; set; }

            /// <summary>
            /// Data Value
            /// </summary>
            public float value { get; set; }
            
            /// <summary>
            /// Data flag
            /// </summary>
            public string flag { get; set; }
        }

    }
}
