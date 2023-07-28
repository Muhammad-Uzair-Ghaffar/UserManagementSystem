using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
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
           return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity> GetByName(string name)
        {
            return await _context.Set<TEntity>().FindAsync(name);//it is wrong it findly the value using primary key of the table while name is not pk here 
        }



        public async Task<IQueryable<TEntity>> Query(Expression<Func<TEntity, bool>>  filter)
        {
            if (filter != null)
            {
                return _context.Set<TEntity>().Where(filter);
            }
            return _context.Set<TEntity>();
        }



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



        public async Task<IEnumerable<TEntity>> GetAllpagedAsync(
        int page = 1,
        int pageSize = 10,
        string searchBy = null,
        string sortBy = null
        )
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            // Apply filtering
            if (!string.IsNullOrEmpty(searchBy))
            {
             
                query = query.Where(searchBy);
            }

            // Apply sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
               
                query =  query.OrderBy(sortBy);
            }

            // Apply pagination
            int skip = (page - 1) * pageSize;
            query = query.Skip(skip).Take(pageSize);

            return await query.ToListAsync();
        }

  
        
    }
}
