using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DealsWhat.Models
{
    public class SampleData
    {
        public static void Seed(DealsContext context)
        {

            var categories = CreateDealCategories(context);
            var merchants = CreateMerchants(context);
            var deals = CreateDeals(context, categories, merchants);
            var users = CreateUsers(context);
            CreateDealOptions(context, deals);
            CreateDealComments(context, deals, users);
            CreateDealImages(context, deals);

            context.SaveChanges();
        }

        private static void CreateDealOptions(DealsContext context, List<Deal> deals)
        {
            var random = new Random();
            var randomNumber = random.Next(0, 5);

            foreach (var deal in deals)
            {
                deal.DealOptions = new List<DealOption>();
                for (int i = 0; i < randomNumber; i++)
                {
                    var dealOption = new DealOption
                    {
                        Id = Guid.NewGuid(),
                        RegularPrice = 10,
                        SpecialPrice = 5,
                        ShortTitle = string.Format("Option {0}", i)
                    };

                    dealOption.Attributes = new List<DealAttribute>();
                    dealOption.Attributes.Add(new DealAttribute
                    {
                        Id = Guid.NewGuid(),
                        Name = "Color",
                        Value = "Black"
                    });
                    dealOption.Attributes.Add(new DealAttribute
                    {
                        Id = Guid.NewGuid(),
                        Name = "Color",
                        Value = "Blue"
                    });
                    dealOption.Attributes.Add(new DealAttribute
                    {
                        Id = Guid.NewGuid(),
                        Name = "Color",
                        Value = "Red"
                    });
                    dealOption.Attributes.Add(new DealAttribute
                    {
                        Id = Guid.NewGuid(),
                        Name = "Size",
                        Value = "S"
                    });
                    dealOption.Attributes.Add(new DealAttribute
                    {
                        Id = Guid.NewGuid(),
                        Name = "Size",
                        Value = "M"
                    });
                    dealOption.Attributes.Add(new DealAttribute
                    {
                        Id = Guid.NewGuid(),
                        Name = "Size",
                        Value = "L"
                    });

                    deal.DealOptions.Add(dealOption);
                }
            }
        }

        private static IList<User> CreateUsers(DealsContext context)
        {
            var users = new List<User>();

            for (int i = 0; i < 10; i++)
            {
                var user = new User
                {
                    UserId = i,
                    Username = string.Format("User{0}", i),
                    EmailAddress = string.Format("user{0}@email.com", i),
                    AddressLine1 = "addressline1",
                    State = "state",
                    City = "city",
                    Street = "street",
                    FirstName = "firstname",
                    LastName = "lastname",
                    ZipCode = "zip"
                };

                users.Add(user);
                context.Users.Add(user);
            }

            return users;
        }

        private static void CreateDealComments(DealsContext context, IList<Deal> deals, IList<User> users)
        {
            var random = new Random();

            foreach (var deal in deals)
            {
                for (int i = 0; i < 3; i++)
                {
                    var randomUser = users[random.Next(0, users.Count)];

                    var comment = new DealComment
                    {
                        Id = Guid.NewGuid(),
                        DatePosted = DateTime.UtcNow,
                        Message = string.Format("Message {0}", i),
                        Poster = randomUser,
                        Deal = deal
                    };

                    context.DealComments.Add(comment);
                }
            }
        }

        private static void CreateDealImages(DealsContext context, IList<Deal> deals)
        {
            var random = new Random();

            foreach (var deal in deals)
            {
                for (int i = 0; i < 3; i++)
                {
                    var randNumber = random.Next(0, 18);
                    var image = new DealImages
                    {
                        Id = Guid.NewGuid(),
                        Order = i,
                        RelativeUrl = string.Format("{0}.jpg", randNumber),
                        Deal = deal
                    };

                    context.DealImages.Add(image);
                }
            }
        }

        private static IList<Merchant> CreateMerchants(DealsContext context)
        {
            var merchants = new List<Merchant>();

            for (int i = 0; i < 10; i++)
            {
                var merchant = new Merchant
                {
                    BusinessRegNumber = "RegNumber",
                    EmailAddress = "email@email.com",
                    Id = Guid.NewGuid(),
                    Website = "http://www.website.com",
                    PhoneNumber = "010-20318122",
                    Name = string.Format("Merchant {0}", i),
                    About = "About merchant",
                    Address = "address"
                };

                merchants.Add(merchant);
                context.Merchants.Add(merchant);
            }

            return merchants;
        }

        private static IList<DealCategory> CreateDealCategories(DealsContext context)
        {
            var categories = new string[] { "Food & Drink", "Beauty & Wellness", "Travel", "Goods", "Shopping" };
            var dealCategories = new List<DealCategory>();

            foreach (var category in categories)
            {
                var dealCategory = new DealCategory
                {
                    Id = Guid.NewGuid(),
                    Name = category
                };

                dealCategories.Add(dealCategory);
                context.DealCategories.Add(dealCategory);
            }



            return dealCategories;
        }

        private static string GetDescription()
        {
            var path = HttpContext.Current.Server.MapPath("~/App_Data/SampleDescription.txt");
            using (var reader = new StreamReader(new FileStream(path, FileMode.Open)))
            {
                return reader.ReadToEnd();
            }

        }
        private static string GetFinePrint()
        {
            var path = HttpContext.Current.Server.MapPath("~/App_Data/SampleFinePrint.txt");
            using (var reader = new StreamReader(new FileStream(path, FileMode.Open)))
            {
                return reader.ReadToEnd();
            }

        }
        private static string GetHighlight()
        {
            var path = HttpContext.Current.Server.MapPath("~/App_Data/SampleHighlight.txt");
            using (var reader = new StreamReader(new FileStream(path, FileMode.Open)))
            {
                return reader.ReadToEnd();
            }

        }

        private static List<Deal> CreateDeals(DealsContext context, IList<DealCategory> categories, IList<Merchant> merchants)
        {
            var description = GetDescription();
            var fineprint = GetFinePrint();
            var highlight = GetHighlight();

            var random = new Random();

            var deals = new List<Deal>();

            for (int i = 0; i < 200; i++)
            {
                var isFeatured = false;

                if (i < 3)
                {
                    isFeatured = true;
                }

                var randomCategory = categories[random.Next(0, categories.Count)];
                var randomMerchant = merchants[random.Next(0, merchants.Count)];

                var deal = new Deal
                {
                    Id = Guid.NewGuid(),
                    SKU = string.Format("SKU{0}{1}", randomCategory.Name, i.ToString()),
                    Merchant = randomMerchant,
                    RegularPrice = 10,
                    SpecialPrice = 5.50,
                    ShortTitle = string.Format("Sukiyaki Buffet Lunch at IOI Mall Puchong {0}{1}", randomCategory.Name, i.ToString()),
                    FinePrint = fineprint,
                    Highlight = highlight,
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow.AddDays(30),
                    IsFeatured = isFeatured,
                    DateAdded = DateTime.UtcNow,
                    LongTitle = "[41% Off] Sukishi: Sukiyaki Buffet Lunch at IOI Mall Puchong for RM24.90",
                    LongDescription = description,
                    ShortDescription = "Authentic Japanese sukiyaki in a casual dining atmosphere.",
                    CanonicalUrl = string.Format("url-for-product-{0}", i),
                    Category = randomCategory,
                    Status = DealStatus.Published
                };

                deals.Add(deal);
                context.Deals.Add(deal);
            }

            return deals;
        }
    }
}