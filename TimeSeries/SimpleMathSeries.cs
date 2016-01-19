using System;
using System.Collections.Generic;
using System.Text;
using Reclamation.Core;

namespace Reclamation.TimeSeries
{
    public enum MathOperation { Add, Subtract };

    /// <summary>
    /// Basic math performed on a list of series.
    /// Computations are done 'on-the-fly' so performance might not be ideal,
    /// but computations are allways based on the latest source data
    /// </summary>
    /// 
    public class SimpleMathSeries: Series
    {
        MathOperation[] m_operation;
        SeriesList m_items;

        public SimpleMathSeries(string name, SeriesList items, MathOperation[] op):base()
        {
            this.Name = name;
            m_items = items;
            this.Appearance.LegendText = name;
            this.SiteID = name;
            m_operation = op;

            if( items.Count ==0)
                throw new ArgumentException("Must have at least 1 series to perform math");
            if( items.Count != op.Length+1)
                throw new ArgumentException("Number of operators (+, -) must agree with number of series");

            this.Units = items[0].Units;
            this.TimeInterval = items[0].TimeInterval;

            ValidateCalculation();

            //= "SeriesList=";
            var idList = new List<string>();
            var opList = new List<string>();
            for (int i = 0; i < items.Count; i++)
            {
                idList.Add(items[i].ID.ToString());
                if( i < op.Length)
                   opList.Add(op[i].ToString());
            }

            ConnectionString = "SeriesList=" + String.Join(",", idList.ToArray())
                + ";OperatorList=" + String.Join(",", opList.ToArray())
               + ";LastUpdate="
               + DateTime.Now.ToString(DateTimeFormatInstantaneous);
            Provider = "SimpleMathSeries";
        }

        private void ValidateCalculation()
        {
            for (int i = 0; i < m_items.Count; i++)
            {
                if( m_items[i].Units != Units)
                    throw new InvalidOperationException(m_items[i].Units + " and " + Units + " are not compatible for the calculation");

                if (m_items[i].TimeInterval != TimeInterval)
                {
                    throw new InvalidOperationException(m_items[i].TimeInterval.ToString() + " and " + TimeInterval.ToString() + " are not compatable");
                }
            }
            
        }


        private bool m_sourceDeleted = false;

        public SimpleMathSeries(TimeSeriesDatabase db,Reclamation.TimeSeries.TimeSeriesDatabaseDataSet.SeriesCatalogRow sr): base(db,sr)
        {
            string[] idList = ConnectionStringUtility.GetToken(ConnectionString, "SeriesList","").Split(',');
            string[] opList = ConnectionStringUtility.GetToken(ConnectionString, "OperatorList","").Split(',');
            m_operation = new MathOperation[opList.Length];


            m_items = new SeriesList();

                for (int i = 0; i < idList.Length; i++)
                {
                    int id = -1;
                    if (!Int32.TryParse(idList[i],out id) || !db.SeriesExists(id))
                    {
                        Logger.WriteLine("Cannot calculate series.  Has the data this calculation needs been deleted?");
                        Name = "Error: source data missing " + Name;
                        m_sourceDeleted = true;
                        break;
                    }
                    else
                    {
                        Series s = db.GetSeries(id);
                        m_items.Add(s);
                    }
                }
                ScenarioName = m_items[0].ScenarioName;
            // TO DO assuming all scenarioNames are the same.

            for (int i = 0; i < opList.Length; i++)
            {
                m_operation[i] = MathOperationFromString(opList[i]);
            }
        }

        //protected override Series CreateFromConnectionString()
        //{
        //    Series a,b;
        //    MathOperation op;
        //    ParseConnectionString(this.ConnectionString, this.TimeSeriesDatabase, out a, out b, out op);
        //    SimpleMathSeries s = new SimpleMathSeries(this.Name, a, b, op);

        //    return s;
        //}

        //private void ParseConnectionString(string connectionString, TimeSeriesDatabase db)
        //{

          
        //}

        private static MathOperation MathOperationFromString(string op)
        {
            MathOperation rval = MathOperation.Add;

            string[] names = Enum.GetNames(typeof(MathOperation));
            if (Array.IndexOf(names, op) >= 0)
            {
                rval = (MathOperation)Enum.Parse(typeof(MathOperation), op, false);
            }
            else
            {
                Logger.WriteLine("Error: math method '" + op + " is not defined.  Using default 'Add'");
            }
            return rval;
        }

        protected override void ReadCore(DateTime t1, DateTime t2)
        {
            if (m_sourceDeleted)
            {
                Logger.WriteLine("Cannot calculate series.  Has the data this calculation needs been deleted?");
                Clear();
                return;
            }

            for (int i = 0; i < m_items.Count; i++)
            {
                m_items[i].Read(t1, t2);
            }

            Series s = m_items[0];

            for (int i = 1; i < m_items.Count; i++)
            {
                s = ApplyOperator(s, m_items[i], m_operation[i - 1]);
            }
            Series dummy = new Series();
            Series.CopyAttributes(this, dummy);
            this.InitTimeSeries(s.Table, s.Units, s.TimeInterval, true, s.Appearance);
            Series.CopyAttributes(dummy, this);
        }

        private Series ApplyOperator(Series s1, Series s2, MathOperation op)
        {
            Series s = null;
            if (op == MathOperation.Subtract)
            {
                 s = Math.Subtract(s1, s2);
            }
            else if (op == MathOperation.Add)
            {
                 s = Math.Add(s1, s2);
            }
            return s;
        }

        public override Series CreateScenario(TimeSeriesDatabaseDataSet.ScenarioRow scenario)
        {
            SeriesList list = new SeriesList();
            
            for (int i = 0; i < m_items.Count; i++)
            {
                list.Add(m_items[i].CreateScenario(scenario));
            }
            SimpleMathSeries rval = new SimpleMathSeries(Name, list, m_operation);
            
            rval.Name = this.Name;
            rval.Appearance.LegendText = scenario.Name + " " + Name;
            rval.ScenarioName = scenario.Name;
            rval.SiteID = this.SiteID;
            rval.TimeInterval = this.TimeInterval;
            return rval;
        }
    }
}