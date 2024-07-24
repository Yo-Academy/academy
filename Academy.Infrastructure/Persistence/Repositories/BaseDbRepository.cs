using Academy.Application.Persistence.Repository;
using Academy.Domain.Common.Contracts;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;

namespace Academy.Infrastructure.Persistence.Repository
{
    public abstract class BaseDbRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T>
        where T : class, IAggregateRoot
    {
        public BaseDbRepository(DbContext dbContext) : base(dbContext)
        {

        }

        protected override IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification) =>
            specification.Selector is not null
                ? base.ApplySpecification(specification)
                : ApplySpecification(specification, false)
                    .ProjectToType<TResult>();
    }
}