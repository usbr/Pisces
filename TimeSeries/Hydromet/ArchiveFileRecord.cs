using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries.Hydromet
{
    internal struct ArchiveFileRecord
    {
        /// <summary>
        /// fixed user id (100)
        /// </summary>
        public short Id;
        public string Site;
        public string Pcode;
        public string Time;
        public float[] Values;

        public bool Valid;

    }
}
