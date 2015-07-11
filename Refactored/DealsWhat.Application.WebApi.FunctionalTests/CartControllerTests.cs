using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using Autofac;
using Autofac.Integration.WebApi;
using DealsWhat.Application.WebApi.Controllers;
using DealsWhat.Domain.Interfaces;
using DealsWhat.Domain.Model;
using DealsWhat.Domain.Services;
using DealsWhat.Domain.Test.Common;
using FluentAssertions;
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
        public void GetCarts_RetrieveUserByIdentity()
        {
            var email = "test@test.com";
            var user = TestModelFactory.CreateUser(emailAddress: email);
            var users = new List<UserModel>();
            users.Add(user);

            var mockedUserRepository = new Mock<IUserRepository>();
            mockedUserRepository.Setup(m => m.FindByEmailAddress(email)).Returns(user);

            var repositoryFactory = new FakeRepositoryFactory(userRepository: mockedUserRepository.Object);
            var cartService = new CartService(repositoryFactory);
            var controller = new CartController(cartService);
            controller.User = new GenericPrincipal(new GenericIdentity(email), new string[] { });

            controller.Get();

            mockedUserRepository.Verify(m => m.FindByEmailAddress(email), Times.Once);
        }

        [TestMethod]
        public void GetCarts_BasedOnUserIdentity()
        {
            var email = "test@test.com";
            var user = TestModelFactory.CreateUser(emailAddress: email);
            var cartItem = TestModelFactory.CreateCartItem();
            user.AddToCart(cartItem);

            var users = new List<UserModel>();
            users.Add(user);

            var controller = CreateCartController(users);
            SetupControllerWithIdentity(controller, email);

            var returnedModel = controller.Get();

            returnedModel.Count().ShouldBeEquivalentTo(1);

            var firstCartItem = returnedModel.First();
            AssertModelEquality(firstCartItem, cartItem);
        }

        [Ignore]
        [TestMethod]
        public void GetCarts_WrongIdentity404Expected()
        {
            var email = "test@test.com";
            var user = TestModelFactory.CreateUser(emailAddress: email);
            var cartItem = TestModelFactory.CreateCartItem();
            user.AddToCart(cartItem);

            var users = new List<UserModel>();
            users.Add(user);

            var controller = CreateCartController(users);
            SetupControllerWithIdentity(controller, "anotheremail@email.com");

            try
            {
                controller.Get();
            }
            catch (HttpResponseException ex)
            {
                ex.Response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.NotFound);
            }
        }

        [TestMethod]
        public void AddCart_RetrieveUserBasedOnIdentity()
        {
            var email = "test@test.com";
            var user = TestModelFactory.CreateUser(emailAddress: email);
            var users = new List<UserModel>();
            users.Add(user);

            var mockedUserRepository = new Mock<IUserRepository>();
            mockedUserRepository.Setup(m => m.FindByEmailAddress(email)).Returns(user);

            var deal = TestModelFactory.CreateCompleteDeal();
            var dealOption = deal.Options.First();
            var dealAttributes = dealOption.Attributes.Select(a => a.Key.ToString()).ToList();
            var dealRepository = new FakeDealRepository(new List<DealModel>() { deal });

            var repositoryFactory = new FakeRepositoryFactory(userRepository: mockedUserRepository.Object, dealRepository: dealRepository);
            var cartService = new CartService(repositoryFactory);
            var controller = new CartController(cartService);
            controller.User = new GenericPrincipal(new GenericIdentity(email), new string[] { });

            var newCartItem = CreateNewCartItem(deal.Key.ToString(), dealOption.Key.ToString(), dealAttributes);
            controller.Post(newCartItem);

            mockedUserRepository.Verify(m => m.FindByEmailAddress(email), Times.Once);
        }

        [TestMethod]
        public void AddCart_UserCartAdded()
        {
            var email = "test@test.com";
            var user = TestModelFactory.CreateUser(emailAddress: email);
            var users = new List<UserModel>();
            users.Add(user);

            var mockedUserRepository = new Mock<IUserRepository>();
            mockedUserRepository.Setup(m => m.FindByEmailAddress(email)).Returns(user);

            var deal = TestModelFactory.CreateCompleteDeal();
            var dealOption = deal.Options.First();
            var dealAttributes = dealOption.Attributes.Select(a => a.Key.ToString()).ToList();
            var dealRepository = new FakeDealRepository(new List<DealModel>() { deal });

            var repositoryFactory = new FakeRepositoryFactory(userRepository: mockedUserRepository.Object, dealRepository: dealRepository);
            var cartService = new CartService(repositoryFactory);
            var controller = new CartController(cartService);
            controller.User = new GenericPrincipal(new GenericIdentity(email), new string[] { });

            var newCartItem = CreateNewCartItem(deal.Key.ToString(), dealOption.Key.ToString(), dealAttributes);

            user.CartItems.Should().BeEmpty();
            controller.Post(newCartItem);

            user.CartItems.Count().ShouldBeEquivalentTo(1);

            var firstCartItem = user.CartItems.First();
            firstCartItem.DealOption.ShouldBeEquivalentTo(dealOption);
            firstCartItem.AttributeValues.ShouldAllBeEquivalentTo(dealOption.Attributes);
        }

        private static NewCartItemViewModel CreateNewCartItem(string dealId, string dealOptionId, IEnumerable<string> dealAttributeIds)
        {
            return new NewCartItemViewModel
            {
                DealId = dealId,
                DealOptionId = dealOptionId,
                SelectedAttributes = dealAttributeIds.ToList()
            };
        }

        private static void AssertModelEquality(CartItemViewModel viewModel, CartItemModel domainModel)
        {
            viewModel.Id.ShouldBeEquivalentTo(domainModel.Key.ToString());
        }

        private static void SetupControllerWithIdentity(CartController controller, string email)
        {
            controller.User = new GenericPrincipal(new GenericIdentity(email), new string[] { });
        }

        private static CartController CreateCartController(List<UserModel> users)
        {
            var userRepository = new FakeUserRepository(users);
            var repositoryFactory = new FakeRepositoryFactory(userRepository: userRepository);
            var cartService = new CartService(repositoryFactory);
            var controller = new CartController(cartService);
            return controller;
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

        private static IRepositoryFactory GetFakeUserRepositoryFactory(IList<UserModel> users)
        {
            var userRepository = new FakeUserRepository(users);

            return new FakeRepositoryFactory(userRepository: userRepository);
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
