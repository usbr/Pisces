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
            if( !File.Exists(filename))
            {
                File.Create(filename).Dispose();
            }
            
            tf = new TextFile(m_fileName);
        }

        public void Save(string server, string password)
        {
           int idx =  tf.IndexOf(server);    
            if( idx <0)
            {
                tf.Add(server);
                //tf.Add( Protect(password));
                tf.Add(StringCipher.Encrypt(password,""));
            }
            else
            {
                tf.FileData[idx + 1] = StringCipher.Encrypt(password,"");
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

            return StringCipher.Decrypt(tf.FileData[idx + 1],"");
        }


    }
}
