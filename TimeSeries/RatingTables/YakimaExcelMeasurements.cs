using Reclamation.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries.RatingTables
{

    /// <summary>
    /// Reads Yakima Excel sheets that contain flow measurements
    /// </summary>
    public class YakimaExcelMeasurements
    {


        public static void FillTable(string filename, MeasurementsDataSet.measurementDataTable table)
        {
            var xls = new NpoiExcel(filename);
            var tbl = xls.ReadDataTable("summary");
            Console.WriteLine(tbl.Rows.Count);



        }
    }
}
