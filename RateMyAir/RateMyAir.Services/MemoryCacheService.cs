using Microsoft.Extensions.Caching.Memory;
using RateMyAir.Entities.Models;
using RateMyAir.Entities.Settings;
using RateMyAir.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace RateMyAir.Services
{
    public class MemoryCacheService : IMemoryCacheService
    {
        private readonly IMemoryCache _cache;

        public MemoryCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void CacheAirQualityLevels(List<IndexLevel> levels)
        {
            // Expires at midnight
            TimeSpan cacheDuration = DateTime.Today.AddDays(1) - DateTime.Now;
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(cacheDuration);
            // Cache the data
            _cache.Set(CacheKeys.AirQualityLevels, levels, cacheEntryOptions);
        }

        public List<IndexLevel> GetCachedAirQualityLevels()
        {
            List<IndexLevel> cacheEntry;
            _cache.TryGetValue(CacheKeys.AirQualityLevels, out cacheEntry);
            return cacheEntry;
        }
    }

}
