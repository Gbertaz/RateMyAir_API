using RateMyAir.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RateMyAir.Interfaces
{
    public interface IIndexLevelsRepository
    {
        Task<List<IndexLevel>> GetLevels();
    }
}
