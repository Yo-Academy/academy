using Academy.Application.Persistence.Repository;
using Academy.Application.UserInfo.Command.Models;
using Entities = Academy.Domain.Entities;


namespace Academy.Application.UserInfo.Command.Validators
{
    public class UpdateUserInfoRequestValidator : CustomValidator<UpdateUserInfoRequest>
    {
        public UpdateUserInfoRequestValidator(IRepository<Entities.UserInfo> repository)
        {
            RuleFor(p => p.UserName)
             .NotEmpty();

            RuleFor(p => p.ContactNo)
             .NotEmpty();

            RuleFor(p => p.City)
             .NotEmpty();

            RuleFor(p => p.DateOfBirth)
            .NotEmpty();

            RuleFor(p => p.DateOfJoin)
            .NotEmpty();

            RuleFor(p => p.Address)
            .NotEmpty();

            RuleFor(p => p.City)
            .NotEmpty();

            RuleFor(p => p.PinCode)
            .NotEmpty();

            RuleFor(p => p.SportsId)
             .NotEmpty();

            RuleFor(p => p.BatchId)
             .NotEmpty();

            RuleFor(p => p.PlanTypeId)
             .NotEmpty();

            RuleFor(p => p.CoachingId)
             .NotEmpty();

            RuleFor(p => p.SubscriptionId)
             .NotEmpty();
        }
    }
}
