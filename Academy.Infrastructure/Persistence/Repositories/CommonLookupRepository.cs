using Academy.Application.Contracts.Persistence;
using Academy.Infrastructure.Persistence.Context;


namespace Academy.Infrastructure.Persistence.Repositories
{
    public class CommonLookupRepository : Repository<CommonLookup>, ICommonLookupRepository
    {
        public CommonLookupRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<CommonLookup> GetList()
        {
            return _dbContext.CommonLookups.Where(x => !x.IsDeleted).OrderBy(x => x.DisplayOrder).AsQueryable();
        }

        public async Task<CommonLookup> GetDetailsByIdAsync(Guid id)
        {
            var data = await _dbContext.CommonLookups.Where(x => !x.IsDeleted && x.Id == id).Include(x => x.CommonLookupTranslations).FirstOrDefaultAsync();
            return data ?? throw new NotFoundException(DbRes.T("CommonLookupDoesNotExistMsg"));
        }

        public async Task<bool> CheckLookupExist(string category, string key, Guid? id = default)
        {
            return await _dbContext.CommonLookups.Where(x => x.Category == category && x.Key == key && (!id.HasValue || id != x.Id) && !x.IsDeleted).AnyAsync();
        }
    }
}
