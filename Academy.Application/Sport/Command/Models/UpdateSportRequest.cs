using Academy.Application.Sport.Dto;
using System.Text.Json.Serialization;

namespace Academy.Application.Sport.Command.Models
{
    public class UpdateSportRequest : IRequest<Result<SportsDto>>
    {
        [JsonIgnore]
        public DefaultIdType Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
    }
}
