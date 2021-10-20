using RateMyAir.Entities.Enums;
using RateMyAir.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RateMyAir.Interfaces.Services
{
    public interface IAirQualityLevelsService
    {
        Task<List<IndexLevel>> GetAirQualityLevelsAsync();
        Task<List<IndexLevel>> GetAirQualityLevelsAsync(Enums.Pollutants pollutant);

    }
}
