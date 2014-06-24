using System;

namespace Reclamation.TimeSeries.Urgsim
{
    public class UrgsimReferenceSeries : Reclamation.TimeSeries.Series
    {
        SeriesList m_sl = new SeriesList();

        public UrgsimReferenceSeries(SeriesList serieslist)
        {
            m_sl = serieslist;
        }

        protected override void ReadCore(DateTime t1, DateTime t2)
        {
            foreach (Series s in m_sl)
            {
                s.Read(t1, t2);
            }

            Series median = m_sl.Median();
            Clear();
            this.Add(median);
        }
    }
}
