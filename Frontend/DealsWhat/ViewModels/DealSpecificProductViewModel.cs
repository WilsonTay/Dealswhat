using System;
using System.Collections.Generic;
using System.Linq;
using DealsWhat.Models;

namespace DealsWhat.ViewModels
{
    public class DealSpecificProductViewModel
    {
        public string Id { get; set; }
        public string Fineprint { get; set; }
        public string Highlight { get; set; }

        public IList<string> ImageUrls { get; set; }
        public double OriginalPrice { get; set; }
        public double DiscountedPrice { get; set; }
        public DateTime EndDate { get; set; }

        public bool DealEnded
        {
            get { return DateTime.UtcNow.CompareTo(EndDate) > 0; }
        }

        public string LongTitle { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }

        public IList<DealOptionViewModel> DealOptions { get; set; }

        public PopularDealsViewModel PopularDeals { get; set; }
        public IList<DealSpecificProductCommentViewModel> Comments { get; set; }
        public DealSpecificProductMerchantViewModel Merchant { get; set; }

        public DealSpecificProductViewModel(Deal model, IEnumerable<Deal> popularDeals)
        {
            ImageUrls = model.Pictures.Select(p => p.RelativeUrl).ToList();

            OriginalPrice = model.RegularPrice;
            DiscountedPrice = model.SpecialPrice;
            EndDate = model.EndTime;

            LongTitle = model.LongTitle;
            ShortDescription = model.ShortDescription;
            LongDescription = model.LongDescription;

            Fineprint = model.FinePrint;
            Highlight = model.Highlight;

            DealOptions = model.DealOptions.Select(d => new DealOptionViewModel(d)).ToList();
            Comments = model.Comments.Select(c => new DealSpecificProductCommentViewModel(c)).ToList();
            Merchant = new DealSpecificProductMerchantViewModel(model.Merchant);
            PopularDeals = new PopularDealsViewModel(popularDeals);


            Id = model.Id.ToString();
        }
    }


}