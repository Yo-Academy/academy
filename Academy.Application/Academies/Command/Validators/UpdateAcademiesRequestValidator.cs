using Academy.Application.Academies.Command.Models;
using Academy.Application.Persistence.Repository;
using Entites = Academy.Domain.Entities;

namespace Academy.Application.Academies.Command.Validators
{
    public class UpdateAcademiesRequestValidator : CustomValidator<UpdateAcademiesRequest>
    {
        public UpdateAcademiesRequestValidator(IRepository<Entites.Academies> repository)
        {

        }
    }
}
