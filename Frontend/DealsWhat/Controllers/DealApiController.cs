using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using AttributeRouting.Web.Http;
using DealsWhat.Helpers;
using DealsWhat.Models;
using DealsWhat.ViewModels;
using Newtonsoft.Json;

namespace DealsWhat.Controllers
{
    public class DealApiController : ApiController
    {
        private DealsContext db = new DealsContext();

        [GET("api/deals/")]
        public HttpResponseMessage GetDealByCategory()
        {
            var query = Request.GetQueryNameValuePairs().ToList();
            var categoryId = query.FirstOrDefault(k => k.Key.Equals("categoryid"));
            var dealId = query.FirstOrDefault(k => k.Key.Equals("dealId"));
            var searchTerm = query.FirstOrDefault(k => k.Key.Equals("search"));
            var sortedBy = query.FirstOrDefault(k => k.Key.Equals("sort"));

            var deals = db.Deals
                .Include(d => d.Category)
                .Include(d => d.Pictures)
                .Include(d => d.DealOptions)
                .Include(d => d.DealOptions)
                .Include(d => d.DealOptions.Select(a => a.Attributes))
                .Where(d => d.Status == (int)DealStatus.Published)
                .AsEnumerable();

            if (KeyHasValue(categoryId))
            {
                Guid categoryIdGuid = new Guid();
                if (Guid.TryParse(categoryId.Value, out categoryIdGuid))
                {

                    //TOOD: Will deal category ever be null?
                    deals = deals.Where(deal => deal.Category != null && deal.Category.Id == categoryIdGuid);
                }
            }
            else if (KeyHasValue(dealId))
            {
                Guid dealGuid = new Guid();
                if (Guid.TryParse(dealId.Value, out dealGuid))
                {
                    deals = deals.Where(deal => deal.Id == dealGuid);
                }
            }
            else if (KeyHasValue(searchTerm))
            {
                var searchTermStr = searchTerm.Value;
                if (searchTermStr.Length > 3)
                {
                    deals =
                        deals.Where(
                            deal =>
                                deal.ShortTitle.IndexOf(searchTermStr, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                deal.ShortDescription.IndexOf(searchTermStr, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                deal.LongTitle.IndexOf(searchTermStr, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                deal.LongDescription.IndexOf(searchTermStr, StringComparison.OrdinalIgnoreCase) >= 0);
                }
            }

            if (KeyHasValue(sortedBy))
            {
                var sortByCondition = sortedBy.Value;
                if (sortByCondition.Equals("dateposted"))
                {
                    deals = deals.OrderByDescending(d => d.DateAdded);
                }
            }

            foreach (var image in deals.SelectMany(a => a.Pictures))
            {
                    image.RelativeUrl =
                       VirtualPathUtility.ToAbsolute(PathHelper.ConvertRelativeToAbsoluteDealImagePath(image.RelativeUrl));
            }

            // Use view model so json doesn't screw up when messing with real db models.
            var viewModels = deals.Select(d => new DealViewModel(d)).ToList();

            var json = JsonConvert.SerializeObject(viewModels, Formatting.Indented);

            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(json);

            return response;
        }

        private static bool KeyHasValue(KeyValuePair<string, string> kvp)
        {
            if (!kvp.Equals(default(KeyValuePair<string, string>)) && !string.IsNullOrEmpty(kvp.Value))
            {
                return true;
            }

            return false;
        }

        // GET api/DealApi/6
        [GET("api/deal/{id}")]
        public HttpResponseMessage GetDeal(Guid id)
        {
            Deal deal = db.Deals.First(d => d.Id == id);
            if (deal == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            var json = JsonConvert.SerializeObject(deal, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(json);

            return response;
        }

        // PUT api/DealApi/5
        public HttpResponseMessage PutDeal(Guid id, Deal deal)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != deal.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(deal).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/DealApi
        public HttpResponseMessage PostDeal(Deal deal)
        {
            if (ModelState.IsValid)
            {
                db.Deals.Add(deal);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, deal);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = deal.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/DealApi/5
        public HttpResponseMessage DeleteDeal(Guid id)
        {
            Deal deal = db.Deals.Find(id);
            if (deal == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Deals.Remove(deal);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, deal);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}