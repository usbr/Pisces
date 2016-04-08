using Reclamation.Core;
using Reclamation.TimeSeries.Hydromet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Nrcs
{

    public enum SnotelParameterCodes
    {
///<summary>AIR TEMPERATURE AVERAGE</summary>     
[Description("AIR TEMPERATURE AVERAGE - Fahrenheit")]  TAVG    ,
///<summary>AIR TEMPERATURE MAXIMUM</summary>     
[Description("AIR TEMPERATURE MAXIMUM - Fahrenheit")]  TMAX    ,
///<summary>AIR TEMPERATURE MINIMUM</summary>     
[Description("AIR TEMPERATURE MINIMUM - Fahrenheit")]  TMIN    ,
///<summary>AIR TEMPERATURE OBSERVED</summary>     
[Description("AIR TEMPERATURE OBSERVED - Fahrenheit")]  TOBS    ,
///<summary>DIVERSION FLOW VOLUME OBSERVED</summary>     
[Description("DIVERSION FLOW VOLUME OBSERVED - acre-feet")]  DIV,
///<summary>DIVERSION DISCHARGE OBSERVED MEAN</summary>     
[Description("DIVERSION DISCHARGE OBSERVED MEAN - cfs")]  DIVD,
///<summary>DISCHARGE MANUAL/EXTERNAL ADJUSTED MEAN</summary>     
[Description("DISCHARGE MANUAL/EXTERNAL ADJUSTED MEAN - cfs")]  SRDOX,
///<summary>PRECIPITATION ACCUMULATION</summary>     
[Description("PRECIPITATION ACCUMULATION - inches")]  PREC    ,
///<summary>PRECIPITATION INCREMENT</summary>     
[Description("PRECIPITATION INCREMENT - inches")]  PRCP    ,
///<summary>PRECIPITATION INCREMENT – SNOW-ADJUSTED</summary>     
[Description("PRECIPITATION INCREMENT – SNOW-ADJUSTED - inches")]  PRCPSA,
///<summary>RESERVOIR STORAGE VOLUME</summary>     
[Description("RESERVOIR STORAGE VOLUME - acre-feet")]  RESC    ,
///<summary>RIVER DISCHARGE OBSERVED MEAN</summary>     
[Description("RIVER DISCHARGE OBSERVED MEAN - cfs")]  SRDOO,
///<summary>SNOW DEPTH</summary>     
[Description("SNOW DEPTH - inches")]  SNWD    ,
///<summary>SNOW WATER EQUIVALENT</summary>     
[Description("SNOW WATER EQUIVALENT - inches")]  WTEQ    ,
///<summary>STREAM VOLUME, ADJUSTED</summary>     
[Description("STREAM VOLUME, ADJUSTED - acre-feet")]  SRVO    ,
///<summary>STREAM VOLUME, ADJUSTED EXTERNALLY</summary>     
[Description("STREAM VOLUME, ADJUSTED EXTERNALLY - acre-feet")]  SRVOX,
///<summary>STREAM VOLUME, OBSERVED</summary>     
[Description("STREAM VOLUME, OBSERVED - acre-feet")]  SRVOO   ,
///<summary>TELECONNECTION INDEX (also known as OSCILLATION INDEX)</summary>     
[Description("TELECONNECTION INDEX (also known as OSCILLATION INDEX) - N/A")]  OI      ,




    }

    /// <summary>
    /// Read Daily snotel , or SnowCourse data from NRCS web service
    /// http:///wcc.sc.egov.usda.gov/awdbWebService/webservice/testwebservice.jsf
    /// </summary>
    public class SnotelSeries:Series
    {
        string m_triplet;
        SnotelParameterCodes m_parameter;

        public SnotelSeries(TimeSeriesDatabase db, TimeSeriesDatabaseDataSet.SeriesCatalogRow sr)
            : base(db, sr)
        {
            m_triplet = ConnectionStringUtility.GetToken(ConnectionString, "Triplet", "");
            var p = ConnectionStringUtility.GetToken(ConnectionString, "SnotelParameter", "");

            m_parameter = (SnotelParameterCodes)Enum.Parse(typeof(SnotelParameterCodes), p);
            TimeInterval = TimeSeries.TimeInterval.Daily;
        }

        public SnotelSeries(string triplet, SnotelParameterCodes parameter)
        {
            m_triplet = triplet;
            this.Parameter = parameter.ToString();
            m_parameter = parameter;
            ConnectionString = "Triplet=" + triplet + ";SnotelParameter=" + m_parameter.ToString();
           // String[] tag = { siteID + ":" + state + ":SNTL" };
            TimeInterval = TimeSeries.TimeInterval.Daily;
        }



        private static string NetworkFromTriplet(string triplet)
        {
            var parts = triplet.Split(':');
            return parts[2];
        }

        protected override void ReadCore(DateTime t1, DateTime t2)
        {
            try
            {

                var ws = new AwdbWebServiceClient();
                System.Net.ServicePointManager.Expect100Continue = false;
                //var ws = new AwdbWebService();

                var dur = duration.DAILY;

                
                if (NetworkFromTriplet(m_triplet) == "SNOW")
                    dur = duration.SEMIMONTHLY;

               

                var data = ws.getData(new string[] { m_triplet }, Parameter, 1, null,
                    dur, true, t1.ToString("yyyy-MM-dd"), t2.ToString("yyyy-MM-dd"),false);

                Console.Write(Parameter+" "+"duration = " + dur + " " + m_triplet);
                Console.Write(" " +t1.ToString("yyyy-MM-dd"));
                Console.Write(" " +t2.ToString("yyyy-MM-dd"));
                 

                if (data.Length == 0)
                    return;
               
                //Logger.WriteLine("data.Length ="+data.Length);
                if (data[0].beginDate == null)
                {
                    Logger.WriteLine("Error: no data at " + m_triplet);
                    return;
                }
                if (data[0].duration != dur)
                    throw new InvalidOperationException("duration returned does not match requested");

                if( data[0].values == null)
                    Console.WriteLine("Error: data[0].vals == null ");
                var vals = data[0].values;
                Console.WriteLine(" " + vals.Length + " records");
               // Logger.WriteLine("Values Length = "+vals.Length);

                var flags = data[0].flags;
                DateTime t = DateTime.Parse(data[0].beginDate).Date;
                
                for (int i = 0; i < vals.Count(); i++)
                {

                    if (vals[i].HasValue)
                    {
                        Add(t, Convert.ToDouble(vals[i].Value), flags[i]);
                    }
                    else
                    {
                        AddMissing(t);
                    }

                    if( dur == duration.DAILY)
                    {
                    t = t.AddDays(1);
                    }
                    else if( dur == duration.SEMIMONTHLY)
                    {
                        if (t.Date.Day == 1)
                            t = t.AddDays(15);
                        else
                            t = t.EndOfMonth().AddDays(1);
                        //Console.WriteLine(data[0].collectionDates[i]);
                        //t = DateTime.Parse(data[0].collectionDates[i]);
                    }


                }
            }
            catch (Exception e)
            {

                Logger.WriteLine(e.Message+"\n"+e.StackTrace);
                Console.WriteLine(e.Message+"\n" + e.StackTrace);
                Messages.Add(e.Message);
                Clear();
            }
            

        }


        

        public static string GetCbtt(string siteID)
        {
            var csv = NrcsSnotelSeries.SnotelSites;
            var rows = csv.Select("SiteID='" + siteID + "'");
            if (rows.Length == 0)
                return "";
            var r = rows[0];

            return r["cbtt"].ToString();

        }
        
        public static string GetTriplet(string cbtt, string network="SNTL")
        {
            var csv = NrcsSnotelSeries.SnotelSites;
            var rows = csv.Select("cbtt='" + cbtt + "'");
            if (rows.Length == 0)
                return "";
            var r = rows[0];

            String siteID = r["SiteID"].ToString();
            String state = r["State"].ToString();

            return siteID + ":" + state + ":"+network;

        }
    }
}
