using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealsWhat.Domain.Services
{
    public sealed class SingleDealSearchQuery
    {
        public string Id { get; set; }
        public string CanonicalUrl { get; set; }
    }
}
