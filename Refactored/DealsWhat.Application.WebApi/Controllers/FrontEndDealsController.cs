using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DealsWhat.Application.WebApi.Models;
using DealsWhat.Domain.Interfaces;
using DealsWhat.Domain.Interfaces.Helpers;
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
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Key.ToString()))
                .AfterMap((dest, src) =>
                {
                    src.ThumbnailUrls = dest.Images.Select(i => ImageHelper.GenerateThumbnailPath(i.RelativeUrl)).ToList();
                });

            AutoMapper.Mapper.CreateMap<DealsWhat.Domain.Model.DealAttributeModel, FrontEndSpecificDealAttribute>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Key.ToString()));

            AutoMapper.Mapper.CreateMap<DealsWhat.Domain.Model.DealOptionModel, FrontDealSpecificDealOption>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Key.ToString()))
                .AfterMap((dest, src) =>
                {
                    if (src.DealAttributes.Any())
                    {
                        return;
                    }

                    foreach (var attr in dest.Attributes)
                    {
                        var converted = AutoMapper.Mapper.Map<FrontEndSpecificDealAttribute>(attr);
                        src.DealAttributes.Add(converted);
                    }
                });

            AutoMapper.Mapper.CreateMap<DealsWhat.Domain.Model.DealModel, FrontEndSpecificDeal>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Key.ToString()))
                .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.Images.Select(i => i.RelativeUrl)))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
                .AfterMap((dest, src) =>
                {
                    if (src.DealOptions.Any())
                    {
                        return;
                    }

                    foreach (var option in dest.Options)
                    {
                        var converted = AutoMapper.Mapper.Map<FrontDealSpecificDealOption>(option);
                        src.DealOptions.Add(converted);
                    }
                });
        }

        // GET api/values
        [HttpGet]
        [Route("api/deals/")]
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

            var convertedSearchResults = this.dealService.SearchDeals(searchQuery)
                .Select(d => AutoMapper.Mapper.Map<FrontEndDeal>(d))
                .ToList();

            foreach (var result in convertedSearchResults)
            {
                for (int i = 0; i < result.ThumbnailUrls.Count; i++)
                {
                    result.ThumbnailUrls[i] =
                        PathHelper.ConvertRelativeToAbsoluteDealImagePath(result.ThumbnailUrls[i]);
                }
            }

            return convertedSearchResults;
        }

        [HttpGet]
        [Route("api/deal/")]
        public FrontEndSpecificDeal GetSingle()
        {
            var query = Request.GetQueryNameValuePairs().ToList();
            var searchQuery = new SingleDealSearchQuery();

            var dealId = query.FirstOrDefault(k => k.Key.Equals("id", StringComparison.OrdinalIgnoreCase));
            var url = query.FirstOrDefault(k => k.Key.Equals("url", StringComparison.OrdinalIgnoreCase));

            if (KeyHasValue(dealId))
            {
                searchQuery.Id = dealId.Value;
            }

            if (KeyHasValue(url))
            {
                searchQuery.CanonicalUrl = url.Value;
            }

            var searchResult = this.dealService.SearchSingleDeal(searchQuery);

            var convertedSearchResult = AutoMapper.Mapper.Map<DealModel, FrontEndSpecificDeal>(searchResult);

            for (int i = 0; i < convertedSearchResult.ImageUrls.Count; i++)
            {
                convertedSearchResult.ImageUrls[i] =
                    PathHelper.ConvertRelativeToAbsoluteDealImagePath(convertedSearchResult.ImageUrls[i]);
            }

            return convertedSearchResult;
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
