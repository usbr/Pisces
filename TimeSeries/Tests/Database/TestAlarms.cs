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

        [Test]
        public void DatabaseAlarmTest()
        {
            // create database with alarm def
            var fn = FileUtility.GetTempFileName(".pdb");
            SQLiteServer svr = new SQLiteServer(fn);
            TimeSeriesDatabase db = new TimeSeriesDatabase(svr);

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
            Assert.AreEqual(1, queue.Count, "expected 1 alarm in the queue");

        }

        [Test]
        public void Above()
        {
            AlarmRegex re = new AlarmRegex("above 4198.20");

            Assert.IsTrue(re.AlarmConditions()[0].Condition == AlarmType.Above);

            //Assert.IsFalse(re.IsAlarm(1.0));

            //Assert.IsFalse(re.IsAlarm(4198.20));

            //Assert.IsTrue(re.IsAlarm(5000.1));
        }
        [Test]
        public void Below()
        {
            AlarmRegex re = new AlarmRegex("below 4198.20");

            Assert.IsTrue(re.AlarmConditions().Length == 1);
            var c = re.AlarmConditions()[0];
            Assert.AreEqual(4198.20, c.Value);
            Assert.AreEqual(AlarmType.Below, c.Condition);

        }
        [Test]
        public void Dropping()
        {
            AlarmRegex re = new AlarmRegex("dropping 0.25");

            //Assert.IsTrue(re.IsAlarm(1.0,2.0));
        }
        [Test]
        public void Rising()
        {
            AlarmRegex re = new AlarmRegex("rising  1");

           // Assert.IsTrue(re.IsAlarm(1.0,2.0));
        }
        

        [Test]
        public void AboveOrRising()
        {

        }

    }
}
