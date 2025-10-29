using System.Collections.Generic;
using PSCommercetools.Provider.EntityServiceLayer.Services;

namespace PSCommercetools.Provider.EntityServiceLayer.Models;

internal sealed class EntitiesCarrier
{
    public long? Count { get; init; }
    public long? Offset { get; init; }
    public long? Limit { get; init; }
    public long? Total { get; init; }
    public required IEnumerable<IBaseEntityService> Items { get; init; }
}