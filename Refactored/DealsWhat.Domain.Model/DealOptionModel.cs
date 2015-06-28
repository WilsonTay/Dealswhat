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

        public object Key { get; internal set; }

        public IEnumerable<DealAttributeModel> Attributes
        {
            get { return attributes; }
        }

        private readonly IList<DealAttributeModel> attributes;

        private DealOptionModel()
        {
            attributes = new List<DealAttributeModel>();
        }

        public static DealOptionModel Create(string shortTitle, double regularPrice, double specialPrice)
        {
            return new DealOptionModel
            {
                ShortTitle = shortTitle,
                RegularPrice = regularPrice,
                SpecialPrice = specialPrice,
                Key = Guid.NewGuid()
            };
        }

        public void AddAttribute(DealAttributeModel attribute)
        {
            attributes.Add(attribute);
        }
    }
}
