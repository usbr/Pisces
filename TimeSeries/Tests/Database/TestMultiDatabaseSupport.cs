using System;
using NUnit.Framework;
using Reclamation.Core;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.TimeSeries;
using Reclamation.Core.Tests;

namespace Pisces.NunitTests.Database
{
    [TestFixture]
    public class TestMultiDatabaseSupport
    {

        public static void Main()
        {
            Logger.EnableLogger();
            var t = new TestMultiDatabaseSupport();
            t.TestMySQL();
        }

        [Test, Category("DatabaseServer")]
        public void TestMySQL()
        {
            var svr = MySqlServer.GetMySqlServer("localhost", "timeseries");

            var db = new TimeSeriesDatabase(svr,false);

            BasicDatabaseTest(db);
        }

      
        [Test]
        public void TestSQLite()
        {
            var fn = FileUtility.GetTempFileName(".pdb");
            System.IO.File.Delete(fn);

            SQLiteServer svr = new SQLiteServer(fn);

            var db = new TimeSeriesDatabase(svr,false);

            BasicDatabaseTest(db);
        }




        [Test, Category("DatabaseServer")]
        public void TestActiveDirectoryConnectionPostgresql()
        {
            var svr = TestPostgreSQL.GetPGServer();
        }

        [Test, Category("DatabaseServer")]
        public void TestPostgresql()
        {
            // using database nunit owned by user running the test
            Logger.EnableLogger();
            var svr = TestPostgreSQL.GetPGServer(dbName:"postgres") as PostgreSQL;
            if( !svr.DatabaseExists("nunit"))
            {
                svr.RunSqlCommand("create database nunit");
            }
            svr = TestPostgreSQL.GetPGServer(dbName: "nunit") as PostgreSQL;

            var tables = svr.TableNames();
            
            foreach (var tn in tables)
            {
                svr.RunSqlCommand("drop table \"" + tn + "\"");
                Console.WriteLine(tn);
            }
            TimeSeriesDatabase db = new TimeSeriesDatabase(svr,false);
            BasicDatabaseTest(db);
        }

//        [Test, Category("DatabaseServer")]
//        public void TestSqlServer()
//        {
//            /*
//             * C:\>sqlcmd -S .\SQLEXPRESS
//> create login [bor\ktarbet] from windows
//             *  select loginname from master..syslogins
//> go
//             sp_addsrvrolemember @loginame='bor\ktarbet' , @rolename='sysadmin'
//> ;
//             */
//            string cStr ="integrated security=SSPI;data source=localhost\\SQLEXPRESS;Database=master"; 
//            var svr = new SqlServer(cStr);
//            var tmp = svr.Table("exists", " SELECT name FROM master..sysdatabases where name = 'test_pisces'");
//            if (tmp.Rows.Count == 1)
//            {
//                svr.CloseAllConnections();
//                svr.DeleteDataBase("test_pisces");
//            }
            
//            Console.WriteLine("about to create database");
//            svr.CreateDataBase("test_pisces");
//            svr = new SqlServer(cStr.Replace("master","test_pisces"));
            
//            TimeSeriesDatabase db = new TimeSeriesDatabase(svr,false);
//            BasicDatabaseTest(db);
//        }


        public static void BasicDatabaseTest(TimeSeriesDatabase db)
        {

            //Assert.IsTrue(db.GetSeriesCatalog().Rows.Count ==1 , " initial catalog should have root");
            Reclamation.TimeSeries.Hydromet.HydrometInfoUtility.AutoUpdate = true;
            DateTime t2 = DateTime.Now.Date.AddDays(-10);
            DateTime t1 = DateTime.Now.Date.AddDays(-30);
            Series s = new HydrometDailySeries("jck", "af");
            int id = db.AddSeries(s);

             s = db.GetSeries(id);
             s.Read(t1, t2);
             s.WriteToConsole();
             Assert.AreEqual(21, s.Count);

            s.Read(t1, DateTime.Now.Date.AddDays(-9)); //force auto update.(HydrometDaily supports this)
            // check if auto update worked.
            Assert.AreEqual(22, s.Count);

            Assert.AreEqual(2, db.GetSeriesCatalog().Rows.Count, "Catalog row count");
            var por = s.GetPeriodOfRecord();
            Assert.AreEqual(22, por.Count,"period of record");

        }


    }
}
