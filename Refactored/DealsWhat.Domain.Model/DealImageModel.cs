using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealsWhat.Domain.Model
{
    public class DealImageModel
    {
        public string RelativeUrl { get; private set; }
        public int Order { get; private set; }

        private DealImageModel()
        {

        }

        public static DealImageModel Create(string relativeUrl, int order)
        {
            return new DealImageModel
            {
                RelativeUrl = relativeUrl,
                Order = order
            };
        }
    }
}
