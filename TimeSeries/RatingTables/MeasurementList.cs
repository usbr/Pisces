using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries.RatingTables
{
    /// <summary>
    /// Manage a list of measurements.
    /// </summary>
    public class MeasurementList
    {
        List<BasicMeasurement> m_measurements = new List<BasicMeasurement>();
        public MeasurementList(BasicMeasurement[] measurements= null )
        {
            if (measurements != null)
                m_measurements.AddRange(measurements);
        }

        public void Add(BasicMeasurement m)
        {
            m_measurements.Add(m);
        }
        public string Text
        {
            get
            {
                var x = m_measurements.Select(a => a.MeasurementRow.siteid).Distinct();
                var b = x.ToArray();
                return String.Join(",",b);
            }

        }


        public double MinDischarge { 
            get
            {
                return m_measurements.Select(a => a.MeasurementRow.discharge).Min();
            }

            }

        public double MaxDischarge
        {
            get
            {
                return m_measurements.Select(a => a.MeasurementRow.discharge).Max(); 
            }
        }

        public double MinStage {
            get
            {
                return m_measurements.Select(a => a.MeasurementRow.stage).Min();
            }
        }

        public double MaxStage
        {
            get
            {
                return m_measurements.Select(a => a.MeasurementRow.stage).Max();
            }
        }

        public BasicMeasurement[] ToArray()
        {
            return m_measurements.ToArray();
        }
    }
}
