using Academy.Application.Contracts.Persistence;
using Academy.Infrastructure.Persistence.Context;

namespace Academy.Infrastructure.Persistence.Repositories
{
    public class EmailTemplateRepository : Repository<EmailTemplate>, IEmailTemplateRepository
    {
        public EmailTemplateRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<EmailTemplate> GetEmailTemplateByCodeAsync(string code)
        {
            return await _dbContext.EmailTemplates.Where(x => x.TemplateCode == code && !x.IsDeleted).FirstOrDefaultAsync();
        }

        public IQueryable<EmailTemplate> GetList()
        {
            return _dbContext.EmailTemplates.Where(x => !x.IsDeleted).OrderByDescending(x => x.CreatedOn).AsQueryable();
        }

        public async Task<bool> CheckEmailTemplateExist(string code, Guid? id = default)
        {
            return await _dbContext.EmailTemplates.Where(x => x.TemplateCode == code && (!id.HasValue || id != x.Id) && !x.IsDeleted).AnyAsync();
        }
    }
}
