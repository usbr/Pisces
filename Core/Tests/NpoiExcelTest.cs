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

//int lastRow = ws.UsedRange.RowCount;
//           string strRange = "A10:A" + (lastRow+5);
//           string[] formats = tbl.DisplayFormat;
//           IRange rng= ws.Cells[strRange];
//           rng = rng.Offset(0, 1);
//           for (int i = 0; i < formats.Length; i++)
//           {
//               rng.NumberFormat = ExcelFormat(formats[i]);
//               rng = rng.Offset(0, 1);
//           }
           
            xls.Save(@"c:\temp\karl.xls");
        }

         [Test]
        public void Test1()
        {
            DataTable tbl = new DataTable();
            tbl.Columns.Add("counter", typeof(int));
            tbl.Columns.Add("datetime", typeof(DateTime));
            tbl.Columns.Add("value", typeof(double));

            for (int i = 0; i <12; i++)
            {
                tbl.Rows.Add(i,DateTime.Now.Date.AddDays(i), i * Math.PI);
            }

            NpoiExcel xls = new NpoiExcel();
            xls.SaveDataTable(tbl, "newsheet");

            string fn=@"c:\temp\karl.xls";
            xls.Save(fn);

            DataTable tbl2 = xls.ReadDataTable("newsheet");
            Assert.AreEqual(12, tbl2.Rows.Count);
            xls.SaveDataTable(tbl2, "sheet1");
            xls.Save(fn);
            
        }

    }
}