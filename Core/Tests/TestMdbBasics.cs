using System;
using NUnit.Framework;
using Reclamation.Core;
using System.Data;
using System.IO;

namespace Reclamation.Core.Tests
{

    [TestFixture]
    public class TestMdbBasics
    {
        public TestMdbBasics()
        {
          
        }

        [Test, Category("Windows")]
        public void TestTable()
        {
            string mdbFilename = Path.Combine(Globals.TestDataPath, "Water Quality Testing.mdb");
            AccessDB a = new AccessDB(mdbFilename);

           DataTable tbl =  a.Table("Water");
           Assert.AreEqual(5880, tbl.Rows.Count);
          // Console.WriteLine(tbl.Rows.Count);
        }


       
    }
}
