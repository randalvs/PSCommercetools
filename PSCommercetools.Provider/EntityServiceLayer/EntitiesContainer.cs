using System.Collections.Generic;

namespace PSCommercetools.Provider.EntityServiceLayer;

public sealed class EntitiesContainer<T>
{
    public required long Count { get; init; }
    public long? Offset { get; init; }
    public long? Limit { get; init; }
    public long? Total { get; init; }
    public required IList<T> Items { get; init; }
}