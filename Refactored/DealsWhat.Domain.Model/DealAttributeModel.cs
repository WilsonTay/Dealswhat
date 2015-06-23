using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealsWhat.Domain.Model
{
    public sealed class DealAttributeModel
    {
        public string Name { get; set; }
        public string Value { get; set; }

        private DealAttributeModel()
        {

        }

        public static DealAttributeModel Create(string name, string value)
        {
            return new DealAttributeModel
            {
                Name = name,
                Value = value
            };
        }
    }
}
