using BarCejas.Data.DataContext;
using BarCejas.Data.Interfaces;
using BarCejas.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BarCejas.Data.Repositories
{
    public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(BardecejasContext context) : base(context)
        { }

        public async Task<Usuario> GetUsuarioByEmail(string email)
        {
            return await _entities.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<Usuario> GetUsuarioByDNI(string dni)
        {
            return await _entities.FirstOrDefaultAsync(x => x.Dni == dni);
        }
    }
}
