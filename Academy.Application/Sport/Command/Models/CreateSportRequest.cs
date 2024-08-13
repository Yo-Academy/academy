using Academy.Application.Sport.Command.Validators;
using Academy.Application.Sport.Dto;

namespace Academy.Application.Sport.Command.Models
{
    public class CreateSportRequest : IRequest<Result<SportsDto>>
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
    }
}
