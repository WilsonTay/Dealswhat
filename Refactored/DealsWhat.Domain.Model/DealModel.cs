using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealsWhat.Domain.Model
{
    public sealed class DealModel : IAggregateRoot, IEntity
    {
        private readonly IList<DealOptionModel> options;
        private readonly IList<DealImageModel> images;

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

        public IEnumerable<DealImageModel> Images
        {
            get { return images; }
        }

        public IEnumerable<DealOptionModel> Options
        {
            get { return options; }
        }

        public string CanonicalUrl { get; internal set; }

        public object Key { get; internal set; }

        private DealModel()
        {
            images = new List<DealImageModel>();
            options = new List<DealOptionModel>();
        }

        public static DealModel Create(
            string shortTitle,
            string shortDescription,
            string longTitle,
            string longDescription,
            string finePrint,
            string highlight)
        {
            var deal = new DealModel
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
                Status = DealStatus.Draft,
                Key = Guid.NewGuid(),
            };

            deal.CanonicalUrl = GenerateCanonicalUrl(deal);
            deal.SKU = GenerateSKU(deal);

            return deal;
        }

        public void SetPrice(double regularPrice, double specialPrice)
        {
            RegularPrice = regularPrice;
            SpecialPrice = specialPrice;
        }

        private static string GenerateSKU(DealModel deal)
        {
            return RemoveSpecialCharacters(string.Concat(deal.ShortTitle, Guid.NewGuid().ToString()));
        }

        private static string GenerateCanonicalUrl(DealModel deal)
        {
            var shortTitleWithPlaceholder = deal.ShortTitle.Replace(" ", "spaceplaceholder1");
            shortTitleWithPlaceholder += Guid.NewGuid().ToString();

            return RemoveSpecialCharacters(shortTitleWithPlaceholder).Replace("spaceplaceholder1", "-");
        }

        private static string RemoveSpecialCharacters(string str)
        {
            var sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is DealModel && Equals((DealModel) obj);
        }

        private bool Equals(DealModel other)
        {
            return Equals(options, other.options) && Equals(images, other.images) && string.Equals(ShortTitle, other.ShortTitle) && string.Equals(LongTitle, other.LongTitle) && string.Equals(ShortDescription, other.ShortDescription) && string.Equals(LongDescription, other.LongDescription) && RegularPrice.Equals(other.RegularPrice) && string.Equals(SKU, other.SKU) && DateAdded.Equals(other.DateAdded) && SpecialPrice.Equals(other.SpecialPrice) && StartTime.Equals(other.StartTime) && IsFeatured.Equals(other.IsFeatured) && string.Equals(CanonicalUrl, other.CanonicalUrl) && Equals(Key, other.Key) && Status == other.Status && string.Equals(Highlight, other.Highlight) && string.Equals(FinePrint, other.FinePrint) && EndTime.Equals(other.EndTime);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (options != null ? options.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (images != null ? images.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (ShortTitle != null ? ShortTitle.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (LongTitle != null ? LongTitle.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (ShortDescription != null ? ShortDescription.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (LongDescription != null ? LongDescription.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ RegularPrice.GetHashCode();
                hashCode = (hashCode*397) ^ (SKU != null ? SKU.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ DateAdded.GetHashCode();
                hashCode = (hashCode*397) ^ SpecialPrice.GetHashCode();
                hashCode = (hashCode*397) ^ StartTime.GetHashCode();
                hashCode = (hashCode*397) ^ IsFeatured.GetHashCode();
                hashCode = (hashCode*397) ^ (CanonicalUrl != null ? CanonicalUrl.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Key != null ? Key.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (int) Status;
                hashCode = (hashCode*397) ^ (Highlight != null ? Highlight.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (FinePrint != null ? FinePrint.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ EndTime.GetHashCode();
                return hashCode;
            }
        }

        public void AddImage(DealImageModel image)
        {
            images.Add(image);
        }

        public void AddOption(DealOptionModel option)
        {
            options.Add(option);
        }
    }
}
