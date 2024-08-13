namespace Academy.Application.Identity.Tokens
{
    public record LoginRequest(string Phonenumber, string Password);

    public class LoginRequestValidator : CustomValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(p => p.Phonenumber).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .EmailAddress()
                    .WithMessage(DbRes.T("InvalidEmailAddressMsg"));

            RuleFor(p => p.Password).Cascade(CascadeMode.Stop)
                .NotEmpty();
        }
    }
}