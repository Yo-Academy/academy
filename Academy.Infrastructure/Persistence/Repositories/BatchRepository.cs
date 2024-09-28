using Academy.Application.Contracts.Persistence;
using Academy.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity = Academy.Domain.Entities;


namespace Academy.Infrastructure.Persistence.Repositories
{
    public class BatchRepository : Repository<Entity.Batch>, IBatchRepository
    {
        protected readonly ICurrentUser _currentUser;

        public BatchRepository(ApplicationDbContext dbContext,
            ICurrentUser currentUser) : base(dbContext)
        {
            _currentUser = currentUser;
        }

        //public IQueryable<Entity.Batch> GetBatchList()
        //{
        //  //  return _dbContext.Batches.Include(x => x.Sports).AsQueryable();   //Where(x => !x.IsDeleted).OrderByDescending(x => x.SentOn).AsQueryable();
        //}
       
    }

}
