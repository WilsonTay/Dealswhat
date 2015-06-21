using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DealsWhat.Domain.Model;

namespace DealsWhat.Domain.Services
{
    public interface IDealService
    {
        IEnumerable<DealModel> SearchDeals(DealSearchQuery query);
    }
}
