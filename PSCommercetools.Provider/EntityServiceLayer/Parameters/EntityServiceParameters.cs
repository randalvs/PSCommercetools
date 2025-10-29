namespace PSCommercetools.Provider.EntityServiceLayer.Parameters;

public sealed class EntityServiceParameters : IEntityServiceParameters
{
    public string[]? Expands { get; init; }
}