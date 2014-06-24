using System;
using System.Collections.Generic;
using System.Text;
using Reclamation.Core;

namespace Reclamation.TimeSeries
{
    internal class SubtractSeries:Series
    {
        Series a, b;
        /// <summary>
        /// Subtracts two series point by point a - b
        /// </summary>
        public SubtractSeries(Series a, Series b)
        {
            this.a = a;
            this.b = b;

            SetUnits(a, b);
        }

        private void SetUnits(Series a, Series b)
        {
            if (a.Units == b.Units)
                this.Units = a.Units;
            else
            {
                Logger.WriteLine("Error: Inconsistent Units in SubtractSeries " + a.Units + " , " + b.Units);
            }
        }

        /// <summary>
        /// Reads data from underlying a and b series 
        /// then subtracts a - b
        /// </summary>
        protected override void ReadCore(DateTime t1, DateTime t2)
        {

            a.Read(t1, t2);
            b.Read(t1, t2);
            SetUnits(a, b);
            Series s = Math.Subtract(a, b);
            this.Add(s);

        }
    }
}
