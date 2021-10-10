using Microsoft.EntityFrameworkCore;
using RateMyAir.Entities.Models;
using RateMyAir.Interfaces.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RateMyAir.Repository
{
    public class IndexLevelsRepository : RepositoryBase<IndexLevel>, IIndexLevelsRepository
    {
        public IndexLevelsRepository(DatabaseContext repositoryContext) : base(repositoryContext) { }

        public async Task<List<IndexLevel>> GetLevels()
        {
            return await FindAll(false).ToListAsync();
        }
    }

}
