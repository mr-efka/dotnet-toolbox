using System.Globalization;

namespace MrEfka.ToolBox.Commons.Extensions;

public static class Strings
{
    ///<summary><inheritdoc cref="TextInfo.ToTitleCase(string)"/></summary>
    ///<param name="value"></param>
    ///<param name="removeSpaces">If <see langword="true"/>, all white-space characters are dropped from the resulting string.</param>
    ///<returns></returns>
    public static string? ToTitleCase(this string? value, bool removeSpaces = false)
    {
        if (value is null) return null;
        if (value.Length == 0) return string.Empty;
            
        var result = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value);
        if (removeSpaces)
            result = System.Text.RegularExpressions.Regex.Replace(
                result, @"\s+", string.Empty,
                System.Text.RegularExpressions.RegexOptions.None, TimeSpan.FromMilliseconds(1000));

        return result;
    }
    ///<summary>Transforms the value, making it human-friendly. Words first characters are uppercase and all white-space characters are dropped.</summary>
    ///<param name="value"></param>
    ///<returns></returns>
    public static string? ToUpperCamelCase(this string? value)
    {
        var result = value != null && System.Text.RegularExpressions.Regex.IsMatch(value, pattern: @"\s+")
            ? ToTitleCase(value, removeSpaces: true)
            : value;

        if (result is null) return null;
        return result.Length <= 1
            ? result.ToUpperInvariant()
            : string.Concat(result[0].ToString().ToUpperInvariant(), result[1..]);
    }
    ///<summary>Transforms the value, making it human-friendly. All white-space characters are dropped and words first characters are uppercase except for the first one.</summary>
    ///<param name="value"></param>
    ///<returns></returns>
    public static string? ToCamelCase(this string? value)
    {
        var result = value != null && System.Text.RegularExpressions.Regex.IsMatch(value, pattern: @"\s+")
            ? ToTitleCase(value, removeSpaces: true)
            : value;

        if (result is null) return null;
        return result.Length <= 1
            ? result.ToLowerInvariant()
            : string.Concat(result[0].ToString().ToLowerInvariant(), result[1..]);
    }
    ///<summary>Transforms the value, making it human-friendly.</summary>
    ///<param name="value"></param>
    ///<param name="separator"></param>
    ///<param name="throwIfValueIsNull"></param>
    ///<param name="lowerCased"></param>
    ///<returns></returns>
    public static string? ToSeparatorCase(this string? value, in char separator, bool throwIfValueIsNull = true, bool lowerCased = true)
    {
        if (value is null) return throwIfValueIsNull ? throw new ArgumentNullException(nameof(value)) : null;
        ArgumentNullException.ThrowIfNull(separator, nameof(separator));

        var result = System.Text.RegularExpressions.Regex.Replace(
            input: value, 
            pattern: "([a-z])([A-Z])", 
            replacement: $"$1{separator}$2",
            options: System.Text.RegularExpressions.RegexOptions.None, 
            matchTimeout: TimeSpan.FromMilliseconds(100));

        return lowerCased ? result.ToLowerInvariant() :result;
    }
    ///<summary>Transforms the value, making it human-friendly. Words are separated by an underscore.</summary>
    ///<param name="value"></param>
    ///<param name="throwIfValueIsNull"></param>
    ///<returns></returns>
    public static string? ToSnakeCase(this string value, bool throwIfValueIsNull = true) => value.ToSeparatorCase('_', throwIfValueIsNull, lowerCased: true);
    ///<summary>Transforms the value, making it human-friendly. Words are separated by a dash.</summary>
    ///<param name="value"></param>
    ///<param name="throwIfValueIsNull"></param>
    ///<returns></returns>
    public static string? ToKebabCase(this string value, bool throwIfValueIsNull = true) => value.ToSeparatorCase('-', throwIfValueIsNull, lowerCased: false);
    ///<summary>Transforms the value, making it human-friendly. Words are separated by a dash and all lowercased.</summary>
    ///<param name="value"></param>
    ///<param name="throwIfValueIsNull"></param>
    ///<returns></returns>
    public static string? ToKebabCaseLower(this string value, bool throwIfValueIsNull = true) => value.ToSeparatorCase('-', throwIfValueIsNull, lowerCased: true);
    ///<summary>Transforms the value, making it human-friendly. Words are separated by an underscore and all lowercased.</summary>
    ///<param name="value"></param>
    ///<returns></returns>
    public static string? Slugify(this string value) => ToKebabCaseLower(value, false);
}