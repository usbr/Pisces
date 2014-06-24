using System;
using System.Data;
using System.Text;

namespace Reclamation.TimeSeries
{
	/// <summary>
	/// PolynomialEquation used when graphing
	/// rating equations along side individual measurements.
	/// or when plotting multiple rating equations.
	/// Also used to generate hard-copy rating table.
	/// </summary>
	public class PolynomialEquation
	{
    double[] A;
    double min,max;
    string name;
    public string Title="";
    public PolynomialEquation(double[] coefficients,double min,double max,
      string name)
    {
      A = coefficients;
      this.max=max;
      this.min=min;
      this.name=name;
    }
    

    /// <summary>
    /// returns SQL command of this equation
    /// </summary>
    /// <param name="parameterName"></param>
    /// <returns></returns>
    public string SQL(string parameterName)
    {
      return SQL(parameterName,0);

    }
    /// <summary>
    /// returns SQL command of this equation
    /// example:  -582.52 + 91.83 * (value - 1300) 
    /// +8.15 * power((value - 1300),2)
    /// </summary>
    /// <returns></returns>
    public string SQL(string parameterName, double offset)
    {
      if( A.Length == 0)
        return "";

      bool anyOffset = System.Math.Abs(offset) > 0.0001;
      string fmt = "F4";
      string sql = A[0].ToString(fmt);

      if( A.Length >1)
      {
        sql += " + " + A[1].ToString(fmt)+" * ("+parameterName;
        if( anyOffset)
        {
         sql += " + "+offset.ToString(fmt) + ")";
        }
        else
        {
        sql +=  " ) ";
        }
      }
      for(int i = 2; i < this.A.Length; i++)
      {
        sql += " + "+A[i].ToString(fmt);
        if( anyOffset)
        {
          sql += " * Power((value + "+offset.ToString(fmt)+"),"+i+") ";
        }
        else
        {
          sql += " * Power(value,"+i+") ";
        }

      }

      return sql;
    }

    public string EquationText()
    {
      double[] coef = A;
       int polynomialDegree = A.Length-1;
      string varName="Stage";
      string t="";
      string fmt = "F2";// "#0.0###";  //"+#0.0###;#0.0###";
      t = coef[0].ToString(fmt);
      if(coef[1] >= 0)
        t = t + "+ ";
      t = t + coef[1].ToString(fmt) + varName+" ";
      if( polynomialDegree<2)
        return t;
      if(coef[2] >= 0)
        t = t + "+ ";
      t = t + coef[2].ToString(fmt) + varName+"^2 ";
      if( polynomialDegree<3)
        return t;
      if(coef[3] >= 0)
        t = t + "+ ";
      t = t + coef[3].ToString(fmt) + varName+"^3 ";
      if( polynomialDegree<4)
        return t;
      if(coef[4] >= 0)
        t = t + "+ ";
      t = t + coef[4].ToString(fmt) + varName+"^4 ";
      return t;
    }

   
    public double Min { get{ return this.min;}}
    public double Max { get{ return this.max;}}

    public string Name { get {return this.name;}}


    public double Eval(double x)
    {
        return Evaluate(this.A,x);
    }
    public static double Evaluate(double[] A,double x)
    {
      double rval = A[0];
      for(int i=1; i<A.Length; i++)
      {
        rval += A[i]*System.Math.Pow(x,i);
      }
      return rval;
    }


    public static DataTable Table(PolynomialEquation eq, double min,
      double max, double inc, string title)
    {
      DataTable table = new DataTable("rating");

      for(int i=0; i<4; i++) // 4 pairs of GateHeight,Flow
      {
        string colName = "GH" + i;
        table.Columns.Add(colName,typeof(double));
        colName = "Q" + i;
        table.Columns.Add(colName,typeof(double));
      }

      int numEntries = Convert.ToInt32((max - min)/inc + 2);
      int numPerColumn =51; //(int)Math.Ceiling((double)numEntries /(double)columnCount);
      
      int colIndex =0;
      int rowIndex = 0;
      int pageCounter =0;
      for(double gh =min; gh<=max; gh +=  inc )
      {
        if( rowIndex >= table.Rows.Count)
        {
          table.Rows.Add(table.NewRow());
        }
         table.Rows[rowIndex][colIndex] = gh;
         table.Rows[rowIndex][colIndex+1] = eq.Eval(gh);

        rowIndex++;
        if( rowIndex >= numPerColumn*(pageCounter+1))
        {
          if( colIndex == 6) // new page.
          {
            colIndex = 0;
            //rowIndex keep going..
            pageCounter ++;
          }
          else
          {
            rowIndex -= numPerColumn;
            colIndex += 2;
          }
        }
      }
      return table;
    }

    public override string ToString()
    {
      return this.EquationText();
    }

	}
}
