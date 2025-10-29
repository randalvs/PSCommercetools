namespace PSCommercetools.Provider.FormatFileGenerator.Models;

internal sealed class EntitiesGroup
{
    public required List<string> TypeNames { get; init; }
    public List<Property>? Properties { get; init; }

    public required bool HasListView { get; init; }
}