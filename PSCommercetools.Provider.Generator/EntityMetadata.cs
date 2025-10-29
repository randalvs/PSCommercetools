using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PSCommercetools.Provider.Generator.Extensions;

namespace PSCommercetools.Provider.Generator;

internal sealed class EntityMetadata
{
    private EntityMetadata(
        string? entityName,
        string? entityNamePlural,
        string? commercetoolsSdkModelNamespaceEntityName,
        bool? skipEntityService)
    {
        EntityNamePlural = entityNamePlural ?? $"{entityName}s";
        EntityInterfaceName = $"I{entityName}";
        CommercetoolsSdkModelNamespaceEntityName = commercetoolsSdkModelNamespaceEntityName ?? EntityNamePlural;
        SkipEntityService = skipEntityService ?? false;
    }

    public string EntityNamePlural { get; }
    public string EntityInterfaceName { get; }
    public string CommercetoolsSdkModelNamespaceEntityName { get; }
    public bool SkipEntityService { get; }

    public static EntityMetadata CreateFrom(ClassDeclarationSyntax classDeclarationSyntax)
    {
        AttributeSyntax? attribute = classDeclarationSyntax.AttributeLists
            .SelectMany(al => al.Attributes)
            .First(a => a.Name.ToString().Contains("GenerateEntity"));

        SeparatedSyntaxList<AttributeArgumentSyntax> arguments =
            attribute.ArgumentList?.Arguments ?? throw new ArgumentNullException();

        var entityName = arguments.GetAttributeValue<string>(nameof(GenerateEntityAttribute.EntityName));
        var entityNamePlural = arguments.GetAttributeValue<string>(nameof(GenerateEntityAttribute.EntityNamePlural));
        var commercetoolsSdkModelNamespaceEntityName = arguments.GetAttributeValue<string>(nameof(GenerateEntityAttribute
            .CommercetoolsSdkModelNamespaceEntityName));
        var skipEntityService = arguments.GetAttributeValue<bool?>(nameof(GenerateEntityAttribute.SkipEntityService));

        var sdkProxyMetadata = new EntityMetadata(
            entityName,
            entityNamePlural,
            commercetoolsSdkModelNamespaceEntityName,
            skipEntityService);

        return sdkProxyMetadata;
    }
}