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
            var query = new DealSearchQuery();

            return this.dealService.SearchDeals(query)
                .Select(d => AutoMapper.Mapper.Map<FrontEndDeal>(d));
        }

        // GET api/values/5
        public DealModel Get(int id)
        {
            return null;
        }

        // POST api/values
        public void Post([FromBody]DealModel value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]DealModel value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
