namespace Academy.Application.Identity.Users.Password
{
    public class ResetPasswordRequest
    {
        public string Email { get; set; } = default!;

        public string Password { get; set; } = default!;

        public string Token { get; set; } = default!;
    }

    public class ResetPasswordRequestValidator : CustomValidator<ResetPasswordRequest>
    {
        public ResetPasswordRequestValidator()
        {
            RuleFor(u => u.Email).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .EmailAddress()
                    .WithMessage(DbRes.T("InvalidEmailAddressMsg"));

            RuleFor(u => u.Password).Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage(DbRes.T("PasswordRequiredMsg"));

            RuleFor(p => p.Token)
                .NotEmpty()
                    .WithMessage(DbRes.T("TokenRequiredMsg"));
        }
    }
}