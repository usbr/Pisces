using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.IO;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.Core;
using System.Data;


namespace Reclamation.TimeSeries.Hydromet
{
    public enum DumpFormat { CSV, SQL, DataTable };
    public class DayFile
    {
        string m_filename;
        DateTime m_datetime;
        bool m_keepQuality;
        bool m_keepRandom;
        public DayFile(string filename, bool keepQuality=false, bool keepRandoms = false)
        {
            m_filename = filename;
            m_keepQuality = keepQuality;
            m_keepRandom = keepRandoms;
            m_datetime = DateTime.Parse(Path.GetFileNameWithoutExtension(m_filename));

        }

        public DataTable GetTable()
        {
            var fn = FileUtility.GetTempFileName(".csv");

            DataTable tbl =  Dump(fn, DumpFormat.DataTable);

            return tbl;
        }

        /// <summary>
        /// Dumps contents of dayfile to the console
        /// </summary>
        public DataTable Dump(string outputFilename, DumpFormat fmt)
        {
            DataTable rval = new DataTable();

            int recordCount = 0;
            
            
            FileStream fs = File.OpenRead(m_filename);
            StreamWriter sw = new StreamWriter(outputFilename);
            if( fmt == DumpFormat.CSV)
                   sw.WriteLine("DateTime, id,format,site,time,Rmode,tType,tZone,cType,pcode,value,flag");
            if (fmt == DumpFormat.DataTable)
            {
                //sw.WriteLine("DateTime, site,pcode,value,flag");
                rval.Columns.Add("DateTime", typeof(DateTime));
                rval.Columns.Add("site");
                rval.Columns.Add("pcode");
                rval.Columns.Add("value", typeof(double));
                rval.Columns.Add("flag");

            }
            if( fmt == DumpFormat.SQL)
                sw.WriteLine("-- Auto Generated SQL --");

            
            BinaryReader br = new BinaryReader(fs,Encoding.ASCII);

            DayFileRecord rec;
            while( br.PeekChar() != -1)
            {

               rec =  ReadRecordV2(br);
               recordCount++;
               if (!rec.Valid)
               {
                   Console.WriteLine("["+br.BaseStream.Position+"/"+br.BaseStream.Length+"]skipping invalid record :"+BuildRecordPrefix(ref rec) );
                   continue;
               }
               if (rec.Rmode == 'R' && !m_keepRandom)
                   continue;

               if (rec.Format == 'Q' && !m_keepQuality)
                   continue;


               if (rec.Id == 0)
                   break;
                if( fmt == DumpFormat.CSV)
                  WriteCsvRecord(sw,rec);
                if (fmt == DumpFormat.DataTable)
                    AddDataTableRow(rval, rec);
                if (fmt == DumpFormat.SQL)
                    WriteSqlRecord(sw, rec);
                

            }

            br.Close();
            sw.Close();
            //p.Report();
            return rval;
        }

        private void WriteSqlRecord(StreamWriter sw, DayFileRecord rec)
        {
            if (rec.Format == 'Q' && !m_keepQuality)
                return;

            string timeStr = rec.TimeString.Trim();
            if (timeStr.Length < 3)
                timeStr = "00:" + timeStr;
            if (timeStr.Length == 4)
                timeStr = timeStr.Substring(0, 2) + ":" + timeStr.Substring(2);// +rec.Tzone;

            //INSERT INTO daily_amf_id(datetime, value, flag) values('2030-08-1 00:00:00', 998877,'')

            string t = m_datetime.ToString("yyyy-MM-d") + " " + timeStr +  ",";
                
                           

            for (int i = 0; i < rec.Npairs; i++)
            {
                string table_name = "instant_"+rec.Site.ToLower().Trim()+"_"+rec.Pcodes[i].ToLower().Trim();

                string sql = "Insert into " + table_name + "(datetime, value, flag) values('"
                 + t + "'," + rec.Values[i] + ",'" + FlagFromQuality(rec.Quality[i]) + "');";

                sw.WriteLine(sql);
            }
        }


        static sbyte[] s_codes = new sbyte[] {32, -1, -3, -5, -7, -0, -2, -4, -6, -8, -10, -12, -14, -16, -18, -20, -22, -24, -26, -28 };
        static string[] s_flg = new string[] {"", " ", "e", "e", "e", "u", "n", "m", "p", "i", "f", "r", "?", "b", "a", "-", "+", "^", "~", "|" };

        private static string FlagFromQuality(sbyte flag)
        {


            int idx = Array.IndexOf(s_codes,flag);

            if( idx >=0)
                return s_flg[idx];


            return "u"; //unknown flag";

        

        }


        private void WriteCsvRecord(StreamWriter sw, DayFileRecord rec)
        {

          string str = BuildRecordPrefix(ref rec);

         for (int i = 0; i < rec.Npairs; i++)
         {
                 sw.WriteLine(str+","+rec.Pcodes[i].Trim() + "," + rec.Values[i]+","+ rec.Quality[i] );
         }

        }

        private string BuildRecordPrefix(ref DayFileRecord rec)
        {
            string timeStr = BuildTimeString(ref rec);

            string str = m_datetime.ToShortDateString() + " " + timeStr + " ," + rec.Id + "," + rec.Format + "," + rec.Site.Trim() + "," + rec.TimeString + ","
                           + rec.Rmode + "," + rec.Ttype + "," + rec.Tzone + ","
                           + rec.Ctype;
            return str;
        }

        private static string BuildTimeString(ref DayFileRecord rec)
        {
            if( rec.TimeString == null)
                return "";
            string timeStr = rec.TimeString.Trim();
            if (timeStr.Length < 3)
                timeStr = "00:" + timeStr;
            if (timeStr.Length == 4)
                timeStr = timeStr.Substring(0, 2) + ":" + timeStr.Substring(2);
            return timeStr;
        }
        /// <summary>
        /// Just print essential 
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="rec"></param>
        private void AddDataTableRow(DataTable tbl, DayFileRecord rec)
        {


           
            for (int i = 0; i < rec.Npairs; i++)
            {
                var row = tbl.NewRow();

                

                row[0] = rec.t;
                row[1] = rec.Site.Trim().ToLower();
                row[2] = rec.Pcodes[i].Trim().ToLower();
                row[3] = rec.Values[i];
                row[4] = FlagFromQuality(rec.Quality[i]);
                tbl.Rows.Add(row);
            }

        }

        private DayFileRecord ReadRecordV2(BinaryReader br)
        {
            DayFileRecord r = new DayFileRecord();
            try
            {

                while (br.PeekChar() == '\n')
                {
                    char c = br.ReadChar();
                    Console.Write(" '\n' ");

                }

                r.Valid = true;
                byte[] buff = new byte[22];
                r.Site = "null";
                int count = br.Read(buff, 0, 22);
                if (count != 22)
                {
                    Console.WriteLine("Error:  Record did not have 22 byte header");
                    r.Valid = false;
                    if (br.BaseStream.Position == br.BaseStream.Length)
                        return r;
                }
                
                r.Id = BitConverter.ToInt16(buff, 0);// 64 (reads two bytes)

                r.Format = Convert.ToChar(buff[2]);

                r.Site = Encoding.ASCII.GetString(buff, 3, 8);

                if (r.Site == null)
                {
                    r.Site = "null";
                    r.Valid = false;
                }
                r.TimeString = Encoding.ASCII.GetString(buff, 11, 4);

                if (r.TimeString == "2400")
                {
                    r.TimeString = "2359";
                }
                string timeStr = BuildTimeString(ref r);

                string str = m_datetime.ToShortDateString() + " " + timeStr;

                if (!DateTime.TryParse(str, out r.t))
                {
                    Console.WriteLine(" invalid date " + str);
                    r.t = DateTime.MinValue;
                    r.Valid = false;
                }

                r.Rmode = Convert.ToChar(buff[15]);
                //if (r.Rmode != 'R' && r.Rmode != 'S')
                //{
                //    //   r.Valid = false;
                //    //Console.WriteLine("bad Rmode = '"+r.Rmode+"'"); //  'B'
                //}

                r.Ttype = Convert.ToChar(buff[16]);

                r.Tzone = (int)(sbyte)buff[17];

                r.Ctype = Convert.ToChar(buff[18]);
                //byte[] unused = br.ReadBytes(2);

                r.Npairs = (int)(sbyte)buff[21];

                if (r.Npairs <= 0)
                    return r;

                byte[] buff2 = new byte[r.Npairs];

                sbyte[] quality;
                string[] pCodes = ReadPcodes(br, r.Npairs, out quality);
                float[] values = ReadValues(br, r.Npairs);
                r.Pcodes = pCodes;
                r.Values = values;
                r.Quality = quality;

                //r.Valid = true;
                
            }
            catch (EndOfStreamException e)
            {
                r.Valid = false;
                Console.WriteLine(e.Message);
              //  throw e;
            }
            return r;
        }

        private DayFileRecord ReadRecordV1(BinaryReader br)
        {
            DayFileRecord r = new DayFileRecord();
            try
            {

                r.Valid = true;
                byte[] buff = new byte[23];
                r.Site = "null";
                int count = br.Read(buff, 0, 2);
                if (count <= 0)
                {
                    r.Valid = false;
                    if (br.BaseStream.Position == br.BaseStream.Length)
                        return r;
                }

                r.Id = BitConverter.ToInt16(buff, 0);// 64
                r.Format = br.ReadChar();
                r.Site = new string(br.ReadChars(8));
                if (r.Site == null)
                {
                    r.Site = "null";
                    r.Valid = false;
                }
                r.TimeString = new string(br.ReadChars(4));

                string timeStr = BuildTimeString(ref r);

                string str = m_datetime.ToShortDateString() + " " + timeStr;

                if (!DateTime.TryParse(str, out r.t))
                {
                    Console.WriteLine(" invalid date " + str);
                    r.t = DateTime.MinValue;
                    r.Valid = false;
                }

                r.Rmode = br.ReadChar();
                //if (r.Rmode != 'R' && r.Rmode != 'S')
                //{
                //    //   r.Valid = false;
                //    //Console.WriteLine("bad Rmode = '"+r.Rmode+"'"); //  'B'
                //}

                r.Ttype = br.ReadChar();

                r.Tzone = br.ReadSByte();
                r.Ctype = br.ReadChar();
                byte[] unused = br.ReadBytes(2);

                r.Npairs = br.ReadSByte();

                if (r.Npairs <= 0)
                    return r;

                sbyte[] quality;
                string[] pCodes = ReadPcodes(br, r.Npairs, out quality);
                float[] values = ReadValues(br, r.Npairs);
                r.Pcodes = pCodes;
                r.Values = values;
                r.Quality = quality;

                //r.Valid = true;

            }
            catch (EndOfStreamException e)
            {
                r.Valid = false;
                Console.WriteLine(e.Message);
                //  throw e;
            }
            return r;
        }
      
        private float[] ReadValues(BinaryReader br, int count)
        {
            float[] rval = new float[count];
            float f;
          //  Console.WriteLine(" OB             15.45 # PC             17.87");
            for (int i = 0; i < count; i++)
            {
                byte[] b= br.ReadBytes(4);
                if (b.Length != 4)
                {
                    Console.WriteLine("Error: only "+b.Length+" bytes.  Expected 4");
                  
                   f = (float) Point.MissingValueFlag;
                }
                else
                  f = VaxBinaryReader.VaxSingleFromBytes(b);

               rval[i] = f;
            }
            return rval;

        }

        
        /// <summary>
        /// http://www.codeproject.com/KB/applications/libnumber.aspx
        /// </summary>
        float VaxSingleFromBytesOld(byte[] bytes)
        {
            uint S;
	        uint E;
	        ulong F;
            uint b1 = bytes[1];
            uint b2 = bytes[0];
            uint b3 = bytes[3];
            uint b4 = bytes[2];

            S = (b1 & 0x80) >> 7;
            E = ((b1 & 0x7f) << 1) + ((b2 & 0x80) >> 7);
            F = ((b2 & 0x7f) << 16) + (b3 << 8) + b4;

            float rval = 0;
            double M, F1, A, B, C, e24;
            A = 2.0;
            B = 128.0;
            C = 0.5;
            e24 = 16777216.0;		// 2^24

            M = (double)F / e24;

            if (S == 0) F1 = 1.0;
            else F1 = -1.0;

            if (0 < E) rval = (float)(F1 * (C + M) * System.Math.Pow(A, E - B));
            else if (E == 0 && S == 0)
                rval = 0;
            else if (E == 0 && S == 1)
                throw new ArgumentOutOfRangeException();
                //return -1; // reserved

            return rval;
        }


        private string[] ReadPcodes(BinaryReader br, int count, out sbyte[] quality)
        {
            string[] rval = new string[count];
            quality = new sbyte[count];
            //sbyte[] x = new sbyte[count];
            for (int i = 0; i < count; i++)
            {
                rval[i] = new string(br.ReadChars(7));
                char spare = br.ReadChar();

                quality[i] = br.ReadSByte();
                //quality[i] = x[i].ToString()[0];//br.ReadChar();
            }

            return rval;
        }


        ///// <summary>
        ///// http://articles.techrepublic.com.com/5100-10878_11-5062324.html#
        ///// </summary>
        //public static object EndianFlip(object oObject)
        //{
        //    string sFieldType;

        //    Type tyObject = oObject.GetType();

        //    FieldInfo[] miMembers;
        //    miMembers = tyObject.GetFields();

        //    for (int Looper = miMembers.GetLowerBound(0);
        //           Looper <= miMembers.GetUpperBound(0);
        //           Looper++)
        //    {
        //        sFieldType = miMembers[Looper].FieldType.FullName;
        //        if ((String.Compare(sFieldType, "System.UInt16", true) == 0))
        //        {
        //            ushort tmpUShort;
        //            tmpUShort = (ushort)miMembers[Looper].GetValue(oObject);
        //            tmpUShort = (ushort)(((tmpUShort & 0x00ff) << 8) +
        //                  ((tmpUShort & 0xff00) >> 8));
        //            miMembers[Looper].SetValue(oObject, tmpUShort);
        //        }
        //        else
        //            if (String.Compare(sFieldType, "System.UInt32", true) == 0)
        //            {
        //                uint tmpInt;
        //                tmpInt = (uint)miMembers[Looper].GetValue(oObject);
        //                tmpInt = (uint)(((tmpInt & 0x000000ff) << 24) +
        //                                  ((tmpInt & 0x0000ff00) << 8) +
        //                                  ((tmpInt & 0x00ff0000) >> 8) +
        //                                  ((tmpInt & 0xff000000) >> 24));
        //                miMembers[Looper].SetValue(oObject, tmpInt);
        //            }
        //    }

        //    return (oObject);

        //}

        //public static byte[] RawSerialize(object anything)
        //{
        //    int rawsize = Marshal.SizeOf(anything);
        //    IntPtr buffer = Marshal.AllocHGlobal(rawsize);
        //    Marshal.StructureToPtr(anything, buffer, false);
        //    byte[] rawdatas = new byte[rawsize];
        //    Marshal.Copy(buffer, rawdatas, 0, rawsize);
        //    Marshal.FreeHGlobal(buffer);
        //    return rawdatas;

        //}


        //public static object RawDeserialize(byte[] rawdatas, Type anytype)
        //{
        //    int rawsize = Marshal.SizeOf(anytype);
        //    if (rawsize > rawdatas.Length)
        //        return null;
        //    IntPtr buffer = Marshal.AllocHGlobal(rawsize);
        //    Marshal.Copy(rawdatas, 0, buffer, rawsize);
        //    object retobj = Marshal.PtrToStructure(buffer, anytype);
        //    Marshal.FreeHGlobal(buffer);
        //    return retobj;

        //}




    }
}
