using System;
using System.Collections.Generic;
using System.Text;

namespace Reclamation.TimeSeries
{
    public class PiscesObjectEventArgs:EventArgs
    {

        public class CustomEventArgs : EventArgs
        {
            PiscesObject o;
            public CustomEventArgs(PiscesObject o)
            {
                this.o = o;
            }
        }
    }
}
