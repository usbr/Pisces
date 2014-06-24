using System;

namespace Reclamation.TimeSeries
{

    public enum TimeWindowType { FullPeriodOfRecord, FromToDates, FromDateToToday, NumDaysFromToday };

    /// <summary>
    /// TimeWindow describes a time window 
    /// </summary>
    public class TimeWindow
    {
        public DateTime FromToDatesT1 { get; set; }
        public DateTime FromToDatesT2 { get; set; }
        public DateTime FromDateToTodayT1 { get; set; }
        public decimal NumDaysFromToday { get; set; }
        public TimeWindowType WindowType { get; set; }

        public TimeWindow()
        {
            FromToDatesT1 = DateTime.Now.Date.AddDays(-5);
            FromToDatesT2 = DateTime.Now;
            FromDateToTodayT1 = DateTime.Now.Date.AddDays(-5);
            NumDaysFromToday = 5;
        }

        public override string ToString()
        {
            if (WindowType == TimeWindowType.FullPeriodOfRecord)
                return "full period of record";
            return T1.ToShortDateString() + " to " + T2.ToShortDateString();
        }

        public DateTime T1
        {
            get
            {
                if (WindowType == TimeWindowType.FullPeriodOfRecord)
                    return DateTime.MinValue;
                else
                    if (WindowType == TimeWindowType.FromToDates)
                        return FromToDatesT1;
                    else
                        if (WindowType == TimeWindowType.FromDateToToday)
                            return FromDateToTodayT1;
                        else
                            if (WindowType == TimeWindowType.NumDaysFromToday)
                                return DateTime.Now.Date.AddDays(Convert.ToDouble(-NumDaysFromToday));

                throw new NotImplementedException();
            }
        }

        public DateTime T2
        {
            get
            {
                if (WindowType == TimeWindowType.FullPeriodOfRecord)
                    return DateTime.MaxValue;
                else
                    if (WindowType == TimeWindowType.FromToDates)
                        return FromToDatesT2;
                    else
                        if (WindowType == TimeWindowType.FromDateToToday || WindowType == TimeWindowType.NumDaysFromToday)
                            return DateTime.Now;

                throw new NotImplementedException();
            }
        }
    }
}