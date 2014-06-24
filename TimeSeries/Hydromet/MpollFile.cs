using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries.Hydromet
{
    /// <summary>
    /// MpollFile manages reading a binary (VAX) 
    /// file with monthly data:  mpoll.ind
    /// </summary>
    public class MpollFile
    {
         string m_fileName;
         public MpollFile(string fileName)
        {
            m_fileName = fileName;
        }

        public DataTable GetTable()
        {
            DataTable tbl = new DataTable();

            tbl.Columns.Add("DateTime", typeof(DateTime));
            tbl.Columns.Add("site");
            tbl.Columns.Add("pcode");
            tbl.Columns.Add("value", typeof(double));
            tbl.Columns.Add("flag");

            FileStream fs = File.OpenRead(m_fileName);
            BinaryReader br = new BinaryReader(fs, Encoding.ASCII);




            return tbl;

        }

    }
}
