using hotel_booking_utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace hotel_booking_data.Repositories.Abstractions
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IList<T>> GetAllAsync(
           Expression<Func<T, bool>> expression = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null,
           List<string> Includes = null);
        Task<IPagedList<T>> GetPageList(
            Paginator pager,
            Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null,
            List<string> Includes = null);
        Task<T> GetAsync(Expression<Func<T, bool>> expression, List<string> Includes = null);
        Task<bool> InsertAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
    }
}
