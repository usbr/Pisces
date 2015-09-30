using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Reclamation.Core;

namespace Reclamation.TimeSeries.Parser
{
    /// <summary>
    /// Extracts Function calls and arguments from an expression
    /// </summary>
    public class ParserUtility
    {

        public static bool TryGetFunctionCall(string expression,out string subExpression,
            out ParserFunction f, bool matchFromBeginning=true)
        {
             subExpression = "";
            f = new ParserFunction();
            f.Name = "";

            // adapted from mastering Regular Expressions 2nd edition page 430
            string exp = @"(?<functionName>[A-Za-z]+[A-Za-z0-9]*)\((?<parameters>  " +
                           "   (?>                 " +
                           "       [^()]+          " +
                           "     |                 " +
                           @"       \( (?<DEPTH>)  " +
                           "     |                 " +
                           @"       \) (?<-DEPTH>) " +
                           "   )*                  " +
                           "   (?(DEPTH)(?!))      " +
                           @" )\)                    ";

            if (matchFromBeginning)
                exp = "^" + exp;

            Regex re = new Regex(exp, RegexOptions.IgnoreCase| RegexOptions.IgnorePatternWhitespace);

            var m = re.Match(expression);
            if (m.Success)
            {
                subExpression = m.Groups[0].Value;
                f.Name = m.Groups["functionName"].Value;
                f.Parameters = ExtractFunctionParameters(m.Groups["parameters"].Value);
                
                return true;
            }

            return false;
        }

        private static string[] ExtractFunctionParameters(string parms)
        {
            // check for '(' within function call, which could be other function calls.
            // example   "daily,EstimateDailyFromMonthly(daily,monthly)"

            // the example above has two commas ','  so a simple split(',') will not work correctly.


            // pull out all function calls and replace with tag:{0}, tag:{1}, etc..
            // until there are no more '('   then split on commas ,
            // and put function calls back inside.

            int tagCounter = 0;
            var calls = new List<string>();
            string functionCall;
            ParserFunction f;
            while (TryGetFunctionCall(parms, out functionCall, out f, false))
            {
                parms = parms.Replace(functionCall, "tag:{" + tagCounter + "}");
                calls.Add(functionCall);
                Logger.WriteLine(functionCall);
                tagCounter++;

            }

            string[] rval = parms.Trim().Split(',');

            for (int i = 0; i < calls.Count; i++)
            {
                for (int j = 0; j < rval.Length; j++)
                {
                    rval[j] = rval[j].Replace("tag:{" + i + "}",calls[i]);
                }
            }

            return rval;
        }

        /// <summary>
        /// Find double quoted string 
        /// http://stackoverflow.com/questions/2148587/finding-quoted-strings-with-escaped-quotes-in-c-sharp-using-a-regular-expression?rq=1
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="subExpression"></param>
        /// <returns></returns>
        internal static bool TryGetString(string expression, out string subExpression)
        {
            Regex r = new Regex(@"^""[^""\\]*(?:\\.[^""\\]*)*""");

            bool rval = r.IsMatch(expression);
            if (rval)
                subExpression = r.Match(expression).Value;
            else
                subExpression = "";

            if (Regex.IsMatch(subExpression, "\".+\""))
            {// strip double quotes
                subExpression = subExpression.Substring(1, subExpression.Length - 2);
            }

            return rval;

        }

        internal static bool TryParseComparison(string token, out string conditional)
        {
            conditional = "";
            return false;
        }

    }
}
