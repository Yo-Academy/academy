using Academy.Application.Sport.Command.Validators;
using Academy.Application.Sport.Dto;
using Microsoft.AspNetCore.Http;

namespace Academy.Application.Sport.Command.Models
{
    public class CreateSportRequest : IRequest<Result<SportsDto>>
    {
        public string Name { get; set; }
        public IFormFile? Image { get; set; } = default!;
        public bool IsActive { get; set; }
    }
}
