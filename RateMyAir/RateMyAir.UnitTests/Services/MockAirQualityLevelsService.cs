using RateMyAir.Entities.Enums;
using RateMyAir.Entities.Models;
using RateMyAir.Interfaces.Repositories;
using RateMyAir.Interfaces.Services;
using RateMyAir.UnitTests.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateMyAir.Tests.Services
{
    public class MockAirQualityLevelsService : IAirQualityLevelsService
    {
        private readonly IRepositoryManager _repoManager;

        public MockAirQualityLevelsService()
        {
            _repoManager = new MockRepositoryManager();
        }

        public async Task<List<IndexLevel>> GetAirQualityLevelsAsync()
        {
            return await _repoManager.IndexLevels.GetLevelsAsync();
        }

        public async Task<List<IndexLevel>> GetAirQualityLevelsAsync(Enums.Pollutants pollutant)
        {
            return (await _repoManager.IndexLevels.GetLevelsAsync()).Where(x => x.Pollutant == pollutant.ToString()).ToList();
        }
    }
}
