using Academy.Application.Contracts.Persistence;
using Academy.Infrastructure.Persistence.Context;
using System.IO.Compression;
using System.Text;
using Entity = Academy.Domain.Entities;


namespace Academy.Infrastructure.Persistence.Repositories
{
    public class EmailHistoryRepository : Repository<Entity.EmailHistory>, IEmailHistoryRepository
    {
        protected readonly ICurrentUser _currentUser;
        public EmailHistoryRepository(ApplicationDbContext dbContext,
            ICurrentUser currentUser) : base(dbContext)
        {
            _currentUser = currentUser;
        }

        public async Task SaveEmailAsync(List<string> to, string body, string fromMail, bool IsSent, List<string>? cc, List<string>? bcc, string? subject)
        {
            Entity.EmailHistory emailHistory = new();
            emailHistory.ToEmailAddress = string.Join(",", to);
            emailHistory.CCEmailAddress = (cc != null && cc.Count > 0) ? string.Join(",", cc) : null;
            emailHistory.Subject = subject;
            emailHistory.BCCEmailAddress = (bcc != null && bcc.Count > 0) ? string.Join(",", bcc) : null;
            emailHistory.SentBy = _currentUser.GetUserId();
            emailHistory.IsSent = IsSent;
            emailHistory.FromEmailAddress = fromMail;
            emailHistory.SentOn = DateTime.UtcNow;
            emailHistory.Body = Compress(Encoding.UTF8.GetBytes(body));
            //emailHistory.Body = Encoding.UTF8.GetString(Decompress(emailHistory.BlobBody));         
            await base.AddAsync(emailHistory);
        }

        public IQueryable<Entity.EmailHistory> GetList()
        {
            return _dbContext.EmailHistory.Where(x => !x.IsDeleted).OrderByDescending(x => x.SentOn).AsQueryable();
        }

        public async Task<Entity.EmailHistory> GetEmailHistoryByIdAsync(Guid id)
        {
            return await _dbContext.EmailHistory.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
        }

        public byte[] Compress(byte[] input)
        {
            using (MemoryStream outputStream = new MemoryStream())
            {
                using (DeflateStream deflateStream = new DeflateStream(outputStream, CompressionMode.Compress))
                {
                    deflateStream.Write(input, 0, input.Length);
                }

                return outputStream.ToArray();
            }
        }

        public byte[] Decompress(byte[] input)
        {
            using (MemoryStream inputStream = new MemoryStream(input))
            using (MemoryStream outputStream = new MemoryStream())
            {
                using (DeflateStream deflateStream = new DeflateStream(inputStream, CompressionMode.Decompress))
                {
                    deflateStream.CopyTo(outputStream);
                }

                return outputStream.ToArray();
            }
        }
    }
}
