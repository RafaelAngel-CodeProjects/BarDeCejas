using BarCejas.Core.CustomEntities;
using BarCejas.Core.Entities;
using BarCejas.Core.Interfaces;
using BarCejas.Core.QueryFilters;

using Microsoft.Extensions.Options;

using System.Linq;
using System.Threading.Tasks;

namespace BarCejas.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PaginationOptions _paginationOptions;

        public UserService(IUnitOfWork unitOfWork, IOptions<PaginationOptions> options)
        {
            _unitOfWork = unitOfWork;
            _paginationOptions = options.Value;
        }

        public async Task<User> GetLoginByCredencials(UserLogin login)
        {
            return await _unitOfWork.UserRepository.GetLoginByCredentials(login);
        }

        public async Task<User> GetUserById(int id)
        {
            return await _unitOfWork.UserRepository.GetById(id);
        }

        public async Task InsertUser(User user)
        {
            await _unitOfWork.UserRepository.Add(user);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<bool> UpdateUser(User user)
        {
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangeAsync();
            return true;
        }

        public PagedList<User> GetUsers(QueryFilter filters)
        {
            filters.PageNumber = filters.PageNumber == 0 ? _paginationOptions.DefaultPageNumber : filters.PageNumber;
            filters.PageSiza = filters.PageSiza == 0 ? _paginationOptions.DefaultPageSize : filters.PageSiza;

            var users = _unitOfWork.UserRepository.GetAll();
            if (filters.Id != null)
                users = users.Where(x => x.Id == filters.Id);

            if (filters.StringValue != null)
                users = users.Where(x => x.FirstName.ToLower().Contains(filters.StringValue.ToLower()) || x.LastName.ToLower().Contains(filters.StringValue.ToLower()) || x.Email.ToLower().Contains(filters.StringValue.ToLower()));

            return PagedList<User>.Create(users, filters.PageNumber, filters.PageSiza);
        }
    }
}
