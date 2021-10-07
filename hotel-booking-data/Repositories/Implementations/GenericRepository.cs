using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace hotel_booking_data.Repositories.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly HbaDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(HbaDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public void DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task InsertAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void UpdateAsync(T entity)
        {
            var a = _dbSet.Update(entity);
        }
    }
}
