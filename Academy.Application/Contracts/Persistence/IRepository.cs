using System.Linq.Expressions;

namespace Academy.Application.Contracts.Persistence
{
    public interface IRepository<T> where T : class
    {
        T GetById(Guid id);
        Task<T> GetByIdAsync(Guid id);
        Task<IReadOnlyList<T>> ListAllAsync();
        T Add(T entity, bool autoSave = true);
        Task<T> AddAsync(T entity, bool autoSave = true);
        IEnumerable<T> AddRange(IEnumerable<T> entities, bool autoSave = true);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, bool autoSave = true);
        void Update(T entity, bool autoSave = true);
        Task UpdateAsync(T entity, bool autoSave = true);
        void Delete(T entity, bool autoSave = true);
        Task DeleteAsync(T entity, bool autoSave = true);
        Task DeleteRangeAsync(IEnumerable<T> entities, bool autoSave = true);

        IQueryable<T> Get(Expression<Func<T, bool>> where);
    }
}
