using System;
using NUnit.Framework;
using Reclamation.Core;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.TimeSeries;
using System.IO;
using Reclamation.TimeSeries.Alarms;

namespace Pisces.NunitTests.Database
{
    /// <summary>
    /// Test Regular Expression used to parse alarm definition
    /// https://github.com/usbr/Pisces/wiki/alarm-description
    /// </summary>
    [TestFixture]
    public class TestAlarms
    {
        public TestAlarms()
        {
            
        }


        private static TimeSeriesDatabase GetNewDatabase()
        {
            var fn = FileUtility.GetTempFileName(".pdb");
            var svr = new SQLiteServer(fn);
            var db = new TimeSeriesDatabase(svr);
            
            Logger.EnableLogger();
            Console.WriteLine("Created Database: "+fn);
            return db;
        }

        [Test]
        public void DatabaseAboveAlarmTest()
        {
            var db = GetNewDatabase();
            // create database with alarm def
            var ds = db.Alarms;
            ds.AddNewAlarmGroup("palisades");
            var def_id = ds.alarm_definition.Addalarm_definitionRow(true, "palisades",
                "pal", "fb", "above 5520", "").id;
            ds.SaveTable(ds.alarm_definition);

            var test = db.Server.Table("alarm_definition");
            Console.WriteLine("alarm_definition has "+test.Rows.Count+" rows");

            ds.alarm_recipient.Addalarm_recipientRow("palisades", 4,
                "5272", "office", "hydromet@usbr.gov");
            ds.SaveTable(ds.alarm_recipient);

            String file = Path.Combine(TestData.DataPath, "alarms", "pal_fb.csv");
            TextSeries s = new TextSeries(file);
            //TO DO .. read flags

            s.Parameter = "fb";
            s.SiteID = "pal";
            s.Read();
            Console.WriteLine("pal/fb series count = "+s.Count);
            Assert.IsTrue(s.Count > 500);

            ds.Check(s);

            var queue = ds.GetAlarmQueue(def_id);//"pal", "fb");
            Console.WriteLine(DataTableOutput.ToJson(queue));
            Assert.AreEqual(1, queue.Rows.Count);
        }


        [Test]
        public void Below()
        {
            var db = GetNewDatabase();
            var ds = db.Alarms;
            int id = SetupAlarm(ds,"lucky","luc","fb", "below 5525");

            Series s = new Series();
            s.Parameter = "fb";
            s.SiteID = "luc";
            s.Add(DateTime.Parse("2016-11-21 02:00"), 5520.12);
            s.Add(DateTime.Parse("2016-11-21 02:15"), 5520.12);
            s.Add(DateTime.Parse("2016-11-21 02:30"), 5520.12);
            s.Add(DateTime.Parse("2016-11-21 02:45"), 5520.12);

            ds.Check(s);

            var queue = ds.GetAlarmQueue(id);//"luc", "fb");
            Assert.AreEqual(1, queue.Rows.Count);

        }

        private static int SetupAlarm(AlarmDataSet ds, string group,
            string cbtt, string pcode, string def)
        {
            ds.AddNewAlarmGroup(group);
            var row = ds.alarm_definition.Newalarm_definitionRow();
            row.enabled = true;
            row.list = group;
            row.siteid = cbtt;
            row.parameter = pcode;
            row.alarm_condition = def;
            row.clear_condition = "";
            row.id = ds.NextID("alarm_definition", "id");
            ds.alarm_definition.Addalarm_definitionRow(row);
            ds.SaveTable(ds.alarm_definition);
            ds.alarm_recipient.Addalarm_recipientRow(group, 4,
                "5272", "office", "hydromet@usbr.gov");
            ds.SaveTable(ds.alarm_recipient);
            return row.id;
        }
        [Test]
        public void Dropping()
        {
            var db = GetNewDatabase();

            var ds = db.Alarms;
            ds.AddNewAlarmGroup("cre");
            ds.alarm_definition.Addalarm_definitionRow(true, "cre",
                "cre", "fb", "dropping 1", "");
            ds.SaveTable(ds.alarm_definition);
            ds.alarm_recipient.Addalarm_recipientRow("cre", 4,
                "5272", "office", "hydromet@usbr.gov");
            ds.SaveTable(ds.alarm_recipient);

            Series s = new Series();
            s.Parameter = "fb";
            s.SiteID = "cre";
            s.Add(DateTime.Parse("2016-11-21 02:00"), 5520.12);
            s.Add(DateTime.Parse("2016-11-21 02:15"), 5519.00);
            s.Add(DateTime.Parse("2016-11-21 02:30"), 5519.00);
            s.Add(DateTime.Parse("2016-11-21 02:45"), 5519.00);

            ds.Check(s);

            var queue = ds.GetAlarmQueue();
            string sql = "list = 'cre' AND siteid = 'cre' "
                + "AND parameter = 'fb' AND status = 'new'";
            Assert.IsTrue(queue.Select(sql).Length == 1);
        }


        /// <summary>
        /// Data is dropping , detected by comparing new data
        /// to data allready in the database.
        /// </summary>
        [Test]
        public void DroppingBetweenTransmissions()
        {
            var db = GetNewDatabase();

            var ds = db.Alarms;
            ds.AddNewAlarmGroup("wicews");
            ds.alarm_definition.Addalarm_definitionRow(true, "wicews",
                "wicews", "gh", "dropping 0.25", "");
            ds.SaveTable(ds.alarm_definition);
            ds.alarm_recipient.Addalarm_recipientRow("wicews",1,
                "5272", "office", "hydromet@usbr.gov");
            ds.SaveTable(ds.alarm_recipient);

            Series s = new Series();
            s.Parameter = "gh";
            s.SiteID = "wicews";
            s.Name = "wices_gh";
            s.Add(DateTime.Parse("2016-11-21 02:00"), 1.34);
            s.Add(DateTime.Parse("2016-11-21 02:15"), 1.33);
            s.Add(DateTime.Parse("2016-11-21 02:30"), 1.33);
            s.Add(DateTime.Parse("2016-11-21 02:45"), 1.34);
                    
            // (1.34 - 1.27) *4 = 0.28 feet / hour
            db.AddSeries(s);



            s.Clear(); // empty out...
            s.Add(DateTime.Parse("2016-11-21 03:00"), 1.27);
            db.ImportSeriesUsingTableName(s);

            ds.Check(s);

            var queue = ds.GetAlarmQueue();
            string sql = "list = 'wicews' AND siteid = 'wicews' "
                + "AND parameter = 'gh' AND status = 'new'";
            Assert.IsTrue(queue.Select(sql).Length == 1);
        }


        [Test]
        public void Rising()
        {
            var db = GetNewDatabase();
            var ds = db.Alarms;
            ds.AddNewAlarmGroup("emi");
            ds.alarm_definition.Addalarm_definitionRow(true, "emi",
                "emi", "fb", "rising 1", "");
            ds.SaveTable(ds.alarm_definition);
            ds.alarm_recipient.Addalarm_recipientRow("emi", 4,
                "5272", "office", "hydromet@usbr.gov");
            ds.SaveTable(ds.alarm_recipient);

            Series s = new Series();
            s.Parameter = "fb";
            s.SiteID = "emi";
            s.Add(DateTime.Parse("2016-11-21 02:00"), 38000.12);
            s.Add(DateTime.Parse("2016-11-21 02:15"), 38002.02);
            s.Add(DateTime.Parse("2016-11-21 02:30"), 38002.02);
            s.Add(DateTime.Parse("2016-11-21 02:45"), 38002.02);

            ds.Check(s);

            var queue = ds.GetAlarmQueue();
            string sql = "list = 'emi' AND siteid = 'emi' "
                + "AND parameter = 'fb' AND status = 'new'";
            Assert.IsTrue(queue.Select(sql).Length == 1);
        }



        /// <summary>
        /// Include two alarms with the same cbtt/pcode
        /// but with different callout lists
        /// </summary>
        [Test]
        public void AboveOrRising()
        {
            var db = GetNewDatabase();
            var ds = db.Alarms;
            ds.AddNewAlarmGroup("uny");
            ds.AddNewAlarmGroup("test");

            ds.alarm_definition.Addalarm_definitionRow(true, "uny",
                "uny", "pc", "above 300 or rising 1", "");
            ds.alarm_definition.Addalarm_definitionRow(true,"test",
                "uny", "pc", "above 400", "");


            ds.SaveTable(ds.alarm_definition);

            ds.alarm_recipient.Addalarm_recipientRow("uny", 4,
                "5272", "office", "hydromet@usbr.gov");
            ds.alarm_recipient.Addalarm_recipientRow("test", 5,
                "5272", "office", "hydromet@usbr.gov");
            ds.SaveTable(ds.alarm_recipient);


            Series s = new Series();
            s.Parameter = "pc";
            s.SiteID = "uny";
            s.Add(DateTime.Parse("2016-11-21 02:00"), 38002.12);
            s.Add(DateTime.Parse("2016-11-21 02:15"), 38005.02);
            s.Add(DateTime.Parse("2016-11-21 02:30"), 38002.02);
            s.Add(DateTime.Parse("2016-11-21 02:45"), 38002.02);

            ds.Check(s);


            var queue = ds.GetAlarmQueue();
            string sql = "siteid = 'uny' "
                + "AND parameter = 'pc' AND status = 'new'";
            Assert.AreEqual(2,queue.Select(sql).Length);
        }

        [Test]
        public void AlarmDisabled()
        {
            var db = GetNewDatabase();
            var ds = db.Alarms;
            ds.AddNewAlarmGroup("karl");
            var r = ds.alarm_definition.Addalarm_definitionRow(false, "karl",
                "karl", "stress", "above 5520", "");
            Console.WriteLine(r.id);
            ds.SaveTable(ds.alarm_definition);

            var test = db.Server.Table("alarm_definition");
            Console.WriteLine("alarm_definition has " + test.Rows.Count + " rows");

            ds.alarm_recipient.Addalarm_recipientRow("karl", 4,
                "5272", "office", "hydromet@usbr.gov");
            ds.SaveTable(ds.alarm_recipient);

            String file = Path.Combine(TestData.DataPath, "alarms", "pal_fb.csv");
            TextSeries s = new TextSeries(file);
            //TO DO .. read flags

            s.Parameter = "stress";
            s.SiteID = "karl";
            s.Read();
            Console.WriteLine("karl/stress series count = " + s.Count);
            Assert.IsTrue(s.Count > 500);

            ds.Check(s);

            var queue = ds.GetAlarmQueue(r.id);//"karl", "stress");
            Assert.AreEqual(0, queue.Rows.Count);
        }


        [Test]
        public void FlaggedDataShouldNotAlarm()
        {
            var db = GetNewDatabase();
            var ds = db.Alarms;
            int id = SetupAlarm(ds, "flagged", "karl", "bp", "above 12");

            Series s = new Series();
            s.Parameter = "bp";
            s.SiteID = "karl";
            s.Add(DateTime.Parse("2016-11-21 02:00"), 5520.12,"+");
            s.Add(DateTime.Parse("2016-11-21 02:15"), 5520.12, "+");
            s.Add(DateTime.Parse("2016-11-21 02:30"), 5520.12, "-");
            s.Add(DateTime.Parse("2016-11-21 02:45"), 5520.12, "m");

            ds.Check(s);

            var queue = ds.GetAlarmQueue(id); 
            Assert.AreEqual(0, queue.Rows.Count);
        }

    }
}
