using Academy.Application.Persistence.Repository;
using Academy.Application.PlanType.Command.Models;
using Entites = Academy.Domain.Entities;

namespace Academy.Application.PlanType.Command.Validators
{
    public class CreatePlanTypeRequestValidator : CustomValidator<CreatePlanTypeRequest>
    {
        public CreatePlanTypeRequestValidator(IRepository<Entites.PlanType> repository)
        {
            RuleFor(m => m.Code).NotEmpty();
            RuleFor(m => m.Name).NotEmpty();
        }
    }
}
