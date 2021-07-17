using Microsoft.EntityFrameworkCore;
using RateMyAir.Entities.Models;
using RateMyAir.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateMyAir.Repository
{
    public class AirDataRepository : RepositoryBase<AirData>, IAirDataRepository
    {
        private DatabaseContext _context;
        public AirDataRepository(DatabaseContext repositoryContext) : base(repositoryContext) 
        {
            _context = repositoryContext;
        }

        public void CreateAirData(AirData model)
        {
            Create(model);
        }

        public async Task<AirData> GetByIdAsync(int id, bool trackChanges)
        {
            return await FindByCondition(x => x.AirDataID == id, trackChanges).FirstOrDefaultAsync();
        }

        public async Task<AirData> GetLastAsync()
        {
            return await _context.AirData.AsNoTracking().OrderByDescending(x => x.CreatedDate).FirstAsync();
        }

        public async Task<List<AirData>> GetRangeAsync(DateTime startDate, DateTime endDate, bool trackChanges)
        {
            return await FindByCondition(x => x.CreatedDate >= startDate && x.CreatedDate <= endDate, trackChanges)
                .OrderBy(d => d.CreatedDate).ToListAsync();
        }

    }
}