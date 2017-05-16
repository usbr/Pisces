using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Reclamation.TimeSeries.IDWR;
using RestSharp;

namespace Pisces.NunitTests.SeriesTypes
{
    [TestFixture]
    public class TestIdwr
    {
        [Test]
        public void TestIdwrApiService()
        {
            var request = new RestRequest("SiteDetails?", Method.GET);
            request.AddParameter("sitelist", "13185000");
            //{"SiteID":13185000,"SiteType":"F","SiteName":"BOISE RIVER NEAR TWIN SPRINGS","HSTCount":32,"ALCCount":20}
            IRestResponse restResponse = IDWRDailySeries.idwrClient.Execute(request);
            Assert.AreEqual(200, (int)restResponse.StatusCode);
        }



    }
}
