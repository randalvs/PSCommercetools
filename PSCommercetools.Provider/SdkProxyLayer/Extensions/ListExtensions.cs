using System.Collections.Generic;
using PSCommercetools.Provider.EntityServiceLayer;

namespace PSCommercetools.Provider.SdkProxyLayer.Extensions;

public static class ListExtensions
{
    public static EntitiesContainer<T> ToEntitiesContainer<T>(this IList<T> source, long count, long? offset, long? limit,
        long? total)
    {
        return new EntitiesContainer<T>
        {
            Count = count,
            Offset = offset,
            Limit = limit,
            Total = total,
            Items = source
        };
    }
}