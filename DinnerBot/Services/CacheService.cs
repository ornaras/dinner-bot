using DinnerBot.Models;
using System.Collections.ObjectModel;

namespace DinnerBot.Services
{
    public class CacheService(CafeteriaExchanger cafe, ILogger<CacheService> logger)
    {
        public ReadOnlyAsyncCachedObject<ReadOnlyCollection<Category>> Categories = 
            new(cafe.OpenCatalog, TimeSpan.FromMinutes(5), logger);
    }
}
