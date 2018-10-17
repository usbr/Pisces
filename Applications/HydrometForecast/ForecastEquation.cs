using System.Collections.Generic;
using Reclamation.Core;
using System.Text.RegularExpressions;
using System;
using System.IO;
using System.Linq;
using System.Data;
using Reclamation.TimeSeries.Hydromet;
namespace HydrometForecast
{

    public class ForecastEquation
    {
        public double[] coefficients;
        public List<ForecastTerm> XTerms = new List<ForecastTerm>();
        public RunoffForecastTerm YTerm = null; 
        public int StartYear = 0;
        public int EndYear = 0;
        string aveargeFlowStation = ""; // example 'ANDI QU' used to get 30yr avg
        public ForecastEquation()
        {
            Name = "";
        }
        public ForecastEquation(string filename)
        {
            Name = "";
            ReadFromFile(filename);
        }

        /// <summary>
        /// Returns a list of cbtt pode pairs.
        /// used to pre-load data using HydrometMonthlyDataCache
        /// </summary>
        /// <returns></returns>
        public List<string> GetCbttPcodeList(bool avgParameter=false)
        {
            var rval = new List<string>();
            foreach (var x in XTerms)
            {
                rval.AddRange(x.cbttPodes);
            }
            rval.AddRange(YTerm.cbttPodes);

            
            // check for calculations (where cbtt_pcode) format is used.
            // example  JCK_AF 
            for (int i = 0; i < rval.Count; i++)
            {
                string pattern = "(?<cbttPcode>[A-Z]{2,8}_[A-Z]{2,8})";
                var mc = Regex.Matches(rval[i], pattern);
                if (mc.Count > 0)
                {
                    rval.RemoveAt(i);
                    foreach (Match item in mc)
                    {
                        rval.Add(item.Groups["cbttPcode"].Value.Replace("_", " "));
                    }
                }
            }

            rval = rval.Distinct().ToList();

            if (avgParameter)
            {// modify parameter codes to represent average.
                List<string> a = new List<string>();
                for (int i = 0; i < rval.Count; i++)
                {
                    var tokens = rval[i].Split(' ');
                    string cbtt = tokens[0];
                    string pcode = tokens[1];
                    pcode = HydrometMonthlySeries.LookupAveargePcode(pcode);
                    if (pcode != "")
                    {
                        a.Add(cbtt + " " + pcode);
                    }
                }
                return a;
            }


            return rval;
        }
        private void ReadFromFile(string filename)
        {
            var tf = new TextFile(filename);
            Name = ReadStringToken(tf, "Name");
            StartYear = Convert.ToInt32(ReadStringToken(tf, "StartYear"));
            EndYear = Convert.ToInt32(ReadStringToken(tf, "EndYear"));
            this.aveargeFlowStation = ReadStringToken(tf, "AverageRunoff");

            this.coefficients = ReadCoefficients(tf);

           
           string pattern = @"(X|Y)[0-9]{1}[A-Za-z]?\s*,";
            int idx = tf.IndexOfRegex(pattern);
            while (idx > 0  )
            {
                var term = CreateTerm(tf, idx);
                
                term.MonthNames = ReadDateRange(tf, idx);
                var isYterm = tf[idx].IndexOf("Y") == 0;
                if (isYterm)
                    YTerm =(RunoffForecastTerm) term;
                else
                    XTerms.Add(term);

                idx++;
                while (idx < tf.Length &&  !Regex.IsMatch(tf[idx], pattern) )
                {
                    if (tf[idx].Length > 0 && tf[idx].IndexOf("#") == 0 || tf[idx].IndexOf("\"#") ==0)
                    {// comment line
                        idx++;
                        continue;
                    }
                    var tokens = CsvFile.ParseCSV(tf[idx]);
                    if (tokens.Length <= 0)
                        break;
                    if (tokens[0].Trim() == "")
                        break;

                    term.siteNames.Add(tokens[0]);
                    // remove any extra space between cbtt and pcode
                    var tmp = TextFile.Split(tokens[1].Trim());
                    if (tmp.Length != 2)
                    {// might be an equation...
                        term.cbttPodes.Add(tokens[1].Trim());
                    }
                    else
                    {
                        term.cbttPodes.Add(tmp[0].Trim().ToUpper() + " " + tmp[1].Trim().ToUpper());
                    }
                    term.siteWeights.Add(Convert.ToDouble(tokens[2]));
                    if (isYterm)
                    {// y-term should not be weighted... put in 1's 
                        double[] w = Array.ConvertAll(term.MonthNames.ToArray(), s => 1.0);
                        term.MonthlyWeights.Clear();
                        term.MonthlyWeights.AddRange(w);
                    }
                    else
                        term.MonthlyWeights = ReadDoubles(tokens, 3, term.MonthNames.Count);
                    idx++;
                }

                if (idx >= tf.Length)
                    break;
                idx = tf.IndexOfRegex(pattern,idx);
                

            }
        }

        /// <summary>
        /// Uses forecast equation for each historical year.
        /// </summary>
        /// <param name="year1"></param>
        /// <param name="year2"></param>
        /// <returns>a table comparing acutal to forecasted values</returns>
        public DataTable RunHistoricalForecasts(int year1, int year2,bool lookAhead, int forecastMonth,double[] estimationFactors, int forecastDay=1)
        {
            DataTable tbl = new DataTable(Name);
            //Logger.EnableLogger();
            tbl.Columns.Add("Notes");
            tbl.Columns.Add("Year");
            tbl.Columns.Add("Actual Forecast Period Volume", typeof(double));
            tbl.Columns.Add("Forecasted Period Volume", typeof(double));
            tbl.Columns.Add("Difference", typeof(double));
            tbl.Columns.Add("Error", typeof(double));

            tbl.Columns.Add("Actual Residual Volume", typeof(double));
            tbl.Columns.Add("Forecasted Residual Volume", typeof(double));
            tbl.Columns.Add("Difference2", typeof(double));
            tbl.Columns.Add("Error2", typeof(double));

            bool firstTableRow = true;

            for (int i = 1; i < estimationFactors.Length; i++)
            {
                tbl.Columns.Add("Forecast "+estimationFactors[i], typeof(double));
                //tbl.Columns.Add("Difference " + estimationFactors[i], typeof(double));
                //tbl.Columns.Add("Error " + estimationFactors[i], typeof(double));
            }

            HydrometData.SetupHydrometData(StartYear,EndYear, GetCbttPcodeList().ToArray(),DateTime.MaxValue, true, true); // date is ignored when reading all years

            string forecastPeriod = "";

            for (int year = year1; year <= year2; year++)
            {
                var row = tbl.NewRow();
                row["Year"] = year;
                Logger.WriteLine(year.ToString(), "ui");
                DateTime t = new DateTime(year, forecastMonth, forecastDay);
                try
                {
                    var fcResult = Evaluate(t, lookAhead, estimationFactors[0], false);
                    var actual = YTerm.SeasonalRunoff(t);
                    row["Actual Forecast Period Volume"] = actual;
                    row["Forecasted Period Volume"] = fcResult.Forecast;
                    var diff =  fcResult.Forecast - actual ; // convention of FORTRAN
                    row["Difference"] = diff;
                    row["Error"] = (diff / actual)*100.0;

                    var actualResid = actual - YTerm.RunoffToDate(t);
                    var forecastResid = fcResult.GetForecastForSummary(lookAhead); 

                    row["Actual Residual Volume"] = actualResid;
                    row["Forecasted Residual Volume"] = forecastResid;
                    row["Difference2"] = forecastResid - actualResid ;
                    row["Error2"] = (forecastResid - actualResid) / actualResid * 100.0;

                    for (int i = 1; i < estimationFactors.Length; i++)
                    {
                        var s = " " + estimationFactors[i];
                        fcResult = Evaluate(t, lookAhead, estimationFactors[i], false);
                        row["Forecasted Period Volume" + s] = fcResult.Forecast;
                        diff = actual - fcResult.Forecast; // convention of FORTRAN
                        //row["Difference"+s] = diff;
                        //row["Error"+s] = (diff / actual) * 100.0;
                    }
                                       
                    forecastPeriod = fcResult.ForecastPeriod;
                    // [JR] Hacks to write the inputs to the MLR process
                    #region
                    var sTEMP = this.YTerm.yData;
                    for (int ithMon = 0; ithMon < sTEMP.Count(); ithMon++)
                    {
                        if (firstTableRow)
                        {
                            tbl.Columns.Add("Y-Runoff-" + sTEMP[ithMon].DateTime.ToString("MMM").ToUpper(), typeof(double));
                        }
                        row["Y-Runoff-" + sTEMP[ithMon].DateTime.ToString("MMM").ToUpper()] = sTEMP[ithMon].Value;
                    }
                    foreach (var term in this.XTerms)
                    {
                        if (term.xData.Count() > 0)
                        {
                            var termName = "X" + term.Number + "-" + term.ForecastTermType;
                            var sList = term.xData;
                            var sSum = new Reclamation.TimeSeries.Series();
                            for (int i = 0; i < sList.Count; i++)
                            {
                                foreach (var point in sList[i])
                                {
                                    if (i == 0)
                                    {
                                        sSum.Add(point.DateTime, 0.0);
                                        if (firstTableRow)
                                        {
                                            tbl.Columns.Add(termName + "-" + point.DateTime.ToString("MMM").ToUpper(), typeof(double));
                                        }
                                    }
                                    sSum[point.DateTime] = new Reclamation.TimeSeries.Point(point.DateTime, sSum[point.DateTime].Value + point.Value);
                                }
                            }
                            foreach (var point in sSum)
                            {
                                row[termName + "-" + point.DateTime.ToString("MMM").ToUpper()] = point.Value;
                            }
                        }
                    }
                    #endregion

                }
                catch (Exception ex)
                {
                    row["Notes"] = ex.Message;
                }

                
                tbl.Rows.Add(row);
                if (firstTableRow)
                {
                    firstTableRow = false;
                }
            }

            // add some notes
            if (tbl.Rows.Count >= 4)
            {
                tbl.Rows[0]["Notes"] = this.Name;
                tbl.Rows[1]["Notes"] = "forecast Month " + forecastMonth;
                tbl.Rows[2]["Notes"] = "forecast Day " + forecastDay;
                tbl.Rows[3]["Notes"] = "forecast Period " + forecastPeriod;
            }

             var summaryRow = tbl.NewRow();

             summaryRow["Year"] = "Average";
             //summaryRow["Actual Forecast Period Volume"] = tbl.AsEnumerable().Average(x => x.Field<double>("Actual Forecast Period Volume"));
             //summaryRow["Forecasted Period Volume"] = tbl.AsEnumerable().Average(x => x.Field<double>("Forecasted Period Volume"));
             //summaryRow["Difference"] = tbl.AsEnumerable().Average(x => x.Field<double>("Difference"));
             //summaryRow["Error"] = tbl.AsEnumerable().Average(x => x.Field<double>("Error"));

             //summaryRow["Actual Residual Volume"] = tbl.AsEnumerable().Average(x => x.Field<double>("Actual Residual Volume"));
             //summaryRow["Forecasted Residual Volume"] = tbl.AsEnumerable().Average(x => x.Field<double>("Forecasted Residual Volume"));
             //summaryRow["Difference2"] = tbl.AsEnumerable().Average(x => x.Field<double>("Difference2"));
             //summaryRow["Error2"] = tbl.AsEnumerable().Average(x => x.Field<double>("Error2"));
            
            for (int i = 2; i< tbl.Columns.Count;i++)
            {
                var colName = tbl.Columns[i].ColumnName;
                summaryRow[colName] = tbl.AsEnumerable().Average(x => x.Field<double>(colName));
            }

            // Get percent of average for MLR inputs
            for (int i = 10; i < tbl.Columns.Count; i++)
            {
                var colName = tbl.Columns[i].ColumnName;

                DataColumn ithColumn = tbl.Columns[colName];
                foreach (DataRow row in tbl.Rows)
                {
                    double oldVal = row.Field<double>(ithColumn);
                    row.SetField(ithColumn, oldVal / Convert.ToDouble(summaryRow[colName].ToString()));
                }
            }

            tbl.Rows.Add(summaryRow);

            return tbl;
        }


        public DataTable ComputeHistoricalCoefficients(int year1, int year2)
        {

            DataTable tbl = new DataTable(Name);
            //Logger.EnableLogger();
            tbl.Columns.Add("Year");
            int previousTerm = -1;
            for (int i = 0; i < XTerms.Count; i++)
            {
                if (XTerms[i].Number != previousTerm) // grouped terms
                {
                    tbl.Columns.Add("X" + XTerms[i].Number, typeof(double));
                    previousTerm = XTerms[i].Number;
                }
            }
            tbl.Columns.Add("Y1");


            int month = ForecastTerm.GetMonthFromString(YTerm.MonthNames[YTerm.MonthNames.Count - 1]);
            int year = 0;
            try
            {
                for (year = year1; year <= year2; year++)
                {
                    var row = tbl.NewRow();
                    row["Year"] = year;


                    DateTime t = new DateTime(year, month, 1);

                    previousTerm = -1;
                    double sum = 0;
                    for (int x = 0; x < XTerms.Count; x++)
                    {
                        var term = XTerms[x];

                        var d = term.Evaluate(t, true, 1.0);
                        if (previousTerm == term.Number)
                        {
                            sum += d;
                        }
                        else
                        {
                            sum = d;
                        }
                        row["X" + term.Number] = sum;
                        previousTerm = term.Number;

                    }
                    row["Y1"] = YTerm.SeasonalRunoff(t); //.Evaluate(t,true,1.0);
                    tbl.Rows.Add(row);
                }

                //CsvFile.WriteToCSV(tbl, @"C:\temp\andcoeff.csv");
            }
            catch(Exception ex1)
            {
                throw new Exception("Issue on year " + year + " \n" + ex1.Message);
            }
            return tbl;
        }


        public void WriteToFile(string filename)
        {
            StreamWriter sw = new StreamWriter(filename);
            sw.WriteLine("Name, " + Name);
            sw.WriteLine("StartYear,1950");
            sw.WriteLine("EndYear,2012");
            //sw.WriteLine("Number of X Terms, " + XTerms.Count);
            //sw.WriteLine("Number of Y Terms, " + YTerms.Count);
            sw.Write("Coefficients");
            for (int i = 0; i < coefficients.Length; i++)
            {
                sw.Write("," + coefficients[i].ToString());
            }

            WriteTerms(sw, XTerms);
            var tmp = new List<ForecastTerm>();
            tmp.Add(YTerm);
            WriteTerms(sw,tmp);
            sw.Close();
        }

        private static void WriteTerms(StreamWriter sw,List<ForecastTerm> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int xNum = i + 1;
                var term = list[i];
                sw.WriteLine();
                string termType = "Y";
                if (term.IsXterm)
                    termType = "X";

                sw.WriteLine(termType + xNum + " ," + term.ForecastTermType + " ,Weight, " + String.Join(", ", term.MonthNames.ToArray()));

                for (int j = 0; j < term.cbttPodes.Count; j++)
                {
                    sw.Write(term.siteNames[j].Trim().PadRight(15) + ", " + term.cbttPodes[j]
                        + ", " + term.siteWeights[j].ToString("F2"));
                    if( term.IsXterm)
                    {
                        string[] months = { "oct", "nov", "dec", "jan", "feb", "mar", "apr", "may", "jun", "jul", "aug", "sep" };
                        int[] monIdx = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
                        for (int k = 0; k < term.MonthNames.Count; k++)
                        {
                            sw.Write(", " + term.MonthlyWeights[k]);
                        }
                        //Console.Write(", " + String.Join(", ", term.MonthlyWeights));
                    }
                    sw.WriteLine();
                }

            }
        }

        private List<double> ReadDoubles(string[] tokens, int pos, int count)
        {
            var rval =new List<double>();

            for (int i = pos; i < count+pos; i++)
            {
                rval.Add(Convert.ToDouble(tokens[i]));
            }
            return rval;
        }


        private List<string> ReadDateRange(TextFile tf, int idx)
        {
            var tokens = tf[idx].Split(',');
            var rval = new List<string>();

            for (int i = 3; i < tokens.Length; i++)
            {
                var a = tokens[i].Trim();
                if (a == "")
                    break;
                rval.Add(a);

            }
            return rval;
        }

        private ForecastTerm CreateTerm(TextFile tf, int idx)
        {
            var s = tf[idx].Split(',')[1].Trim();

            
            ForecastTerm rval = null;
            if (s == "Antecedent Runoff")
                rval = new AntecedentRunoffForecastTerm();
            else if (s == "Precipitation")
                rval  = new PrecipitationForecastTerm();
            else if (s == "Snow")
                rval = new SnowForecastTerm();
            else if (s == "Runoff")
                rval = new RunoffForecastTerm();
            else
            throw new NotImplementedException();

            
            rval.Number = Convert.ToInt32(tf[idx].Split(',')[0].Substring(1, 1));// X1, Y1, X2...
            return rval;

        }

        private static string ReadStringToken(TextFile tf, string tag)
        {
            string rval = "";
            int idx = tf.IndexOf(tag);
            if (idx >= 0)
            {
                var tokens = tf[idx].Split(',');
                if( tokens.Length >1)
                  rval = tokens[1];
            }

            return rval;
        }

        private static double[] ReadCoefficients(TextFile tf)
        {
            var rval = new List<double>();
            int idx = tf.IndexOfRegex("^Coefficients");
            if (idx >= 0)
            {
                var tokens = tf[idx].Split(',');
                for (int i = 1; i < tokens.Length; i++)
                {
                    double d=0;
                    if (tokens[i].Trim() == "")
                        break;
                    if (!double.TryParse(tokens[i], out d))
                    {
                        throw new FormatException("Error reading coefficients " + tf[idx]);
                    }
                    rval.Add(d);
                }
                
            }
            return rval.ToArray();
        }


        public string Name { get; set; }

        public ForecastResult Evaluate(DateTime t, bool lookAhead, double estimationScaleFactor, bool setupCache=true)
        {
            Logger.WriteLine("Evaluate( " + this.Name + ")");
            var rval = new ForecastResult();
            rval.Equation = this;
            rval.EstimationFactor = estimationScaleFactor;
            string msg = t.ToShortDateString() + " Forecast " + this.Name + "    " + DateTime.Now.ToShortDateString();
            rval.Details.Add(msg);
            Logger.WriteLine(msg);

            if (setupCache)
            {
                HydrometData.SetupHydrometData(StartYear,EndYear,GetCbttPcodeList().ToArray(),t,true,false);
            }

            rval.Forecast = 0.0;
            //var total = 0;
            int i=0;
             for ( i = 0; i < this.XTerms.Count; i++)
                {
                    
                    var X = XTerms[i];
                    Logger.WriteLine("Evaluating X" + X.Number);
                    var d = X.Evaluate(t,lookAhead,estimationScaleFactor);
                    rval.Forecast += d * this.coefficients[X.Number-1];// apply coeficients...
                    rval.Details.AddRange(X.Details());
                    rval.Details.Add("X"+X.Number+" = "+d.ToString("F2"));
                    rval.Details.Add("");
                Logger.WriteLine("X" + X.Number + "= " + rval.Forecast);
                }

             rval.Details.Add("");
            
            rval.Forecast += coefficients[coefficients.Length-1];// k term
            //  Y = C
            string s = "Coefficients: ";
            for (i = 0; i < coefficients.Length; i++)
            {
                s += " " + coefficients[i].ToString("F4"); 
            }
            rval.Details.Add(s);

            

             
             //var runoff = YTerm.Evaluate(t, lookAhead,estimationScaleFactor);
            var runoffToDate = YTerm.RunoffToDate(t);
            var seasonalRunoff = YTerm.SeasonalRunoff(t);

             rval.Details.AddRange(YTerm.Details());
             var dr = YTerm.MonthNames;

             rval.Details.Add("");
             rval.ForecastPeriod = (t.ToString("MMM") + "-" + dr[dr.Count - 1]).ToUpper();
             rval.AverageRunoff = Lookup30YearAverageRunoff(t);

             string fullPeriod = (dr[0] + "-" + dr[dr.Count - 1]).ToUpper();
             rval.Details.Add(fullPeriod + "  = " + rval.Forecast.ToString("F2"));

           
            if( lookAhead)
            {
                // actual runoff
                rval.Details.Add("++ using look ahead for future data");
                rval.Details.Add(dr[0] + "  - " + dr[dr.Count - 1] + " runoff : " + seasonalRunoff.ToString("F1"));
                
            }
            else
            {
                rval.Details.Add("Runoff to Date : " + runoffToDate.ToString("F1"));
                // forecast forward
                var fc = rval.Forecast - runoffToDate;
                rval.ResidualForecast = fc;
                rval.Details.Add("Date - " + dr[dr.Count - 1] + " forecast : " + fc.ToString("F1"));
            }

            

           
            rval.Details.Add("average "+rval.ForecastPeriod+" runoff  = " + rval.AverageRunoff.ToString("F2"));
            

            return rval;
        }

        private double Lookup30YearAverageRunoff(DateTime t)
        {
            var tokens = TextFile.Split(aveargeFlowStation);

            if( aveargeFlowStation == "" || tokens.Length != 2)
               return 0;

           return HydrometMonthlySeries.AverageValue30Year(tokens[0], tokens[1], t.Month, YTerm.Month2);
            
        }

        
    }
}