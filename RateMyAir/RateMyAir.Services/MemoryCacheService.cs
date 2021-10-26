using Microsoft.Extensions.Caching.Memory;
using RateMyAir.Entities.Enums;
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
            TimeSpan cacheDuration = DateTime.Today.AddDays(5) - DateTime.Now;
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(cacheDuration);
            _cache.Set(CacheKeys.AirQualityLevels, levels, cacheEntryOptions);
        }

        public void CacheAirQualityLevels(List<IndexLevel> levels, Enums.Pollutants pollutant)
        {
            string cacheKey = string.Empty;

            switch (pollutant)
            {
                case Enums.Pollutants.Pm10:
                    cacheKey = CacheKeys.Pm10AirQualityLevels;
                    break;
                case Enums.Pollutants.Pm25:
                    cacheKey = CacheKeys.Pm25AirQualityLevels;
                    break;
            }

            TimeSpan cacheDuration = DateTime.Today.AddDays(5) - DateTime.Now;
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(cacheDuration);
            _cache.Set(cacheKey, levels, cacheEntryOptions);
        }

        public List<IndexLevel> GetCachedAirQualityLevels()
        {
            List<IndexLevel> cacheEntry;
            _cache.TryGetValue(CacheKeys.AirQualityLevels, out cacheEntry);
            return cacheEntry;
        }

        public List<IndexLevel> GetCachedAirQualityLevels(Enums.Pollutants pollutant)
        {
            string cacheKey = string.Empty;

            switch (pollutant)
            {
                case Enums.Pollutants.Pm10:
                    cacheKey = CacheKeys.Pm10AirQualityLevels;
                    break;
                case Enums.Pollutants.Pm25:
                    cacheKey = CacheKeys.Pm25AirQualityLevels;
                    break;
            }

            List<IndexLevel> cacheEntry;
            _cache.TryGetValue(cacheKey, out cacheEntry);
            return cacheEntry;
        }
    }

}
