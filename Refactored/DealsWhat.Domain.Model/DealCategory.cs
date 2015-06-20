using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealsWhat.Domain.Model
{
    public sealed class DealCategory : IEntity
    {
        public object Key { get; internal set; }

        public string Name { get; private set; }

        public IList<Deal> Deals { get; }

        private DealCategory()
        {
            Deals = new List<Deal>();
        }

        public static DealCategory Create(string categoryName)
        {
            return new DealCategory
            {
                Key = Guid.NewGuid(),
                Name = categoryName
            };
        }

        public void AddDeal(Deal deal)
        {
            if (Deals.Contains(deal))
            {
                throw new InvalidOperationException("Deal already exist.");
            }

            Deals.Add(deal);
        }
    }
}
