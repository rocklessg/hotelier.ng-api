using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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

        public void Update(T entity)
        {
             _dbSet.Update(entity);
        }
    }
}
