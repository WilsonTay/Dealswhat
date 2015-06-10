using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using DealsWhat_Admin.Models;
using Newtonsoft.Json;

namespace DealsWhat_Admin.Controllers
{
    public class DealsApiController : ApiController
    {
        private DealsContext db = new DealsContext();

        // GET api/DealsApi
        public IEnumerable<Deal> GetDeals()
        {
            var deals = db.Deals.Include(d => d.DealCategory).Include(d => d.Merchant).OrderByDescending(d=>d.DateAdded);
            return deals.AsEnumerable();
        }

        // GET api/DealsApi/5
        public HttpResponseMessage GetDeal(Guid id)
        {
            Deal deal = db.Deals.Find(id);
            if (deal == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            var response = Request.CreateResponse(HttpStatusCode.OK);

            var json = JsonConvert.SerializeObject(deal, Formatting.Indented,
                                new JsonSerializerSettings
                                {
                                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                                    PreserveReferencesHandling = PreserveReferencesHandling.None
                                });
            response.Content = new StringContent(json);
            return response;
        }

        // PUT api/DealsApi/5
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

        // POST api/DealsApi
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

        // DELETE api/DealsApi/5
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