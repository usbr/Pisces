using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;
using Reclamation.Core;

namespace Reclamation.Core.Tests
{
    [TestFixture]
    public class TestDbaseIV
    {

        [Test, Category("Windows")]
        public void Test()
        {
            string fn = Path.Combine(Globals.TestDataPath, "a.dbf");
            var tbl = DbaseIV.Read(fn);
            Console.WriteLine(tbl.Rows.Count);
        }
        [Test, Category("Windows")]
        public void TestLongFileName()
        {
            string fn = Path.Combine(Globals.TestDataPath, "streamreaches.dbf");
            var tbl = DbaseIV.Read(fn);
            Console.WriteLine(tbl.Rows.Count);
        }
    }
}
