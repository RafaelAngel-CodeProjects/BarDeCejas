using Microsoft.Data.SqlClient;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BarCejas.Data.Interfaces
{
    public interface IRepository<T> where T : class
    {
        //List<T> GetAll();

        IEnumerable<T> GetAll();
        Task<T> GetById(int id);
        Task<T> GetById(long id);
        Task<IEnumerable<T>> GetByEagerLoad(string[] children);
        Task<IEnumerable<T>> GetByEagerLoad(Expression<Func<T, bool>> filter);
        Task<IEnumerable<T>> GetByEagerLoad(Expression<Func<T, bool>> filter, string[] children);
        Task Add(T element);
        void Update(T element);
        Task Delete(long id);

        #region Newness
        Task<bool> ChangeStatus(string pQuery, List<SqlParameter> pParameters);

        IEnumerable<T> GetAllNoTracking();

        #endregion

        #region Order
        public  Task<bool> Reorder(string pQuery, object input, List<SqlParameter> pParameters = null);

        #endregion

        #region ExecuteSentence
        public Task<bool> ExecuteSentence(string pQuery, List<SqlParameter> pParameters = null);
        #endregion

    }
}
