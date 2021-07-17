using RateMyAir.Entities.DTO;
using RateMyAir.Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RateMyAir.Interfaces
{
    public interface IAirDataRepository
    {
        Task<AirData> GetByIdAsync(int id, bool trackChanges);
        Task<AirData> GetLastAsync();
        Task<List<AirData>> GetRangeAsync(DateTime startDate, DateTime endDate, bool trackChanges);
        void CreateAirData(AirData model);
    }
}
