using Academy.Application.Sport.Dto;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace Academy.Application.Sport.Command.Models
{
    public class UpdateSportRequest : IRequest<Result<SportsDto>>
    {
        [JsonIgnore]
        public DefaultIdType Id { get; set; }
        public string Name { get; set; }
        public IFormFile? Image { get; set; } = default!;
        public bool IsActive { get; set; }
    }
}
