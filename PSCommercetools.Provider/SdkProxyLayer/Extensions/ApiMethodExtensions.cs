using commercetools.Base.Client;

namespace PSCommercetools.Provider.SdkProxyLayer.Extensions;

internal static class ApiMethodExtensions
{
    public static void WithWhereClause<T>(this T instance, string? where) where T : ApiMethod<T>
    {
        if (string.IsNullOrEmpty(where))
        {
            return;
        }

        instance.AddQueryParam("where", where);
    }

    public static void WithLimitClause<T>(this T instance, long? limit) where T : ApiMethod<T>
    {
        var limitString = limit?.ToString();

        if (string.IsNullOrEmpty(limitString))
        {
            return;
        }

        instance.AddQueryParam("limit", limitString);
    }

    public static void WithOffsetClause<T>(this T instance, long? offset) where T : ApiMethod<T>
    {
        var offsetString = offset?.ToString();

        if (string.IsNullOrEmpty(offsetString))
        {
            return;
        }

        instance.AddQueryParam("offset", offsetString);
    }

    public static void WithSortClause<T>(this T instance, string? sort) where T : ApiMethod<T>
    {
        if (string.IsNullOrEmpty(sort))
        {
            return;
        }

        instance.AddQueryParam("sort", sort);
    }

    public static void WithExpandClauses<T>(this T instance, string[]? expands) where T : ApiMethod<T>
    {
        if (expands is null || expands.Length == 0)
        {
            return;
        }

        foreach (string expand in expands)
        {
            instance.AddQueryParam("expand", expand);
        }
    }

    public static void WithWithTotalClause<T>(this T instance, bool? withTotal) where T : ApiMethod<T>
    {
        var withTotalClauseString = withTotal?.ToString();

        if (string.IsNullOrEmpty(withTotalClauseString))
        {
            return;
        }

        instance.AddQueryParam("withTotal", withTotalClauseString);
    }
}