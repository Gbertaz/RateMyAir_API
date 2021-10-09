using System;
using System.Threading.Tasks;

namespace RateMyAir.Interfaces
{
    public interface IRepositoryManager : IDisposable
    {
        IAirQualityRepository AirQuality { get; }
        Task<int> SaveAsync();
    }
}
