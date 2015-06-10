using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DealsWhat.Models;

namespace DealsWhat.Controllers
{
    public class DealOptionAttribute
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public class NewOrderModel
    {
        public string DealOptionId { get; set; }
        public IList<DealOptionAttribute> SelectedAttributes { get; set; }
    }

    public class OrdersApiController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody]NewOrderModel newOrder)
        {
            var emailAddress = User.Identity.Name;
            var dealOptionGuid = default(Guid);

            Guid.TryParse(newOrder.DealOptionId, out dealOptionGuid);

            using (var context = new DealsContext())
            {
                var user = context.Users.First(u => u.EmailAddress == emailAddress);
                var dealOption = context.DealOptions.First(d => d.Id == dealOptionGuid);
                var attributes = newOrder.SelectedAttributes
                    .Select(attr=> { return Guid.Parse(attr.Value); })
                    .Select(attr =>
                    {
                        return context.DealAttributes.First(a => a.Id.Equals(attr));
                    });

                var cart = new Cart
                {
                    Id = Guid.NewGuid(),
                    DealOption = dealOption,
                    DealAttributes = attributes.ToList(),
                    Quantity = 1,
                    User = user
                };

                context.Carts.Add(cart);
                context.SaveChanges();
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}