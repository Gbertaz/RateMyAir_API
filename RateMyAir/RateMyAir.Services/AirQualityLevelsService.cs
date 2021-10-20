using RateMyAir.Entities.Enums;
using RateMyAir.Entities.Models;
using RateMyAir.Interfaces.Repositories;
using RateMyAir.Interfaces.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateMyAir.Services
{
    public class AirQualityLevelsService : IAirQualityLevelsService
    {
        private readonly IRepositoryManager _repoManager;
        private readonly IMemoryCacheService _cacheService;

        public AirQualityLevelsService(IRepositoryManager repoManager, IMemoryCacheService cache)
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

        public async Task<List<IndexLevel>> GetAirQualityLevelsAsync(Enums.Pollutants pollutant)
        {
            List<IndexLevel> indexLevels = _cacheService.GetCachedAirQualityLevels(pollutant);

            if (indexLevels == null)
            {
                indexLevels = (await GetAirQualityLevelsAsync()).Where(x => x.Pollutant == pollutant.ToString()).ToList();
                _cacheService.CacheAirQualityLevels(indexLevels, pollutant);
            }

            return indexLevels;
        }
    }

}
