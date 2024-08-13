using Academy.Application.Persistence.Repository;
using Academy.Application.Sport.Command.Models;
using Academy.Domain.Entities;

namespace Academy.Application.Sport.Command.Validators
{
    public class UpdateSportRequestValidator : CustomValidator<UpdateSportRequest>
    {
        public UpdateSportRequestValidator(IRepository<Sports> repository)
        {

        }
    }
}
