using RateMyAir.Entities.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RateMyAir.Interfaces.Repositories
{
    public interface IAirQualityRepository
    {
        Task<AirQuality> GetAirQualityByIdAsync(int airQualityId, bool trackChanges);
        Task<AirQuality> GetLastAsync(bool trackChanges);
        IQueryable<AirQuality> GetAllAirQuality(bool trackChanges);
        void InsertAirQuality(AirQuality model);
    }
}
