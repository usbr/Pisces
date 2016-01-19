using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace HydrometNotifications
{
    [TestFixture]
    public class TestCases
    {

        [TestCase]
        public void TestTodaysState()
        {
            // crashing for todays date... no daily value yet for today...
            var s = RogueMinimumFlow.DetermineSystemState(DateTime.Now);
        }


        [TestCase]
        public void TestFeb1994State()
        {
            DateTime t = new DateTime(1994, 2, 26);
            var s = RogueMinimumFlow.DetermineSystemState(t);

            Assert.AreEqual(s, RogueMinimumFlow.SystemState.Median);

            t = new DateTime(1994, 2, 27);
            s = RogueMinimumFlow.DetermineSystemState(t);
            Assert.AreEqual(s, RogueMinimumFlow.SystemState.Dry);


            t = new DateTime(1995, 12, 4);
            s = RogueMinimumFlow.DetermineSystemState(t);
            Assert.AreEqual(s, RogueMinimumFlow.SystemState.Median);


            t = new DateTime(1995, 12, 5);
            s = RogueMinimumFlow.DetermineSystemState(t);
            Assert.AreEqual(s, RogueMinimumFlow.SystemState.Wet);


        }


        /// <summary>
        /// Test for Rogue System State Changes
        ///  feb 25, 1994  Avg
        ///  feb 26, 1994  Avg
        ///  feb 27, 1994  Dry
        /// </summary>
        [TestCase]
        public void TestStateChanges()
        {


            DateTime t = new DateTime(1994, 2, 26); 
            var a = new AlarmGroup("test_rogue_state", false, true);
            a.ProcessGroup(t);
            Assert.AreEqual(0, a.AlarmCount);

            t = new DateTime(1994, 2, 27);
            a = new AlarmGroup("test_rogue_state", false, true);
            a.ProcessGroup(t);
            Assert.AreEqual(1, a.AlarmCount);


        }

        [TestCase]
        public void BasoDuringWinter()
        {
            DateTime t = new DateTime(2001, 11, 15); // no minimums during November
            
            var a = new AlarmGroup("test_baso", false, true);
            a.UpdateRows("active", false);
            a.ProcessGroup(t);

            Assert.AreEqual(0, a.AlarmCount);
        }

        [TestCase]
        public void BasoApril()
        {
            DateTime t = new DateTime(1992, 4, 10); // this should create an alarm.

            var a = new AlarmGroup("test_baso", false, true);
            a.UpdateRows("active", false);
            a.ProcessGroup(t);

            Assert.AreEqual(1, a.AlarmCount);

        }

        /// <summary>
        /// Flows are low on April 14, 2014.  However, the canal TALO is not running
        /// so the alert should not be raised.
        /// </summary>
        [TestCase]
        public void BasoApril2014()
        {
            DateTime t = new DateTime(2014, 4, 10); // this should NOT create an alarm. 

            var a = new AlarmGroup("test_baso", false, true);
            a.UpdateRows("active", false);
            a.ProcessGroup(t);

            Assert.AreEqual(0, a.AlarmCount);

        }

        [TestCase]
        public void LimitAlarm()
        {
         var a = new AlarmGroup("test_cmo",true,true);
         a.UpdateRows("active", false);
         a.ProcessGroup(new DateTime(2012,6,5)); // reservoir is above spillway.
         Assert.AreEqual(1, a.ProcessCount);

        // now clear the alarm when reservoir has gone back down.
         a.ProcessGroup(new DateTime(2012, 9, 12));

         Assert.AreEqual(1, a.ClearCount);
         Assert.AreEqual(1, a.ProcessCount);
        }

        [TestCase]
        public void SiteDown()
        {
            var a = new AlarmGroup("test_down", true, true);
            a.UpdateRows("active", false);
            a.ProcessGroup(DateTime.Now);  // non existant site will give error

            Assert.AreEqual(1, a.AlarmCount);
        }

        /// <summary>
        /// Flags at Phillips Lake on Feb 15, 2013
        /// </summary>
        [TestCase]
        public void AnyFlags()
        {
            var a = new AlarmGroup("test_flags", true, true);
            a.UpdateRows("active", false);
            a.ProcessGroup(new DateTime(2013,2,15)); 

            Assert.AreEqual(1, a.AlarmCount);
        }
    }
}
