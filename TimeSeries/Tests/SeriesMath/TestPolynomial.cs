//using System;
using NUnit.Framework;
using Reclamation.TimeSeries;
using DateTime = System.DateTime;
using Console = System.Console;
namespace Pisces.NunitTests.SeriesMath
{
	/// <summary>
	/// Summary description for TestPolynomial.
	/// </summary>
	[TestFixture]
	public class TestPolynomial
	{
		public TestPolynomial()
		{
		}

    [Test]
    public void PolynomialRatingEquationListTimeConstraints()
    {
      //equation to test ( y = 2*x )
      PolynomialEquation eq1 = new PolynomialEquation(
        new double[]{0, 2.0},10, 50,"test equation");
      //equation to test ( y = 1*x )
      PolynomialEquation eq2 = new PolynomialEquation(
        new double[]{0, 1.0},10, 50,"test equation");
      Series s = new Series(null,"cfs",TimeInterval.Daily);
      s.Add(DateTime.Parse("2005-01-01"),10);
      s.Add(DateTime.Parse("2005-01-02"),20);
      s.Add(DateTime.Parse("2005-01-03"),50);
      s.Add(DateTime.Parse("2005-01-04"),60);
      s.Add(DateTime.Parse("2005-01-05"),2);

      Console.WriteLine("--- input series ----");
      s.WriteToConsole(true);
      DateTime[] t1 = { new DateTime(2005,1,1), new DateTime(2005,1,2)};
      DateTime[] t2 = { new DateTime(2005,1,1), new DateTime(2005,2,2)};

      Series sp = Math.Polynomial(s,new PolynomialEquation[] {eq1,eq2},
        t1,t2,
        DateTime.Parse("2005-01-01"),
        DateTime.Parse("2006-01-01"));
      Console.WriteLine(eq1.ToString());
      Console.WriteLine(eq2.ToString());

      Console.WriteLine("--- polynomial computed series ----");
      sp.WriteToConsole(true);

      Assert.AreEqual(5,sp.Count); // should have 4 return values

      double v = sp.Lookup(DateTime.Parse("2005-01-01"));
      Assert.AreEqual(20,v,0.00001);
      
      
      v = sp.Lookup(DateTime.Parse("2005-01-02"));
      Assert.AreEqual(20,v,0.00001);
      
      v = sp.Lookup(DateTime.Parse("2005-01-03"));
      
      Assert.AreEqual(sp[sp.LookupIndex(DateTime.Parse("2005-01-03"))].Flag,PointFlag.Computed);

      
      Assert.AreEqual(50,v,0.00001);
 
      // should return missing (null) for  jan 4 and jan 6
      
      v = sp.Lookup(DateTime.Parse("2005-01-04"));
      Assert.AreEqual(Point.MissingValueFlag,v,0.00001);
      v = sp.Lookup(DateTime.Parse("2005-01-05"));
      Assert.AreEqual(Point.MissingValueFlag,v,0.00001);
      
    }

    [Test]
    public void PolynomialRatingEquationList()
    {
      //equation to test ( y = 2*x )
      PolynomialEquation eq1 = new PolynomialEquation(
        new double[]{0, 2.0},10, 50,"test equation");
    //equation to test ( y = 1*x )
      PolynomialEquation eq2 = new PolynomialEquation(
        new double[]{0, 1.0},10, 50,"test equation");
      Series s = new Series(null,"cfs",TimeInterval.Daily);
      s.Add(DateTime.Parse("2005-01-01"),10);
      s.Add(DateTime.Parse("2005-01-02"),20);
      s.Add(DateTime.Parse("2005-01-03"),50);
      s.Add(DateTime.Parse("2005-01-04"),60);
      s.Add(DateTime.Parse("2005-01-05"),2);

      Console.WriteLine("--- input series ----");
      s.WriteToConsole(true);
      Series sp = Math.Polynomial(s,new PolynomialEquation[] {eq1,eq2},
        DateTime.Parse("2005-01-02"),
        DateTime.Parse("2006-01-01"));
      Console.WriteLine(eq1.ToString());
      Console.WriteLine(eq2.ToString());

      Console.WriteLine("--- polynomial computed series ----");
      sp.WriteToConsole(true);

      Assert.AreEqual(4,sp.Count); // should have 4 return values

      double v = sp.Lookup(DateTime.Parse("2005-01-02"));
      Assert.AreEqual(40,v,0.00001);
      
      v = sp.Lookup(DateTime.Parse("2005-01-03"));
      
      Assert.AreEqual(sp[sp.LookupIndex(DateTime.Parse("2005-01-03"))].Flag,PointFlag.Computed);

      
      Assert.AreEqual(100,v,0.00001);
 
      // should return missing (null) for  jan 4 and jan 6
      
      v = sp.Lookup(DateTime.Parse("2005-01-04"));
      Assert.AreEqual(Point.MissingValueFlag,v,0.00001);
      v = sp.Lookup(DateTime.Parse("2005-01-05"));
      Assert.AreEqual(Point.MissingValueFlag,v,0.00001);
      
    }
    [Test]
    public void PolynomialRatingEquation()
    {
      //simple equation to test ( y = 2*x )
      PolynomialEquation eq = new PolynomialEquation(
        new double[]{0, 2.0},10, 50,"test equation");
     Series s = new Series(null,"cfs",TimeInterval.Daily);
      s.Add(DateTime.Parse("2005-01-01"),10);
      s.Add(DateTime.Parse("2005-01-02"),20);
      s.Add(DateTime.Parse("2005-01-03"),50);
      s.Add(DateTime.Parse("2005-01-04"),60);
      s.Add(DateTime.Parse("2005-01-05"),2);

      Console.WriteLine("--- input series ----");
    s.WriteToConsole(true);
      Series sp = Math.Polynomial(s,eq,
        DateTime.Parse("2005-01-02"),
        DateTime.Parse("2006-01-01"));
      Console.WriteLine(eq.ToString());

      Console.WriteLine("--- polynomial computed series ----");
      sp.WriteToConsole(true);

      Assert.AreEqual(4,sp.Count); // should have 4 return values

      double v = sp.Lookup(DateTime.Parse("2005-01-02"));
      Assert.AreEqual(40,v,0.00001);
      
      v = sp.Lookup(DateTime.Parse("2005-01-03"));
      
      Assert.AreEqual(sp[sp.LookupIndex(DateTime.Parse("2005-01-03"))].Flag,PointFlag.Computed);

      
        Assert.AreEqual(100,v,0.00001);
 
      // should return missing (null) for  jan 4 and jan 6
      
      v = sp.Lookup(DateTime.Parse("2005-01-04"));
      Assert.AreEqual(Point.MissingValueFlag,v,0.00001);
      v = sp.Lookup(DateTime.Parse("2005-01-05"));
      Assert.AreEqual(Point.MissingValueFlag,v,0.00001);
      
    }

    [Test]
    public void PiecewisePolynomialRatingEquationLindCouleeWasteway1()
    {
      PolynomialEquation eq1 = new PolynomialEquation(
        new double[]{0.0},-1.0, 1.86 ,"-1 < stage <= 1.86 ");

      PolynomialEquation eq2 = new PolynomialEquation(
        new double[]{-28.4314,15.2857},1.861, 2.00," 1.86 < stage <= 2.0");

      PolynomialEquation eq3 = new PolynomialEquation(
        new double[]{-0.3522,88.1421,-96.6995,31.4217,-2.3978},2.001, 6.00," 2.0 < stage <= 6.0 ");

      PolynomialEquation eq4 = new PolynomialEquation(
        new double[]{-769.4138,249.0490},6.001, 10.00," 6.0 < stage ");
      
      PolynomialEquation[] equationList = {eq1,eq2,eq3,eq4};

      Series s  = TestData.LindCouleeWW1DailyAverageStage2004;      
      Series instant = TestData.LindCouleeWW1InstantanousStage2004;

      DateTime t1 = new DateTime(2004,1,2);
      DateTime t2 = new DateTime(2004,12,18); // at 12:00 am.. will capture 17th..not 18 th
      
      // compute polynomial based on daily average stage.
      Series p = Math.Polynomial(s,equationList,t1,t2);
      
      // compute instantanious flow first
      Series p2 = Math.Polynomial(instant,equationList,t1,t2);
      // get average second
      Series avg = Math.TimeWeightedDailyAverage(p2);

      SeriesList list = new SeriesList();
      list.Add(s);
      list.Add(p);
      list.Add(avg);
      
      list.WriteToConsole();
       //p.WriteToConsole();
    }
	}
}
