// This file is not compiled
//  for reference only
//
namespace ConsoleApplication1
{
    /*
C#: The Complete Reference 
by Herbert Schildt 

Publisher: Osborne/McGraw-Hill (March 8, 2002)
ISBN: 0072134852
*/
    /*  
       This module contains the recursive descent 
       parser that recognizes variables. 
    */

    using System;

    // Exception class for parser errors. 
    class ParserException : ApplicationException
    {
        public ParserException(string str) : base(str) { }

        public override string ToString()
        {
            return Message;
        }
    }

    class Parser
    {
        // Enumerate token types. 
        enum Types { NONE, DELIMITER, VARIABLE, NUMBER };
        // Enumerate error types. 
        enum Errors { SYNTAX, UNBALPARENS, NOEXP, DIVBYZERO };

        string exp;    // refers to expression string 
        int expIdx;    // current index into the expression 
        string token;  // holds current token 
        Types tokType; // holds token's type 

        // Array for variables. 
        double[] vars = new double[26];

        public Parser()
        {
            // Initialize the variables to zero. 
            for (int i = 0; i < vars.Length; i++)
                vars[i] = 0.0;
        }

        // Parser entry point. 
        public double Evaluate(string expstr)
        {
            double result;

            exp = expstr;
            expIdx = 0;

            try
            {
                GetToken();
                if (token == "")
                {
                    SyntaxErr(Errors.NOEXP); // no expression present 
                    return 0.0;
                }

                EvalExp1(out result); // now, call EvalExp1() to start 

                if (token != "") // last token must be null 
                    SyntaxErr(Errors.SYNTAX);

                return result;
            }
            catch (ParserException exc)
            {
                // Add other error handling here, as desired. 
                Console.WriteLine(exc);
                return 0.0;
            }
        }

        // Process an assignment. 
        void EvalExp1(out double result)
        {
            int varIdx;
            Types ttokType;
            string temptoken;

            if (tokType == Types.VARIABLE)
            {
                // save old token 
                temptoken = String.Copy(token);
                ttokType = tokType;

                // Compute the index of the variable. 
                varIdx = Char.ToUpper(token[0]) - 'A';

                GetToken();
                if (token != "=")
                {
                    PutBack(); // return current token 
                    // restore old token -- not an assignment 
                    token = String.Copy(temptoken);
                    tokType = ttokType;
                }
                else
                {
                    GetToken(); // get next part of exp 
                    EvalExp2(out result);
                    vars[varIdx] = result;
                    return;
                }
            }

            EvalExp2(out result);
        }

        // Add or subtract two terms. 
        void EvalExp2(out double result)
        {
            string op;
            double partialResult;

            EvalExp3(out result);
            while ((op = token) == "+" || op == "-")
            {
                GetToken();
                EvalExp3(out partialResult);
                switch (op)
                {
                    case "-":
                        result = result - partialResult;
                        break;
                    case "+":
                        result = result + partialResult;
                        break;
                }
            }
        }

        // Multiply or divide two factors. 
        void EvalExp3(out double result)
        {
            string op;
            double partialResult = 0.0;

            EvalExp4(out result);
            while ((op = token) == "*" ||
                   op == "/" || op == "%")
            {
                GetToken();
                EvalExp4(out partialResult);
                switch (op)
                {
                    case "*":
                        result = result * partialResult;
                        break;
                    case "/":
                        if (partialResult == 0.0)
                            SyntaxErr(Errors.DIVBYZERO);
                        result = result / partialResult;
                        break;
                    case "%":
                        if (partialResult == 0.0)
                            SyntaxErr(Errors.DIVBYZERO);
                        result = (int)result % (int)partialResult;
                        break;
                }
            }
        }

        // Process an exponent. 
        void EvalExp4(out double result)
        {
            double partialResult, ex;
            int t;

            EvalExp5(out result);
            if (token == "^")
            {
                GetToken();
                EvalExp4(out partialResult);
                ex = result;
                if (partialResult == 0.0)
                {
                    result = 1.0;
                    return;
                }
                for (t = (int)partialResult - 1; t > 0; t--)
                    result = result * (double)ex;
            }
        }

        // Evaluate a unary + or -. 
        void EvalExp5(out double result)
        {
            string op;

            op = "";
            if ((tokType == Types.DELIMITER) &&
                token == "+" || token == "-")
            {
                op = token;
                GetToken();
            }
            EvalExp6(out result);
            if (op == "-") result = -result;
        }

        // Process a parenthesized expression. 
        void EvalExp6(out double result)
        {
            if ((token == "("))
            {
                GetToken();
                EvalExp2(out result);
                if (token != ")")
                    SyntaxErr(Errors.UNBALPARENS);
                GetToken();
            }
            else Atom(out result);
        }

        // Get the value of a number or variable. 
        void Atom(out double result)
        {
            switch (tokType)
            {
                case Types.NUMBER:
                    try
                    {
                        result = Double.Parse(token);
                    }
                    catch (FormatException)
                    {
                        result = 0.0;
                        SyntaxErr(Errors.SYNTAX);
                    }
                    GetToken();
                    return;
                case Types.VARIABLE:
                    result = FindVar(token);
                    GetToken();
                    return;
                default:
                    result = 0.0;
                    SyntaxErr(Errors.SYNTAX);
                    break;
            }
        }

        // Return the value of a variable. 
        double FindVar(string vname)
        {
            if (!Char.IsLetter(vname[0]))
            {
                SyntaxErr(Errors.SYNTAX);
                return 0.0;
            }
            return vars[Char.ToUpper(vname[0]) - 'A'];
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
            else if (Char.IsLetter(exp[expIdx]))
            { // is variable 
                while (!IsDelim(exp[expIdx]))
                {
                    token += exp[expIdx];
                    expIdx++;
                    if (expIdx >= exp.Length) break;
                }
                tokType = Types.VARIABLE;
            }
            else if (Char.IsDigit(exp[expIdx]))
            { // is number 
                while (!IsDelim(exp[expIdx]))
                {
                    token += exp[expIdx];
                    expIdx++;
                    if (expIdx >= exp.Length) break;
                }
                tokType = Types.NUMBER;
            }
        }

        // Return true if c is a delimiter. 
        bool IsDelim(char c)
        {
            if ((" +-/*%^=()".IndexOf(c) != -1))
                return true;
            return false;
        }
    }


    // Demonstrate the parser. 


    public class ParserDemo1
    {
        public static void Main()
        {
            string expr;
            Parser p = new Parser();

            Console.WriteLine("Enter an empty expression to stop.");

            for (; ; )
            {
                Console.Write("Enter expression: ");
                expr = Console.ReadLine();
                if (expr == "") break;
                Console.WriteLine("Result: " + p.Evaluate(expr));
            }
        }
    }



}
