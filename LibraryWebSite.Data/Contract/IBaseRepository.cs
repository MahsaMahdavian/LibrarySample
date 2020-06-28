using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LibraryWebSite.Data.Contract
{
    public interface IBaseRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> FindAllAsync();
        Task<TEntity> FindByIdAsync(Object id);
        Task<IEnumerable<TEntity>> FindByConditionAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes);
        Task CreateAsync(TEntity entity);
        IEnumerable<TEntity> FindAll();           
        Task CreateRangeAsync(IEnumerable<TEntity> entities);       
        Task<List<TEntity>> GetPaginateResultAsync(int CurrentPage, int PageSize = 1);
        int CountEntities();
        void DeleteRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
    }
}
