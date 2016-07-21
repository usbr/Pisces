using Reclamation.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Reclamation.Core
{
    /// <summary>
    /// Manage  TextFile of server names and credientals
    /// </summary>
    public class TextFileCredentials
    {
        TextFile tf;
        string m_fileName = "credentials.txt";
        public TextFileCredentials(string filename)
        {
            m_fileName = filename;
            
            tf = new TextFile(m_fileName);
        }

        public void Save(string server, string password)
        {
           int idx =  tf.IndexOf(server);    
            if( idx <0)
            {
                tf.Add(server);
                tf.Add(Protect(password));
            }
            else
            {
                tf.FileData[idx+1] = Protect(password);
            }
            tf.SaveAs(tf.FileName);
        }

        public bool Contains(string server)
        {
            return tf.IndexOf(server.Trim()) >= 0;
        }

        public string GetPassword(string server)
        {
            int idx = tf.IndexOf(server.Trim());
            if(idx < 0)
            return "";

            return Unprotect(tf.FileData[idx + 1]);
        }



        static byte[] entropy = new byte[] { 19, 2, 12 };

        /// <summary>
        /// http://www.thomaslevesque.com/2013/05/21/an-easy-and-secure-way-to-store-a-password-using-data-protection-api/
        /// </summary>
        /// <param name="clearText"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
            public static string Protect(
                string clearText,
                DataProtectionScope scope = DataProtectionScope.CurrentUser)
            {
                if (clearText == null)
                    throw new ArgumentNullException("clearText");
                byte[] clearBytes =  GetBytes(clearText);
                byte[] encryptedBytes = ProtectedData.Protect(clearBytes, entropy, scope);
                return String.Join(",", encryptedBytes.Select(p => p.ToString()).ToArray());
            }

            static byte[] GetBytes(string str)
            {
                byte[] bytes = new byte[str.Length * sizeof(char)];
                System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
                return bytes;
            }

            public static string Unprotect(
                string encryptedText,
                DataProtectionScope scope = DataProtectionScope.CurrentUser)
            {
                byte[] encryptedBytes = GetBytesFromCSV(encryptedText); 
                byte[] clearBytes = ProtectedData.Unprotect(encryptedBytes,entropy, scope);
                return GetString(clearBytes);
            }
            static string GetString(byte[] bytes)
            {
                char[] chars = new char[bytes.Length / sizeof(char)];
                System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
                return new string(chars);
            }
            static byte[] GetBytesFromCSV(string str)
            {
                var tokens = str.Split(',');
                byte[] bytes = new byte[tokens.Length];
                for (int i = 0; i < tokens.Length; i++)
                {
                    bytes[i] =  Convert.ToByte(tokens[i]);
                }
                return bytes;
            }

    }
}
