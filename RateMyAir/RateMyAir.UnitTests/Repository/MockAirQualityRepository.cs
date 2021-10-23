using RateMyAir.Entities.Models;
using RateMyAir.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateMyAir.UnitTests.Repository
{
    public class MockAirQualityRepository : IAirQualityRepository
    {
        private readonly List<AirQuality> _airQuality;

        public MockAirQualityRepository()
        {
            _airQuality = new List<AirQuality>()
            {
                new AirQuality() { AirQualityId = 1, TemperatureOutdoor = 10, TemperatureIndoor = 20, Humidity = 30, Pressure = 1011, Pm25 = 15, Pm10 = 20, CreatedAt = DateTime.UtcNow },
                new AirQuality() { AirQualityId = 2, TemperatureOutdoor = 12, TemperatureIndoor = 22, Humidity = 40, Pressure = 1011, Pm25 = 15, Pm10 = 20, CreatedAt = DateTime.UtcNow },
                new AirQuality() { AirQualityId = 3, TemperatureOutdoor = 13, TemperatureIndoor = 26, Humidity = 50, Pressure = 1011, Pm25 = 15, Pm10 = 20, CreatedAt = DateTime.UtcNow },
                new AirQuality() { AirQualityId = 4, TemperatureOutdoor = 14, TemperatureIndoor = 21, Humidity = 60, Pressure = 1011, Pm25 = 15, Pm10 = 20, CreatedAt = DateTime.UtcNow },
                new AirQuality() { AirQualityId = 5, TemperatureOutdoor = -5, TemperatureIndoor = 18, Humidity = 70, Pressure = 1011, Pm25 = 15, Pm10 = 20, CreatedAt = DateTime.UtcNow },
                new AirQuality() { AirQualityId = 6, TemperatureOutdoor = 18, TemperatureIndoor = 23, Humidity = 80, Pressure = 1011, Pm25 = 15, Pm10 = 20, CreatedAt = DateTime.UtcNow },
                new AirQuality() { AirQualityId = 7, TemperatureOutdoor = 0, TemperatureIndoor = 24, Humidity = 90, Pressure = 1011, Pm25 = 15, Pm10 = 20, CreatedAt = DateTime.UtcNow },
                new AirQuality() { AirQualityId = 8, TemperatureOutdoor = 23, TemperatureIndoor = 27, Humidity = 100, Pressure = 1011, Pm25 = 15, Pm10 = 20, CreatedAt = DateTime.UtcNow },
                new AirQuality() { AirQualityId = 9, TemperatureOutdoor = -20, TemperatureIndoor = 22, Humidity = 0, Pressure = 1011, Pm25 = 15, Pm10 = 20, CreatedAt = DateTime.UtcNow },
                new AirQuality() { AirQualityId = 10, TemperatureOutdoor = 35, TemperatureIndoor = 20, Humidity = 10, Pressure = 1011, Pm25 = 15, Pm10 = 20, CreatedAt = DateTime.UtcNow }
            };
        }

        public IQueryable<AirQuality> GetAllAirQuality(bool trackChanges)
        {
            return _airQuality.AsQueryable();
        }

        public async Task<AirQuality> GetLastAsync(bool trackChanges)
        {
            return await Task.FromResult(_airQuality.Where(x => x.AirQualityId > 0).OrderByDescending(x => x.CreatedAt).FirstOrDefault());
        }

        public async Task<AirQuality> GetAirQualityByIdAsync(int airQualityId, bool trackChanges)
        {
            return await Task.FromResult(_airQuality.Where(x => x.AirQualityId == airQualityId).FirstOrDefault());
        }

        public void InsertAirQuality(AirQuality model)
        {
            _airQuality.Add(model);
        }
    }
}
