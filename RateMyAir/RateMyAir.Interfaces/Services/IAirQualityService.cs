using RateMyAir.Entities.DTO;
using RateMyAir.Entities.RequestFeatures;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RateMyAir.Interfaces.Services
{
    public interface IAirQualityService
    {
        Task<AirQualityDtoOut> GetAirQualityByIdAsync(int airQualityId, bool trackChanges);
        Task<AirQualityDtoOut> GetLastAirQualityAsync(bool trackChanges);
        Task<List<AirQualityIndexDtoOut>> GetDailyAirQualityIndexAsync(GetAirQualityParameters filter);
        Task<AirQualityDtoOut> InsertAirQualityAsync(AirQualityDtoIn model);
        Task<List<AirQualityDtoOut>> GetPagedAirQualityAsync(GetAirQualityParameters filter);
        Task<int> CountAirQualityAsync(GetAirQualityParameters filter);
    }
}
