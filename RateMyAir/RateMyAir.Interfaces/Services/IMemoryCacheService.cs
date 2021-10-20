using RateMyAir.Entities.Enums;
using RateMyAir.Entities.Models;
using System.Collections.Generic;

namespace RateMyAir.Interfaces.Services
{
    public interface IMemoryCacheService
    {
        List<IndexLevel> GetCachedAirQualityLevels();
        List<IndexLevel> GetCachedAirQualityLevels(Enums.Pollutants pollutant);
        void CacheAirQualityLevels(List<IndexLevel> levels);
        void CacheAirQualityLevels(List<IndexLevel> levels, Enums.Pollutants pollutant);
    }
}
