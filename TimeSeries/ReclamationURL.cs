using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using Reclamation.Core;

namespace Reclamation.TimeSeries
{
    /// <summary>
    /// ReclamationURL manages the URL's to hydromet CGI executables
    /// for three different internal servers and one public facing server depending 
    /// on the client location.
    /// </summary>
    public static class ReclamationURL
    {

        static DataTable m_table = null;

        public static DataTable Table
        {
            get {
                if (m_table == null)
                {
                    var fn = FileUtility.GetFileReference("reclamationcgi.csv");
                    m_table = new CsvFile(fn, CsvFile.FieldTypes.AllText);
                }
                
                return m_table; 
            }
            
        }

        public static string GetUrlToDataCgi(object svr, TimeInterval interval)
        {

            string net = "www";
            if (NetworkUtility.Intranet)
                net = "recnet";

            var qry = "Server = '" + svr.ToString() + "' and TimeInterval = '" 
                + interval.ToString() + "' and Network = '"+net+"'" ;

            var rows = Table.Select(qry);
            if (rows.Length != 1)
            {
                Console.WriteLine("Error: found "+rows.Length+" matches. Expected 1");
                throw new Exception("Error: could not lookup " + qry);
            }

            return rows[0]["CGI"].ToString();
           
        }

    }
}
