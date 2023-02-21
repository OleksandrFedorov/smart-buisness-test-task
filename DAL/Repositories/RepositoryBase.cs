using TestTask.DAL.Models;

namespace TestTask.DAL.Repositories
{
    public abstract class RepositoryBase<T> where T : BaseDbEntity
    {
        protected readonly TestTaskDbContext DbContext;
        protected RepositoryBase(TestTaskDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public virtual async Task<bool> CreateAsync(T entity)
        {
            await DbContext.AddAsync(entity);
            return await DbContext.SaveAndCompareAffectedRowsAsync();
        }

        public virtual async Task<bool> UpdateAsync(T entity)
        {
            DbContext.Update(entity);
            return await DbContext.SaveAndCompareAffectedRowsAsync();
        }

        public virtual async Task<bool> DeleteAsync(T entity)
        {
            DbContext.Remove(entity);
            return await DbContext.SaveAndCompareAffectedRowsAsync();
        }
    }
}
