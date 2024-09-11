
using Academy.Domain.Common.Contracts;
using Academy.Infrastructure.Persistence.Context;

namespace Academy.Infrastructure.Persistence.Repository
{
    public class DbRepository<T> : BaseDbRepository<T> where T : class, IAggregateRoot
    {
        protected ApplicationDbContext _dbContext { get; set; }
        public DbRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

    }

    public class DbRepositoryWA<T> : BaseDbRepositoryWA<T> where T : class
    {
        protected ApplicationDbContext _dbContext { get; set; }
        public DbRepositoryWA(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

    }
}