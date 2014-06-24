using System;
using System.Collections.Generic;
using System.Text;

namespace Reclamation.TimeSeries
{
  public class ConstantSeries:Series
    {
      double m_value;
      public ConstantSeries(string name, string parameter, double value)
      {
          m_value = value;
          this.Name = name;
          this.Parameter = parameter;
      }

      protected override void ReadCore(DateTime t1, DateTime t2)
      {
          DateTime t = t1.Date;
          while (t < t2)
          {
              Add(t, m_value);
              t = t.AddDays(1);
          }
      }

    }
}
