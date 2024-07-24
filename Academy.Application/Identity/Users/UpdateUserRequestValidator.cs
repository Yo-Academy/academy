namespace Academy.Application.Identity.Users
{
    public class UpdateUserRequestValidator : CustomValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator(IUserService userService)
        {
            RuleFor(p => p.Id)
                .NotEmpty();

            RuleFor(p => p.FirstName)
                .NotEmpty()
                .MaximumLength(75);

            RuleFor(p => p.LastName)
                .NotEmpty()
                .MaximumLength(75);

            RuleFor(p => p.Image);

            RuleFor(u => u.PhoneNumber).Cascade(CascadeMode.Stop)
                .MinimumLength(10)
                .MaximumLength(10)
                .Matches(@"^[0-9]*$").WithMessage("InvalidPhoneNumberMsg")
                .MustAsync(async (user, phone, _) => !await userService.ExistsWithPhoneNumberAsync(phone!, user.Id))
                    .WithMessage((_, phone) => string.Format(DbRes.T("PhoneNumberAlreadyRegisteredWithParamsMsg"), phone))
                    .Unless(u => string.IsNullOrWhiteSpace(u.PhoneNumber));
        }
    }
}