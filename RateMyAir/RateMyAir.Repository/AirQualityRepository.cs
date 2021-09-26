using Microsoft.EntityFrameworkCore;
using RateMyAir.Entities.Models;
using RateMyAir.Entities.RequestFeatures;
using RateMyAir.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RateMyAir.Repository
{
    public class AirQualityRepository : RepositoryBase<AirQuality>, IAirQualityRepository
    {
        public AirQualityRepository(DatabaseContext repositoryContext) : base(repositoryContext) { }

        public async Task<AirQuality> GetAirQualityByIdAsync(int airQualityId, bool trackChanges)
        {
            return await FindByCondition(x => x.AirDataID == airQualityId, trackChanges).FirstOrDefaultAsync();
        }

        public Task<AirQuality> GetLastAsync(bool trackChanges)
        {
            return FindByCondition(x => x.AirDataID > 0, trackChanges).OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync();
        }

        public IQueryable<AirQuality> GetAirQuality(GetAirQualityParameters filter, bool trackChanges)
        {
            return FindByCondition(x => x.CreatedDate >= filter.FromDate && x.CreatedDate <= filter.ToDate, trackChanges);
        }

        public void CreateAirData(AirQuality model)
        {
            Create(model);
        }

    }
}