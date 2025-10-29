using System;

namespace PSCommercetools.Provider.Tests.Generator;

public sealed class TestDataProviderMeta
{
    public TestDataProviderMeta(
        bool hasKey,
        bool hasVersion,
        bool hasContainer,
        bool hasLastModifiedAt,
        string? entityName,
        string? entityNamePlural,
        string? commercetoolsSdkModelNamespaceEntityName,
        string? commercetoolsSdkPagedQueryResponseName)
    {
        HasKey = hasKey;
        HasVersion = hasVersion;
        HasContainer = hasContainer;
        HasLastModifiedAt = hasLastModifiedAt;
        EntityName = entityName ?? throw new ArgumentNullException(entityName);
        EntityNamePlural = entityNamePlural ?? $"{entityName}s";
        EntityInterfaceName = $"I{entityName}";
        CommercetoolsSdkModelNamespaceEntityName = commercetoolsSdkModelNamespaceEntityName ?? EntityNamePlural;
        CommercetoolsSdkPagedQueryResponseName = commercetoolsSdkPagedQueryResponseName ?? $"{EntityName}PagedQueryResponse";
    }

    public bool HasKey { get; }
    public bool HasVersion { get; }
    public bool HasContainer { get; }
    public bool HasLastModifiedAt { get; }
    public string EntityName { get; }
    public string EntityNamePlural { get; }
    public string EntityInterfaceName { get; }
    public string CommercetoolsSdkModelNamespaceEntityName { get; }
    public string CommercetoolsSdkPagedQueryResponseName { get; }
}