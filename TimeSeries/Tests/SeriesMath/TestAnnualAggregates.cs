using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Reclamation.TimeSeries;
using Reclamation;
using Math = Reclamation.TimeSeries.Math;
using Reclamation.Core;

namespace Pisces.NunitTests.SeriesMath
{
    [TestFixture]
    public class TestAnnualAggregates
    {
        /// <summary>
        /// use Specification DataSet 'MonthValue' column
        /// </summary>
        [Test]
        public void AnnualNarrowRangeFeb1Only()
        {
            Series s = TestData.SpecificationMonthValue;
            DateTime t1 = new DateTime(2003, 10, 1);
            DateTime t2 = new DateTime(2004, 9, 30);
            s.Read(t1, t2);
            Console.WriteLine(s.Count);
            Assert.AreEqual(366, s.Count);

            MonthDayRange rng = new MonthDayRange(2, 1, 2, 1);
            s = Reclamation.TimeSeries.Math.AnnualMax(s, rng, 10);

            Assert.AreEqual(1, s.Count);

            Assert.AreEqual(2, s[0].Value, "AnnualMax test");


            s = Reclamation.TimeSeries.Math.AnnualMin(s, rng, 10);

            Assert.AreEqual(1, s.Count);

            Assert.AreEqual(2, s[0].Value, "AnnualMin test");
        }


        [Test]
        public void AnnualSumJanuaryThroughJuly()
        {
            string fn = TestData.DataPath + "\\heii_qd.csv";

            TextSeries ts = new TextSeries(fn);
            ts.Read();
            Console.WriteLine(ts.Count);

            Assert.AreEqual(28905, ts.Count, fn + " has been modified");
            MonthDayRange rng = new MonthDayRange(1, 1, 7, 31);
            Series sum = Reclamation.TimeSeries.Math.AnnualSum(ts, rng,10);
            sum.WriteToConsole();
            Assert.AreEqual(1928, sum[0].DateTime.Year);
            Assert.AreEqual(2293240, sum[0].Value);

            Series min = Math.AnnualMin(ts,rng, 10);
            Series max = Math.AnnualMax(ts, rng,10);

        }

        /*
         * 
         DAILY VALUES SUMMATION          PROCESS DATE: 20-NOV-06
 
 Station name        HEII               Begin and end year  1928-2007
 Parameter code      QD                 Begin and end date  JAN  1-JUL 31
 Option              VOL
 
          BY YEAR                RANKED
 ------------------------  ------------------
 YEAR     SUMMATION  MISS  YEAR     SUMMATION
 
 1928    2293240.00     0  1997    3322440.00
 1929    1504780.00     0  1986    2851130.00
 1930    1509300.00     0  1996    2753980.00
 1931    1066600.00     0  1971    2750350.00
 1932    1526910.00     0  1974    2559210.00
 1933    1467350.00     0  1956    2500290.00
 1934    1052350.00     0  1972    2424620.00
 1935    1391450.00     0  1943    2390440.00
 1936    1917680.00     0  1983    2382440.00
 1937    1333160.00     0  1984    2371640.00
 1938    1817670.00     0  1982    2356390.00
 1939    1526020.00     0  1976    2338730.00
 1940    1211930.00     0  1965    2310170.00
 1941    1222940.00     0  1951    2300810.00
 1942    1446270.00     0  1928    2293240.00
 1943    2390440.00     0  1999    2282980.00
 1944    1406760.00     0  1952    2227440.00
 1945    1515230.00     0  1998    2191770.00
 1946    2035550.00     0  1950    2136600.00
 1947    1855420.00     0  1946    2035550.00
 1948    1823370.00     0  1975    1973520.00
 1949    1749090.00     0  1936    1917680.00
 1950    2136600.00     0  1985    1915970.00
 1951    2300810.00     0  1947    1855420.00
 1952    2227440.00     0  1979    1838690.00
 1953    1566610.00     0  1969    1835660.00
 1954    1760850.00     0  1948    1823370.00
 1955    1349440.00     0  1938    1817670.00
 1956    2500290.00     0  1980    1790680.00
 1957    1590720.00     0  1964    1780300.00
 1958    1487730.00     0  1978    1764870.00
 1959    1359600.00     0  1954    1760850.00
 1960    1338460.00     0  1949    1749090.00
 1961    1295170.00     0  2006    1721171.63
 1962    1494530.00     0  1970    1712960.00
 1963    1533552.00     0  2000    1697310.00
 1964    1780300.00     0  1966    1611840.00
 1965    2310170.00     0  1981    1594330.00
 1966    1611840.00     0  1957    1590720.00
 1967    1495650.00     0  1968    1589010.00
 1968    1589010.00     0  1953    1566610.00
 1969    1835660.00     0  1963    1533552.00
 1970    1712960.00     0  1932    1526910.00
 1971    2750350.00     0  1939    1526020.00
 1972    2424620.00     0  1994    1525530.00
 1973    1520250.00     0  1973    1520250.00
 1974    2559210.00     0  1945    1515230.00
 1975    1973520.00     0  1930    1509300.00
 1976    2338730.00     0  1929    1504780.00
 1977    1290146.00     0  1967    1495650.00
 1978    1764870.00     0  1962    1494530.00
 1979    1838690.00     0  1958    1487730.00
 1980    1790680.00     0  1995    1482720.00
 1981    1594330.00     0  1987    1474930.00
 1982    2356390.00     0  1933    1467350.00
 1983    2382440.00     0  1942    1446270.00
 1984    2371640.00     0  1944    1406760.00
 1985    1915970.00     0  1935    1391450.00
 1986    2851130.00     0  1990    1384800.00
 1987    1474930.00     0  1992    1379060.00
 1988    1269010.00     0  1959    1359600.00
 1989    1286730.00     0  2003    1359390.00
 1990    1384800.00     0  1955    1349440.00
 1991    1192490.00     0  1960    1338460.00
 1992    1379060.00     0  1937    1333160.00
 1993    1213100.00     0  2004    1299637.38
 1994    1525530.00     0  1961    1295170.00
 1995    1482720.00     0  1977    1290146.00
 1996    2753980.00     0  1989    1286730.00
 1997    3322440.00     0  1988    1269010.00
 1998    2191770.00     0  1941    1222940.00
 1999    2282980.00     0  2001    1218220.00
 2000    1697310.00     0  1993    1213100.00
 2001    1218220.00     0  1940    1211930.00
 2002    1126530.00     0  1991    1192490.00
 2003    1359390.00     0  2005    1145953.63
 2004    1299637.38     0  2002    1126530.00
 2005    1145953.63     0  1931    1066600.00
 2006    1721171.63     0  1934    1052350.00
 2007     998877.00   212
 
 
 Average of  79 years:             1739134.75         
         */
        [Test]
        public void AnnualSumFullWaterYear()
        {

            // using RIR QD from hydromet 
            // compare to check program
            DataTable checkProgram = new DataTable();
            checkProgram.Columns.Add("Year",typeof(int));
            checkProgram.Columns.Add("Sum",typeof(double));
            checkProgram.Columns.Add("missing",typeof(int));
            checkProgram.Columns.Add("maxDate", typeof(DateTime));
            checkProgram.Columns.Add("max", typeof(double));
            checkProgram.Columns.Add("minDate", typeof(DateTime));
            checkProgram.Columns.Add("min", typeof(double));
//            Year	sum	missing	Maxdate	max	MinDate	min




            checkProgram.Rows.Add(1963, 32137.7, 0, DateTime.Parse("2/03/2006"), 700, DateTime.Parse("8/30/2006"), 6.1);
            checkProgram.Rows.Add(1964, 63817.0, 0, DateTime.Parse("5/15/2006"), 1570, DateTime.Parse("10/05/2006"), 17);
            checkProgram.Rows.Add(1965, 77084.0, 0, DateTime.Parse("4/24/2006"), 1740, DateTime.Parse("10/01/2006"), 39);
            checkProgram.Rows.Add(1966, 34863.0, 0, DateTime.Parse("4/18/2006"), 628, DateTime.Parse("8/12/2006"), 10);
            checkProgram.Rows.Add(1967, 39152.0, 0, DateTime.Parse("5/11/2006"), 1090, DateTime.Parse("8/28/2006"), 17);
            checkProgram.Rows.Add(1968, 41857.0, 0, DateTime.Parse("5/03/2006"), 814, DateTime.Parse("11/28/2006"), 21);
            checkProgram.Rows.Add(1969, 69656.0, 0, DateTime.Parse("4/25/2006"), 2300, DateTime.Parse("11/27/2006"), 27);
            checkProgram.Rows.Add(1970, 56442.0, 0, DateTime.Parse("5/19/2006"), 1820, DateTime.Parse("12/06/2006"), 23);
            checkProgram.Rows.Add(1971, 102310.0, 0, DateTime.Parse("5/04/2006"), 2660, DateTime.Parse("1/07/2006"), 24);
            checkProgram.Rows.Add(1972, 92143.0, 0, DateTime.Parse("5/09/2006"), 1510, DateTime.Parse("1/04/2006"), 44);
            checkProgram.Rows.Add(1973, 61642.0, 0, DateTime.Parse("5/06/2006"), 1320, DateTime.Parse("8/31/2006"), 36);
            checkProgram.Rows.Add(1974, 76544.5, 0, DateTime.Parse("4/26/2006"), 2170, DateTime.Parse("8/12/2006"), 1.2);
            checkProgram.Rows.Add(1975, 78953.0, 0, DateTime.Parse("5/20/2006"), 2290, DateTime.Parse("5/07/2006"), 23);
            checkProgram.Rows.Add(1976, 87413.1, 0, DateTime.Parse("5/08/2006"), 1520, DateTime.Parse("1/06/2006"), 0.27);
            checkProgram.Rows.Add(1977, 14110.9, 0, DateTime.Parse("10/28/2006"), 310, DateTime.Parse("2/22/2006"), 8.7);
            checkProgram.Rows.Add(1978, 22459.3, 0, DateTime.Parse("8/03/2006"), 206, DateTime.Parse("11/30/2006"), 4.5);
            checkProgram.Rows.Add(1979, 33610.8, 0, DateTime.Parse("9/19/2006"), 396, DateTime.Parse("1/09/2006"), 0.1);
            checkProgram.Rows.Add(1980, 37074.0, 0, DateTime.Parse("6/04/2006"), 415, DateTime.Parse("1/01/2006"), 0);
            checkProgram.Rows.Add(1981, -999.0, 3, DateTime.Parse("10/10/2006"), 503, DateTime.Parse("12/10/2006"), 0.73);
            checkProgram.Rows.Add(1982, 87835.9, 0, DateTime.Parse("5/20/2006"), 1284.11, DateTime.Parse("2/24/2006"), 0);
            checkProgram.Rows.Add(1983, 98116.5, 0, DateTime.Parse("5/27/2006"), 1320.13, DateTime.Parse("12/30/2006"), 0);
            checkProgram.Rows.Add(1984, 114705.8, 0, DateTime.Parse("5/22/2006"), 1720.05, DateTime.Parse("2/16/2006"), 0);
            checkProgram.Rows.Add(1985, -999.0, 1, DateTime.Parse("4/19/2006"), 729.47, DateTime.Parse("3/31/2006"), 0);
            checkProgram.Rows.Add(1986, 82530.0, 0, DateTime.Parse("5/07/2006"), 1230, DateTime.Parse("11/22/2006"), 0);
            checkProgram.Rows.Add(1987, 34610.1, 0, DateTime.Parse("10/11/2006"), 592, DateTime.Parse("12/11/2006"), 0);
            checkProgram.Rows.Add(1988, 20880.3, 0, DateTime.Parse("9/23/2006"), 847, DateTime.Parse("11/17/2006"), 0);
            checkProgram.Rows.Add(1989, 29083.8, 0, DateTime.Parse("9/23/2006"), 871, DateTime.Parse("11/02/2006"), 0);
            checkProgram.Rows.Add(1990, 20569.0, 0, DateTime.Parse("10/01/2006"), 807, DateTime.Parse("11/02/2006"), 0);
            checkProgram.Rows.Add(1991, 16571.4, 0, DateTime.Parse("9/24/2006"), 571, DateTime.Parse("11/23/2006"), 0);
            checkProgram.Rows.Add(1992, 26423.0, 0, DateTime.Parse("10/05/2006"), 872, DateTime.Parse("11/01/2006"), 0);
            checkProgram.Rows.Add(1993, 42308.4, 0, DateTime.Parse("9/29/2006"), 773, DateTime.Parse("11/03/2006"), 0);
            checkProgram.Rows.Add(1994, 35480.0, 0, DateTime.Parse("8/05/2006"), 750, DateTime.Parse("11/01/2006"), 0);
            checkProgram.Rows.Add(1995, 28779.0, 0, DateTime.Parse("5/26/2006"), 478, DateTime.Parse("11/01/2006"), 0);
            checkProgram.Rows.Add(1996, 51189.0, 0, DateTime.Parse("5/20/2006"), 888, DateTime.Parse("11/02/2006"), 0);
            checkProgram.Rows.Add(1997, 107622.1, 0, DateTime.Parse("5/07/2006"), 1750, DateTime.Parse("11/22/2006"), 0);
            checkProgram.Rows.Add(1998, 72138.0, 0, DateTime.Parse("5/24/2006"), 745, DateTime.Parse("11/13/2006"), 0);
            checkProgram.Rows.Add(1999, 64000.0, 0, DateTime.Parse("5/06/2006"), 885, DateTime.Parse("11/14/2006"), 0);
            checkProgram.Rows.Add(2000, 29604.0, 0, DateTime.Parse("10/06/2006"), 414, DateTime.Parse("11/30/2006"), 0);
            checkProgram.Rows.Add(2001, 28381.0, 0, DateTime.Parse("8/18/2006"), 429, DateTime.Parse("11/16/2006"), 0);
            checkProgram.Rows.Add(2002, 11817.0, 0, DateTime.Parse("8/29/2006"), 455, DateTime.Parse("10/01/2006"), 0);
            checkProgram.Rows.Add(2003, 14270.0, 0, DateTime.Parse("8/26/2006"), 439, DateTime.Parse("10/01/2006"), 0);
            checkProgram.Rows.Add(2004, 16328.2, 0, DateTime.Parse("8/31/2006"), 348.2, DateTime.Parse("10/01/2006"), 0);
            checkProgram.Rows.Add(2005, 11442.2, 0, DateTime.Parse("9/21/2006"), 202.02, DateTime.Parse("10/13/2006"), 0);
            checkProgram.Rows.Add(2006, -999.0, 91, DateTime.Parse("5/02/2006"), 1119.98, DateTime.Parse("11/22/2006"), 0);

            string fn = TestData.DataPath + @"\rire hydromet daily.txt";
            TextSeries ts = new TextSeries(fn);
            ts.Read();

            Assert.AreEqual(16814, ts.Count, fn + " has been modified");

            MonthDayRange rr = new MonthDayRange(10, 1, 9, 30);
            Series sum = Math.AnnualSum( ts,rr, 10);
            Series min = Math.AnnualMin(ts,rr, 10);
            Series max = Math.AnnualMax(ts,rr, 10);

            Assert.AreEqual(sum.TimeInterval, TimeInterval.Yearly);
            Assert.AreEqual(min.TimeInterval, TimeInterval.Yearly);
            Assert.AreEqual(max.TimeInterval, TimeInterval.Yearly);

            Assert.IsTrue(sum.Count == min.Count && sum.Count == max.Count, "counts for sum,min,max are not the same");

            for (int i = 0; i < sum.Count; i++)
            {
                Point ptSum = sum[i];
                Point ptMin = min[i];
                Point ptMax = max[i];
                int yr = ptSum.DateTime.Year;

                DataRow[] rows = checkProgram.Select("Year=" + yr);
                if (rows.Length > 0)
                {
                    double dsum = Convert.ToDouble(rows[0]["Sum"]);
                    double dmin = Convert.ToDouble(rows[0]["min"]);
                    double dmax = Convert.ToDouble(rows[0]["max"]);
                    int numMissing = Convert.ToInt32(rows[0]["missing"]);

                    Console.WriteLine("yr = " + yr + " sum = '" + sum+"'");
                    if (dsum != Point.MissingValueFlag)
                    { // we compute sum anyway, but flag it.
                        Assert.AreEqual(dsum, ptSum.Value, 0.25, "sum different in year " + yr);
                    }
                    if(numMissing ==0)
                    {
                        Assert.AreEqual("", ptSum.Flag);
                    }
                    else
                    {
                        Assert.AreEqual(numMissing+" missing", ptSum.Flag);
                    }
                    Assert.AreEqual(dmin, ptMin.Value, 0.01, "min different in year " + yr);
                    Assert.AreEqual(dmax, ptMax.Value, 0.01, "max different in year " + yr);
                }
            }
            //min.WriteToConsole();
            //max.WriteToConsole();




        }

        /*
         * CHECK > run


                MAX/MIN SUMMARY          PROCESS DATE: 18-JUL-06

    Station name        RIR                Begin and end year  1963-2006
    Parameter code      QD                 Begin and end date  OCT  1-SEP 30
    Option              MAX

    YEAR   DATE        MAXIMUM     DATE        MINIMUM  MISS

    1963  FEB  3        700.00    AUG 30          6.10     0
    1964  MAY 15       1570.00    OCT  5         17.00     0
    1965  APR 24       1740.00    OCT  1         39.00     0
    1966  APR 18        628.00    AUG 12         10.00     0
    1967  MAY 11       1090.00    AUG 28         17.00     0
    1968  MAY  3        814.00    NOV 28         21.00     0
    1969  APR 25       2300.00    NOV 27         27.00     0
    1970  MAY 19       1820.00    DEC  6         23.00     0
    1971  MAY  4       2660.00    JAN  7         24.00     0
    1972  MAY  9       1510.00    JAN  4         44.00     0
    1973  MAY  6       1320.00    AUG 31         36.00     0
    1974  APR 26       2170.00    AUG 12          1.20     0
    1975  MAY 20       2290.00    MAY  7         23.00     0
    1976  MAY  8       1520.00    JAN  6          0.27     0
    1977  OCT 28        310.00    FEB 22          8.70     0
    1978  AUG  3        206.00    NOV 30          4.50     0
    1979  SEP 19        396.00    JAN  9          0.10     0
    1980  JUN  4        415.00    JAN  1          0.00     0
    1981  OCT 10        503.00    DEC 10          0.73     3
    1982  MAY 20       1284.11    FEB 24          0.00     0
    1983  MAY 27       1320.13    DEC 30          0.00     0
    1984  MAY 22       1720.05    FEB 16          0.00     0
    1985  APR 19        729.47    MAR 31          0.00     1
    1986  MAY  7       1230.00    NOV 22          0.00     0
    1987  OCT 11        592.00    DEC 11          0.00     0
    1988  SEP 23        847.00    NOV 17          0.00     0
    1989  SEP 23        871.00    NOV  2          0.00     0
    1990  OCT  1        807.00    NOV  2          0.00     0
    1991  SEP 24        571.00    NOV 23          0.00     0
    1992  OCT  5        872.00    NOV  1          0.00     0
    1993  SEP 29        773.00    NOV  3          0.00     0
    1994  AUG  5        750.00    NOV  1          0.00     0
    1995  MAY 26        478.00    NOV  1          0.00     0
    1996  MAY 20        888.00    NOV  2          0.00     0
    1997  MAY  7       1750.00    NOV 22          0.00     0
    1998  MAY 24        745.00    NOV 13          0.00     0
    1999  MAY  6        885.00    NOV 14          0.00     0
    2000  OCT  6        414.00    NOV 30          0.00     0
    2001  AUG 18        429.00    NOV 16          0.00     0
    2002  AUG 29        455.00    OCT  1          0.00     0
    2003  AUG 26        439.00    OCT  1          0.00     0
    2004  AUG 31        348.20    OCT  1          0.00     0
    2005  SEP 21        202.02    OCT 13          0.00     0
    2006  MAY  2       1119.98    NOV 22          0.00    75

   Avg of 44 yrs       1010.95                    6.88
         * */

        /// <summary>
        /// Test annual sum and compare to check program
        /// VOLAF option for heise unregulated flow
        /// </summary>
        public void Vol_AF_HEII()
        {
            //heii_quAF.csv
            string fn = TestData.DataPath + @"\heii_quAF.csv";
            TextSeries ts = new TextSeries(fn);
            ts.Read();

            Assert.AreEqual(28916, ts.Count,"file has changed");
            MonthDayRange r = new MonthDayRange(3,12,5,1);
            Series sum = Math.AnnualSum(ts, r, 10);
            double[] expected = {380313.47,	424559.03,	476518.66,	522272.94,	451835.38,	702204.13};

//            sum.WriteToConsole();
            for (int yr = 2001; yr <= 2006; yr++)
			{
			 DateTime t1 = new DateTime(yr,1,1);
             DateTime t2 = new DateTime(yr,12,31);
             Series sYear =Math.Subset(sum, t1, t2);
             Assert.AreEqual(expected[yr-2001], sYear[0].Value,3);
			}
            
        }
        /*
                     DAILY VALUES SUMMATION          PROCESS DATE: 20061130

  Station name        HEII               Begin and end year  2001-2006
  Parameter code      QU                 Begin and end date  MAR 12-MAY  1
  Option              VOLAF     

           BY YEAR                RANKED
  ------------------------  ------------------
  YEAR     SUMMATION  MISS  YEAR     SUMMATION

  2001     380313.47     0  2006     702204.13
  2002     424559.03     0  2004     522272.94
  2003     476518.66     0  2003     476518.66
  2004     522272.94     0  2005     451835.38
  2005     451835.38     0  2002     424559.03
  2006     702204.13     0  2001     380313.47

  Average of   6 years:              492950.63

         */
    }
}
