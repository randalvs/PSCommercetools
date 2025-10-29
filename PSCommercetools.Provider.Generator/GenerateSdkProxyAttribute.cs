using System;

namespace PSCommercetools.Provider.Generator;

[AttributeUsage(AttributeTargets.Class)]
public sealed class GenerateSdkProxyAttribute : Attribute
{
    public string EntityName { get; set; } = string.Empty;

    public bool SupportsCreate { get; set; } = true;

    public string? SubtypeToReturnFromCreate { get; set; }

    public string? EntityNamePlural { get; set; }

    public string? CommercetoolsSdkModelNamespaceEntityName { get; set; }

    public string? CommercetoolsSdkPagedQueryResponseName { get; set; }
}