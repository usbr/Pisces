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

        public SeriesDataModel.PiscesTimeSeriesData GetSeriesData(SeriesModel.PiscesSeries series, DateTime t1, DateTime t2)
        {
            var ts = GetSeriesData(series.tablename, t1, t2);
            return (ts);
        }

        public SeriesDataModel.PiscesTimeSeriesData GetSeriesData(string seriesTable, DateTime t1, DateTime t2)
        {
            IDbConnection db = Controllers.DatabaseConnectionController.Connect();

            // Get TS data points
            string sqlStringData = "select * from " + seriesTable + " where datetime >= str_to_date('" + t1 + "','%m/%d/%Y %r') "+
                "and datetime <= str_to_date('" + t2 + "','%m/%d/%Y %r') ";
            List<SeriesDataModel.Point> tsData = (List<SeriesDataModel.Point>)db.Query<SeriesDataModel.Point>(sqlStringData);

            // Get TS metadata
            string sqlStringMeta = "select * from seriescatalog a, sitecatalog b, parametercatalog c " +
                "where a.siteid=b.siteid and a.parameter=c.id and a.tablename = '" + seriesTable + "'";
            //List<SeriesDataModel.PiscesTimeSeriesData> ts = (List<SeriesDataModel.PiscesTimeSeriesData>)db.Query<SeriesDataModel.PiscesTimeSeriesData>(sqlStringMeta);

            var ts = (List<SeriesDataModel.PiscesTimeSeriesData>)db.Query< //Dapper MultiMap
                SeriesDataModel.PiscesTimeSeriesData,
                SeriesModel.PiscesSeries,
                SiteModel.PiscesSite,
                ParameterModel.PiscesParameter,
                SeriesDataModel.PiscesTimeSeriesData>(
                sqlStringMeta,
                (a, b, c, d) =>
                {
                    a = new SeriesDataModel.PiscesTimeSeriesData
                    {
                        series = b,
                        site = c,
                        parameter = d,
                        data = new List<SeriesDataModel.Point>()
                    };
                    return a;
                },
                commandType: CommandType.Text,
                splitOn: "siteid,id"
            );

            ts[0].data = tsData;
            return (ts[0]);
        }

        public List<SeriesModel.PiscesSeries> AddOrUpdateSeriesData(List<SeriesModel.PiscesSeries> input)
        {
            throw new NotImplementedException();
        }

        public List<SeriesModel.PiscesSeries> DeleteSeriesData(List<SeriesModel.PiscesSeries> input)
        {
            throw new NotImplementedException();
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
