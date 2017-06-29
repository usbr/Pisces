using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Depth
{
    public enum Scaling {LogLog, Linear };

    public class RatingTable
    {

        public RatingTable()
        {

            Scaling = Scaling.LogLog;
            Points = new ObservationPoint[] { };
        }

        public Scaling Scaling 
        {
            get; set;
        }
        public string Title { get; set; }

        public ObservationPoint[] Points { get; set; }
        public double Offset { get;  set; }
    }
}
