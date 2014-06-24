using System.Data;
using System.IO;
using System.Data.OleDb;

namespace Reclamation.Core
{
    public class DbaseIV
    {

        public static DataTable Read(string filename)
        {
            string fn = Path.GetFileName(filename);
            if (fn.Length > 11)
            {
                string tmp = Path.GetTempFileName();
                File.Copy(filename, Path.ChangeExtension(tmp,".dbf"),true);
                return DbaseIV.Read(tmp);
            }

            string connString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source="+ Path.GetDirectoryName(filename) + ";Extended Properties = DBASE IV;";
            OleDbConnection conn = new OleDbConnection(connString);
            string sql = "Select * from " + fn;
            OleDbDataAdapter daGetTableData = new OleDbDataAdapter(sql, conn);
            DataSet ds = new DataSet();
            daGetTableData.Fill(ds);

            return ds.Tables[0];
        }
    }

   
}
