namespace PSCommercetools.Provider.EntityServiceLayer.Models;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
public sealed class ProjectChildEntity(string name)
{
    public string Name { get; } = name;
    public string? ChildCount { get; set; }
}