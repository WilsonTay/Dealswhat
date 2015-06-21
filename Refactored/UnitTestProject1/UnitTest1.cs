using System;
using System.Net.Http;
using Microsoft.Owin.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication1;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        private static IDisposable webApp;

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            string baseAddress = "http://localhost:9000/";

            webApp = WebApp.Start<Startup>(url: baseAddress);
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            webApp.Dispose();
        }

        [TestMethod]
        public void TestMethod1()
        {
            HttpClient client = new HttpClient();

            var response = client.GetAsync("http://localhost:9000/api/deals").Result;

            Console.WriteLine(response);
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        }
    }
}
