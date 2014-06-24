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
    public class TestDailyMinMax
    {
        public TestDailyMinMax()
        {
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        
        [Test]
        public void MaxMin()
        {
            string fn = TestData.DataPath + "\\temp example 7 day max.xls";
            var s = new ExcelDataReaderSeries(fn, "457373", "C", "D");
            s.Read();

            Series max = Math.DailyMax(s);
            max.WriteCsv(@"c:\temp\a.csv");
            Assert.AreEqual(7, max[0].DateTime.Day);
            Assert.AreEqual(14.68, max[0].Value, 0.01);
            Assert.AreEqual(17.21, max["8/5/2004"].Value, 0.01);
            Assert.AreEqual(1965.0, max[max.Count - 1].Value, .001);

            Series min = Math.DailyMin(s);

            Assert.AreEqual(7, min[0].DateTime.Day);
            Assert.AreEqual(12.98, min[0].Value, 0.01);
            Assert.AreEqual(15.31, min["8/5/2004"].Value, 0.01);

            Assert.AreEqual(1965.0, min[min.Count - 1].Value, 0.001);
        }

    }
}
