using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using NUnit.Framework;
namespace Reclamation.Core.Tests
{
    [TestFixture]
    public class NpoiExcelTest
    {

        static void Main(string[] args)
        {
            NpoiExcelTest t = new NpoiExcelTest();
            t.Test2();
            //t.Test1();
        }

        [Test]
        public void Test2()
        {
         var fn =  Path.Combine(Globals.TestDataPath,"hdb_report.xls");
        
          NpoiExcel xls = new NpoiExcel(fn);

           
          xls.SetCellText(0,"D7","oracle.service.name");
          xls.SetCellText(0, "D8", DateTime.Now.ToString());
            var tbl =CreateTestTable(4);
            xls.InsertDataTable(0,"A9",tbl);


            int lastRow = 10 + tbl.Rows.Count;
            string strRange = "A10:A" + (lastRow+5);
           // CellStyle

            xls.FormatRange(0, "B10:B12", "0.00");
            xls.FormatRange(0, "C10:C13", "0.0"); 
            xls.FormatRange(0, "D10:D14", "0");


            string[] formats = new string[] { "",""  };
//           IRange rng= ws.Cells[strRange];
//           rng = rng.Offset(0, 1);
//           for (int i = 0; i < formats.Length; i++)
//           {
//               rng.NumberFormat = ExcelFormat(formats[i]);
//               rng = rng.Offset(0, 1);
//           }

            string fnk = FileUtility.GetTempFileName(".xls");// @"c:\temp\karl.xls"; 
            xls.Save(fnk);
            //System.Diagnostics.Process.Start(fnk);

        }

         [Test]
        public void Test1()
        {
            DataTable tbl = CreateTestTable();

            NpoiExcel xls = new NpoiExcel();
            xls.SaveDataTable(tbl, "newsheet");
             
            string fn= FileUtility.GetTempFileName(".xls");// @"c:\temp\karl.xls";
            File.Delete(fn);
            xls.Save(fn);

            DataTable tbl2 = xls.ReadDataTable("newsheet");
            Assert.AreEqual(12, tbl2.Rows.Count);
            xls.SaveDataTable(tbl2, "sheet1");
            xls.Save(fn);
            
        }

         private static DataTable CreateTestTable(int numValueColums=1)
         {
             DataTable tbl = new DataTable();
             tbl.Columns.Add("datetime", typeof(DateTime));
             tbl.Columns.Add("value", typeof(double));
             for (int i = 1; i < numValueColums; i++)
             {
                 tbl.Columns.Add("value" + i,typeof(double));
             }
             for (int i = 0; i < 12; i++)
             {
                 var row = tbl.NewRow();
                 row[0]= DateTime.Now.Date.AddDays(i);
                 for (int ii = 1; ii < numValueColums; ii++)
                 {
                     row[ii] = ii * Math.PI;
                 }
                 tbl.Rows.Add(row);
             }
             return tbl;
         }

    }
}