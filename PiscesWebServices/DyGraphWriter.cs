using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PiscesWebServices
{
    class DyGraphWriter
    {
        /// <summary>
        /// Writes a HTML tagged C# List for dyGraphs output
        /// </summary>
        /// <param name="outFile"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static void writeHTML_dyGraphs(DataTable sites, string filename)
        {
            // The data in the outFile has to be preceded by a line that says "BEGIN DATA"
            //      and followed by a line that says "END DATA"
            // The series info has to be in outFile[12]
            StreamWriter sr = new StreamWriter(filename, false);
            
            // Populate chart HTML requirements
            #region
            sr.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01//EN"" ""http://www.w3.org/TR/html4/strict.dtd"">");
            sr.Write("<html>");
            sr.Write("<head>");
            sr.Write(@"<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"">");
            sr.Write("<title>HDB CGI Data Query Graph</title>");
            sr.Write("<!-- Call DyGraphs JavaScript Reference -->");
            sr.Write(@"<script type=""text/javascript""  src=""//cdnjs.cloudflare.com/ajax/libs/dygraph/1.1.1/dygraph-combined.js""></script>");
            sr.Write(@"<style type=""text/css"">");
            sr.Write("#graphdiv {position: absolute; left: 50px; right: 50px; top: 75px; bottom: 50px;}");
            sr.Write("#graphdiv .dygraph-legend {width: 300px !important; background-color: transparent !important; left: 75px !important;}");
            sr.Write("</style>");
            sr.Write("</head>");
            sr.Write("<body>");
            sr.Write("<!-- Place DyGraphs Chart -->");
            sr.Write(@"<div id=""status"" style=""width:1000px; font-size:0.8em; padding-top:5px;""></div>");
            sr.Write("<br>");
            sr.Write(@"<div id=""graphdiv""></div>");
            sr.Write("");
            sr.Write("<!-- Build DyGraphs Chart -->");
            sr.Write(@"<script type=""text/javascript"">");
            sr.Write("g = new Dygraph(");
            sr.Write(@"document.getElementById(""graphdiv""),");
            sr.Write("");
            #endregion

            // Populate html data
            #region
            string headerString = @"""Date,";
            for (int i = 1; i < sites.Columns.Count; i++)
            {
                headerString = headerString + "Value" + i + ",";
            }
            sr.Write(headerString.Remove(headerString.Length - 1) + @"\n "" +");
            // POPULATE DATA
            for (int i = 0; i < sites.Rows.Count; i++)
            {
                var t = DateTime.Parse(sites.Rows[i][0].ToString()).ToString("yyyy-MM-dd HH:mm");
                string dataRow = "$" + t + ", ";
                for (int j = 1; j < sites.Columns.Count; j++)
                {
                    double jthVal;// = sites.Rows[i][j].ToString().Trim();
                    string jthString;
                    try
                    {
                        jthVal = Convert.ToDouble(sites.Rows[i][j].ToString().Trim());
                        jthString = jthVal.ToString();
                    }
                    catch
                    {
                        jthVal = double.NaN;
                        jthString = "NaN";
                    }
                    dataRow = dataRow + jthString + ", ";
                }
                if (i + 1 == sites.Rows.Count)
                { sr.Write((dataRow.Remove(dataRow.Length - 2) + @"\n$").Replace('$', '"')); }
                else
                { sr.Write((dataRow.Remove(dataRow.Length - 2) + @"\n$ +").Replace('$', '"')); }
            }
            #endregion

            // Populate chart HTML requirements
            #region
            sr.Write(", {fillGraph: true, showRangeSelector: true, legend: 'always'");
            //sr.Write(", rangeSelectorPlotStrokeColor: '#0000ff', rangeSelectorPlotFillColor: '#0000ff'");
            sr.Write(", xlabel: 'Date', ylabel: 'value', labelsSeparateLines: true");
            sr.Write(", labelsDiv: document.getElementById('status'), axisLabelWidth: 75");
            sr.Write(", highlightCircleSize: 5, pointSize: 1.5, strokeWidth: 1.5");
            sr.Write("}");
            sr.Write(");");
            sr.Write("");
            sr.Write("</script>");
            sr.Write("</body>");
            sr.Write("</html>");
            #endregion
            sr.Close();
        }



    }
}