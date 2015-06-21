using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using DealsWhat.Domain.Interfaces;
using DealsWhat.Domain.Model;

namespace WebApplication1.Controllers
{
    public class DealsController : ApiController
    {
        private readonly IRepositoryFactory repositoryFactory;

        public DealsController(IRepositoryFactory repositoryFactory)
        {
            this.repositoryFactory = repositoryFactory;
        }

        // GET api/values
        public IEnumerable<Deal> Get()
        {
            return this.repositoryFactory.CreateDealRepository().GetAll();
        }

        // GET api/values/5
        public Deal Get(int id)
        {
            return null;
        }

        // POST api/values
        public void Post([FromBody]Deal value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]Deal value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
