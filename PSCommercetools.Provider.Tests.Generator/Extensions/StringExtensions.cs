namespace PSCommercetools.Provider.Tests.Generator.Extensions;

public static class StringExtensions
{
    public static string FirstCharToLower(this string input)
    {
        return input[0].ToString().ToLower() + input.Substring(1);
    }
}