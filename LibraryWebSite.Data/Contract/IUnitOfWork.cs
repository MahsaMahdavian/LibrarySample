using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LibraryWebSite.Data.Contract
{
    public interface IUnitOfWork
    {
        IBaseRepository<TEntity> BaseRepository<TEntity>() where TEntity : class;
        
        LibraryDBContext _Context { get; }
        Task Commit();
    }
}
