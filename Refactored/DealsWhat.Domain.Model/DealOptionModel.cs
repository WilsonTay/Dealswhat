using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealsWhat.Domain.Model
{
    public sealed class DealOptionModel
    {
        public string ShortTitle { get; set; }
        public double RegularPrice { get; set; }
        public double SpecialPrice { get; set; }

        public IList<DealAttributeModel> Attributes { get; set; }
    }
}
