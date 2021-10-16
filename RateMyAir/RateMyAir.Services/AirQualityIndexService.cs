using RateMyAir.Entities.Models;
using RateMyAir.Interfaces.Repositories;
using RateMyAir.Interfaces.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RateMyAir.Services
{
    public class AirQualityIndexService : IAirQualityIndexService
    {
        private readonly IRepositoryManager _repoManager;
        private readonly IMemoryCacheService _cacheService;

        public AirQualityIndexService(IRepositoryManager repoManager, IMemoryCacheService cache)
        {
            _repoManager = repoManager;
            _cacheService = cache;
        }

        /// <summary>
        /// Get the Air Quality levels either from database or memory cache
        /// </summary>
        /// <returns>List of IndexLevel</returns>
        public async Task<List<IndexLevel>> GetAirQualityLevelsAsync()
        {
            List<IndexLevel> indexLevels = _cacheService.GetCachedAirQualityLevels();

            if(indexLevels == null)
            {
                //Air quality index map table. The data set is already sorted
                indexLevels = await _repoManager.IndexLevels.GetLevelsAsync();

                _cacheService.CacheAirQualityLevels(indexLevels);
            }

            return indexLevels;
        }
    }

}
