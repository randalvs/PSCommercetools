using commercetools.Base.Client;
using PSCommercetools.Provider.SdkProxyLayer.Extensions;

namespace PSCommercetools.Provider.SdkProxyLayer;

internal class SdkProxyBase
{
    protected static void AddWhereClause<T>(T instance, string? where) where T : ApiMethod<T>
    {
        instance.WithWhereClause(where);
    }

    protected static void AddLimitClause<T>(T instance, long? limit) where T : ApiMethod<T>
    {
        instance.WithLimitClause(limit);
    }

    protected static void AddOffsetClause<T>(T instance, long? offset) where T : ApiMethod<T>
    {
        instance.WithOffsetClause(offset);
    }

    protected static void AddSortClause<T>(T instance, string? sortClause) where T : ApiMethod<T>
    {
        instance.WithSortClause(sortClause);
    }

    protected static void AddExpandClauses<T>(T instance, string[]? expands) where T : ApiMethod<T>
    {
        instance.WithExpandClauses(expands);
    }

    protected static void AddWithTotalClause<T>(T instance, bool? withTotal) where T : ApiMethod<T>
    {
        instance.WithWithTotalClause(withTotal);
    }
}