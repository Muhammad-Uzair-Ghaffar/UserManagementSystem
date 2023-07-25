using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using UserManagementSystem.Context;

namespace UserManagementSystem.GenericRepository
{
    public class Repository<TEntity> : IRepository<TEntity>
         where TEntity : class
    {    
        private readonly AppDBContext _context;

        public Repository(AppDBContext context)
        {
            _context = context;
        }


        public void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetById(string id)
        {
           return await _context.Set<TEntity>().FindAsync(id);         }

        public IQueryable<TEntity> GetQueryable()
        {
            return _context.Set<TEntity>();
        }

        public void Insert(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);

        }

        public Task SaveChangesAsync()
        {
             return  _context.SaveChangesAsync();
        }

        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }
    }
}
