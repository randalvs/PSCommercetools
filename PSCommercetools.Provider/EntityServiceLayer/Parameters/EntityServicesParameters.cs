namespace PSCommercetools.Provider.EntityServiceLayer.Parameters;

public sealed class EntityServicesParameters : IEntityServiceParameters
{
    public bool WithCount { get; init; }
    public long? Limit { get; init; }
    public string? Filter { get; init; }
    public long? Offset { get; init; }
    public bool WithTotal { get; init; }
    public string? Sort { get; init; }
    public string[]? Expands { get; init; }
}