namespace Academy.Infrastructure.Identity
{
    internal static class IdentityResultExtensions
    {
        public static List<string> GetErrors(this IdentityResult result) =>
            result.Errors.Select(e => DbRes.T(e.Description)).ToList();
    }
}