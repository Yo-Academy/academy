namespace Academy.Application.CommonLookups.Command.Validators
{
    public class CommonLookUpTranslationModel
    {
        public string LanguageCode { get; set; } = default!;
        public string Value { get; set; } = default!;
    }

    public class CommonLookUpTranslationModelValidator : CustomValidator<CommonLookUpTranslationModel>
    {
        public CommonLookUpTranslationModelValidator()
        {
            RuleFor(p => p.LanguageCode)
           .NotEmpty()
               .WithMessage(DbRes.T("LookupLanguageRequiredMsg"));

            RuleFor(p => p.Value)
            .NotEmpty()
                .WithMessage(DbRes.T("LookupValueRequiredMsg"));

        }
    }
}
