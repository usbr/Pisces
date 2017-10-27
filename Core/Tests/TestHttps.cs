using System;
using NUnit.Framework;
using System.IO;
using System.Net;

namespace Reclamation.Core.Tests
{
    [TestFixture]
    public class TestHttps
    {
        [Test]
        public void usbr_http()
        {
          


          var fn  = FileUtility.GetTempFileName(".usbr.html");
           Web.GetFile("https://www.usbr.gov/",fn); // fails
        }
        [Test]
        public void google_http()
        {
            var fn = FileUtility.GetTempFileName(".google.html");
            Web.GetFile("https://www.google.com/", fn);  // Works
        }

    }
}
