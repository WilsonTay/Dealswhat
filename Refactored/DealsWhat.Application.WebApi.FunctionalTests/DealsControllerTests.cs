using System;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DealsWhat.Application.WebApi.FunctionalTests
{
    [TestClass]
    public class DealsControllerTests : TestBase
    {
        [TestMethod]
        public void TestMethod1()
        {
            HttpClient client = new HttpClient();

            var response = client.GetAsync("http://localhost:9000/api/deals/").Result;

            Console.WriteLine(response);
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        }
    }
}
