using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using HydrometForecast;
using Reclamation.Core;
using SpreadsheetGear;
using System.IO;
using Reclamation.TimeSeries.Hydromet;

namespace ReclamationTesting.TimeSeries.Forecasting
{
    [TestFixture]
    public class ForecastPerformance
    {
        static DateTime t = new DateTime(2011, 1, 1);

        [Test]
        public void DumpMpollList()
        {

        }
        [Test]
        public void Anderson()
        {
            Go(t, "Anderson");
        }

        private void Go(DateTime t, string sheetName)
        {
            Logger.EnableLogger();
            HydrometInfoUtility.WebCaching = true;
            Performance p = new Performance();
            Console.WriteLine(sheetName);
        //    string fn = ReclamationTesting.TestData.DataPath + @"\Forecasting\Forecasting 2011Test.xlsx";
            string fn = @"\\ibr1pnrfp01\Common\PN6200\Operations\Runoff Forecasting\SpreadsheetForecasts\Forecasting 2013.xlsx";
            Reclamation.TimeSeries.Excel.SpreadsheetGearExcel xls = new Reclamation.TimeSeries.Excel.SpreadsheetGearExcel(fn);
            xls.Workbook.Sheets[sheetName].Select();
            string csv = FileUtility.GetTempFileName(".csv");
            xls.Workbook.SaveAs(csv, FileFormat.CSV);
            
            ForecastEquation eq = new ForecastEquation(csv);
            var tbl = eq.RunHistoricalForecasts(2000, 2007,false,t.Month,new double[]{1.0},t.Day);

            string csv2 = FileUtility.GetTempFileName(".csv");
            CsvFile.WriteToCSV(tbl, csv2, false, true);
            System.Diagnostics.Process.Start(csv2);
            //Assert.AreEqual(expectedForecast, r.ResidualForecast,tolerance);
            Console.WriteLine();
            p.Report(); // 18.7 seconds,  drop logger 4.8 seconds, improve app.config  4 seconds, 
            // web cache  0.7 seconds

        }


       
    }


}
