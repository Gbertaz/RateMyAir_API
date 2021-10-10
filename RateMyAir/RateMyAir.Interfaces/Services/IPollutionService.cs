using RateMyAir.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RateMyAir.Interfaces.Services
{
    public interface IPollutionService
    {
        Task<List<AirQualityIndexDtoOut>> GetAirQualityIndexAsync(DateTime fromDate, DateTime toDate);
    }

}
