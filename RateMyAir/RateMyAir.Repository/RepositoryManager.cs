using RateMyAir.Entities.Models;
using RateMyAir.Interfaces.Repositories;
using System;
using System.Threading.Tasks;

namespace RateMyAir.Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private DatabaseContext _context;
        private bool _disposed = false;

        public IAirQualityRepository _airQuality;
        public IIndexLevelsRepository _indexLevels;

        public RepositoryManager(DatabaseContext repositoryContext)
        {
            _context = repositoryContext;
        }

        public IAirQualityRepository AirQuality
        {
            get { if (_airQuality == null) _airQuality = new AirQualityRepository(_context); return _airQuality; }
        }

        public IIndexLevelsRepository IndexLevels
        {
            get { if (_indexLevels == null) _indexLevels = new IndexLevelsRepository(_context); return _indexLevels; }
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
