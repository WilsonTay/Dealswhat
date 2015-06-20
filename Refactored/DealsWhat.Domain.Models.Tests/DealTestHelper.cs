using DealsWhat.Domain.Model;

static internal class DealTestHelper
{
    internal static Deal CreateDeal(
        string shortTitle = "short title",
        string shortDescription = "short description",
        string longTitle = "long title",
        string longDescription = "long description",
        string finePrint = "fineprint",
        string highlight = "highlight",
        string canonicalUrl = "url",
        object id = null)
    {
        var deal = Deal.Create(shortTitle, shortDescription, longTitle, longDescription, finePrint, highlight);

        if (id != null)
        {
            deal.Key = id;
        }

        return deal;
    }

    internal static DealCategory CreateDealCategory(object key = null, string name = "category1")
    {
        return DealCategory.Create(name);
    }
}