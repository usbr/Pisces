using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries.Parser
{
    class DatabaseVariableResolver: VariableResolver
    {
        TimeSeriesDatabase db;
        CalculationSeries s;
        public DatabaseVariableResolver(TimeSeriesDatabase db, CalculationSeries s)
        {
            this.db = db;
            this.s = s;
        }

        public CalculationSeries CalculationSeries
        {
        get { return s;}
       }

        public ParserResult Lookup(string name)
        {
            if (name == "this")
                return new ParserResult(s);
            var s2 = db.GetSeriesFromName(name);
            //  s2.SetMissingDataToZero = s.SetMissingDataToZero;
            return new ParserResult(s2);
        }
    }

}
