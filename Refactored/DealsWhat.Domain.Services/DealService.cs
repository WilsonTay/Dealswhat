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
        private readonly IRepositoryFactory repositoryFactory;

        public DealService(IRepositoryFactory repositoryFactory)
        {

            this.repositoryFactory = repositoryFactory;
        }

        public IEnumerable<DealModel> SearchDeals(DealSearchQuery query)
        {
            IEnumerable<DealModel> deals;

            if (query.CategoryId != null)
            {
                var category =
                    this.repositoryFactory.CreateDealCategoryRepository()
                        .GetAll()
                        .FirstOrDefault(c => c.Key.Equals(query.CategoryId));

                if (category == null)
                {
                    return new List<DealModel>();
                }

                deals = category.Deals;
            }
            else
            {
                deals = this.repositoryFactory.CreateDealRepository().GetAll();
            }

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
        public DealModel SearchSingleDeal(SingleDealSearchQuery query)
        {
            var repository = this.repositoryFactory.CreateDealRepository();

            if (!string.IsNullOrEmpty(query.Id))
            {
                return repository.GetAll().FirstOrDefault(d => d.Key.ToString().Equals(query.Id));
            }

            if (!string.IsNullOrEmpty(query.CanonicalUrl))
            {
                return repository.GetAll().FirstOrDefault(d => d.CanonicalUrl.Equals(query.CanonicalUrl));
            }

            return null;
        }
    }
}
