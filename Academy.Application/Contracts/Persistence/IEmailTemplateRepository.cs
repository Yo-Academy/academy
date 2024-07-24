using Academy.Domain.Entities;

namespace Academy.Application.Contracts.Persistence
{
    public interface IEmailTemplateRepository : IScopedService, IRepository<EmailTemplate>
    {
        Task<EmailTemplate> GetEmailTemplateByCodeAsync(string code);
        IQueryable<EmailTemplate> GetList();

        Task<bool> CheckEmailTemplateExist(string code, Guid? id = default);
    }
}
