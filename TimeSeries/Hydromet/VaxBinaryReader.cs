using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Reclamation.TimeSeries.Hydromet
{
    public class VaxBinaryReader
    {
        BinaryReader br;
        public VaxBinaryReader(BinaryReader br)
        {
            this.br = br;
        }

        public string ReadCharacter(int count)
        {
            return new string(br.ReadChars(count));
        }
        public int ReadIntFromTwoBytes()
        {
            byte[] buff = new byte[2];
            int x = br.Read(buff, 0, 2);
            if (x <= 0)
            {
                Console.WriteLine("Error filing buff[]");
            }
            return BitConverter.ToInt16(buff, 0);
                
        }

        [StructLayout(LayoutKind.Explicit)]
        struct convertstruct
        {
            [FieldOffset(0)]
            public byte b0;
            [FieldOffset(1)]
            public byte b1;
            [FieldOffset(2)]
            public byte b2;
            [FieldOffset(3)]
            public byte b3;
            [FieldOffset(0)]
            public float f;
        }


        public static float VaxSingleFromBytes(byte[] bytes)
        {
            convertstruct cs = new convertstruct();
            cs.b0 = bytes[2];
            cs.b1 = bytes[3];
            cs.b2 = bytes[0];
            cs.b3 = bytes[1];
            return cs.f / 4;
        }
    }
}
