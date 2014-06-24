using System;
using System.Collections.Generic;
using System.Text;

namespace Reclamation.TimeSeries.Forms
{
    public class SortChangedEventArgs:EventArgs
    {
        
        public SortChangedEventArgs(PiscesObject o, int sortOrder)
        {
            this.m_PiscesObject = o;
            this.m_sortOrder = sortOrder;
        }
        PiscesObject m_PiscesObject;

        public PiscesObject PiscesObject
        {
            get { return m_PiscesObject; }
            set { m_PiscesObject = value; }
        }
        int m_sortOrder;

        public int SortOrder
        {
            get { return m_sortOrder; }
            set { m_sortOrder = value; }
        }


    }
}
