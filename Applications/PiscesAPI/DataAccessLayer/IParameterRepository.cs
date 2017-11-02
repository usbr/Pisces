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
    interface IParameterRepository
    {
    }

    public class ParameterRepository : IParameterRepository
    {

        public List<ParameterModel.PiscesParameter> GetParameters(string id = "", bool exactMatch = false)
        {
            IDbConnection db = Controllers.DatabaseConnectionController.Connect();
            string sqlString = "select * from parametercatalog ";
            if (id != "")
            {
                if (!exactMatch)
                {
                    sqlString += "where lower(id) like '%" + id + "%'";
                }
                else
                {
                    sqlString += "where lower(id) = '" + id + "'";
                }
            }
            return (List<ParameterModel.PiscesParameter>)db.Query<ParameterModel.PiscesParameter>(sqlString);
        }

        public List<ParameterModel.PiscesParameter> AddOrUpdateParameters(List<ParameterModel.PiscesParameter> input)
        {
            var addedSites = new List<ParameterModel.PiscesParameter>();
            IDbConnection db = Controllers.DatabaseConnectionController.Connect();
            foreach (ParameterModel.PiscesParameter item in input)
            {
                var siteExists = GetParameters(item.id, true).Count > 0;
                var sqlString = "";
                if (siteExists)
                { sqlString = GetUpdateSQL(); }
                else
                { sqlString = GetInsertSQL(); }
                
                db.Execute(sqlString, item);
                addedSites.Add(item);
            }
            if (addedSites.Count == 0)
            {
                throw new Exception("No sites added...");
            }
            else
            {
                return addedSites;
            }
        }

        public List<ParameterModel.PiscesParameter> DeleteParameters(List<ParameterModel.PiscesParameter> input)
        {
            var deletedSites = new List<ParameterModel.PiscesParameter>();
            IDbConnection db = Controllers.DatabaseConnectionController.Connect();
            foreach (ParameterModel.PiscesParameter item in input)
            {
                var siteExists = GetParameters(item.id, true).Count > 0;
                var sqlString = "";
                if (siteExists)
                {
                    sqlString = GetDeleteSQL();
                    db.Execute(sqlString, item);
                    deletedSites.Add(item);
                }
            }
            if (deletedSites.Count == 0)
            {
                throw new Exception("No sites deleted...");
            }
            else
            {
                return deletedSites;
            }
        }

        private string GetInsertSQL()
        {
            return "insert into parametercatalog(" +
                        "id," +
                        "timeinterval," +
                        "units," +
                        "statistic," +
                        "name," +
                        "unitstext" + 
                    ") values (" +
                        "@id," +
                        "@timeinterval," +
                        "@units," +
                        "@statistic," +
                        "@name," +
                        "@unitstext" +
                    ")";
        }

        private string GetUpdateSQL()
        {
            return "update parametercatalog set " +
                        "id = @id," +
                        "timeinterval = @timeinterval," +
                        "units = @units," +
                        "statistic = @statistic," +
                        "name = @name," +
                        "unitstext = @unitstext " +
                    "where " +
                        "id = @id";
        }

        private string GetDeleteSQL()
        {
            return "delete from parametercatalog " +
                    "where " +
                        "id = @id";
        }


    }

}
