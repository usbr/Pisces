
namespace Reclamation.TimeSeries.Alarms
{
    public enum AlarmType { Above, Below, Rising, Dropping };
    public class AlarmCondition
    {
      public AlarmCondition(AlarmType t, double value)
        {
            this.Condition = t;
            this.Value = value;

        }

        public double Value { get; set; }

        public AlarmType Condition { get; set; }
    }


}
