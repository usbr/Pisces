using NUnit.Framework;
using Reclamation.TimeSeries;
using DateTime = System.DateTime;
using Reclamation.TimeSeries.Excel;

namespace Pisces.NunitTests.SeriesMath
{
    /// <summary>
    /// Summary description for TestMinMax
    /// </summary>
    [TestFixture]
    public class TestMerge
    {
        public TestMerge()
        {
        }

        [Test]
        public void Merge()
        {

            Series observed = new Series();
            observed.Name = "observed";
            observed.TimeInterval = TimeInterval.Daily;

            Series estimated = new Series();
            estimated.Name = "estimated";
            estimated.TimeInterval = TimeInterval.Daily;

            DateTime t = new DateTime(2000,1,1);
            for (int i = 1; i <= 10; i++)
            {
                estimated.Add(t, i, PointFlag.Estimated);
                

                if( i >=5 && i <=8 ) // create observed data time steps 5,6,7,8
                  observed.Add(t, 100,PointFlag.None);

                t = t.AddDays(1).Date;
            }

            observed.WriteToConsole(true);
            estimated.WriteToConsole(true);
            var m = Math.Merge(observed, estimated);
            m.WriteToConsole(true);
            for (int i = 0; i < m.Count; i++)
            {

                var pt = m[i];

                if (pt.Value > 90)
                    Assert.IsTrue(pt.Flag == PointFlag.None);
                else
                    Assert.IsTrue(pt.Flag == PointFlag.Estimated);

            }


        }

    }
}
