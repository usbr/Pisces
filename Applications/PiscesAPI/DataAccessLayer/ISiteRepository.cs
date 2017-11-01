using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PiscesAPI.Models;
using Dapper;
using System.Data;
using MySql.Data;

namespace PiscesAPI.DataAccessLayer
{
    interface ISiteRepository
    {
    }

    public class SiteRepository : ISiteRepository
    {

        public List<SiteModel.PiscesSite> GetSites(string id = "")
        {
            IDbConnection db = Controllers.DatabaseConnectionController.Connect();
            string sqlString = "select * from sitecatalog ";
            if (id != "")
            {
                sqlString += "where lower(siteid) like '%" + id + "%'";
            }
            return (List<SiteModel.PiscesSite>)db.Query<SiteModel.PiscesSite>(sqlString);
        }


    }

}
