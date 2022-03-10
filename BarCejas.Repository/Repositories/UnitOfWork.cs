using BarCejas.Core.Entities;
using BarCejas.Core.Interfaces;
using BarCejas.Infrastructure.Data;

using System.Threading.Tasks;

namespace BarCejas.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BarCejasContext _context;
        private readonly IUserRepository _userRepository;

        public UnitOfWork(BarCejasContext context) => _context = context;

        public IUserRepository UserRepository => _userRepository ?? new UserRepository(_context);


        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }

        public void SaveChange()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
