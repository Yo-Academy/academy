using Academy.Application.Contracts.Persistence;
using Academy.Domain.Common.Contracts;
using Academy.Infrastructure.Persistence.Context;
using System.Linq.Expressions;

namespace Academy.Infrastructure.Persistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _dbContext;

        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public T GetById(Guid id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public T Add(T entity, bool autoSave = true)
        {
            _dbContext.Set<T>().Add(entity);
            if (autoSave) _dbContext.SaveChanges();

            return entity;
        }

        public async Task<T> AddAsync(T entity, bool autoSave = true)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            if (autoSave) await _dbContext.SaveChangesAsync();

            return entity;
        }

        public IEnumerable<T> AddRange(IEnumerable<T> entities, bool autoSave = true)
        {
            _dbContext.Set<T>().AddRange(entities);
            if (autoSave) _dbContext.SaveChanges();

            return entities;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, bool autoSave = true)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
            if (autoSave) await _dbContext.SaveChangesAsync();

            return entities;
        }

        public void Update(T entity, bool autoSave = true)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            if (autoSave) _dbContext.SaveChanges();
        }

        public async Task UpdateAsync(T entity, bool autoSave = true)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            if (autoSave) await _dbContext.SaveChangesAsync();
        }

        public void Delete(T entity, bool autoSave = true)
        {
            if (entity is AuditableEntity auditableEntity)
            {
                auditableEntity.IsDeleted = true;
                _dbContext.Entry(entity).State = EntityState.Deleted;
                if (autoSave) _dbContext.SaveChanges();
            }
            else
            {
                _dbContext.Set<T>().Remove(entity);
                if (autoSave) _dbContext.SaveChanges();
            }
        }

        public async Task DeleteAsync(T entity, bool autoSave = true)
        {
            if (entity is AuditableEntity auditableEntity)
            {
                auditableEntity.IsDeleted = true;
                _dbContext.Entry(entity).State = EntityState.Deleted;
                if (autoSave) await _dbContext.SaveChangesAsync();
            }
            else
            {
                _dbContext.Set<T>().Remove(entity);
                if (autoSave) await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteRangeAsync(IEnumerable<T> entities, bool autoSave = true)
        {
            foreach (var entity in entities)
            {
                if (entity is AuditableEntity auditableEntity)
                {
                    auditableEntity.IsDeleted = true;
                    _dbContext.Entry(entity).State = EntityState.Modified; // Mark entity as modified to update IsDeleted flag
                }
                else
                {
                    _dbContext.Set<T>().Remove(entity);
                }
            }

            if (autoSave)
            {
                await _dbContext.SaveChangesAsync();
            }
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> where)
        {
            return _dbContext.Set<T>().Where(where);
        }
    }
}
