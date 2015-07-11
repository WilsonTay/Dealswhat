using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using Autofac.Core;
using DealsWhat.Application.WebApi.Models;
using DealsWhat.Domain.Interfaces;
using DealsWhat.Domain.Model;
using DealsWhat.Domain.Services;

namespace DealsWhat.Application.WebApi.Controllers
{
    public class NewCartItemViewModel
    {
        public string DealId { get; set; }
        public string DealOptionId { get; set; }
        public IList<string> SelectedAttributes { get; set; }
    }

    public class CartItemViewModel
    {
        public string ShortName { get; set; }
        public double RegularPrice { get; set; }
        public double SpecialPrice { get; set; }
        public string Id { get; set; }

        public IList<CartItemAttribute> Attributes { get; }

        public CartItemViewModel()
        {
            Attributes = new List<CartItemAttribute>();
        }
    }

    public class CartItemAttribute
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    [Authorize]
    public class CartController : ApiController
    {
        private readonly ICartService cartService;

        public CartController(ICartService cartService)
        {
            this.cartService = cartService;

            AutoMapper.Mapper.CreateMap<NewCartItemViewModel, NewCartItemModel>()
                .AfterMap((dest, src) =>
                {
                    if (src.SelectedAttributes.Any())
                    {
                        return;
                    }

                    foreach (var attr in dest.SelectedAttributes)
                    {
                        src.AddSelectedAttributeId(attr);
                    }
                });

            AutoMapper.Mapper.CreateMap<DealAttributeModel, CartItemAttribute>();

            AutoMapper.Mapper.CreateMap<CartItemModel, CartItemViewModel>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Key.ToString()))
               .ForMember(dest => dest.ShortName, opt => opt.MapFrom(src => src.DealOption.ShortTitle))
               .ForMember(dest => dest.RegularPrice, opt => opt.MapFrom(src => src.DealOption.RegularPrice))
               .ForMember(dest => dest.SpecialPrice, opt => opt.MapFrom(src => src.DealOption.SpecialPrice))
               .AfterMap((dest, src) =>
               {
                   if (src.Attributes.Any())
                   {
                       return;
                   }

                   foreach (var option in dest.AttributeValues)
                   {
                       var converted = AutoMapper.Mapper.Map<CartItemAttribute>(option);
                       src.Attributes.Add(converted);
                   }
               });
        }

        // GET api/<controller>
        public IEnumerable<CartItemViewModel> Get()
        {
            var emailAddress = User.Identity.Name;

            var cartItems = cartService.GetCartItems(emailAddress)
                .Select(item => AutoMapper.Mapper.Map<CartItemViewModel>(item));

            return cartItems;
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]NewCartItemViewModel value)
        {
            var emailAddress = User.Identity.Name;
            var newCartModel = AutoMapper.Mapper.Map<NewCartItemModel>(value);

            cartService.AddCartItem(emailAddress, newCartModel);
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        [AllowAnonymous]
        public HttpResponseMessage Options()
        {
            return new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
        }
    }
}