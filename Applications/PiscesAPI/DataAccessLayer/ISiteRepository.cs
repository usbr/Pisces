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

        public List<SiteModel.PiscesSite> GetSites(string id = "", bool exactMatch = false)
        {
            IDbConnection db = Database.Connect();
            string sqlString = "select * from sitecatalog ";
            if (id != "")
            {
                if (!exactMatch)
                {
                    sqlString += "where lower(siteid) like '%" + id + "%'";
                }
                else
                {
                    sqlString += "where lower(siteid) = '" + id + "'";
                }
            }
            return (List<SiteModel.PiscesSite>)db.Query<SiteModel.PiscesSite>(sqlString);
        }

        public List<SiteModel.PiscesSite> AddOrUpdateSites(List<SiteModel.PiscesSite> input)
        {
            var addedSites = new List<SiteModel.PiscesSite>();
            IDbConnection db = Database.Connect();
            foreach (SiteModel.PiscesSite item in input)
            {
                var siteExists = GetSites(item.siteid, true).Count > 0;
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

        public List<SiteModel.PiscesSite> DeleteSites(List<SiteModel.PiscesSite> input)
        {
            var deletedSites = new List<SiteModel.PiscesSite>();
            IDbConnection db = Database.Connect();
            foreach (SiteModel.PiscesSite item in input)
            {
                var siteExists = GetSites(item.siteid, true).Count > 0;
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
            return "insert into sitecatalog(" +
                        "siteid," +
                        "description," +
                        "state," +
                        "latitude," +
                        "longitude," +
                        "elevation," +
                        "timezone," +
                        "install," +
                        "horizontal_datum," +
                        "vertical_datum," +
                        "vertical_accuracy," +
                        "elevation_method," +
                        "tz_offset," +
                        "active_flag," +
                        "type," +
                        "responsibility," +
                        "agency_region" +
                    ") values (" +
                        "@siteid," +
                        "@description," +
                        "@state," +
                        "@latitude," +
                        "@longitude," +
                        "@elevation," +
                        "@timezone," +
                        "@install," +
                        "@horizontal_datum," +
                        "@vertical_datum," +
                        "@vertical_accuracy," +
                        "@elevation_method," +
                        "@tz_offset," +
                        "@active_flag," +
                        "@type," +
                        "@responsibility," +
                        "@agency_region" +
                    ")";
        }

        private string GetUpdateSQL()
        {
            return "update sitecatalog set " +
                        "description = @description," +
                        "state = @state," +
                        "latitude = @latitude," +
                        "longitude = @longitude," +
                        "elevation = @elevation," +
                        "timezone = @timezone," +
                        "install = @install," +
                        "horizontal_datum = @horizontal_datum," +
                        "vertical_datum = @vertical_datum," +
                        "vertical_accuracy = @vertical_accuracy," +
                        "elevation_method = @elevation_method," +
                        "tz_offset = @tz_offset," +
                        "active_flag = @active_flag," +
                        "type = @type," +
                        "responsibility = @responsibility," +
                        "agency_region = @agency_region " +
                    "where " +
                        "siteid = @siteid";
        }

        private string GetDeleteSQL()
        {
            return "delete from sitecatalog "+
                    "where " +
                        "siteid = @siteid";
        }


    }

}
