using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PSCommercetools.Provider.Generator.Extensions;

namespace PSCommercetools.Provider.Generator;

internal sealed class SdkProxyMetadata
{
    private SdkProxyMetadata(
        string? entityName,
        string? entityNamePlural,
        bool? supportsCreate,
        string? subtypeToReturnFromCreate,
        string? commercetoolsSdkModelNamespaceEntityName,
        string? commercetoolsSdkPagedQueryResponseName)
    {
        EntityName = entityName ?? throw new ArgumentNullException(entityName);
        EntityNamePlural = entityNamePlural ?? $"{entityName}s";
        EntityInterfaceName = $"I{entityName}";
        SupportsCreate = supportsCreate ?? true;
        SubtypeToReturnFromCreate = subtypeToReturnFromCreate is null ? string.Empty : $".{subtypeToReturnFromCreate}";
        CommercetoolsSdkModelNamespaceEntityName = commercetoolsSdkModelNamespaceEntityName ?? EntityNamePlural;
        CommercetoolsSdkPagedQueryResponseName = commercetoolsSdkPagedQueryResponseName ?? $"I{EntityName}PagedQueryResponse";
    }

    public string EntityName { get; }
    public string EntityNamePlural { get; }
    public string EntityInterfaceName { get; }
    public bool SupportsCreate { get; }
    public string? SubtypeToReturnFromCreate { get; }
    public string CommercetoolsSdkModelNamespaceEntityName { get; }
    public string CommercetoolsSdkPagedQueryResponseName { get; }

    public static SdkProxyMetadata CreateFrom(ClassDeclarationSyntax classDeclarationSyntax)
    {
        AttributeSyntax? attribute = classDeclarationSyntax.AttributeLists
            .SelectMany(al => al.Attributes)
            .First(a => a.Name.ToString().Contains("GenerateSdkProxy"));

        SeparatedSyntaxList<AttributeArgumentSyntax> arguments =
            attribute.ArgumentList?.Arguments ?? throw new ArgumentNullException();

        var entityName = arguments.GetAttributeValue<string>(nameof(GenerateSdkProxyAttribute.EntityName));
        var entityNamePlural = arguments.GetAttributeValue<string>(nameof(GenerateSdkProxyAttribute.EntityNamePlural));
        var supportsCreate = arguments.GetAttributeValue<bool?>(nameof(GenerateSdkProxyAttribute.SupportsCreate));
        var subtypeToReturnFromCreate =
            arguments.GetAttributeValue<string>(nameof(GenerateSdkProxyAttribute.SubtypeToReturnFromCreate));
        var commercetoolsSdkModelNamespaceEntityName = arguments.GetAttributeValue<string>(nameof(GenerateSdkProxyAttribute
            .CommercetoolsSdkModelNamespaceEntityName));
        var commercetoolsSdkPagedQueryResponseName = arguments.GetAttributeValue<string>(
            nameof(GenerateSdkProxyAttribute.CommercetoolsSdkPagedQueryResponseName));

        var sdkProxyMetadata = new SdkProxyMetadata(
            entityName,
            entityNamePlural,
            supportsCreate,
            subtypeToReturnFromCreate,
            commercetoolsSdkModelNamespaceEntityName,
            commercetoolsSdkPagedQueryResponseName);

        return sdkProxyMetadata;
    }
}