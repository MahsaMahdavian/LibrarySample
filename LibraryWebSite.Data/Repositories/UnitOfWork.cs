using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LibraryWebSite.Data.Contract;

namespace LibraryWebSite.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public LibraryDBContext _Context { get; }
   
        public UnitOfWork(LibraryDBContext context)
        {
            _Context = context;
          
        }

        public IBaseRepository<TEntity> BaseRepository<TEntity>() where TEntity : class
        {
            IBaseRepository<TEntity> repository = new BaseRepository<TEntity, LibraryDBContext>(_Context);
            return repository;
        }

        public async Task Commit()
        {
            await _Context.SaveChangesAsync();
        }
    }
}
