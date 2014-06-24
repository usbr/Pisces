using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries.Hydromet
{
    internal struct DayFileRecord
    {
        /// <summary>
        /// fixed user id (100)
        /// </summary>
        public short Id;
        /// <summary>
        /// D=data Q=diagnostic
        /// </summary>
        public char Format;
        public string Site;
        public string TimeString;
        /// <summary>
        /// Reporting model  S=self timed ,  R=random
        /// </summary>
        public char Rmode;
        /// <summary>
        /// Telemetry type
        /// </summary>
        public char Ttype;

        public int Tzone;
        public char Ctype;
        public int Npairs;
        public sbyte[] Quality;
        public string[] Pcodes;
        public float[] Values;

        public bool Valid;
        public DateTime t;



    }
}
