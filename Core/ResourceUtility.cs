using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
namespace Reclamation.Core
{
    public static class ResourceUtility
    {

        public static void ExtractResource(Type type, string resourceName, string filename)
        {
            Assembly asm = Assembly.GetAssembly(type);
            //Assembly asm = Assembly.GetExecutingAssembly();
            string[] names = asm.GetManifestResourceNames();
            foreach (var s in names)
            {
                Console.WriteLine(s);  
            }

            Stream stream = asm.GetManifestResourceStream(resourceName);//
            if (stream == null)
            {
                throw new Exception("Resource not found " + resourceName);
            }

            byte[] buffer = new byte[stream.Length];

            int numBytesToRead = (int)stream.Length;
            int numBytesRead = 0;
            while (numBytesToRead > 0)
            {
                // Read may return anything from 0 to numBytesToRead.
                int n = stream.Read(buffer, numBytesRead, numBytesToRead);
                // The end of the file is reached.
                if (n == 0)
                    break;
                numBytesRead += n;
                numBytesToRead -= n;
            }
            stream.Close();



            File.Delete(filename);
            FileStream fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);

            bw.Write(buffer);

            bw.Close();
            fs.Close();
        }

    }
}
