using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DealsWhat.Domain.Interfaces;
using DealsWhat.Domain.Model;
using DealsWhat.Domain.Test.Common;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Moq;

namespace DealsWhat.Domain.Services.Tests
{
    [TestClass]
    public class CartServiceTests
    {
        private IFixture fixture;
        private string emailAddress = "email@email.com";

        [TestInitialize]
        public void Initialize()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());

            fixture.Register<IRepositoryFactory>(() => fixture.Create<FakeRepositoryFactory>());
        }


        [TestMethod]
        public void AddCartItem_GetUserFromRepository()
        {
            var mockedRepository = Mock.Get(fixture.Freeze<IUserRepository>());
            mockedRepository.Setup(m => m.FindByEmailAddress(emailAddress)).Returns(TestModelFactory.CreateUser());

            var deal = TestModelFactory.CreateCompleteDeal();
            var dealOption = deal.Options.First();
            var dealOptionAttributes = dealOption.Attributes.Select(a => a.Key.ToString()).ToList();

            var mockedDealRepository = Mock.Get(fixture.Freeze<IRepository<DealModel>>());
            mockedDealRepository.Setup(m => m.FindByKey(It.IsAny<object>()))
                .Returns(deal);

            var service = CreateCartService(userRepository: mockedRepository.Object, dealRepository: mockedDealRepository.Object);

            service.AddCartItem(emailAddress, TestModelFactory.CreateNewCartItem(deal.Key.ToString(), dealOption.Key.ToString(), dealOptionAttributes));

            mockedRepository.Verify(m => m.FindByEmailAddress(emailAddress), Times.Once);
        }

        [TestMethod]
        public void AddCartItem_AddToUserCart()
        {
            var user = TestModelFactory.CreateUser();

            var users = new List<UserModel>();
            users.Add(user);

            var deal = TestModelFactory.CreateCompleteDeal();
            var deals = new List<DealModel>() { deal };

            var dealOption = deal.Options.First();
            var dealAttributes = dealOption.Attributes.ToList();

            var cartItem = TestModelFactory.CreateNewCartItem(
                deal.Key.ToString(),
                dealOption.Key.ToString(),
                dealAttributes.Select(a => a.Key.ToString()).ToList());

            var service = CreateCartService(users, deals: deals);
            service.AddCartItem(user.EmailAddress.ToString(), cartItem);

            var userCartItem = user.CartItems.First();
            userCartItem.DealOption.ShouldBeEquivalentTo(dealOption);
            userCartItem.AttributeValues.ShouldAllBeEquivalentTo(dealAttributes);
        }

        [TestMethod]
        public void AddCartItem_AfterAdded_SaveCalled()
        {
            var mockedRepository = Mock.Get(fixture.Freeze<IUserRepository>());
            mockedRepository.Setup(m => m.FindByEmailAddress(emailAddress)).Returns(TestModelFactory.CreateUser());

            var deal = TestModelFactory.CreateCompleteDeal();
            var dealOption = deal.Options.First();
            var dealOptionAttributes = dealOption.Attributes.Select(a => a.Key.ToString()).ToList();
            var cartItem = TestModelFactory.CreateNewCartItem(deal.Key.ToString(), dealOption.Key.ToString(), dealOptionAttributes);

            var service = CreateCartService(
                userRepository: mockedRepository.Object,
                deals: new List<DealModel>() { deal });

            service.AddCartItem(emailAddress, cartItem);

            mockedRepository.Verify(m => m.Save(), Times.Once);
        }

        [TestMethod]
        public void AddCartItem_AllFieldMatches()
        {
            var dealModel = TestModelFactory.CreateDeal();

            var dealOptionTitle = "Shirt Style #3";
            var colorKey = "Color";
            var sizeKey = "Size";
            var dealOption = TestModelFactory.CreateDealOptionWithAttributes(dealOptionTitle);
            var attr1 = TestModelFactory.CreateDealAttribute(colorKey, "Red");
            var attr2 = TestModelFactory.CreateDealAttribute(sizeKey, "M");

            dealOption.AddAttribute(attr1);
            dealOption.AddAttribute(attr2);
            dealModel.AddOption(dealOption);

            var selectedAttributeValues = new List<DealAttributeModel>();
            selectedAttributeValues.Add(attr1);
            selectedAttributeValues.Add(attr2);

            //TODO: Add validation when adding new order. Check and make sure
            // all attributes under the deal has a value.
            var cartItem = NewCartItemModel.Create(
                dealModel.Key.ToString(),
                dealOption.Key.ToString(),
                selectedAttributeValues.Select(a => a.Key.ToString()));

            var user = TestModelFactory.CreateUser();

            var users = new List<UserModel>();
            users.Add(user);

            var service = CreateCartService(users, deals: new List<DealModel>() { dealModel });
            service.AddCartItem(user.EmailAddress.ToString(), cartItem);

            var actualCartItem = user.CartItems.First();
            actualCartItem.DealOption.ShouldBeEquivalentTo(dealOption);
            actualCartItem.AttributeValues.Should().Contain(attr1);
            actualCartItem.AttributeValues.Should().Contain(attr2);
            actualCartItem.AttributeValues.Count().ShouldBeEquivalentTo(selectedAttributeValues.Count);
        }

        [TestMethod]
        public void GetCartItems_AccessUserCartsFromRepository()
        {
            var user = TestModelFactory.CreateUser();
            var cartItem = TestModelFactory.CreateCartItem();

            user.AddToCart(cartItem);

            var users = new List<UserModel>();
            users.Add(user);

            var service = CreateCartService(users);
            var cartItems = service.GetCartItems(user.EmailAddress.ToString());

            cartItems.Count().ShouldBeEquivalentTo(1);
            cartItems.First().ShouldBeEquivalentTo(cartItem);
        }

        [TestMethod]
        public void GetCartItems_UserNoCartItem_ReturnEmptyList()
        {
            var user = TestModelFactory.CreateUser();

            var users = new List<UserModel>();
            users.Add(user);

            var service = CreateCartService(users);
            var cartItems = service.GetCartItems(user.EmailAddress.ToString());

            cartItems.Should().BeEmpty();
        }

        [TestMethod]
        public void RemoveCartItem_ItemRemovedFromUser()
        {
            var user = TestModelFactory.CreateUser();
            var cartItem1 = TestModelFactory.CreateCartItem();
            var cartItem2 = TestModelFactory.CreateCartItem();

            user.AddToCart(cartItem1);
            user.AddToCart(cartItem2);

            var users = new List<UserModel>();
            users.Add(user);

            var service = CreateCartService(users);
            service.RemoveCartItem(user.EmailAddress.ToString(), cartItem1.Key.ToString());

            user.CartItems.Count().Should().Be(1);
            user.CartItems.First().ShouldBeEquivalentTo(cartItem2);
        }

        private CartService CreateCartService(
            List<UserModel> users = null,
            IUserRepository userRepository = null,
            List<DealModel> deals = null,
            IRepository<DealModel> dealRepository = null)
        {
            if (userRepository == null)
            {
                userRepository = new FakeUserRepository(users ?? new List<UserModel>());
            }

            if (dealRepository == null)
            {
                dealRepository = new FakeDealRepository(deals ?? new List<DealModel>());
            }

            fixture.Register<IUserRepository>(() => userRepository);
            fixture.Register<IRepository<DealModel>>(() => dealRepository);

            var fakeRepositoryFactory = fixture.Create<FakeRepositoryFactory>();
            fixture.Register<IRepositoryFactory>(() => fakeRepositoryFactory);

            var cartService = fixture.Create<CartService>();

            return cartService;
        }
    }

}
