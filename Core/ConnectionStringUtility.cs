using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace Reclamation.Core
{
    public static class ConnectionStringUtility
    {


        /// <summary>
        /// Returns a value from a connection string
        /// </summary>
        public static string GetToken(
            string connectionString, string tokenName, string defaultValue)
        {
            Match m = Regex.Match(connectionString,
                tokenName + "=([^;]+)(;|$)", RegexOptions.IgnoreCase);
            if (m.Groups.Count != 3)
            {
                return defaultValue;
            }
            return m.Groups[1].Value;
        }

        /// <summary>
        /// Returns integer from connection string 
        /// returns -1 on failure
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="tokenName"></param>
        /// <returns></returns>
        public static int GetIntFromConnectionString(string connectionString, string tokenName)
        {
            string s = GetToken(connectionString, tokenName,"");
            int rval =-1;
            if( !Int32.TryParse(s, out rval))
                return -1;
            return rval;
        }

        /// <summary>
        /// Sets a variable in connection string.
        /// for example set the password
        /// </summary>
        /// <param name="tokenName"></param>
        /// <returns></returns>
        public static string Modify(string connStr, string tokenName, string newValue)
        {
            // insert new password in connection string.
            string rval = connStr;
            string prevValue = GetToken(connStr, tokenName,"");
            string find = tokenName + "=" + prevValue;
            string replace = tokenName + "=" + newValue;
            int idx = connStr.IndexOf(find);
            if (idx < 0)
            {// create token
                //   throw new Exception("Error setting " + tokenName);
                rval = connStr;
                if (!connStr.EndsWith(";"))
                    rval += ";";

                rval = rval + tokenName + "=" + newValue + ";";
            }
            else
            {
                rval = connStr.Replace(find, replace);
            }
            return rval;
        }

        /// <summary>
        /// Modifies FileName in connection string to be relative to the specified path
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static string MakeFileNameRelative(string connectionString, string piscesDatabasePath)
        {
            string fn = ConnectionStringUtility.GetToken(connectionString, "FileName","");
            string rval = connectionString;

            if (fn.Length > 0 &&  Directory.Exists(Path.GetDirectoryName(piscesDatabasePath)))
            {// modify to be relative path to database
                fn = FileUtility.RelativePathTo(Path.GetDirectoryName(piscesDatabasePath), fn);
                rval = ConnectionStringUtility.Modify(connectionString, "FileName", fn);
            }
            return rval;
        }



        public static bool GetBoolean(string connectionString, string tokenName, bool defaultValue)
        {
            string t = GetToken(connectionString, tokenName, "");

            bool b = false;
            if (Boolean.TryParse(t, out b))
                return b;

            return defaultValue;

        }

        public static string GetFileName(string ConnectionString, string databasePath)
        {
            string fileName = ConnectionStringUtility.GetToken(ConnectionString, "FileName", "");
            if (!Path.IsPathRooted(fileName))
            {
                string dir = Path.GetDirectoryName(databasePath);
                fileName = Path.Combine(dir, fileName);
            }
            return fileName;
        }
    }
}
