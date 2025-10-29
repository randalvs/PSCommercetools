using System;

namespace PSCommercetools.Provider.Generator;

[AttributeUsage(AttributeTargets.Class)]
public sealed class GenerateEntityAttribute : Attribute
{
    public string EntityName { get; set; } = string.Empty;

    public string? EntityNamePlural { get; set; }

    public string? CommercetoolsSdkModelNamespaceEntityName { get; set; }

    public bool SkipEntityService { get; set; }
}