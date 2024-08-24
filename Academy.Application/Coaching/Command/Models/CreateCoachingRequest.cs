using Academy.Application.Coaching.Command.Validators;
using Academy.Application.Coaching.Dto;

namespace Academy.Application.Coaching.Command.Models
{
    public class CreateCoachingRequest : IRequest<Result<CoachingDto>>
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
