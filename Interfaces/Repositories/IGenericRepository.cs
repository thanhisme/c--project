using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace WebApplication1.Interfaces.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        public List<TEntity> GetMany(
            int page,
            int pageSize,
            Expression<Func<TEntity, bool>>? predicate = null,
            Expression<Func<TEntity, TEntity>>? projection = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = ""
        );

        public TEntity? GetOne(
            Expression<Func<TEntity, bool>>? predicate,
            Expression<Func<TEntity, TEntity>>? projection = null,
            string includeProperties = ""
        );

        public EntityEntry<TEntity> Create(TEntity entity);

        public EntityEntry<TEntity> Update(TEntity entityToUpdate);

        public EntityEntry<TEntity> Remove(TEntity entity);
    }
}
