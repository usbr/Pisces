using Reclamation.TimeSeries.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries
{
    public partial class Math
    {


        [FunctionAttribute("Performs simple Rating table lookup from a file",
      "FileRatingTable(series1,\"table.csv\")")]
        public static Series FileRatingTable(Series s, string fileName)
        {

            var rval = TimeSeriesDatabaseDataSet.RatingTableDataTable.ComputeSeries(s, fileName);
            rval.RemoveMissing();
            return rval;

        }
    }
}
