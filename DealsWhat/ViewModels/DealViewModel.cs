using DealsWhat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DealsWhat.ViewModels
{
    public class DealViewModel
    {
        public string Id { get; set; }
        public string ShortTitle { get; set; }
        public string ShortDescription { get; set; }
        public double RegularPrice { get; set; }
        public double SpecialPrice { get; set; }
        public string CanonicalUrl { get; set; }
        public DateTime EndTime { get; set; }
        public IList<string> ImageUrls { get; set; }
        public IList<DealOptionViewModel> DealOptions { get; set; }
        public IList<DealImageViewModel> Pictures { get; set; }
        public IList<DealThumbnailViewModel> Thumbnails { get; set; } 

        public DealViewModel(Deal deal)
        {
            Id = deal.Id.ToString();
            ShortTitle = deal.ShortTitle;
            ShortDescription = deal.ShortDescription;
            RegularPrice = deal.RegularPrice;
            SpecialPrice = deal.SpecialPrice;
            CanonicalUrl = deal.CanonicalUrl;
            EndTime = deal.EndTime;

            ImageUrls = deal.Pictures.Select(p => p.RelativeUrl).ToList();
            DealOptions = deal.DealOptions.Select(d => new DealOptionViewModel(d)).ToList();
            Pictures = deal.Pictures.Select(d => new DealImageViewModel(d)).ToList();
            Thumbnails = deal.Pictures.Select(d => new DealThumbnailViewModel(d)).ToList();
        }
    }
}