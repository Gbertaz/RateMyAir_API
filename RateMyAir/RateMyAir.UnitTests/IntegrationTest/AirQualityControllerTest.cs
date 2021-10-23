using AutoMapper;
using RateMyAir.Entities.DTO;
using RateMyAir.Entities.RequestFeatures;
using RateMyAir.Interfaces.Repositories;
using RateMyAir.Interfaces.Services;
using RateMyAir.Services;
using RateMyAir.Tests.Services;
using RateMyAir.UnitTests.Repository;
using Xunit;

namespace RateMyAir.Tests.IntegrationTest
{
    public class AirQualityControllerTest
    {
        private readonly IAirQualityService _airQualityService;
        private readonly IAirQualityLevelsService _airQualityLevelService;
        private readonly IRepositoryManager _repoManager;
        private static IMapper _mapper;

        public AirQualityControllerTest()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new MappingProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }

            _repoManager = new MockRepositoryManager();
            _airQualityLevelService = new MockAirQualityLevelsService();
            _airQualityService = new AirQualityService(_repoManager, _airQualityLevelService, _mapper);
        }

        [Fact]
        public async void GetAirQualityById()
        {
            // Arrange
            int expectedId = 1;

            // Act
            var dtoOut = await _airQualityService.GetAirQualityByIdAsync(expectedId, false);

            // Assert
            Assert.NotNull(dtoOut);
            Assert.IsType<AirQualityDtoOut>(dtoOut);
            Assert.Equal(expectedId, dtoOut.Id);
        }

        [Fact]
        public async void GetDailyAirQualityIndexAsync()
        {
            // Arrange
            GetAirQualityParameters filter = new GetAirQualityParameters();

            // Act
            var airQualityIndex = await _airQualityService.GetDailyAirQualityIndexAsync(filter);

            // Assert
            Assert.NotNull(airQualityIndex);
            Assert.IsType<AirQualityIndexDtoOut>(airQualityIndex);
        }


    }
}
