using System.Diagnostics;
using System.Text;

namespace Academy.Application.Common.Helpers;

[DebuggerStepThrough]
public static class Check
{
    public static T NotNull<T>(T value, string parameterName)
    {
        if (value == null)
        {
            throw new ArgumentNullException(parameterName);
        }

        return value;
    }

    public static string RemoveSpecialCharacters(this string str)
    {
        StringBuilder sb = new();
        foreach (char c in str)
        {
            if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_' || c == '-')
            {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }
}
