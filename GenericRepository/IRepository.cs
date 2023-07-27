using System.Linq.Expressions;

namespace UserManagementSystem.GenericRepository
{
    public interface IRepository<TEntity>
    {

        Task<List<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllpagedAsync(int page, int pageSize, string filter, string searchBy, string sortBy, bool ascending);
        Task<TEntity> GetById(string id);
        Task<TEntity> GetByName(string name);
        IQueryable<TEntity> GetQueryable();
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void Insert(TEntity entity);
        Task SaveChangesAsync();
        Task<IQueryable<TEntity>> Query(Expression<Func<TEntity, bool>> filter);
    }
}
