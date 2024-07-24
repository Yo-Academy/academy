using Academy.Application.Auditing;
using Academy.Infrastructure.Persistence.Context;
using Mapster;

namespace Academy.Infrastructure.Auditing
{
    public class AuditService : IAuditService
    {
        private readonly ApplicationDbContext _context;

        public AuditService(ApplicationDbContext context) => _context = context;

        public async Task<List<AuditDto>> GetUserTrailsAsync(DefaultIdType userId)
        {
            var trails = await _context.AuditTrails
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.DateTime)
                .Take(250)
                .ToListAsync();

            return trails.Adapt<List<AuditDto>>();
        }
    }
}