using NUnit.Framework;
using Reclamation.Core;
using System;
using System.Data;
using System.Linq;

namespace Reclamation.Core.Tests
{
    [TestFixture]
    public class TestPostgreSQL
    {

        public static BasicDBServer GetPGServer(string ipaddress = "localhost", string dbName = "nunit")
        {
            return PostgreSQL.GetPostgresServer(dbName, ipaddress, "test", "test");
        }

        [Test, Category("DatabaseServer")]
        [Ignore("Ignore DatabaseServer tests per KTarbet.")]
        public void ADLogin()
        {
            var psql = GetPGServer();
            var tbl = psql.Table("test","show search_path");
            DataTableOutput.Write(tbl, FileUtility.GetTempFileName(".csv"), false, true);
        }

        private DataTable TestTimeSeriesData(string tableName, DateTime startDate, int count, int minutesIncrement=15)
        {
            var rval = new DataTable(tableName);
            rval.Columns.Add("datetime", typeof(DateTime));
            rval.Columns.Add("value", typeof(double));
            rval.Columns.Add("flag", typeof(string));

            DateTime t = startDate;
            for (int i = 0; i < count; i++)
            {
                rval.Rows.Add(t, i * Math.PI, " ");
                t = t.AddMinutes(minutesIncrement); 
            }

            return rval;
        }

        /// <summary>
        /// Test inserting many smaller batches 
        /// </summary>
        [Test, Category("DatabaseServer")]
        [Ignore("Ignore DatabaseServer tests per KTarbet.")]
        public void InsertSpeedTest()
        {
            var svr = GetPGServer() as PostgreSQL;
            CreateEmptyTimeSeriesTable(svr, "test_insert");
            CreateEmptyTimeSeriesTable(svr, "test_insert2");
            double d1 = SaveTable(svr, true,"test_insert");
            double d2 = SaveTable(svr, false, "test_insert2");

            Console.WriteLine("Custom = "+d1.ToString("F2"));
            Console.WriteLine("Save   = "+d2.ToString("F2"));
        }

        /// <summary>
        /// Test inserting many smaller batches, with overlapping data
        /// </summary>
        [Test, Category("DatabaseServer")]
        [Ignore("Ignore DatabaseServer tests per KTarbet.")]
        public void UpsertSpeedTest()
        {
            var svr = GetPGServer() as PostgreSQL;
            CreateEmptyTimeSeriesTable(svr, "test_upsert");
            CreateEmptyTimeSeriesTable(svr, "test_upsert2");
            double d1 = SaveTable(svr, true, "test_upsert",6,true);
            double d2 = SaveTable(svr, true, "test_upsert2",6,true);

            Console.WriteLine("Custom = " + d1.ToString("F2"));
            Console.WriteLine("Save   = " + d2.ToString("F2"));
        }



        private double SaveTable(PostgreSQL svr, bool customSave,
            string tableName, int recordsPerRun=4, bool upsert=false)
        {
            DateTime t = DateTime.Now;
            Performance p = new Performance();
            for (int i = 0; i < 400; i++)
            {
                var tbl = TestTimeSeriesData(tableName, t, recordsPerRun);
                if (!customSave)
                    svr.SaveTable(tbl);
                else
                    svr.InsertTimeSeriesTable(tbl);
                t = t.AddMinutes(60);
            }

            p.Report();
            return p.ElapsedSeconds;
        }


      
      private static void CreateEmptyTimeSeriesTable(BasicDBServer svr, string tableName)
      {
          if (!svr.TableExists(tableName))
          {
              string sql = " CREATE TABLE "+tableName+" ("
              + " datetime timestamp without time zone NOT NULL, "
              + "  value double precision,"
              + "  flag character varying(50), "
              + "  CONSTRAINT "+tableName+"_pkey PRIMARY KEY (datetime))";
              svr.CreateTable(sql);
          }

          svr.RunSqlCommand("truncate table "+tableName);
      }



        /// <summary>
        /// AgriMet crops charts use a reserved word in the column name
        /// </summary>
        [Test, Category("DatabaseServer")]
        [Ignore("Ignore DatabaseServer tests per KTarbet.")]
        public void ColumnNameReservedWord()
        {
            var psql = GetPGServer();

            if (psql.TableExists("test_column_name_reserved_word"))
                psql.RunSqlCommand(" drop table test_column_name_reserved_word");
            string sql =
"CREATE TABLE test_column_name_reserved_word "
+ "( "
+ "  index integer NOT NULL primary key, "
+ "  year integer, "
+ "  sortindex integer, "
+ "  \"group\" integer,"  // group is a reserved word..
+ "  cbtt character varying(10), "
+ "  cropcurvenumber integer, "
+ "  cropname character varying(40), "
+ "  startdate timestamp without time zone, "
+ "  fullcoverdate timestamp without time zone, "
+ "  terminatedate timestamp without time zone "
+ ")";

            psql.RunSqlCommand(sql);

            var table = new DataTable();
            table.TableName = "test_column_name_reserved_word";
            table = psql.Table("test_column_name_reserved_word");

            

            int id = 1;
            if( table.Rows.Count >0)
                id = ((int)table.Compute("Max(index)", "") + 1);

            var newRow = table.NewRow();

            newRow["index"] = id;
            newRow["year"] = DateTime.Now.Year;
            newRow["group"] = 99;
            // insert a row.
            table.Rows.Add(newRow);
            psql.SaveTable(table);

            table.Clear();
            table.AcceptChanges();
            psql.FillTable(table);
            // update a row

            table.Rows[0]["group"] = 1;
            psql.SaveTable(table);

        }

    }
}
