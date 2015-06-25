using System.Data.Entity;
using System.Data.Entity.Core.Common;
using EFCache;

namespace DealsWhat.Models
{
    public class CachedDbConfiguration : DbConfiguration
    {
        public CachedDbConfiguration()
        {
            var transactionHandler = new CacheTransactionHandler(new InMemoryCache());

            AddInterceptor(transactionHandler);

            var cachingPolicy = new CachingPolicy();

            Loaded +=
              (sender, args) => args.ReplaceService<DbProviderServices>(
                (s, _) => new CachingProviderServices(s, transactionHandler,
                  cachingPolicy));
        }
    }
}