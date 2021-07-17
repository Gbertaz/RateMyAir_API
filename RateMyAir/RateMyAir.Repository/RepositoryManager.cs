using RateMyAir.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateMyAir.Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private DatabaseContext _context;
        private bool _disposed = false;

        public IAirDataRepository _airDataRepo;

        public RepositoryManager(DatabaseContext repositoryContext)
        {
            _context = repositoryContext;
        }

        public IAirDataRepository AirData
        {
            get
            {
                if (_airDataRepo == null)
                {
                    _airDataRepo = new AirDataRepository(_context);
                }

                return _airDataRepo;
            }
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
