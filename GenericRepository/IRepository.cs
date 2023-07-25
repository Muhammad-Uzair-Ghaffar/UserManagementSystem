namespace UserManagementSystem.GenericRepository
{
    public interface IRepository<TEntity>
    {

        Task<List<TEntity>> GetAllAsync();
        Task<TEntity> GetById(string id);
        IQueryable<TEntity> GetQueryable();
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void Insert(TEntity entity);
        Task SaveChangesAsync();
    }
}
