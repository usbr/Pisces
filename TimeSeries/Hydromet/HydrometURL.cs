using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using Reclamation.Core;

namespace Reclamation.TimeSeries.Hydromet
{
    /// <summary>
    /// HydrometURL manages the URL's to hydromet CGI executables
    /// for three different internal servers and one public facing server depending 
    /// on the client location.
    /// </summary>
    public static class HydrometURL
    {

        static DataTable m_table = null;

        public static DataTable Table
        {
            get {
                if (m_table == null)
                {
                    var fn = FileUtility.GetFileReference("hydrometcgi.csv");
                    m_table = new CsvFile(fn, CsvFile.FieldTypes.AllText);
                }
                
                return m_table; 
            }
            
        }

        public static string GetUrlToDataCgi(HydrometHost svr, TimeInterval interval)
        {

            string net = "www";
            if (NetworkUtility.Intranet)
                net = "recnet";

            var qry = "HydrometServer = '" + svr.ToString() + "' and TimeInterval = '" 
                + interval.ToString() + "' and Network = '"+net+"'" ;

            var rows = Table.Select(qry);
            if( rows.Length != 1)
                throw new Exception("Error: could not lookup "+qry);

            return rows[0]["CGI"].ToString();
           
        }

    }
}
