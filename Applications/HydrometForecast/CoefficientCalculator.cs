using System;
using System.Collections.Generic;
using Reclamation.Core;
using System.IO;

namespace HydrometForecast
{
    class CoefficientCalculator
    {

        public double[] Coefficients = new double[] { };
        public string[] Output = new string[] { };
        public string dataFile = ""; //
        public CoefficientCalculator()
        {

        }

        public double[] ComputeCoefficients(ForecastEquation eq, string pathToR )
        {
            var tbl = eq.ComputeHistoricalCoefficients(eq.StartYear, eq.EndYear);

            // perf.Report("done."); // 1.2 seconds with cache/  33 seconds without
            dataFile = FileUtility.GetTempFileName(".csv");
            CsvFile.WriteToCSV(tbl, dataFile, false);


            dataFile = dataFile.Replace("\\", "/");
            var rInput = new List<string>();
            rInput.Add("# Forecast " + eq.Name);
            rInput.Add("a <- read.csv(\"" + dataFile + "\")");
            rInput.Add("b <- subset(a, select=-Year)");
            rInput.Add("cor(b)");
            rInput.Add("summary(b)");

            string s = "fit <- lm(Y1 ~ ";
            for (int i = 1; i < tbl.Columns.Count-1; i++)
            {
                s += " + X" + i;
            }
            s += ", data=a)";
            rInput.Add(s);
            rInput.Add("options(width=240)");
            rInput.Add("summary(fit)");
            rInput.Add("coefficients(fit)");

            string rFile = FileUtility.GetTempFileName(".txt");
            TextFile rtf = new TextFile(rInput.ToArray());
            rtf.SaveAs(rFile);
            rFile = rFile.Replace("\\", "/");

            var exe = Path.Combine(pathToR, "R\\bin\\R.exe");

            if (!File.Exists(exe))
            {
                throw new Exception("Could not find the R statistic program.  It should be in a sub directory R");
            }

            ProgramRunner pr = new ProgramRunner();
            pr.Run(exe, "--no-restore --no-save --quiet < \"" + rFile);
            pr.WaitForExit();

            Coefficients = GetCoefficients(pr.Output);

            
            Output = pr.Output;

            return Coefficients;
        }

        


        /*
> coefficients(fit)
(Intercept)          X1          X2          X3          X4 
-881.113167    3.051249   12.644935    4.149389    9.879917 
         */
        /// <summary>
        /// Gets coeficients from R output.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private static double[] GetCoefficients(string[] p)
        {
            var rval = new List<double>();

            TextFile tf = new TextFile(p);
            int idx = tf.IndexOf("> coefficients(fit)");
            if (idx >= 0 && idx + 1 < tf.Length)
            {
                var tokens = TextFile.Split(tf[idx+2].Trim());
                for (int i = 0; i < tokens.Length; i++)
                {
                    var d = 0.0;
                    if (double.TryParse(tokens[i], out d))
                    {
                        rval.Add(d);
                    }
                }

                if (rval.Count > 0)
                { // move intercept to the end, consistend with Fotran and excel format
                    double intercept = rval[0];
                    rval.RemoveAt(0);
                    rval.Add(intercept);
                }
            }
            return rval.ToArray();
        }

        public static string FormatCoefficients(double[] c)
        {
            var rval = "";

            for (int i = 0; i < c.Length; i++)
            {
                rval += c[i].ToString("F2").PadLeft(5) + " ";
            }

            return rval;
        }

    }
}
