using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Reclamation.TimeSeries.Parser
{
    /// <summary>
    /// The VariableParser class is used
    /// to read variable tokens, and optional time parameters to that 
    /// variable
    /// </summary>
    public class VariableParser
    {

        string timePattern;
        /// <summary>
        /// this expression uses word boundary \b to match all variables at once.
        /// </summary>
        string patternAllVariables;


        /// <summary>
        /// This expression matches a single variable name
        /// </summary>
        string variablePattern;

        public VariableParser()
        {
            SetupRegularExpressions();

        }

        public string SpecialCharacters = "";

        private void SetupRegularExpressions()
        {
            //end with optional time like [t-1] and not ending with '(' which indicates function call, not a variable
            timePattern = @"(?<time>\[[t\-+0-9 ]+\]){0,1}(?!\()";

            // variables start with a letter and don't have spaces.
            // If it is surronded by double quotes it is a string, not a variable.
            // However, special case variable may have spaces if surrounded with single quotes.
            // Special case with single quotes also allows variable starting with a number (for a Yakima specail case 1146 wasteway)
            variablePattern = @"(?<alias>^[a-zA-Z]{1}\w*\b)" + timePattern // begins with letter
                           + @"|(?<alias>^(?:')[a-zA-Z_\s0-9]*(?:'))" + timePattern; // wrapped with single quote 'variable' and can have space



            // get multiple matches in a single pattern.
            patternAllVariables = @"(?<!"")(?<alias>\b[a-zA-Z]{1}\w*\b)(?!"")" + timePattern
                                + @"|'(?<alias>[a-zA-Z_\s0-9]*\b)'" + timePattern;
        }

        public static VariableParser Default()
        {
            return new VariableParser();
        }
         


        

        /// <summary>
        /// Given partial expression check beginning of input expresion
        /// to see if this is a variable
        /// </summary>
        /// <param name="token">portion of input expression being evaluated by the parser</param>
        /// <param name="variableNameWithTime">if sucessfull contains variable name</param>
        /// <returns>true if this is a valid variable (must begin with letter and be a word) </returns>
        public bool GetVariableToken(string token, out string variableNameWithTime)
        {
            int timeOffset = 0;
            string alias = "";
           return ParseVariableToken(token, out alias,out variableNameWithTime, out timeOffset);

        }


        public bool ParseVariableToken(string token, out string alias, out int timeOffset)
        {
            string aliasWithTime = "";
            return ParseVariableToken(token, out alias, out aliasWithTime, out timeOffset);
        }


        internal string[] GetAllVariables(string expression)
        {
            // remove double quoted strings, they are not variables
            var m = Regex.Match(expression, "\".+\"", RegexOptions.None);
            if (m.Success)
            {
                expression = expression.Replace(m.Value, "");
            }
            var rval = new List<string>();
            var mc = Regex.Matches(expression, patternAllVariables);
            foreach (Match item in mc)
            {
                rval.Add(item.Groups["alias"].Value);
            }

            return rval.ToArray();
        }


        

        /// <summary>
        /// Given partial expression check beginning of input expresion
        /// to see if this is a variable
        /// </summary>
        /// <param name="token">portion of input expression being evaluated by the parser</param>
        /// <param name="alias">if sucessfull contains variable name</param>
        /// <param name="timeOffset">if sucessful match contains time offset (or default of zero)</param>
        /// <returns>true if this is a valid variable (must begin with letter and be a word) </returns>
        public bool ParseVariableToken(string token, out string alias, out string aliasWithTime , out int timeOffset)
        {

            RegexOptions options = ((RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline)
                        | RegexOptions.IgnoreCase);
            Regex re = new Regex(variablePattern, options);
            alias = "";
            aliasWithTime = "";
            timeOffset = 0;

            var m = re.Match(token);
//            expIncrement = m.Length;

            if (m.Success)
            {
                aliasWithTime = m.Groups[0].Value;
                alias = m.Groups["alias"].Value;
                string time = m.Groups["time"].Value.Trim();

                if (time != "")
                {
                   // Console.WriteLine(time);
                    /*
                     --- Time expressions ---
                        [t-1]
                        [t]
                        [t+1]
                        [t +   5 ]
                        -- invalid times 
                        [t +    ]
                        [t4]
                        [ 3]
                        t 3
                     */
                    var m2 = Regex.Match(time, @"\s*\[\s*t\s*((?<sign>\+|-)\s*(?<offset>\d+))*\s*\]\s*",
                        ((RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline)
                        | RegexOptions.IgnoreCase));
                    Regex r = new Regex(@"\s*\[\s*t\s*((?<sign>\+|-)\s*(?<offset>\d+))*\s*\]\s*");
                    m2 = r.Match(time);
                    if (!m2.Success)
                    {
                        throw new ParserException("Invalid time expression '" + time + "'");
                    }

                    string sign = m2.Groups["sign"].Value;
                    string offset = m2.Groups["offset"].Value;
                    if (offset != "")
                    {
                        timeOffset = Int32.Parse(offset);
                        if (sign == "-")
                            timeOffset = timeOffset * -1;
                    }
                }

                //Console.WriteLine(alias);
                // Console.WriteLine(time);
            }
            return m.Success;
        }

    }
}
