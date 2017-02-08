using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Reclamation.TimeSeries;
using System.Linq;
using System.Web;
using System.IO;
using System.Data;
using Reclamation.Core;
using System.Net;


namespace PiscesWebServices
{
    public class WaterMLWriter
    {
        /// <summary>
        /// Writes output in WaterML2.0 Format
        /// Format adapted from sample at http://www.waterml2.org/KiWIS-WML2-Example.wml
        /// and documentation at http://def.seegrid.csiro.au/sissvoc/ogc-def/resource?uri=http://www.opengis.net/def/waterml/2.0/
        /// Search for [JR] within this code file to see areas that need refinement...
        /// </summary>
        /// <param name="data"></param>
        /// <param name="filename"></param>
        public static void writeWaterML2Data(DataTable data, string filename)
        {
            StreamWriter sr = new StreamWriter(filename, false);

            // [JR] need methods to get site info and metadata from query
            string siteName = "";
            string parameterName = "";

            // Add WaterMl2 Header
            #region
            sr.WriteLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
            sr.Write(@"<wml2:Collection ");
            sr.Write(@"xmlns:wml2=""http://www.opengis.net/waterml/2.0"" ");
            sr.Write(@"xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" ");
            sr.Write(@"xmlns:gml=""http://www.opengis.net/gml/3.2"" ");
            sr.Write(@"xmlns:om=""http://www.opengis.net/om/2.0"" ");
            sr.Write(@"xmlns:sa=""http://www.opengis.net/sampling/2.0"" ");
            sr.Write(@"xmlns:sams=""http://www.opengis.net/samplingSpatial/2.0"" ");
            sr.Write(@"xmlns:xlink=""http://www.w3.org/1999/xlink"" ");
            sr.Write(@"xsi:schemaLocation=""http://www.opengis.net/waterml/2.0");
            sr.Write(@"http://www.opengis.net/waterml/2.0/waterml2.xsd"" ");
            sr.WriteLine(@"gml:id=""Pisces.Col.1"">");
            #endregion

            // Add WaterML2 Metadata
            #region
            sr.WriteLine(@"<gml:description>Pisces WaterML2.0</gml:description>");
            sr.WriteLine(@"<wml2:metadata>");
            sr.WriteLine(@"	<wml2:DocumentMetadata gml:id=""Pisces.DocMD.1"">");
            sr.WriteLine(@"		<wml2:generationDate>" + DateTime.Now.ToString("s") + "</wml2:generationDate>");
            sr.WriteLine(@"		<wml2:generationSystem>Pisces</wml2:generationSystem>");
            sr.WriteLine(@"	</wml2:DocumentMetadata>");
            sr.WriteLine(@"</wml2:metadata>");
            sr.WriteLine(@"<wml2:temporalExtent>");
            sr.WriteLine(@"	<gml:TimePeriod gml:id=""Pisces.TempExt.1"">");
            string colName = data.Columns[0].ColumnName;
            DateTime maxDate = Convert.ToDateTime(data.Compute("MAX(" + colName + ")", null));
            DateTime minDate = Convert.ToDateTime(data.Compute("MIN(" + colName + ")", null));
            sr.WriteLine(@"		<gml:beginPosition>" + minDate.ToString("s") + "</gml:beginPosition>");
            sr.WriteLine(@"		<gml:endPosition>" + maxDate.ToString("s") + "</gml:endPosition>");
            sr.WriteLine(@"	</gml:TimePeriod>");
            sr.WriteLine(@"</wml2:temporalExtent>");
            sr.WriteLine(@"	<wml2:observationMember>");
            sr.WriteLine(@"		<om:OM_Observation gml:id=""Pisces.OM_Obs.1"">");
            sr.WriteLine(@"			<om:phenomenonTime>");
            sr.WriteLine(@"				<gml:TimePeriod gml:id=""Pisces.ObsTime.1"">");
            sr.WriteLine(@"					<gml:beginPosition>" + minDate.ToString("s") + "</gml:beginPosition>");
            sr.WriteLine(@"					<gml:endPosition>" + maxDate.ToString("s") + "</gml:endPosition>");
            sr.WriteLine(@"				</gml:TimePeriod>");
            sr.WriteLine(@"			</om:phenomenonTime>");
            sr.WriteLine(@"			<om:resultTime>");
            sr.WriteLine(@"				<gml:TimeInstant gml:id=""Pisces.resTime.1"">");
            sr.WriteLine(@"					<gml:timePosition>" + maxDate.ToString("s") + "</gml:timePosition>");
            sr.WriteLine(@"				</gml:TimeInstant>");
            sr.WriteLine(@"			</om:resultTime>");
            // [JR] need to dynamically place specific metadata values and links here!
            sr.WriteLine(@"			<om:procedure xlink:href=""http://www.usbr.gov"" xlink:title=""Pisces Web Service Query""/>");
            sr.WriteLine(@"			<om:observedProperty xlink:href=""http://www.usbr.gov"" xlink:title=""" + parameterName + @"""/>");
            sr.WriteLine(@"			<om:featureOfInterest xlink:href=""http://www.usbr.gov"" xlink:title=""" + siteName + @"""/>");
            sr.WriteLine(@"			<om:result>");
            sr.WriteLine(@"				<wml2:MeasurementTimeseries gml:id=""Pisces.Ts." + siteName.Replace(" ", "") + parameterName.Replace(" ", "") + @""">");
            sr.WriteLine(@"					<wml2:temporalExtent>");
            sr.WriteLine(@"						<gml:TimePeriod gml:id=""Pisces.TsTime.1"">");
            sr.WriteLine(@"							<gml:beginPosition>" + minDate.ToString("s") + "</gml:beginPosition>");
            sr.WriteLine(@"							<gml:endPosition>" + maxDate.ToString("s") + "</gml:endPosition>");
            sr.WriteLine(@"						</gml:TimePeriod>");
            sr.WriteLine(@"					</wml2:temporalExtent>");
            sr.WriteLine(@"					<wml2:defaultPointMetadata>");
            sr.WriteLine(@"						<wml2:DefaultTVPMeasurementMetadata>");
            // [JR] define metadata datatype information here
            if (parameterName.ToLower().Contains("average"))
            {
                sr.WriteLine(@"							<wml2:interpolationType xlink:href=""http://www.opengis.net/def/waterml/2.0/interpolationType/AveragePrec"" " +
                    @"xlink:title=""Average in Preceding Interval""/>");
            }
            else if (parameterName.ToLower().Contains("total") || parameterName.ToLower().Contains("sum"))
            {
                sr.WriteLine(@"							<wml2:interpolationType xlink:href=""http://www.opengis.net/def/waterml/2.0/interpolationType/TotalPrec"" " +
                    @"xlink:title=""Preceding Total""/>");
            }
            else
            {
                sr.WriteLine(@"							<wml2:interpolationType xlink:href=""http://www.opengis.net/def/waterml/2.0/interpolationType/Continuous"" " +
                    @"xlink:title=""Instantaneous""/>");
            }
            sr.WriteLine(@"							<wml2:qualifier xlink:href=""http://www.usbr.gov"" xlink:title=""ProvisionalData""/>");
            sr.WriteLine(@"							<wml2:uom uom=""cumec""/>");
            sr.WriteLine(@"						</wml2:DefaultTVPMeasurementMetadata>");
            sr.WriteLine(@"					</wml2:defaultPointMetadata>");
            #endregion

            // Populate data points
            for (int i = 0; i < data.Rows.Count; i++)
            {
                sr.WriteLine("<wml2:point>");
                sr.WriteLine("	<wml2:MeasurementTVP>");
                var t = DateTime.Parse(data.Rows[i][0].ToString()).ToString("s");
                sr.WriteLine("		<wml2:time>" + t + "</wml2:time>");
                var val = data.Rows[i][1].ToString();
                sr.WriteLine("		<wml2:value>" + val + "</wml2:value>");
                sr.WriteLine("	</wml2:MeasurementTVP>");
                sr.WriteLine("</wml2:point>");                
            }

            // Add closing tags
            #region
            sr.WriteLine("				</wml2:MeasurementTimeseries>");
            sr.WriteLine("			</om:result>");
            sr.WriteLine("		</om:OM_Observation>");
            sr.WriteLine("	</wml2:observationMember>");
            sr.WriteLine("</wml2:Collection>");
            #endregion

            sr.Close();
        }
    }
}
