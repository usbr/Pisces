using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Reclamation.TimeSeries.Hydromet
{
    /// <summary>
    /// Provides Read access to binary Rating Table file from OpenVMS Sutron DMS system
    /// </summary>
    public class SutronRatingTable
    {

        string m_fileName;
        public SutronRatingTable()
        {
            m_fileName = Path.Combine(Reclamation.Core.Globals.LocalConfigurationDataPath, "sutron_rtf.dat");

        }

        public void WriteToConsole()
        {
            FileStream fs = File.OpenRead(m_fileName);
            BinaryReader br = new BinaryReader(fs, Encoding.ASCII);

            var rec = ReadRecord(br);

            br.Close();

        }


        private SutronRatingTableRecord ReadRecord(BinaryReader br)
        {
            SutronRatingTableRecord r = new SutronRatingTableRecord();
            r.Site = 
            r.Ylabel = new string(br.ReadChars(7));
            r.Expires = new string(br.ReadChars(5));
            r.RecordNumber = br.ReadSByte();
            r.Description = new string(br.ReadChars(37));
            r.InterperpolationCode = br.ReadSByte();
            r.Xlabel = new string(br.ReadChars(7));
            r.Yunits = new string(br.ReadChars(5));
            r.Xunits = new string(br.ReadChars(5));
            r.Sigdigits = br.ReadSByte();
            r.CreateDate = br.ReadInt16();
            r.EditDate = br.ReadInt16();
            r.ExpireDate = br.ReadInt16();
            r.XferDate = br.ReadInt16();
            r.CreateDate = br.ReadInt16();


            Console.WriteLine();
            /*r.Format = br.ReadChar();

            byte[] buff = new byte[2];
            int count = br.Read(buff, 0, 2);
            if (count <= 0)
                return r;
            r.Id = BitConverter.ToInt16(buff, 0);// 12;// br.ReadInt16();
            r.Format = br.ReadChar();
           
            if (r.Site == null)
                Console.WriteLine("");
            r.TimeString = new string(br.ReadChars(4));
            r.Rmode = br.ReadChar();
            r.Ttype = br.ReadChar();

            r.Tzone = br.ReadSByte();
            r.Ctype = br.ReadChar();
            byte[] unused = br.ReadBytes(2);

            r.Npairs = br.ReadSByte();



            char[] quality;
            r.Pcodes = pCodes;
            r.Values = values;
            r.Quality = quality;
            */
            return r;
        }


       

    }
}
