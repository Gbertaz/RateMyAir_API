using RateMyAir.Entities.Models;
using RateMyAir.Interfaces.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RateMyAir.UnitTests.Repository
{
    public class MockIndexLevelsRepository : IIndexLevelsRepository
    {
        private readonly List<IndexLevel> _indexLevels;

        public MockIndexLevelsRepository()
        {
            _indexLevels = new List<IndexLevel>()
            {
                new IndexLevel() { IndexLevelId = 1, Pollutant = "Pm25", RangeLow = 0, RangeHigh = 10, AirQualityIndex = "Good", Description = "Good", Color = "50f0e6" },
                new IndexLevel() { IndexLevelId = 1, Pollutant = "Pm25", RangeLow = 0, RangeHigh = 10, AirQualityIndex = "Fair", Description = "Fair", Color = "50f0e6" },
                new IndexLevel() { IndexLevelId = 1, Pollutant = "Pm25", RangeLow = 0, RangeHigh = 10, AirQualityIndex = "Moderate", Description = "Moderate", Color = "50f0e6" },
                new IndexLevel() { IndexLevelId = 1, Pollutant = "Pm25", RangeLow = 0, RangeHigh = 10, AirQualityIndex = "Poor", Description = "Poor", Color = "50f0e6" },
                new IndexLevel() { IndexLevelId = 1, Pollutant = "Pm25", RangeLow = 0, RangeHigh = 10, AirQualityIndex = "Very poor", Description = "Very poor", Color = "50f0e6" },
                new IndexLevel() { IndexLevelId = 1, Pollutant = "Pm25", RangeLow = 0, RangeHigh = 10, AirQualityIndex = "Extremely poor", Description = "Extremely poor", Color = "50f0e6" },
                new IndexLevel() { IndexLevelId = 1, Pollutant = "Pm10", RangeLow = 0, RangeHigh = 10, AirQualityIndex = "Good", Description = "Good", Color = "50f0e6" },
                new IndexLevel() { IndexLevelId = 1, Pollutant = "Pm10", RangeLow = 0, RangeHigh = 10, AirQualityIndex = "Fair", Description = "Fair", Color = "50f0e6" },
                new IndexLevel() { IndexLevelId = 1, Pollutant = "Pm10", RangeLow = 0, RangeHigh = 10, AirQualityIndex = "Moderate", Description = "Moderate", Color = "50f0e6" },
                new IndexLevel() { IndexLevelId = 1, Pollutant = "Pm10", RangeLow = 0, RangeHigh = 10, AirQualityIndex = "Poor", Description = "Poor", Color = "50f0e6" },
                new IndexLevel() { IndexLevelId = 1, Pollutant = "Pm10", RangeLow = 0, RangeHigh = 10, AirQualityIndex = "Very poor", Description = "Very poor", Color = "50f0e6" },
                new IndexLevel() { IndexLevelId = 1, Pollutant = "Pm10", RangeLow = 0, RangeHigh = 10, AirQualityIndex = "Extremely poor", Description = "Extremely poor", Color = "50f0e6" }
            };
        }

        public async Task<List<IndexLevel>> GetLevelsAsync()
        {
            return await Task.FromResult(_indexLevels);
        }
    }
}
