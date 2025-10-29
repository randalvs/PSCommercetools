using PSCommercetools.Provider.EntityServiceLayer.Services;

namespace PSCommercetools.Provider.EntityServiceLayer.Models;

internal sealed class EntityCarrier
{
    public required IBaseEntityService Item { get; init; }
}