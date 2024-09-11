using Academy.Shared;
using System.Text;

namespace Academy.Application.Common.Helpers;

public static class StringExtensions
{
    /// <summary>
    /// Get Bytes of string.
    /// </summary>
    public static byte[] GetBytes(this string value)
    {
        return Encoding.ASCII.GetBytes(value);
    }

    /// <summary>
    /// Get string from Byte.
    /// </summary>
    public static string GetString(this byte[] value)
    {
        return Encoding.UTF8.GetString(value);
    }

    /// <summary>
    /// Indicates whether this string is null or an System.String.Empty string.
    /// </summary>
    public static bool IsNullOrEmpty(this string str)
    {
        return string.IsNullOrEmpty(str);
    }

    /// <summary>
    /// indicates whether this string is null, empty, or consists only of white-space characters.
    /// </summary>
    public static bool IsNullOrWhiteSpace(this string str)
    {
        return string.IsNullOrWhiteSpace(str);
    }

    /// <summary>
    /// Get the subdomain name from string
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string GetSubdomainFromShortName(this string str)
    {
        if (string.IsNullOrWhiteSpace(str))
        {
            return string.Empty;
        }

        // Split the string into words (assuming space as a delimiter)
        var words = str.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        // Get the first character of each word and concatenate them
        var abbreviation = string.Concat(words.Select(word => word[0]));

        return abbreviation.ToLower();
    }

    /// <summary>
    /// Get Localization string From Resources !
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string GetResourcesString(string key, string res = default!)
    {
        return DbRes.T(key, res ?? Constants.LocalizationResource);
    }
}
