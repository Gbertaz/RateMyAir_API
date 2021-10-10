using System;
using System.Threading.Tasks;

namespace RateMyAir.Interfaces.Repositories
{
    public interface IRepositoryManager : IDisposable
    {
        IAirQualityRepository AirQuality { get; }
        IIndexLevelsRepository IndexLevels { get; }
        Task<int> SaveAsync();
    }
}
