using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reclamation.Core
{
    //DateTime utc = TimeZoneInfo.ConvertTimeToUtc(ta, mst);
    //var localTime = utc.ToLocalTime();
    //bool invalidDate = TimeZoneInfo.Local.IsInvalidTime(localTime);
    //var local = TimeZoneInfo.ConvertTime(ta, TimeZoneInfo.Local);

    //http://noda-time.blogspot.com/2011/08/what-wrong-with-datetime-anyway.html
    //http://stackoverflow.com/questions/246498/creating-a-datetime-in-a-specific-time-zone-in-c-sharp-fx-3-5
    public struct DateTimeWithZone
    {
        private readonly DateTime utcDateTime;
        private readonly TimeZoneInfo timeZone;

        public DateTimeWithZone(DateTime dateTime, TimeZoneInfo timeZone)
        {
            if( dateTime.Kind != DateTimeKind.Unspecified)
              dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);

            utcDateTime = TimeZoneInfo.ConvertTimeToUtc(dateTime, timeZone);
            this.timeZone = timeZone;
        }

        public DateTime UniversalTime { get { return utcDateTime; } }

        public TimeZoneInfo TimeZone { get { return timeZone; } }

        public DateTime LocalTime
        {
            get
            {
                return TimeZoneInfo.ConvertTime(utcDateTime, timeZone);
            }
        }

        public bool TryConvertToTimeZone(TimeZoneInfo timeZone,out DateTime dateTime )
        {
            dateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, timeZone);

            return true;
        }
    }
}
