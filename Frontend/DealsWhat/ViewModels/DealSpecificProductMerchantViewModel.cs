using DealsWhat.Models;

namespace DealsWhat.ViewModels
{
    public class DealSpecificProductMerchantViewModel
    {
        public string CompanyName { get; set; }
        public string About { get; set; }
        public string Website { get; set; }
        public string FullAddress { get; set; }
        public string AddressLat { get; set; }
        public string AddressLng { get; set; }

        public DealSpecificProductMerchantViewModel(Merchant model)
        {
            CompanyName = model.Name;
            About = model.About;
            Website = model.Website;
            FullAddress = model.Address;

            if (model.AddressLat.HasValue)
            {
                AddressLat = model.AddressLat.Value.ToString();
            }

            if (model.AddressLng.HasValue)
            {
                AddressLng = model.AddressLng.Value.ToString();
            }
        }
    }
}