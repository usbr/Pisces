using System;
using Reclamation.TimeSeries;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Reclamation.Core;

namespace Reclamation.TimeSeries.Parser
{
    /*
     * Code hacked by Karl Tarbet based on:
     * 
C#: The Complete Reference 
by Herbert Schildt 

Publisher: Osborne/McGraw-Hill (March 8, 2002)
ISBN: 0072134852
*/
    /*  
       This module contains the recursive descent 
       parser that recognizes variables. 
     * 
     * REad(t1,t2) is called on a function that returns a series, but has no series arguments
     * 
    */


    // Exception class for parser errors. 
    public class ParserException : Exception
    {
        public ParserException(string str) : base(str) { }

        public override string ToString()
        {
            return Message;
        }
    }

    /// <summary>
    /// Parses and evaluates seires expressions
    /// used by a CalculationSeries to calculate time series data.
    /// </summary>
    public class SeriesExpressionParser
    {
        // Enumerate token types. 
        enum Types { NONE, DELIMITER, VARIABLE, DOUBLE,INTEGER, FUNCTION, STRING, COMPARISON  }; 
        // Enumerate error types. 
        enum Errors { SYNTAX, UNBALPARENS, NOEXP, DIVBYZERO };

        string exp;    // refers to expression string 
        int expIdx;    // current index into the expression 
        string token;  // holds current token 
        Types tokType; // holds token's type 

        TimeInterval defaultTimeInterval = TimeInterval.Irregular;
        public   static bool Debug = false;
        //CalculationSeries series;

        private VariableResolver m_variableResolver;
        private VariableParser m_VariableParser;

        public VariableResolver VariableResolver
        {
            get { return this.m_variableResolver; }
            set { this.m_variableResolver = value; }
        }

        public bool RecursiveCalculations = false;

        TimeSeriesDatabase m_db;

        List<string> stack = new List<string>();

        public SeriesExpressionParser(TimeSeriesDatabase db,LookupOption lookup= LookupOption.SeriesName)
        {
            m_db = db;
            m_variableResolver = new VariableResolver(db, lookup);
            m_VariableParser = new VariableParser();
        }
        public SeriesExpressionParser()
        {
            // default resolver requires adding variables in advance
            m_variableResolver = new VariableResolver();
            m_VariableParser = new VariableParser();
        }

        DateTime t1, t2;


        internal bool IsValidExpression(string expression ,out string errorMessage)
        {
            try
            {
                SeriesList sl = SeriesInExpression(expression);
            }
            catch (Exception ex )
            {
                errorMessage = ex.Message;
                stack.Add("invalide expression: '" + expression + "'");
                return false;
            }
            errorMessage = "";
            return true;
        }


        public ParserResult Evaluate(string expression)
        {
            return Evaluate(expression, TimeSeriesDatabase.MinDateTime, TimeSeriesDatabase.MaxDateTime,defaultTimeInterval);
        }

        /// <summary>
        /// Evaluates expression using time range (t1,t2)
        /// </summary>
        /// <param name="expstr"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public ParserResult Evaluate(string expression, DateTime t1, DateTime t2,TimeInterval interval)
        {
            defaultTimeInterval = interval;
            exp = expression;
            AssignDates(t1, t2);
            ParserResult result;
            expIdx = 0;

            stack.Add(" begin evaluate : " + exp);
            if( Debug)
                Logger.WriteLine("begin Evaluate('"+exp +"'");
            
           // try
           // {
                GetToken();
                if (token == "")
                {
                    SyntaxErr(Errors.NOEXP); // no expression present 
                    return new ParserResult(0.0);
                }

                EvalExp1Comparison(out result); // now, call EvalExp1() to start 

                if (token != "") // last token must be null 
                    SyntaxErr(Errors.SYNTAX);
                Logger.WriteLine("completed Evaluate()");
                return result;
           // }
            //catch (ParserException exc)
            //{
            //    // Add other error handling here, as desired. 
            //    Logger.WriteLine("ERROR:");
            //    Logger.WriteLine(exc.Message);
            //    throw exc;
            //    //return new ParserResult(0.0);
            //}
            
        }

        /// <summary>
        /// Assign Dates. if full period of record is requested, find
        /// all series and determine period of record from the data.
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        private void AssignDates(DateTime t1, DateTime t2)
        {
            if (t1 == TimeSeriesDatabase.MinDateTime
                || t2 == TimeSeriesDatabase.MaxDateTime)
            {
                Logger.WriteLine("computing composite period of record");
                SeriesList sl = SeriesInExpression(exp);
                var por = sl.PeriodOfRecord();
                this.t1 = por.T1;
                this.t2 = por.T2;
                Logger.WriteLine(por.ToString());
            }
            else
            {
                this.t1 = t1;
                this.t2 = t2;
            }
        }

        /// <summary>
        /// Finds all series in the expression.
        /// </summary>
        /// <returns></returns>
        private SeriesList SeriesInExpression(string expresion)
        {
            var sl = new SeriesList();
            string[] variables = m_VariableParser.GetAllVariables(expresion);
            for (int i = 0; i < variables.Length; i++)
			{
                var alias = variables[i];
                //if (Regex.IsMatch(alias, "\".+\""))
                //{// strip double quotes
                //    alias = alias.Substring(1, alias.Length - 2);
                //}
                var v = VariableResolver.Lookup(alias,defaultTimeInterval);
                if (v.IsSeries)
                {
                    sl.Add(v.Series);
                }
			}

            return sl;
        }


        // Process comparison operators
        void EvalExp1Comparison(out ParserResult result)
        {
            string comp;
            ParserResult partialResult;

            Eval2Add(out result);
            while ((comp = token) == ">" || comp == "<")
            {
                stack.Add(" comparison : " + comp);
                GetToken();
                Eval2Add(out partialResult);

                if (Debug && result.IsSeries && partialResult.IsSeries)
                {
                    Console.WriteLine("Part A ");
                    partialResult.Series.WriteToConsole();
                    Console.WriteLine("Part B");
                    result.Series.WriteToConsole();
                }
                switch (comp)
                {
                    case ">":
                        result = result.GreaterThan(partialResult);
                        break;
                    case "<":
                        result = result.LessThan(partialResult);
                        break;
                }
                if ((Debug && result.IsSeries && partialResult.IsSeries))
                {
                    Console.WriteLine("Part A " + comp + " Part B ");
                    result.Series.WriteToConsole();
                }
            }




            //Eval2Add(out result);
        }

        // Add or subtract two terms. 
        void Eval2Add(out ParserResult result)
        {
            string op;
            ParserResult partialResult;

            Eval3Multiply(out result);
            while ((op = token) == "+" || op == "-")
            {
                stack.Add(" operator : " + op);
                GetToken();
                Eval3Multiply(out partialResult);

                if (Debug && result.IsSeries && partialResult.IsSeries)
                {
                    Console.WriteLine("Part A ");
                    partialResult.Series.WriteToConsole();
                    Console.WriteLine("Part B");
                    result.Series.WriteToConsole();
                }
                switch (op)
                {
                    case "-":
                        result = result - partialResult;
                        break;
                    case "+":
                        result = result + partialResult;
                        break;
                }
                if( (Debug && result.IsSeries && partialResult.IsSeries))
                {
                    Console.WriteLine("Part A "+op+" Part B ");
                    result.Series.WriteToConsole();
                }
            }
            
        }

        // Multiply or divide two factors. 
        void Eval3Multiply(out ParserResult result)
        {
            string op;
            ParserResult partialResult = new ParserResult(0.0);

            Eval4Exponent(out result);
            while ((op = token) == "*" ||
                   op == "/") //|| op == "%")
            {
                stack.Add(" operator : " + op);
                GetToken();
                Eval4Exponent(out partialResult);

                switch (op)
                {
                    case "*":
                        result = result * partialResult;
                        break;
                    case "/":
                        if (partialResult.IsZero)
                            SyntaxErr(Errors.DIVBYZERO);
                        result = result / partialResult;
                        break;
                    //case "%":
                    //    if (partialResult == 0.0)
                    //        SyntaxErr(Errors.DIVBYZERO);
                    //    result = (int)result % (int)partialResult;
                    //    break;
                }
            }
        }

        // Process an exponent. 
        void Eval4Exponent(out ParserResult result)
        {
            ParserResult partialResult;

            Eval5Sign(out result);
            if (token == "^")
            {
                stack.Add(" exponent: ^");
                GetToken();
                Eval4Exponent(out partialResult);
                result = ParserResult.Pow(result, partialResult);
            }
        }

        // Evaluate a unary + or -. 
        void Eval5Sign(out ParserResult result)
        {
            string op;

            op = "";
            if ((tokType == Types.DELIMITER) &&
                token == "+" || token == "-")
            {
                stack.Add(" operator : "+token);
                op = token;
                GetToken();
            }
            Eval6Parenthesis(out result);
            if (op == "-") result = new ParserResult(-1) * result;
        }

        // Process a parenthesized expression. 
        void Eval6Parenthesis(out ParserResult result)
        {
            if ((token == "("))
            {
                stack.Add("opening parenthesis ( ");
                GetToken();
                Eval2Add(out result);
                if (token != ")")
                    SyntaxErr(Errors.UNBALPARENS);
                GetToken();
            }
            else Atom(out result);
        }

        // Get the value of a number or variable. 
        void Atom(out ParserResult result)
        {
            double m_double;
            int m_int;
            switch (tokType)
            {
                case Types.DOUBLE:
                    stack.Add("double: " + token);
                    if (Double.TryParse(token, out m_double))
                    {
                          result = new ParserResult(m_double);
                    }
                    else
                    {
                       result = new ParserResult(0.0);
                       SyntaxErr(Errors.SYNTAX);
                    }
                    GetToken();
                    return;
                case Types.INTEGER:

                    stack.Add("integer: " + token);
                    if (Int32.TryParse(token, out m_int))
                    {
                        result = new ParserResult(m_int);
                    }
                    else
                    {
                        result = new ParserResult(0);
                        SyntaxErr(Errors.SYNTAX);
                    }
                    GetToken();
                    return;
                case Types.STRING:
                    stack.Add("string: " + token);
                     result = new ParserResult(token);
                    GetToken();
                    return;

                case Types.VARIABLE:
                    string alias = "";
                    int timeOffset = 0;
                    stack.Add("variable: " + token);
                    m_VariableParser.ParseVariableToken(token, out alias, out timeOffset);

                    if (Regex.IsMatch(alias, "\'.+\'")) 
                    {// strip single quotes
                        alias = alias.Substring(1, alias.Length - 2);
                    }
                    result = VariableResolver.Lookup(alias,defaultTimeInterval);
                    if( Debug)
                    Logger.WriteLine("SeriesLookup, Series:'"+alias +"'  Time offset: "+timeOffset+", IsString:"+result.IsString);
                    if (result.IsSeries)
                    {
                        Series s = result.Series;
                        s.Clear();
                        if ( RecursiveCalculations &&  s is CalculationSeries && s.Expression.Trim() != "" && s.Expression.IndexOf("this") < 0)
                        {
                            (s as CalculationSeries ).Calculate(t1, t2); // might depend on calculation
                        }
                        else
                        { // read data
                            s = ReadAndShiftData(-timeOffset, s,t1,t2);

                            if (Debug)
                                s.WriteToConsole();

                            Logger.WriteLine("Read "+s.Count+" points.  Missing Count = "+s.CountMissing());
                            Logger.WriteLine(s.ToString());

                           result =  new ParserResult(s);
                        }
                    }
                    GetToken();
                    return;
                case Types.FUNCTION:
                    ParserFunction function;
                    string subExpression = "";
                    ParserUtility.TryGetFunctionCall(token, out subExpression, out function);
                    stack.Add("function call: " + subExpression);
                    SeriesExpressionParser parser = new SeriesExpressionParser(m_db);
                    parser.VariableResolver = this.VariableResolver;
                    List<ParserResult> args = new List<ParserResult>();
                    bool anySeriesArgs = false;
                    for (int i = 0; i < function.Parameters.Length; i++)
                    {
                        var  a = parser.Evaluate(function.Parameters[i], this.t1, this.t2,defaultTimeInterval);
                        if (a.IsSeries)
                            anySeriesArgs = true;
                        args.Add(a);
                    }
                    if( Debug)
                       Logger.WriteLine("Evaluating function '" + function.Name + "'");
                    result =  function.Evaluate(args);
                    if (result.IsSeries && !anySeriesArgs)
                        result.Series.Read(this.t1, this.t2);

                    if( result.IsSeries && Debug)
                    {
                        Logger.WriteLine("Result is a series with:");
                        Logger.WriteLine("Count = " + result.Series.Count);
                        Logger.WriteLine("Missing Count = " + result.Series.CountMissing());
                        Logger.WriteLine("AverageOfSeries = "+Math.AverageOfSeries(result.Series));
                      }
                    //result = new ParserResult(2);

                    GetToken();
                    return;
                default:
                    result = new ParserResult(0.0);
                    SyntaxErr(Errors.SYNTAX);
                    break;
            }
        }

        /// <summary>
        /// Read Data and then shift in days or months
        /// </summary>
        /// <param name="timeOffset"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Series ReadAndShiftData(int timeOffset, Series s, DateTime t1, DateTime t2)
        {
            if (timeOffset == 0)
            {
                s.Read(t1, t2);
                return s;
            }
            else
            {
                if (s.TimeInterval == TimeInterval.Daily)
                {
                    s.Read(t1.AddDays(-timeOffset), t2.AddDays(-timeOffset));
                }
                else
                    if (s.TimeInterval == TimeInterval.Monthly)
                    {
                        s.Read(t1.AddMonths(-timeOffset), t2.AddMonths(-timeOffset));
                    }
                    else
                    {
                        throw new ArgumentException(" Error unsupported interval :" + s.TimeInterval);
                    }
                return Math.Shift(s, timeOffset);
            }


        }



        // Return a token to the input stream. 
        void PutBack()
        {
            for (int i = 0; i < token.Length; i++) expIdx--;
        }

        // Handle a syntax error. 
        void SyntaxErr(Errors error)
        {
            string[] err = { 
      "Syntax Error", 
      "Unbalanced Parentheses", 
      "No Expression Present", 
      "Division by Zero" 
         };

            throw new ParserException(err[(int)error]);
        }

        // Obtain the next token. 
        void GetToken()
        {
            tokType = Types.NONE;
            token = "";
            string variableName = "";
            string subExpr = "";
            ParserFunction function;
            if (expIdx == exp.Length) return; // at end of expression 

            // skip over white space 
            while (expIdx < exp.Length &&
                  Char.IsWhiteSpace(exp[expIdx])) ++expIdx;

            // trailing whitespace ends expression 
            if (expIdx == exp.Length) return;

            if (IsDelim(exp[expIdx]))
            { // is operator 
                token += exp[expIdx];
                expIdx++;
                tokType = Types.DELIMITER;
            }
            else if(ParserUtility.TryGetString(exp.Substring(expIdx),out subExpr) ) 
            {
                token = subExpr;
                expIdx += subExpr.Length+2; // two extra for literal double quotes
                tokType = Types.STRING;
            }
                
            else if ( m_VariableParser.GetVariableToken(exp.Substring(expIdx), out variableName))
            {
                token = variableName;
                expIdx += variableName.Length; 
                tokType = Types.VARIABLE;
            }
            else if (ParserUtility.TryGetFunctionCall(exp.Substring(expIdx), out subExpr,out function))
            {
                token = subExpr;
                expIdx += subExpr.Length;
                tokType = Types.FUNCTION;
            }
            else if (Char.IsDigit(exp[expIdx]) || exp[expIdx] == '.' )
            { // is number 
                while (!IsDelim(exp[expIdx]))
                {
                    token += exp[expIdx];
                    expIdx++;
                    if (expIdx >= exp.Length) break;
                }

                if (token.IndexOf(".") >= 0)
                    tokType = Types.DOUBLE;
                else
                    tokType = Types.INTEGER;
            }
            else if(ParserUtility.TryParseComparison(exp.Substring(expIdx), out subExpr))
            {
                token = subExpr;
                expIdx += subExpr.Length;
                tokType = Types.COMPARISON;
            }
            else
            {
                Logger.WriteLine("Unknown Token...");
            }
            
            if (Debug)
            { 
                var msg = "token = '" + token + "' type = " + tokType.ToString();
                stack.Add(msg);
                Logger.WriteLine(msg);
            }
        }




        // Return true if c is a delimiter. 
        bool IsDelim(char c)
        {
            if ((" +-/*^()".IndexOf(c) != -1))
                return true;
            return false;
        }

        
    }



}
