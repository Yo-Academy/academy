using Academy.Application.Batch.Command.Models;
using Academy.Application.Persistence.Repository;
using Entites = Academy.Domain.Entities;

namespace Academy.Application.Batch.Command.Validators
{
    public class UpdateBatchRequestValidator : CustomValidator<UpdateBatchRequest>
    {
        public UpdateBatchRequestValidator(IRepository<Entites.Batch> repository)
        {

        }
    }
}
