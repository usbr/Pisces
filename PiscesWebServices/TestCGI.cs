using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace PiscesWebServices
{
    [TestFixture]    
    class TestCGI
    {
        public static void Main()
        {
            TestCGI t = new TestCGI();
            t.CsvTest();
        }

        [Test]
        public void CsvTest()
        {
            Program.Main(new string[] { "--cgi=instant", "--payload=?parameter=mabo q,mabo type,mabo wf,&syer=2015&smnth=1&sdy=18&eyer=2015&emnth=1&edy=20&format=3" });
        }

        [Test]
        public void DumpTest()
        {
            Program.Main(new string[] { "--cgi=sites" });
        }
    }
}
