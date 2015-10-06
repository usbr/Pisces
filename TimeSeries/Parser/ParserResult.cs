using System;
using System.Collections.Generic;
using System.Text;

namespace Reclamation.TimeSeries.Parser
{
    public class ParserResult
    {

        Series series;

        public Series Series
        {
            get { return series; }
            set { series = value; }
        }
        double double_val;

        public double Double
        {
            get {
                if (IsInteger)
                    return int_val;

                return double_val; 
            }
        }

        int int_val;
        public int Integer
        {
            get { return int_val; }
        }

        bool m_isDouble = false;
        public bool IsDouble { get { return m_isDouble; } }

        bool m_isInteger = false;
        public bool IsInteger { get { return m_isInteger; } }

        bool isSeries;

        public bool IsSeries
        {
            get { return isSeries; }
           // set { isSeries = value; }
        }

        bool m_isBool = false;

        public bool IsBool
        {
            get { return m_isBool; }
           // set { m_isBool = value; }
        }

        bool m_bool = false;

        public bool Bool
        {
            get { return m_bool; }
        }


        string m_string = "";
        bool m_isString = false;

        public bool IsString
        {
            get { return m_isString; }
            set { m_isString = value; }
        }
        public string Str
        {
            get { return m_string; }
        }

       
        public ParserResult(string s)
        {
            m_string = s;
            m_isString = true;
        }

        public ParserResult(bool b)
        {
            m_bool = b;
            m_isBool = true;
        }

        public ParserResult(Series s)
        {
            series = s;
            isSeries = true;
        }

        public ParserResult(double val)
        {
            this.double_val = val;
            m_isDouble = true;
        }

        public ParserResult(int val)
        {
            this.int_val = val;
            this.m_isInteger = true;
        }

        public bool IsZero
        {
            get { return 
                (m_isDouble && double_val == 0)
                ||
                (m_isInteger && int_val == 0 ) 
                ; }
        }

        /// <summary>
        /// a - b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static ParserResult operator -(ParserResult a, ParserResult b)
        {
            if (a.isSeries)
            {
                if (b.isSeries)
                    return new ParserResult(a.series - b.series);
                else
                    if( b.IsDouble)
                    return new ParserResult(a.series - b.Double);
                    else if( b.IsInteger)
                        return new ParserResult(a.series - b.Integer);

                throw new ParserException("Invalid operation, subracting from a series must be another series or a number");
            }
            else
            {// a and b are numbers
                if (!b.isSeries)
                    return new ParserResult(a.Double - b.Double);
                else// constant - series  = -series + constant
                    return new ParserResult(-b.series + a.Double);
            }
        }

        /// <summary>
        /// a + b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static ParserResult operator +(ParserResult a, ParserResult b)
        {

            if (a.isSeries)
            {
                if (b.isSeries)
                    return new ParserResult(a.series + b.series);
                else // series + constant
                    return new ParserResult(a.series + b.Double);
            }
            else
            {// a is double or integer
                if (!b.isSeries)
                    return new ParserResult(a.Double + b.Double);
                else // constant + series  = series + constant
                    return new ParserResult(b.series + a.Double);
            }

        }

        public static ParserResult operator *(ParserResult a, ParserResult b)
        {

            if (a.isSeries)
            {
                if (b.isSeries)
                {
                    var m = Math.Multiply(a.series, b.series);
                    return new ParserResult(m);
                }
                else
                    return new ParserResult(a.series * b.Double);
            }
            else
            {// a is double
                if (!b.isSeries)
                    return new ParserResult(a.Double * b.Double);
                else
                    return new ParserResult(b.series * a.Double);
            }
        }


        public static ParserResult operator /(ParserResult a, ParserResult b)
        {

            if (a.isSeries)
            {
                if (b.isSeries)
                    return new ParserResult(a.series / b.series);
                else
                    return new ParserResult(a.series / b.Double);
            }
            else
            {
                if (!b.isSeries) // both numbers
                {
                    return new ParserResult(a.Double / b.Double);
                }
                else
                {
                    throw new ParserException("Error: dividing by a series is not supported. You can divide by a number");
                }
            }
        }



        internal static ParserResult Pow(ParserResult result, ParserResult ex)
        {

            if (!result.isSeries && !ex.isSeries)
                return new ParserResult(System.Math.Pow(result.Double, ex.Double));
            else if (ex.IsDouble && result.IsSeries)
            {
                return new ParserResult(Math.Pow(result.series, ex.Double));
            }
            else
                throw new ParserException("Error: (Exponent ^) can't raise a number to a series");

        }
        public override string ToString()
        {
            if (isSeries)
                return this.series.ToString();
            else
                if (IsInteger)
                    return this.int_val.ToString();
                else
                    return this.double_val.ToString();
        }

        internal ParserResult GreaterThan(ParserResult partialResult)
        {
            ParserResult rval;
            if( IsSeries && partialResult.isSeries )
            {
                rval = new ParserResult(Math.Compare( this.series, partialResult.series,GT));
                return rval;
            }
            if (IsSeries && partialResult.IsDouble )
            {
                rval = new ParserResult(Math.Compare(this.series, partialResult.Double,GT));
                return rval;
            }

            throw new NotImplementedException();
        }



        static double GT(double a, double b)
        {
            if ( a > b)
                return 1;
            return 0;
        }

        static double LT(double a, double b)
        {
            if (a < b)
                return 1;
            return 0;
        }

        internal ParserResult LessThan(ParserResult partialResult)
        {
            ParserResult rval;
            if (IsSeries && partialResult.isSeries)
            {
                rval = new ParserResult(Math.Compare(this.series, partialResult.series,LT));
                return rval;
            }
            if (IsSeries && partialResult.IsDouble)
            {
                rval = new ParserResult(Math.Compare(this.series, partialResult.Double,LT));
                return rval;
            }


            throw new NotImplementedException();
        }
    }
}
