using System;

namespace PSCommercetools.Provider.Tests.Generator;

[AttributeUsage(AttributeTargets.Class)]
public sealed class GenerateTestDataProviderAttribute : Attribute
{
    public string EntityName { get; set; } = string.Empty;

    public string? EntityNamePlural { get; set; }

    public string? CommercetoolsSdkModelNamespaceEntityName { get; set; }

    public string? CommercetoolsSdkPagedQueryResponseName { get; set; }

    public bool HasKey { get; set; } = true;
    public bool HasVersion { get; set; } = true;
    public bool HasContainer { get; set; }
    public bool HasLastModifiedAt { get; set; } = true;
}