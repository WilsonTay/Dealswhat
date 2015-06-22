using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DealsWhat.Application.WebApi.Models;
using DealsWhat.Domain.Interfaces;
using DealsWhat.Domain.Model;
using DealsWhat.Domain.Services;

namespace DealsWhat.Application.WebApi.Controllers
{
    //[Authorize]
    public class FrontEndDealsController : ApiController
    {
        private readonly IDealService dealService;

        public FrontEndDealsController(IDealService dealService)
        {
            this.dealService = dealService;

            AutoMapper.Mapper.CreateMap<DealsWhat.Domain.Model.DealModel, FrontEndDeal>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Key.ToString()));
        }

        // GET api/values
        public IEnumerable<FrontEndDeal> Get()
        {
            var query = Request.GetQueryNameValuePairs().ToList();
            var searchQuery = new DealSearchQuery();

            var searchTerm = query.FirstOrDefault(k => k.Key.Equals("search", StringComparison.OrdinalIgnoreCase));
            var categoryId = query.FirstOrDefault(k => k.Key.Equals("categoryid", StringComparison.OrdinalIgnoreCase));

            // Category id, search term, sorted by, all
            if (KeyHasValue(categoryId))
            {
                searchQuery.CategoryId = categoryId.Value;
            }

            if (KeyHasValue(searchTerm))
            {
                searchQuery.SearchTerm = searchTerm.Value;
            }

            //TODO: Combine search term and category.

            return this.dealService.SearchDeals(searchQuery)
                .Select(d => AutoMapper.Mapper.Map<FrontEndDeal>(d));
        }

        private static bool KeyHasValue(KeyValuePair<string, string> kvp)
        {
            if (!kvp.Equals(default(KeyValuePair<string, string>)) && !string.IsNullOrEmpty(kvp.Value))
            {
                return true;
            }

            return false;
        }
    }
}
