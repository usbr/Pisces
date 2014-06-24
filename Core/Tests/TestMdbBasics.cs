using System;
using NUnit.Framework;
using Reclamation.Core;
using System.Data;

namespace Reclamation.Core.Tests
{

    [TestFixture]
    public class TestMdbBasics
    {
        public TestMdbBasics()
        {
          
        }

        [Test]
        public void TestTable()
        {
            string mdbFilename = Globals.TestDataPath    + "\\Water Quality Testing.mdb";
            AccessDB a = new AccessDB(mdbFilename);

           DataTable tbl =  a.Table("Water");
           Assert.AreEqual(5880, tbl.Rows.Count);
          // Console.WriteLine(tbl.Rows.Count);
        }


       
    }
}
