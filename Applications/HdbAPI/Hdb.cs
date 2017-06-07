using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace HdbApi
{
    /* This class contains all the code that the API needs to intract with HDB.
     * Place all stored procedure, SQL, and other code that is meant to talk to
     * HDB in this file.
     * 
     */

    class Hdb
    {
        private OracleConnection _con;

        public OracleConnection Connect()
        {
            _con = new OracleConnection();
            _con.ConnectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST="
                + ConfigurationManager.AppSettings["HdbServer"] // "100.100.100.100"
                + ")(PORT="
                + ConfigurationManager.AppSettings["HdbPort"] //"1521"
                + "))(CONNECT_DATA=(SERVICE_NAME="
                + ConfigurationManager.AppSettings["HdbInstance"] //"myservice.com"
                + ")));User Id="
                + ConfigurationManager.AppSettings["HdbUser"] //"scott"
                + ";Password="
                + ConfigurationManager.AppSettings["HdbPass"] //"tiger"
                + ";"; ;
            _con.Open();
            return _con;
        }

        public OracleConnection Connect(string dbServer, string dbUser, string dbPass)
        {
            OracleConnection _con = new OracleConnection();
            _con.ConnectionString = "Data Source=" + dbServer + ";User Id=" + dbUser + ";Password=" + dbPass + ";";            
            _con.Open();
            return _con;
        }

    }
}
