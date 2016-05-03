using System;
using System.Data;
using System.IO;
using Reclamation.Core;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Parser;

namespace VicUnregToModsimScenarios
{
class Program
{
    /// <summary>
    /// Organizing VIC climate unregulated data from one large  VIC Pisces database
    /// into 20 Monthly MODSIM compatible *.PDB.
    /// </summary>
    /// <param name="args"></param>
    static void Main(string[] args)
    {
        if (args.Length != 2)
        {
            PrintUsage();
            return;
        }

        var fileNamePdb = args[0];
        if (!File.Exists(fileNamePdb) || !fileNamePdb.EndsWith(".pdb"))
        {
            PrintUsage();
            return;
        }

        var fileNameExcel = args[1];
        if (!File.Exists(fileNameExcel) || (!fileNameExcel.EndsWith(".xls")
                                            && !fileNameExcel.EndsWith(".xlsx")))
        {
            PrintUsage();
            return;
        }

        //inputs should be good, get to work
        SQLiteServer svrVic = new SQLiteServer(fileNamePdb);
        Console.WriteLine("opening " + fileNamePdb);
        TimeSeriesDatabase dbVic = new TimeSeriesDatabase(svrVic);

        var xls = new NpoiExcel(fileNameExcel);
        Console.WriteLine("reading " + fileNameExcel);
        var correlation = xls.ReadDataTable("Locals");
        var forecasts = xls.ReadDataTable("Forecasts");

        var period = new string[] { "2020", "2040", "2060", "2080" };
        var scenario = new string[] { "Median", "MoreWarmingDry", "MoreWarmingWet", "LessWarmingDry", "LessWarmingWet" };

        CreatePiscesDatabaseWithModsimNodeNames(dbVic, correlation, "", "Baseline");
        AddForecastsToPiscesDatabase(dbVic, forecasts, "", "Baseline");
        for (int i = 0; i < period.Length; i++)
        {
            for (int j = 0; j < scenario.Length; j++)
            {
                CreatePiscesDatabaseWithModsimNodeNames(dbVic, correlation, period[i],
                                                        scenario[j]);
                AddForecastsToPiscesDatabase(dbVic, forecasts, period[i], scenario[j]);
            }
        }
    }

    private static void PrintUsage()
    {
        Console.WriteLine("Usage:  vicBiasCorrected.pdb correlation.xls");
        Console.WriteLine("Where:");
        Console.WriteLine("    vicBiasCorrected.pdb contains multiple climate scenarios");
        Console.WriteLine("    correlation.xls contains mapping between vic and MODSIM names");
        Console.WriteLine("output is:   multiple pisces databases for each scenario and period");
        Console.WriteLine("MODSIM names in output come from correlation.xls");
    }

    private static void AddForecastsToPiscesDatabase(TimeSeriesDatabase dbVic,
            DataTable forecasts, string period, string scenario)
    {
        for (int i = 0; i < forecasts.Rows.Count; i++)
        {
            string vicName = forecasts.Rows[i]["VicName"].ToString().Trim();
            if (string.IsNullOrEmpty(vicName))
            {
                continue;
            }
            string mNode = forecasts.Rows[i]["ModsimNode"].ToString().Trim();
            int thruMonth = Convert.ToInt32(
                                forecasts.Rows[i]["ThruMonth"].ToString().Trim());
            AddForecastSeries(period, scenario, dbVic, mNode, thruMonth, vicName);
        }
    }

    private static void AddForecastSeries(string period, string scenario,
                                          TimeSeriesDatabase dbVic, string name,
                                          int thruMonth, string vicName)
    {
        string fn = period + scenario + ".pdb";
        SQLiteServer svr = new SQLiteServer(fn);
        TimeSeriesDatabase db = new TimeSeriesDatabase(svr);

        Series sVic = dbVic.GetSeriesFromName(vicName + period + scenario);
        sVic.Read();

        Series s = new Series(name + "_Forecast");
        s.TimeInterval = TimeInterval.Monthly;
        s.TimeSeriesDatabase = db;
        s.Units = "acre-feet";

        //initial model data start date and value
        s.Add(sVic[0].DateTime, sVic[0].Value * 1.98347 * sVic[0].DateTime.EndOfMonth().Day);

        for (int i = 0; i < sVic.Count; i++)
        {
            int month = sVic[i].DateTime.Month;
            if (month <= 6)
            {
                Point pt = new Point();
                pt.DateTime = sVic[i].DateTime;
                pt.Value = SumThruMonthToAcreFt(sVic, pt.DateTime, thruMonth);
                s.Add(pt);
            }
        }

        db.AddSeries(s);
        SetSeriesDatesToBeginningOfMonth(s);
        ConsolePrintSeriesNameAndCount(s);
    }

    private static double SumThruMonthToAcreFt(Series sVic, DateTime start,
            int thruMonth)
    {
        DateTime end = start.AddMonths(thruMonth - start.Month);

        Series sub = Reclamation.TimeSeries.Math.Subset(sVic, start, end);

        //convert to acre-feet
        for (int i = 0; i < sub.Count; i++)
        {
            Point pt = sub[i];
            pt.Value *= 1.98347 * pt.DateTime.EndOfMonth().Day;
            sub[i] = pt;
        }

        return Reclamation.TimeSeries.Math.Sum(sub);
    }

    private static void CreatePiscesDatabaseWithModsimNodeNames(
        TimeSeriesDatabase dbVic,
        DataTable correlation, string period, string scenario = "")
    {
        string fn = period + scenario + ".pdb";
        if (File.Exists(fn))
        {
            Console.WriteLine("Warning: Overwriting existing file " + fn);
            File.Delete(fn);
        }
        else
        {
            Console.WriteLine("creating " + fn);
        }

        SQLiteServer svr = new SQLiteServer(fn);
        TimeSeriesDatabase db = new TimeSeriesDatabase(svr);

        VariableResolver vr = new VariableResolver(dbVic, LookupOption.SeriesName);

        Console.WriteLine("-------------------------------");
        Console.WriteLine("database               records");
        Console.WriteLine("Series                 saved");
        Console.WriteLine("-------------------------------");
        for (int i = 0; i < correlation.Rows.Count; i++)
        {
            var gain = correlation.Rows[i]["ModsimGain"].ToString();
            var neg = correlation.Rows[i]["ModsimNeg"].ToString();
            var equation = correlation.Rows[i]["Equation"].ToString().Trim();

            if (equation != "")
            {
                equation = IncludeScenarioAndPeriod(equation, period, scenario);
                AddSeries(period, scenario, db, vr, gain, equation, ModsimType.Gain);
                AddSeries(period, scenario, db, vr, neg, equation, ModsimType.Negative);
            }
        }
    }

    enum ModsimType {Gain, Negative};
    private static void AddSeries(string period, string scenario,
                                  TimeSeriesDatabase db, VariableResolver vr,
                                  string name, string equation, ModsimType mType)
    {
        if (name.Trim().ToLower() == "nan")
        {
            return;
        }

        if (name.Trim() == "")
        {
            Console.WriteLine("--- WARNING modsim Node Name is missing. Type = " +
                              mType.ToString());
            return;
        }

        CalculationSeries cs = new CalculationSeries(name);
        cs.TimeInterval = TimeInterval.Monthly;

        if (mType == ModsimType.Gain)
        {
            cs.Expression = string.Format("Max({0}, 0)", equation);
        }
        if (mType == ModsimType.Negative)
        {
            cs.Expression = string.Format("Abs(Min({0}, 0))", equation);
        }
        cs.TimeSeriesDatabase = db;
        cs.Parser.VariableResolver = vr;
        cs.Units = "cfs";
        db.AddSeries(cs);    // add series before calcualte to get an id assigned.
        cs.Calculate(); // Calculate also saves the data.

        SetSeriesDatesToBeginningOfMonth(cs);
        ConsolePrintSeriesNameAndCount(cs);
    }

    private static void SetSeriesDatesToBeginningOfMonth(Series s)
    {
        // change all dates to beginning of month.
        for (int i = 0; i < s.Count; i++)
        {
            var pt = s[i];
            pt.DateTime = pt.DateTime.FirstOfMonth();
            s[i] = pt;
        }
        s.Save();
    }

    private static void ConsolePrintSeriesNameAndCount(Series s)
    {
        Console.Write(s.Name.PadRight(23));
        Console.WriteLine(s.Count);
    }

    private static string IncludeScenarioAndPeriod(string equation, string period,
            string scenario)
    {
        //modify names in equation to include scenario and period.
        var rval = equation;
        var removeTokens = new char[] { ' ', '+', '-', ')', '(' };
        var tokens = equation.Split(removeTokens,
                                    StringSplitOptions.RemoveEmptyEntries);

        foreach (var t in tokens)
        {
            if (Array.IndexOf(removeTokens, t) < 0)
            {
                rval = rval.Replace(t, t + period + scenario);
            }
        }

        return rval;
    }

} //class
} //namespace
