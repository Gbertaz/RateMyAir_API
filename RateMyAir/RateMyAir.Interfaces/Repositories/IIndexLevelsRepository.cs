using RateMyAir.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RateMyAir.Interfaces.Repositories
{
    public interface IIndexLevelsRepository
    {
        Task<List<IndexLevel>> GetLevelsAsync();
    }
}
