using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DealsWhat.Domain.Services
{
    public sealed class DealSearchQuery
    {
        public string SearchTerm { get; set; }
        public object CategoryId { get; set; }
        public object DealId { get; set; }
    }
}
