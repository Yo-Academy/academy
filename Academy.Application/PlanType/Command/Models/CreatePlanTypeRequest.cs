using Academy.Application.PlanType.Command.Validators;
using Academy.Application.PlanType.Dto;

namespace Academy.Application.PlanType.Command.Models
{
    public class CreatePlanTypeRequest : IRequest<Result<PlanTypeDto>>
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
