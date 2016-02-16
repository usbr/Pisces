using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Reclamation.TimeSeries;
using Reclamation.Core;
using System;

namespace Pisces.NunitTests.SeriesFeatures
{
    [TestFixture]
    public class TestShiftSeriesBug
    {

        [Test]
        public void Test1()
        {
            Series s = new Series();

            DateTime t = new DateTime(2011, 2, 28);
            DateTime t2 = new DateTime(2011, 12, 31);


            do
            {
                s.Add(t, t.Day);
                t = t.AddDays(1);
            } while (t <= t2);

            //check from non leap year
           var s2 = Reclamation.TimeSeries.Math.ShiftToYear(s, 2008);

            s2 = Reclamation.TimeSeries.Math.ShiftToYear(s, 2016);

            s2 = Reclamation.TimeSeries.Math.ShiftToYear(s, 2009);

            s2 = Reclamation.TimeSeries.Math.ShiftToYear(s, 2017);

            Series s3 = new Series();

            t = new DateTime(2016, 2, 28);
            t2 = new DateTime(2016, 12, 31);


            do
            {
                s2.Add(t, t.Day);
                t = t.AddDays(1);
            } while (t <= t2);

            //check from leap year
            s2 = Reclamation.TimeSeries.Math.ShiftToYear(s, 2008);

            s2 = Reclamation.TimeSeries.Math.ShiftToYear(s, 2020);

            s2 = Reclamation.TimeSeries.Math.ShiftToYear(s, 2009);

            s2 = Reclamation.TimeSeries.Math.ShiftToYear(s, 2017);
        }

    }
}

