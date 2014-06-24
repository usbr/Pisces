using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries.Hydromet
{
    public struct SutronRatingTableRecord
    {
        public string Site;
        public string Ylabel;
        public string Expires;
        public int RecordNumber;
        public string Description;
        public int InterperpolationCode;
        public string Xlabel;
        public string Yunits;
        public string Xunits;
        public int Sigdigits;
        public short CreateDate;
        public short ExpireDate;
        public short EditDate;
        public short XferDate;


    }
}
