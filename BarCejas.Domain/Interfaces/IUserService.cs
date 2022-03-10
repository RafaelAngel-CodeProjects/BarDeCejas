using BarCejas.Core.CustomEntities;
using BarCejas.Core.Entities;
using BarCejas.Core.QueryFilters;

using System.Threading.Tasks;

namespace BarCejas.Core.Interfaces
{
    public interface IUserService
    {
        Task<User> GetLoginByCredencials(UserLogin login);
        Task InsertUser(User user);
        Task<bool> UpdateUser(User post);
        Task<User> GetUserById(int id);
        PagedList<User> GetUsers(QueryFilter filters);
    }
}