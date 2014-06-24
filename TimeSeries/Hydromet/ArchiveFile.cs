using Reclamation.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries.Hydromet
{
    /// <summary>
    /// ArchiveFile manages reading a binary (VAX) 
    /// 'archive' file 
    /// </summary>
    public class ArchiveFile
    {

        string m_fileName;
        public ArchiveFile(string fileName)
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
            BinaryReader br = new BinaryReader(fs,Encoding.ASCII);

            ArchiveFileRecord rec;
            while (br.PeekChar() != -1)
            {

                rec = ReadRecord(br);

                if (!rec.Valid)
                {
                    Console.WriteLine("[" + br.BaseStream.Position + "/" + br.BaseStream.Length + "] "+Path.GetFileName(m_fileName)
                    +" skipping invalid record : '"+rec.Site+"/"+rec.Pcode+"'" );
                    continue;
                }

                // wy2012.acf
                int yr = int.Parse( Path.GetFileNameWithoutExtension(m_fileName).Substring(2));

                DateTime t = new DateTime(yr-1, 10, 1);

                for (int i = 0; i < 366; i++)
                {
                    var row = tbl.NewRow();

                    row[0] = t;
                    row[1] = rec.Site.Trim().ToLower();
                    row[2] = rec.Pcode.Trim().ToLower();
                    row[3] = rec.Values[i];
                    string flag = "";
                    if (System.Math.Abs(rec.Values[i] - 998877.0) < 0.01)
                        flag = PointFlag.Missing;
                    row[4] = flag;

                    if( t.Date < DateTime.Now) // don't enter future data?? (in most cases).
                        tbl.Rows.Add(row);

                    if (t.Month == 9 && t.Day == 30)
                        break;

                    t = t.AddDays(1).Date;
                }


            }

            return tbl;
        }

        private ArchiveFileRecord ReadRecord(BinaryReader br)
        {
            ArchiveFileRecord r = new ArchiveFileRecord();

            r.Valid = true;
            r.Time = "";
            byte[] buff = new byte[2];
            r.Site = "null";
            int count = br.Read(buff, 0, 2);
            if (count <= 0)
            {
                r.Valid = false;
                if (br.BaseStream.Position == br.BaseStream.Length)
                    return r;
            }
            r.Id = BitConverter.ToInt16(buff, 0);

            r.Site = new string(br.ReadChars(12));
            if (r.Site == null)
            {
                r.Site = "null";
                r.Valid = false;
            }

            r.Pcode = new string(br.ReadChars(9));
            r.Time = new string(br.ReadChars(4));
            br.ReadChars(5); // spare bytes.
            
            r.Values = new float[366];
            for (int i = 0; i < 366; i++)
            {
             byte[] b= br.ReadBytes(4);
             if (b.Length != 4)
             {
                 r.Valid = false;
                 break;
             }
             float f = VaxBinaryReader.VaxSingleFromBytes(b);
             r.Values[i] = f;
            }

            br.ReadChars(5); // spare bytes.
            return r;
                        
        }



    }
}
