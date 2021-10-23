using RateMyAir.Entities.Models;
using RateMyAir.Interfaces.Repositories;
using RateMyAir.UnitTests.Repository;
using System.Linq;
using Xunit;

namespace RateMyAir.UnitTests.UnitTest
{
    public class AirQualityRepositoryTests
    {
        private readonly IAirQualityRepository _airQualityRepository;

        public AirQualityRepositoryTests()
        {
            _airQualityRepository = new MockAirQualityRepository();
        }

        [Fact]
        public void CanGetAll()
        {
            // Act
            var result = _airQualityRepository.GetAllAirQuality(false);

            // Assert
            Assert.IsAssignableFrom<IQueryable<AirQuality>>(result);
        }

        [Fact]
        public async void CanGetLast()
        {
            // Act
            var result = await _airQualityRepository.GetLastAsync(false);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<AirQuality>(result);
        }

        [Fact]
        public async void CanGetById()
        {
            // Arrange
            int expectedId = 1;

            // Act
            var result = await _airQualityRepository.GetAirQualityByIdAsync(expectedId, false);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<AirQuality>(result);
            Assert.Equal(expectedId, result.AirQualityId);
        }

    }
}
