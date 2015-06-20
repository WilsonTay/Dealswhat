using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DealsWhat.Domain.Interfaces;
using DealsWhat.Domain.Model;

namespace DealsWhat.Domain.Services
{
    public class DealService : IDealService
    {
        private IRepository<Deal> dealRepository;

        public DealService(IRepository<Deal> dealRepository)
        {
            this.dealRepository = dealRepository;
        }

        public IEnumerable<Deal> SearchDeals(DealSearchQuery query)
        {
            var deals = this.dealRepository.GetAll().ToList();

            if (!string.IsNullOrEmpty(query.SearchTerm))
            {
                deals = deals.Where(d =>
                            ContainsIgnoreCase(d.ShortTitle, query.SearchTerm) ||
                            ContainsIgnoreCase(d.ShortDescription, query.SearchTerm) ||
                            ContainsIgnoreCase(d.LongTitle, query.SearchTerm) ||
                            ContainsIgnoreCase(d.LongDescription, query.SearchTerm)
                ).ToList();
            }

            return deals;
        }

        private static bool ContainsIgnoreCase(string compared, string searchTerm)
        {
            return compared.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) > -1;
        }

        /// <summary>
        /// We could actually make single and multiple deals grouped under the same method but
        /// due to performance considerations it's wiser to split them into two.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Deal SearchSingleDeal(SingleDealSearchQuery query)
        {
            if (!string.IsNullOrEmpty(query.Id))
            {
                return this.dealRepository.GetAll().FirstOrDefault(d => d.Key.ToString().Equals(query.Id));
            }

            if (!string.IsNullOrEmpty(query.CanonicalUrl))
            {
                return this.dealRepository.GetAll().FirstOrDefault(d => d.CanonicalUrl.Equals(query.CanonicalUrl));
            }

            return null;
        }
    }
}
