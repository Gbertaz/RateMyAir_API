using RateMyAir.Entities.Models;
using RateMyAir.Entities.RequestFeatures;
using System.Linq;
using System.Threading.Tasks;

namespace RateMyAir.Interfaces
{
    public interface IAirQualityRepository
    {
        Task<AirQuality> GetAirQualityByIdAsync(int airQualityId, bool trackChanges);
        Task<AirQuality> GetLastAsync(bool trackChanges);
        IQueryable<AirQuality> GetAirQuality(GetAirQualityParameters filter, bool trackChanges);
        void CreateAirQuality(AirQuality entity);
    }
}
