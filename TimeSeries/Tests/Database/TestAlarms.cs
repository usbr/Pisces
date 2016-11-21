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
        SQLiteServer svr;
        TimeSeriesDatabase db;
        public TestAlarms()
        {
            var fn = FileUtility.GetTempFileName(".pdb");
            this.svr = new SQLiteServer(fn);
            this.db = new TimeSeriesDatabase(svr);
        }

        [Test]
        public void DatabaseAboveAlarmTest()
        {
            // create database with alarm def
            var ds = db.Alarms;
            ds.AddNewAlarmGroup("palisades");
            ds.alarm_definition.Addalarm_definitionRow(true, "palisades",
                "pal", "fb", "above 5520", "", 10);
            ds.SaveTable(ds.alarm_definition);
            ds.alarm_recipient.Addalarm_recipientRow("palisades", 4,
                "5272", "office", "hydromet@usbr.gov");
            ds.SaveTable(ds.alarm_recipient);

            String file = Path.Combine(TestData.DataPath, "alarms", "pal_fb.csv");
            TextSeries s = new TextSeries(file);
            //TO DO .. read flags
            
            s.Parameter = "fb";
            s.SiteID = "pal";
            s.Read();
            Assert.IsTrue(s.Count > 500);

            ds.Check(s);

            var queue = ds.GetAlarmQueue();
            string sql = "list = 'palisades' AND siteid = 'pal' "
                + "AND parameter = 'fb' AND status = 'new'";
            Assert.IsTrue(queue.Select(sql).Length == 1);
            
        }

    
        [Test]
        public void Below()
        {
            var ds = db.Alarms;
            ds.AddNewAlarmGroup("lucky");
            ds.alarm_definition.Addalarm_definitionRow(true, "lucky",
                "luc", "fb", "below 5525", "", 10);
            ds.SaveTable(ds.alarm_definition);
            ds.alarm_recipient.Addalarm_recipientRow("lucky", 4,
                "5272", "office", "hydromet@usbr.gov");
            ds.SaveTable(ds.alarm_recipient);

            Series s = new Series();
            s.Parameter = "fb";
            s.SiteID = "luc";
            s.Add(DateTime.Parse("2016-11-21 02:00"), 5520.12);
            s.Add(DateTime.Parse("2016-11-21 02:15"), 5520.12);
            s.Add(DateTime.Parse("2016-11-21 02:30"), 5520.12);
            s.Add(DateTime.Parse("2016-11-21 02:45"), 5520.12);

            ds.Check(s);

            var queue = ds.GetAlarmQueue();
            string sql = "list = 'lucky' AND siteid = 'luc' "
                + "AND parameter = 'fb' AND status = 'new'";
            Assert.IsTrue(queue.Select(sql).Length == 1);

        }
        [Test]
        public void Dropping()
        {
            var ds = db.Alarms;
            ds.AddNewAlarmGroup("cre");
            ds.alarm_definition.Addalarm_definitionRow(true, "cre",
                "cre", "fb", "dropping 1", "", 10);
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
        [Test]
        public void Rising()
        {
            var ds = db.Alarms;
            ds.AddNewAlarmGroup("emi");
            ds.alarm_definition.Addalarm_definitionRow(true, "emi",
                "emi", "fb", "rising 1", "", 10);
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
        

        [Test]
        public void AboveOrRising()
        {

        }

    }
}
