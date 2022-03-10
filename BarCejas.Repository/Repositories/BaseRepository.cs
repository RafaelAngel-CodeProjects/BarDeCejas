using Microsoft.EntityFrameworkCore;
using BarCejas.Core.Entities;
using BarCejas.Core.Interfaces;
using BarCejas.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarCejas.Infrastructure.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly BarCejasContext _context;
        protected DbSet<T> _entities;

        public BaseRepository(BarCejasContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public async Task Add(T element)
        {
            await _entities.AddAsync(element);
        }

        public async Task Delete(int id)
        {
            _entities.Remove(await GetById(id));
        }

        public IEnumerable<T> GetAll()
        {
            return _entities.AsEnumerable();
        }

        public async Task<T> GetById(int id)
        {
            var element = await _entities.FindAsync(id);
            return element;
        }

        public void Update(T element)
        {
            _entities.Update(element);
        }
    }
}
