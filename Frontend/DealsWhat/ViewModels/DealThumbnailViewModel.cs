using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using DealsWhat.Models;

namespace DealsWhat.ViewModels
{
    public class DealThumbnailViewModel
    {
        public Guid Id { get; set; }
        public string RelativeUrl { get; set; }
        public int Order { get; set; }

        public DealThumbnailViewModel(DealImages dealImage)
        {
            Id = dealImage.Id;

            var extension = Path.GetExtension(dealImage.RelativeUrl);
            var thumbPostfix = "_thumb" + extension;
            var thumbUrl = dealImage.RelativeUrl.Replace(extension, thumbPostfix);

            RelativeUrl = thumbUrl;
            Order = dealImage.Order;
        }
    }
}