using Academy.Application.Batch.Command.Models;
using Academy.Application.Persistence.Repository;
using Entites = Academy.Domain.Entities;


namespace Academy.Application.Batch.Command.Validators
{
    public class CreateBatchRequestValidator : CustomValidator<CreateBatchRequest>
    {
        public CreateBatchRequestValidator(IRepository<Entites.Batch> repository)
        {
            RuleFor(p => p.StartTime)
              .NotEmpty();

            RuleFor(p => p.EndTime)
              .NotEmpty();

            RuleFor(p => p.BatchName)
             .NotEmpty()
             .MaximumLength(50);

            RuleFor(p => p.Coaching)
             .NotEmpty();
        }
    }
}
