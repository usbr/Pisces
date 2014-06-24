using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using Reclamation.TimeSeries.NoaaAWDB;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.Core;
namespace TestWebService
{
    public class Class1
    {

        public static void Main()
        {


        //   var mcf =  DataTableUtility.Select(Hydromet.Site,"ACL='SNOT' and AltID <>''","");


            var d = new Reclamation.TimeSeries.NoaaAWDB.AwdbWebServiceClient();

            var meta =  d.getStationMetadata("667:MT:SNTL");

            var empty = new string[] { "*" };

            var stations = d.getStations(new string[] { "*", "*" }, empty,
                 new string[] { "SNTL" },
                 null, null,
                 -99999999, 99999999,
                 -99999999, 99999999,
                 -99999999, 99999999,
             null, null, null, true);
            Console.WriteLine(stations); //[0] = "0302:UT:COOP"//[35] = "15B07:ID:SNOW"
        }
    }
}
