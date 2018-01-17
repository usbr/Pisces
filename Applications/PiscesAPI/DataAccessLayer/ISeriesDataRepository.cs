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
    interface ISeriesDataRepository
    {
    }

    public class SeriesDataRepository : ISeriesDataRepository
    {

        public SeriesDataModel.TimeSeriesData GetSeriesData(SeriesModel.Series series, DateTime t1, DateTime t2)
        {
            var ts = GetSeriesData(series.tablename, t1, t2);
            return (ts);
        }

        public SeriesDataModel.TimeSeriesData GetSeriesData(string seriesTable, DateTime t1, DateTime t2)
        {
            IDbConnection db = Database.Connect();

            // Get TS data points
            string sqlStringData = "select * from " + seriesTable + " where datetime >= str_to_date('" + t1 + "','%m/%d/%Y %r') "+
                "and datetime <= str_to_date('" + t2 + "','%m/%d/%Y %r') ";
            List<SeriesDataModel.Point> tsData = (List<SeriesDataModel.Point>)db.Query<SeriesDataModel.Point>(sqlStringData);

            // Get TS metadata
            string sqlStringMeta = "select * from seriescatalog a, sitecatalog b, parametercatalog c " +
                "where a.siteid=b.siteid and a.parameter=c.id and a.tablename = '" + seriesTable + "'";
            //List<SeriesDataModel.PiscesTimeSeriesData> ts = (List<SeriesDataModel.PiscesTimeSeriesData>)db.Query<SeriesDataModel.PiscesTimeSeriesData>(sqlStringMeta);

            var ts = (List<SeriesDataModel.TimeSeriesData>)db.Query< //Dapper MultiMap
                SeriesDataModel.TimeSeriesData, //a
                SeriesModel.Series, //b
                SiteModel.PiscesSite, //c
                ParameterModel.PiscesParameter, //d
                SeriesDataModel.TimeSeriesData>(
                sqlStringMeta,
                (a, b, c, d) =>
                {
                    a = new SeriesDataModel.TimeSeriesData
                    {
                        series = b,
                        site = c,
                        parameter = d,
                        data = tsData//new List<SeriesDataModel.Point>()
                    };
                    return a;
                },
                commandType: CommandType.Text,
                splitOn: "siteid,id"
            );

            return (ts[0]);
        }

        public List<SeriesDataModel.Point> AddOrUpdateSeriesData(List<SeriesDataModel.TimeSeriesData> input)
        {
            IDbConnection db = Database.Connect();

            var addedPoints = new List<SeriesDataModel.Point>();
            foreach (SeriesDataModel.TimeSeriesData item in input)
            {
                foreach (SeriesDataModel.Point pt in item.data)
                {
                    var pointExists = GetSeriesData(item.series.tablename, pt.datetime, pt.datetime).data.Count > 0;

                    string sqlString = "";
                    if (pointExists)
                    {
                        sqlString = GetUpdateSQL(item, pt);
                    }
                    else
                    {
                        sqlString = GetInsertSQL(item, pt);
                    }

                    db.Execute(sqlString);
                    addedPoints.Add(pt);
                }
            }

            return addedPoints;
        }

        public List<SeriesDataModel.Point> DeleteSeriesData(List<SeriesDataModel.TimeSeriesData> input)
        {
            IDbConnection db = Database.Connect();

            var deletedPoints = new List<SeriesDataModel.Point>();
            foreach (SeriesDataModel.TimeSeriesData item in input)
            {
                string sqlString = GetDeleteSQL(item);

                db.Execute(sqlString);
                deletedPoints.AddRange(item.data);
            }

            return deletedPoints;
        }

        private string GetMassInsertSQL(SeriesDataModel.TimeSeriesData input)
        {
            // MANUAL SQL LOOP
            string sqlString = "insert into " + input.series.tablename + " (datetime,value,flag) values ";
            foreach (SeriesDataModel.Point pt in input.data)
            {
                sqlString += "(str_to_date('" + pt.datetime + "','%m/%d/%Y %r'),'" + pt.value + "','" + pt.flag + "'),";
            }
            sqlString = sqlString.Remove(sqlString.Length - 1);

            return sqlString;
        }

        private string GetInsertSQL(SeriesDataModel.TimeSeriesData input, SeriesDataModel.Point pt)
        {
            // MANUAL SQL
            string sqlString = "insert into " + input.series.tablename + " (datetime,value,flag) values " + 
                "(str_to_date('" + pt.datetime + "','%m/%d/%Y %r'),'" + pt.value + "','" + pt.flag + "')";
            return sqlString;
        }

        private string GetUpdateSQL(SeriesDataModel.TimeSeriesData input, SeriesDataModel.Point pt)
        {
            // MANUAL SQL
            string sqlString = "update " + input.series.tablename + " set value='" +
                pt.value + "' where datetime=" + "str_to_date('" + pt.datetime + "','%m/%d/%Y %r')";
            return sqlString;
        }

        private string GetDeleteSQL(SeriesDataModel.TimeSeriesData input)
        {
            // MANUAL SQL LOOP
            string sqlString = "delete from " + input.series.tablename + " where datetime in (";
            foreach (SeriesDataModel.Point pt in input.data)
            {
                sqlString += "str_to_date('" + pt.datetime + "','%m/%d/%Y %r'),";
            }
            sqlString = sqlString.Remove(sqlString.Length - 1);
            sqlString += ")";

            return sqlString;
        }


    }

}
