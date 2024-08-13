using Academy.Application.Identity.Users.Command.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Application.Identity.Users.Command.Validator
{
    public class UserLoginValidator : CustomValidator<UserLoginRequest>
    {
        public UserLoginValidator(IUserService userService)
        {
            RuleFor(u => u.PhoneNumber).Cascade(CascadeMode.Stop)
               .MinimumLength(10)
               .MaximumLength(10)
               .Matches(@"^[0-9]*$").WithMessage("InvalidPhoneNumberMsg")
               .MustAsync(async (phone, _) => await userService.ExistsWithPhoneNumberAsync(phone!))
                   .WithMessage((_, phone) => string.Format(DbRes.T("PhoneNumberAlreadyRegisteredWithParamsMsg"), phone))
                   .Unless(u => string.IsNullOrWhiteSpace(u.PhoneNumber));
        }
    }
}
