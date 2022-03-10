using BarCejas.Core.Entities;
using BarCejas.Core.Interfaces;
using BarCejas.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;

namespace BarCejas.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(BarCejasContext context) : base(context)
        { }

        public async Task<User> GetLoginByCredentials(UserLogin login)
        {
            return await _entities.FirstOrDefaultAsync(x => x.Email == login.Email);
            
        }
    }
}
