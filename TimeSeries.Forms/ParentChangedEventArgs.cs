using System;
using System.Collections.Generic;
using System.Text;

namespace Reclamation.TimeSeries.Forms
{
    /// <summary>
    /// Events to notify that a PiscesObject has a new folder (new Parent)
    /// </summary>
    public class ParentChangedEventArgs:EventArgs
    {    
        public ParentChangedEventArgs(PiscesObject o, PiscesFolder folder)
        {
            m_piscesObject = o;
            m_folder = folder;
        }
        PiscesObject m_piscesObject;

      
        public PiscesObject PiscesObject
        {
            get { return m_piscesObject; }
            set { m_piscesObject = value; }
        }
        PiscesFolder m_folder;

        public PiscesFolder Folder
        {
            get { return m_folder; }
            set { m_folder = value; }
        }
 



    }
}
