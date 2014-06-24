using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Reclamation.Core;
using System.IO;
using Reclamation.TimeSeries;
namespace Pisces.NunitTests.SeriesTypes
{
    [TestFixture]
    public class UrgwomTest
    {

        public void Basic()
        {
            string fn = Path.Combine( Globals.TestDataPath,"RiverWare");
            fn = Path.Combine(fn,"20120403_SHARP_MRM_Forecast-Mar-50pct.xlsx");
            UrgwomSeries s = new UrgwomSeries(fn, "Run0","Heron.Storage");
            s.Read();
        }

    }
}
