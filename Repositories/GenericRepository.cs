using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using WebApplication1.Entities;
using WebApplication1.Interfaces.Repositories;

namespace WebApplication1.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal ASMContext Context;
        internal DbSet<TEntity> DbSet;

        public GenericRepository(ASMContext context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
        }

        public virtual List<TEntity> GetMany(
            int page,
            int pageSize,
            Expression<Func<TEntity, bool>>? predicate = null,
            Expression<Func<TEntity, TEntity>>? projection = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = ""
        )
        {
            IQueryable<TEntity> query = DbSet;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (projection != null)
            {
                query = query.Select(projection);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            int skip = (page - 1) * pageSize;
            query = query.Skip(skip).Take(pageSize);

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }

            return query.ToList();
        }

        public virtual TEntity? GetOne(
            Expression<Func<TEntity, bool>>? predicate,
            Expression<Func<TEntity, TEntity>>? projection = null,
            string includeProperties = ""
        )
        {
            IQueryable<TEntity> query = DbSet;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (projection != null)
            {
                query = query.Select(projection);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return query.FirstOrDefault();
        }

        public virtual EntityEntry<TEntity> Create(TEntity entity)
        {
            return DbSet.Add(entity);
        }

        public virtual EntityEntry<TEntity> Remove(TEntity entity)
        {
            return DbSet.Remove(entity);
        }

        public EntityEntry<TEntity> Update(TEntity entityToUpdate)
        {
            return DbSet.Update(entityToUpdate);
        }
    }
}
