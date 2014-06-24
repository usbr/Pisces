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
    public class TestEstimateDailyFromMonthly
    {
        public TestEstimateDailyFromMonthly()
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
        public void EstimateDaily()
        {
            string fn = TestData.DataPath + "\\EstimateDailyFromMonthly.xlsx";
            var daily = new ExcelDataReaderSeries(fn, "daily", "Date", "Value");
            daily.Read();

            var monthly = new ExcelDataReaderSeries(fn, "monthly", "Date", "Farm_Own");
            monthly.Read();


            Series s = Math.EstimateDailyFromMonthly(daily, monthly);



        }

    }
}
