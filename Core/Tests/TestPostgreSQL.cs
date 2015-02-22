using NUnit.Framework;
using Reclamation.Core;
using System;
using System.Data;

namespace Reclamation.Core.Tests
{
    [TestFixture]
    public class TestPostgreSQL
    {

        public static BasicDBServer GetPGServer(string ipaddress = "localhost", string dbName = "nunit")
        {
            var cs = PostgreSQL.CreateADConnectionString(ipaddress, dbName);

            PostgreSQL svr = new PostgreSQL(cs);
            return svr;
        }

        [Test, Category("DatabaseServer")]
        public void ADLogin()
        {

            var psql = GetPGServer();

            var tbl = psql.Table("test","show search_path");
            DataTableOutput.Write(tbl, FileUtility.GetTempFileName(".csv"), false, true);


        }

        /// <summary>
        /// AgriMet crops charts use a reserved word in the column name
        /// </summary>
        [Test]
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
