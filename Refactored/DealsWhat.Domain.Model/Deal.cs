using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealsWhat.Domain.Model
{
    public class Deal : IEntity
    {
        public string ShortTitle { get; private set; }
        public string LongTitle { get; private set; }
        public string ShortDescription { get; private set; }
        public string LongDescription { get; private set; }
        public double RegularPrice { get; private set; }
        public double SpecialPrice { get; private set; }
        public string SKU { get; private set; }
        public DateTime DateAdded { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public string FinePrint { get; private set; }
        public string Highlight { get; private set; }
        public bool IsFeatured { get; private set; }
        public DealStatus Status { get; private set; }

        public object Key { get; private set; }

        public static Deal Create(
            string shortTitle,
            string shortDescription,
            string longTitle,
            string longDescription,
            string finePrint,
            string highlight)
        {
            return new Deal
            {
                ShortTitle = shortTitle,
                ShortDescription = shortDescription,
                LongTitle = longTitle,
                LongDescription = longDescription,
                FinePrint = finePrint,
                Highlight = highlight,
                DateAdded = DateTime.UtcNow,
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddDays(7),
                IsFeatured = false,
                RegularPrice = 0,
                SpecialPrice = 0,
                Status = DealStatus.Draft
            };
        }

        public void SetPrice(double regularPrice, double specialPrice)
        {
            RegularPrice = regularPrice;
            SpecialPrice = specialPrice;
        }
    }
}
