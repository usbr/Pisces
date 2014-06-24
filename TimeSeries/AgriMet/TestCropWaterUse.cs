using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Reclamation.TimeSeries.AgriMet;
using NUnit.Framework;

namespace ReclamationTesting.AgriMet
{
    /// <summary>
    /// Summary description for CropWaterUse
    /// </summary>
    [TestClass]
    public class TestCropWaterUse
    {
        public TestCropWaterUse()
        {
            //
            // TODO: Add constructor logic here
            //
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

        [TestMethod]
        public void WaterUse()
        {

            
           var data = EstimatedCropWaterUse.CreateReport("PMAI", DateTime.Now.Date);

            // 113 = April 23.
            //var et = .17;
            // 74 = march 5
           double[] kc_values = CropCurve.ReadCoefficients(50);
            var kc = CropCurve.CropCoefficient(kc_values, 113, 64, 64, 283);
            Assert.AreEqual(1, kc);
             kc_values = CropCurve.ReadCoefficients(1);
            kc = CropCurve.CropCoefficient(kc_values, 113, 74, 135, 283);
            Assert.AreEqual(0.9393, kc,.0001);
            

            //Assert.AreEqual(


        }
    }
}
