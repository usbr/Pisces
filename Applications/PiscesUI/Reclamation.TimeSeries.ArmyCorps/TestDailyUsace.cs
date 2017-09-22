using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Reclamation.TimeSeries.Usace;

namespace Pisces.NunitTests.SeriesTypes
{
    [TestFixture]    
    public class TestDailyUsace
    {

        public TestDailyUsace()
        {

        }


        [Test]
        public void ChiefJosephTDG()
        {
            UsaceSeries s = new UsaceSeries("//CHJ/YT//IR-MONTH/IRVZZBZD/");
            s.Read(DateTime.Now.AddDays(-5).Date, DateTime.Now.Date);
            Assert.IsTrue(s.Count>3);
            s.WriteToConsole();
        }

        [Test]
        public void SimpleWebRequest()
        {
            string path = "//DWR/HF//IR-MONTH/DRXZZAZD/"; //+pk:dworshack";
               
            UsaceSeries s = new UsaceSeries(path);
            s.Read(DateTime.Now.AddDays(-5).Date, DateTime.Now.Date);


            Assert.IsTrue(s.Count >= 4);
            s.WriteToConsole();
        }
    }
}
