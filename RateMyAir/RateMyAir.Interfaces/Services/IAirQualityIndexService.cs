using RateMyAir.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RateMyAir.Interfaces.Services
{
    public interface IAirQualityIndexService
    {
        Task<List<IndexLevel>> GetAirQualityLevelsAsync();
    }
}
