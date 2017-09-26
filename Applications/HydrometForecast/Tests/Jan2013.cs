using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using HydrometForecast;
using Reclamation.Core;
using SpreadsheetGear;
using System.IO;

namespace ReclamationTesting.TimeSeries.Forecasting
{
    [TestFixture]
    public class Jan2013
    {
        string dir = @"\\ibr1pnrfp001\pndata\PN6200\Operations\Runoff Forecasting\SpreadsheetForecasts\TestData\";

        public Jan2013()
        {
           HydrometData.FileName = Path.Combine(dir,"mpolljan2013.db");
           // HydrometData.FileName = @"c:\temp\mpolljan2013.db";
        }

        static DateTime t = new DateTime(2013, 1, 1);
        

        [Test]
        public void ANDERSON() { VerifyForecast(t, "ANDERSON", 0.1); }
       [Test]
        public void BAKER() { VerifyForecast(t, "BAKER", 1); }
       [Test]
       public void BEULAH_LTF() { VerifyForecast(t, "BEULAH LTF", 0.1); }
       [Test]
       public void BOISE() { VerifyForecast(t, "BOISE", 0.1); }
       [Test]
       public void BULLY_CREEK_LTF() { VerifyForecast(t, "BULLY CREEK LTF", 0.1); }
       [Test]
       public void CASCADE() { VerifyForecast(t, "CASCADE", 0.2); }
       [Test]
       public void DEADWOOD() { VerifyForecast(t, "DEADWOOD", 0.1); }
       [Test]
       public void HEISE() { VerifyForecast(t, "HEISE", 0.2); }
       [Test]
       public void HUNGRY_HORSE() { VerifyForecast(t, "HUNGRY HORSE", .5); } // gdmm  pc changed, see feb forcast for difference
       [Test]
       public void ISLAND_PARK() { VerifyForecast(t, "ISLAND PARK", 0.1); }
       [Test]
       public void JACKSON_LAKE() { VerifyForecast(t, "JACKSON LAKE", 0.1,"JACKSON"); }
       [Test]
       public void KACHESS() { VerifyForecast(t, "KACHESS", 0.1); }
       [Test]
       public void LITTLE_WOOD() { VerifyForecast(t, "LITTLE WOOD", 0.1); }
       [Test]
       public void BEULAH() { VerifyForecast(t, "BEULAH", 0.1, "MALHEUR"); }
       [Test]
       public void WARM_SPRINGS() { VerifyForecast(t, "WARM SPRINGS", 0.1, "MALHEUR"); }
       [Test]
       public void BULLY_CREEK() { VerifyForecast(t, "BULLY CREEK", 0.1, "MALHEUR"); }
       [Test]
       public void MAL_NR_DREWSEY() { VerifyForecast(t, "MAL. NR. DREWSEY", 0.1); }
       
       [Test]
       public void MCKAY() { VerifyForecast(t, "MCKAY", 0.1); }
       [Test]
       public void N_F_MALHEUR_R() { VerifyForecast(t, "N. F. MALHEUR R.", 0.1); }
       [Test]
       public void OCHOCO() { VerifyForecast(t, "OCHOCO", 0.1); }
       [Test]
       public void OWYHEE() { VerifyForecast(t, "OWYHEE", 0.1); }
       [Test]
       public void OWYHEE_NR_ROME() { VerifyForecast(t, "OWYHEE NR. ROME", 0.1); }
       [Test]
       public void Payette_BEND() { VerifyForecast(t, "HORSESHOE BEND", 0.1, "PAYETTE"); }
       [Test]
       public void PRINEVILLE() { VerifyForecast(t, "PRINEVILLE", 0.1); }
       [Test]
       public void RIRIE_NEW() { VerifyForecast(t, "RIRIE_NEW", 0.1); }
       [Test]
       public void TETON() { VerifyForecast(t, "TETON", 0.1); }
       [Test]
       public void UNITY() { VerifyForecast(t, "UNITY", 0.1); }
       [Test]
       public void WARM_SPRINGS_LTF() { VerifyForecast(t, "WARM SPRINGS LTF", 0.1); }
       [Test]
       public void WILDHORSE() { VerifyForecast(t, "WILDHORSE", 0.3); }
       [Test]
       
        public void CLE_ELUM() { VerifyForecast(t, "CLE ELUM", 0.1,"YAKIMA ABOVE CLE ELUM"); }
       [Test]
       public void KEECHELUS() { VerifyForecast(t, "KEECHELUS", 0.1, "YAKIMA ABOVE CLE ELUM"); }
       [Test]
       public void YAKIMA_R_AT_CLE_ELU() { VerifyForecast(t, "YAKIMA R AT CLE ELUM", 0.1,"YAKIMA ABOVE CLE ELUM"); }
       
        [Test]
       public void BUMPING() { VerifyForecast(t, "BUMPING", 0.1,"YAKIMA ABOVE NACHES"); }
       [Test]
       public void RIMROCK() { VerifyForecast(t, "RIMROCK", 0.1,"YAKIMA ABOVE NACHES"); }
       [Test]
       public void NATURAL_NEAR_NACHES() { VerifyForecast(t, "NATURAL NEAR NACHES", 0.1,"YAKIMA ABOVE NACHES"); }

       [Test]
       public void YAKIMA_R_NR_PARKER() { VerifyForecast(t, "YAKIMA NEAR PARKER", 0.5); }
       [Test]
       public void YAKIMA_XNR_PARKER_NE() { VerifyForecast(t, "YAKIMA XNR PARKER NEW", .5); }



       private void VerifyForecast(DateTime t, string sheetName, double tolerance, string forecastName = "")
        {
         
            Console.WriteLine(sheetName);
            string fn = Path.Combine(dir,"Jan2013.xlsx");
            Reclamation.TimeSeries.Excel.SpreadsheetGearExcel xls = new Reclamation.TimeSeries.Excel.SpreadsheetGearExcel(fn);
            xls.Workbook.Sheets[sheetName].Select();
            string csv = FileUtility.GetTempFileName(".csv");
            xls.Workbook.SaveAs(csv, FileFormat.CSV);
            
            ForecastEquation eq = new ForecastEquation(csv);
            var r = eq.Evaluate(t, false,1.0,true);

            string fortranOutput = Path.Combine(dir, "Forecast.jan2013");
           var expectedResidualForecast = ReadExpectedForecast(fortranOutput,sheetName,forecastName);

            Assert.AreEqual(expectedResidualForecast, r.ResidualForecast,tolerance);
        }

        private double ReadExpectedForecast(string outputFileName, string sheetName, string forecastName)
        {
            double rval = 0;

            var tf = new TextFile(outputFileName);
            if (forecastName == "")
                forecastName = sheetName;

            int idx =tf.IndexOf("FORECAST " + forecastName+"       ");
            if (idx < 0)
                return - 1;

            idx = tf.IndexOf("K  FORECAST  TO DATE FORECAST",idx);
            if (idx < 0)
                return -1;
            

            if (sheetName == "YAKIMA NEAR PARKER")
            {
                var hack = "YAKIMA R. NR. PARKER";
                sheetName = hack;
            }

            if (sheetName == "YAKIMA R AT CLE ELUM")
            {
                sheetName = "YAKIMA R. AT CLE ELUM";
            }
            int idx2 = tf.IndexOf(" " + sheetName + "    ",idx); // find proper place in summary outpt
            if (idx2 < 0)
                return -1;

            if (idx2 - idx2 > 20) 
                return -1;

            idx2 += 1;
            var residText = TextFile.Split(tf[idx2]).Last();
            rval = Convert.ToDouble(residText);
            return rval;
        }

        
       
    }


}
