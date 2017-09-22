using NUnit.Framework;
using System;
using System.Data;

namespace PiscesWebServices.Tests
{
    [TestFixture]
    class DataTableJSON
    {

        [Test]
        public void DataTableToJsonTest()
        {
            DataTable tbl = new DataTable("test");
            tbl.Columns.Add("install", typeof(DateTime));
            tbl.Columns.Add("elevation", typeof(double));
            tbl.Columns.Add("notes");

            tbl.Rows.Add(DateTime.Now.Date.AddDays(-1), 123.45, "hello");
            tbl.Rows.Add(DateTime.Now.Date, 678.0, "day2");

            var s = Newtonsoft.Json.JsonConvert.SerializeObject(tbl);
            Console.WriteLine(s);
        }

    }
    
}