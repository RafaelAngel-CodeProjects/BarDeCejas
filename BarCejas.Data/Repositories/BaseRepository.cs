using BarCejas.Data.DataContext;
using BarCejas.Data.Interfaces;
using BarCejas.Entities;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BarCejas.Data.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        private readonly BardecejasContext _context;
        protected DbSet<T> _entities;

        public BaseRepository(BardecejasContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public async Task Add(T element)
        {
            await _entities.AddAsync(element);
        }

        public async Task Delete(long id)
        {
            _entities.Remove(await GetById(id));
        }

        public IEnumerable<T> GetAll()
        {
            return _entities.AsEnumerable();
        }

        public async Task<T> GetById(long id)
        {
            var element = await _entities.FindAsync(id);
            return element;
        }

        public async Task<T> GetById(int id)
        {
            var element = await _entities.FindAsync(id);
            return element;
        }

        public async Task<IEnumerable<T>> GetByEagerLoad(Expression<Func<T, bool>> filter)
        {
            try
            {
                IQueryable<T> query = _entities;
                return await query.Where(filter).ToListAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<T>> GetByEagerLoad(Expression<Func<T, bool>> filter, string[] children)
        {
            try
            {
                IQueryable<T> query = _entities;
                foreach (string entity in children)
                    query = query.Include(entity);

                return await query.Where(filter).ToListAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<T>> GetByEagerLoad(string[] children)
        {
            try
            {
                IQueryable<T> query = _entities;
                foreach (string entity in children)
                    query = query.Include(entity);

                return await query.ToListAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Update(T element)
        {
            _entities.Update(element);
        }

      
        #region others
        public async Task<bool> ChangeStatus(string pQuery, List<SqlParameter> pParameters)
        {

            try
            {
                int exists = await _context.Database.ExecuteSqlRawAsync(pQuery, pParameters);

                return exists > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<T> GetAllNoTracking()
        {
            try
            {
                return _entities.AsNoTracking().AsEnumerable();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        #endregion

        #region Order
        public async Task <bool>Reorder(string pQuery, object input, List<SqlParameter> pParameters = null) 
        {
            try
            {
                var paramList = new List<SqlParameter>()
            {
                new SqlParameter("Data", JsonConvert.SerializeObject(input).ToString())
            };
                if (pParameters != null)
                {
                    paramList.AddRange(pParameters);
                }

                int exists = await _context.Database.ExecuteSqlRawAsync(pQuery, paramList);

                return exists > 0;

            }
            catch (Exception ex)
            {

                throw ex;
            }
          
        }
        #endregion

        #region  ExecuteStoreprocedure
        public async Task<bool> ExecuteSentence(string pQuery, List<SqlParameter> pParameters)
        {

            try
            {
                int exists = await _context.Database.ExecuteSqlRawAsync(pQuery, pParameters);

                return exists > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}
