using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using Autofac;
using Autofac.Integration.WebApi;
using DealsWhat.Application.WebApi.Controllers;
using DealsWhat.Domain.Interfaces;
using DealsWhat.Domain.Model;
using DealsWhat.Domain.Services;
using DealsWhat.Domain.Test.Common;
using Microsoft.Owin.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace DealsWhat.Application.WebApi.FunctionalTests
{
    [TestClass]
    public class CartControllerTests : TestBase
    {
        private IFixture fixture;

        [TestInitialize]
        public void TestInitialize()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());
        }

        [TestMethod]
        public void GetCarts_BasedOnUserIdentity()
        {
            var email = "test@test.com";
            var user = TestModelFactory.CreateUser(emailAddress: email);
            var users = new List<UserModel>();
            users.Add(user);

            var mockedContext = new Mock<HttpContextBase>();
            mockedContext.Setup(m => m.Request.IsAuthenticated).Returns(true);

            var controller = new CartController();
            controller.User = new GenericPrincipal(new GenericIdentity(email), new string[] { });

            // Find by email address.-
            controller.Get();
            //HttpContext.Current = new HttpContext(new HttpRequest(null, "http://tempuri.org", null), new HttpResponse(null));

            //HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(email), new string[] { });

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(email), new string[] { });
            var response = CreateWebApiServiceAndGetResponse(users, "http://localhost:9000/api/cart");
        }

        private static string CreateWebApiServiceAndGetResponse(
         IList<UserModel> users,
         string endpoint)
        {
            var response = "";

            using (var service = CreateWebApiService(users))
            {
                HttpClient client = new HttpClient();

                var result = client.GetAsync(endpoint).Result;

                using (var reader = new StreamReader(result.Content.ReadAsStreamAsync().Result))
                {
                    response = reader.ReadToEnd();
                }
            }

            return response;
        }

        private static IDisposable CreateWebApiService(
         IList<UserModel> users)
        {
            var userRepository = new FakeUserRepository(users);

            var builder = new ContainerBuilder();

            builder.RegisterInstance<IRepositoryFactory>(new FakeRepositoryFactory(userRepository: userRepository));
            builder.RegisterApiControllers(typeof(CartController).Assembly);

            builder.RegisterType<CartService>().As<ICartService>();

            var container = builder.Build();

            var resolver = new AutofacWebApiDependencyResolver(container);
            WebApiContext.DefaultResolver = resolver;

            var baseAddress = "http://localhost:9000/";

            return WebApp.Start<Startup>(url: baseAddress);
        }
    }
}
