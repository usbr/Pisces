using System;
using NUnit.Framework;
namespace Reclamation.Core.Tests
{
    /// <summary>
    /// Test converting from Mountain standard to Mountain savings times
    /// </summary>
    [TestFixture]
    public class TestTimeZoneMath
    {

        [Test]
        public void MarchForward()
        {
            DateTime t = new DateTime(2014, 3, 9, 1, 0, 0);
            DateTime t2 = new DateTime(2014,3,9,4,0,0);

            IterateAndConvertDates(t, t2);
        }
        [Test]
        public void NovemberBack()
        {
            DateTime t = new DateTime(2014, 11, 2, 0, 0, 0);
            DateTime t2 = t.AddHours(3);
            t = IterateAndConvertDates(t, t2);
        }

        private static DateTime IterateAndConvertDates(DateTime t, DateTime t2)
        {

            TimeZoneInfo mst = TimeZoneInfo.FindSystemTimeZoneById("US Mountain Standard Time"); // no daylight shift
            TimeZoneInfo mdt = TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"); // with daylight shifting
            Console.WriteLine("  IsDaylightSavingTime  MST                  mdt       ");
            while (t <= t2)
            {
                
                DateTimeWithZone d = new DateTimeWithZone(t, mst);
                DateTime t_mdt;
                string s_mdt = " errror ";
                if (d.TryConvertToTimeZone(mdt, out t_mdt))
                    s_mdt = t_mdt.ToString();

                Console.WriteLine(t.IsDaylightSavingTime()+"   "+ t.ToString() + "     " + s_mdt);

                t = t.AddMinutes(15);
            }
            return t;
        }
    }
}
