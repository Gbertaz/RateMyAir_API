using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateMyAir.Interfaces
{
    public interface IRepositoryManager : IDisposable
    {
        IAirDataRepository AirData { get; }
        Task<int> SaveAsync();
    }
}
