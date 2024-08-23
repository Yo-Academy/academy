namespace Academy.Application.Identity.Tokens
{
    public record LoginRequest(string PhoneNumber, string Password);

    public class LoginRequestValidator : CustomValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(p => p.PhoneNumber).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MinimumLength(10)
                .MaximumLength(10)
                .Matches(@"^[0-9]*$")
                .WithMessage(DbRes.T("InvalidPhoneNumberMsg"));

            RuleFor(p => p.Password).Cascade(CascadeMode.Stop)
                .NotEmpty();
        }
    }
}