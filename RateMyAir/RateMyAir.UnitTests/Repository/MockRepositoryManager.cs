using RateMyAir.Interfaces.Repositories;
using System;
using System.Threading.Tasks;

namespace RateMyAir.UnitTests.Repository
{
    public class MockRepositoryManager : IRepositoryManager
    {
        private IAirQualityRepository _airQuality;
        private IIndexLevelsRepository _indexLevels;

        public IAirQualityRepository AirQuality
        {
            get { if (_airQuality == null) _airQuality = new MockAirQualityRepository(); return _airQuality; }
        }

        public IIndexLevelsRepository IndexLevels
        {
            get { if (_indexLevels == null) _indexLevels = new MockIndexLevelsRepository(); return _indexLevels; }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveAsync()
        {
            throw new NotImplementedException();
        }
    }
}
