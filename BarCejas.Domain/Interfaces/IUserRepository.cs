using BarCejas.Core.Entities;

using System.Threading.Tasks;

namespace BarCejas.Core.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetLoginByCredentials(UserLogin login);
    }
}