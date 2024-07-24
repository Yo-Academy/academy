namespace Academy.Application.Identity.Users
{
    public class CreateUserRequestValidator : CustomValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator(IUserService userService)
        {
            RuleFor(u => u.Email).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .EmailAddress()
                    .WithMessage(DbRes.T("InvalidEmailAddressMsg"))
                .MustAsync(async (email, _) => !await userService.ExistsWithEmailAsync(email))
                    .WithMessage((_, email) => string.Format(DbRes.T("EmailAlreadyRegisteredWithParamsMsg"), email));

            RuleFor(u => u.UserName).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MinimumLength(6)
                .MustAsync(async (name, _) => !await userService.ExistsWithNameAsync(name))
                    .WithMessage((_, name) => string.Format(DbRes.T("UsernameAlreadyTakenWithParamsMsg", name)));

            RuleFor(u => u.PhoneNumber).Cascade(CascadeMode.Stop)
                .MinimumLength(10)
                .MaximumLength(10)
                .Matches(@"^[1-9]+\d{9}").WithMessage("InvalidPhoneNumberMsg")
                .MustAsync(async (phone, _) => !await userService.ExistsWithPhoneNumberAsync(phone!))
                    .WithMessage((_, phone) => string.Format(DbRes.T("PhoneNumberAlreadyRegisteredWithParamsMsg", phone!)))
                    .Unless(u => string.IsNullOrWhiteSpace(u.PhoneNumber));

            RuleFor(p => p.FirstName).Cascade(CascadeMode.Stop)
                .NotEmpty();

            RuleFor(p => p.LastName).Cascade(CascadeMode.Stop)
                .NotEmpty();

            RuleFor(p => p.Password).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MinimumLength(6);

            RuleFor(p => p.ConfirmPassword).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Equal(p => p.Password)
                 .WithMessage(DbRes.T("ConfirmPasswordMismatchMsg"));
        }
    }
}