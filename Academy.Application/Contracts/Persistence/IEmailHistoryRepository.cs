using Academy.Application.EmailHistory;
using Entity = Academy.Domain.Entities;

namespace Academy.Application.Contracts.Persistence
{
    public interface IEmailHistoryRepository : IScopedService, IRepository<Entity.EmailHistory>
    {
        Task SaveEmailAsync(List<string> to, string body, string fromMail, bool IsSent, List<string> cc, List<string> bcc, string subject);

        IQueryable<Entity.EmailHistory> GetList();

        Task<Entity.EmailHistory> GetEmailHistoryByIdAsync(Guid id);

        byte[] Compress(byte[] input);

        byte[] Decompress(byte[] input);
    }
}
