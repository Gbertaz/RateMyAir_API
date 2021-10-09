﻿using Microsoft.EntityFrameworkCore;
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
            return await FindByCondition(x => x.AirQualityId == airQualityId, trackChanges).FirstOrDefaultAsync();
        }

        public Task<AirQuality> GetLastAsync(bool trackChanges)
        {
            return FindByCondition(x => x.AirQualityId > 0, trackChanges).OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync();
        }

        public IQueryable<AirQuality> GetAirQuality(GetAirQualityParameters filter, bool trackChanges)
        {
            return FindByCondition(x => x.CreatedAt >= filter.FromDate && x.CreatedAt <= filter.ToDate, trackChanges);
        }

        public void CreateAirQuality(AirQuality entity)
        {
            Create(entity);
        }

    }
}