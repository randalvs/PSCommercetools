namespace PSCommercetools.Provider.PowerShellLayer.Extensions;

public static class StringExtensions
{
    public static string RemoveTrailingSlash(this string value)
    {
        return value.TrimEnd('\\', '/');
    }
}