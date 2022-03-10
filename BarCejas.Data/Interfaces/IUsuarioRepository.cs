using BarCejas.Entities;
using System.Threading.Tasks;

namespace BarCejas.Data.Interfaces
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task<Usuario> GetUsuarioByEmail(string email);
        Task<Usuario> GetUsuarioByDNI(string dni);
    }
}
