using RateMyAir.Entities.Models;
using System.Collections.Generic;

namespace RateMyAir.Interfaces.Services
{
    public interface IMemoryCacheService
    {
        List<IndexLevel> GetCachedAirQualityLevels();
        void CacheAirQualityLevels(List<IndexLevel> levels);
    }
}
