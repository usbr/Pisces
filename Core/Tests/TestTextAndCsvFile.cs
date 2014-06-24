using System;
using System.Data;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Reclamation.Core;
namespace Reclamation.Core.Tests
{
    [TestFixture]
    public class TestTextAndCsvFile
    {


        [Test]
        public void ParseConnectionString()
        {
            string s = @"file=c:\temp\myfile.xls;SheetName=sheet1;dateColumn=Date;ValueColumn=JCK AF";

            string fn = ConnectionStringUtility.GetToken(s, "File", "");
            Assert.AreEqual(@"c:\temp\myfile.xls", fn);

            string sheet = ConnectionStringUtility.GetToken(s, "sheetName", "");
            Assert.AreEqual(@"sheet1", sheet);
            string dc = ConnectionStringUtility.GetToken(s, "dateColumn", "");
            Assert.AreEqual(@"Date", dc);
            string vc = ConnectionStringUtility.GetToken(s, "valuecolumn", "");
            Assert.AreEqual(@"JCK AF", vc);

        }

        /// <summary>
        /// first columns can be empty in csv file.
        /// </summary>
        [Test]
        public void EmptyColumnsCSV()
        {
            string s = ",,Boise River Basin,2,,,,,";
            var tokens = CsvFile.ParseCSV(s);
            Assert.AreEqual(9, tokens.Length);
        }

        [Test]
        public void CSVToken()
        {
            //abc,"""hi there,","this, or that'"   // from Excel..
            string line = "abc,\"\"\"hi there,\",\"this, or that'\"";
            string[] tokens = CsvFile.ParseCSV(line);
            for (int i = 0; i < tokens.Length; i++)
            {
                Console.WriteLine(i+" [" + tokens[i] + "]");
            }

            Assert.AreEqual(3, tokens.Length);
            Assert.AreEqual("abc", tokens[0]);
            Assert.AreEqual("\"hi there,", tokens[1]);
            Assert.AreEqual("this, or that'", tokens[2]);

        }
        [Test]
        public void CSVFile()
        {
            DataTable table = new DataTable();
            table.Columns.Add("col, 1");// ouch!! comma in column name
            table.Columns.Add("col2");
            table.Columns.Add("col3");

            DataRow r = table.NewRow();
            r[0] =  "abc";
            r[1] = "\"hi there,";
            r[2] = "this, or that'";
            
            table.Rows.Add(r);
            string fn = FileUtility.GetTempPath()+"\\csvtest.csv";
            CsvFile.WriteToCSV(table, fn, true);
            //File.Delete(fn);
            CsvFile db = new CsvFile(fn);

            Assert.AreEqual(1, db.Rows.Count, "row count");
            Assert.AreEqual(3, db.Columns.Count, "column count");

            Assert.AreEqual(table.Columns[0].ColumnName,
                            db.Columns[0].ColumnName);
            Assert.AreEqual(table.Columns[1].ColumnName,
                                        db.Columns[1].ColumnName);
            Assert.AreEqual(table.Columns[2].ColumnName,
                                        db.Columns[2].ColumnName);



            DataRow r2 = db.Rows[0];

            Assert.AreEqual(r[0].ToString(), r2[0].ToString());
            Assert.AreEqual(r[1].ToString(), r2[1].ToString());
            Assert.AreEqual(r[2].ToString(), r2[2].ToString());

        }

        [Test]
        public void NthIndexOf()
        {
            TextFile tf = new TextFile(data);
            int idx = tf.NthIndexOf("END_SLOT", 3);
            Assert.AreEqual(14, idx);
        }

static        string[] data ={
"6726.24",  //0
"END_COLUMN",
"END_SLOT",
"object_type: AggReach",
"object_name: JacksonToPalisades", //4
"slot_name: Outflow",
"END_SLOT_PREAMBLE",
"units: cfs",
"scale: 1",
" object_name: Jackson", // 9
"3530.80",
"3463.87",
"3456.42",
"object_name: Jackson",  // 13
"END_SLOT",
"object_type: StorageReservoir",
"object_name: Jackson",  // 16
"slot_name: Inflow",
"END_SLOT_PREAMBLE" };


[Test]
        public void IndexOfRegex()
        {
            TextFile tf = new TextFile(data);

            Assert.AreEqual(9, tf.IndexOfRegex("Jackson$"));
            Assert.AreEqual(4, tf.IndexOfRegex("object_name: Jackson"));
            Assert.AreEqual(13, tf.IndexOfRegex("^object_name: Jackson$"));

            Assert.AreEqual(16,
                tf.IndexOfBothRegex("^object_name: Jackson$", "^slot_name: Inflow$"));
                               
        }

[Test]
        public void EstimateColumnTypes()
        {
            string content =
                "txt, notint, double,bool,int\n"
               + "text,12, 22.4,TRUE,1\n"
               + "text2,1.2, 22.4,TRUE,2";
             
            string filename= Path.GetTempFileName();
            File.WriteAllText(filename, content);

            CsvFile db = new CsvFile(filename);

            Assert.AreEqual( "System.String" ,db.Columns[0].DataType.ToString(),"col1 ");
            Assert.AreEqual( "System.Double" ,db.Columns[1].DataType.ToString(), "col2 ");
            Assert.AreEqual( "System.Double" ,db.Columns[2].DataType.ToString(), "col3 ");
            Assert.AreEqual( "System.Boolean",db.Columns[3].DataType.ToString(), "col4 ");
            Assert.AreEqual( "System.Int32"  ,db.Columns[4].DataType.ToString(), "col5 ");
            Assert.AreEqual(2, db.Rows.Count,"row count");

        }

    }
}
