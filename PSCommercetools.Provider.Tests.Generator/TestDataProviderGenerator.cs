using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PSCommercetools.Provider.Tests.Generator.Extensions;

namespace PSCommercetools.Provider.Tests.Generator;

[Generator]
public class TestDataProviderGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<(ClassDeclarationSyntax declaration, TestDataProviderMeta testDataProviderMeta)>
            classDeclarations =
                context.SyntaxProvider
                    .CreateSyntaxProvider(
                        (node, _) => node is ClassDeclarationSyntax,
                        (transformContext, _) => (ClassDeclarationSyntax)transformContext.Node)
                    .Where(c => c.AttributeLists
                        .SelectMany(al => al.Attributes)
                        .Any(a => a.Name.ToString().Contains("GenerateTestDataProvider")))
                    .Select((declaration, _) => BuildTestDataProviderMetadata(declaration));

        context.RegisterSourceOutput(classDeclarations, (productionContext, source) =>
        {
            (ClassDeclarationSyntax classDeclaration, TestDataProviderMeta testDataProviderMeta) = source;

            string className = classDeclaration.Identifier.Text;
            string sourceCode = new TestDataProviderSourceBuilder(testDataProviderMeta).Build();

            productionContext.AddSource($"{className}Provider.g.cs", sourceCode);
        });
    }

    private static (ClassDeclarationSyntax declaration, TestDataProviderMeta) BuildTestDataProviderMetadata(
        ClassDeclarationSyntax declaration)
    {
        AttributeSyntax? attribute = declaration.AttributeLists
            .SelectMany(al => al.Attributes)
            .First(a => a.Name.ToString().Contains("GenerateTestDataProvider"));

        SeparatedSyntaxList<AttributeArgumentSyntax> arguments =
            attribute.ArgumentList?.Arguments ?? throw new ArgumentNullException();

        bool hasKey = arguments.GetAttributeValue<bool?>(nameof(GenerateTestDataProviderAttribute.HasKey)) ?? true;
        bool hasVersion = arguments.GetAttributeValue<bool?>(nameof(GenerateTestDataProviderAttribute.HasVersion)) ?? true;
        bool hasContainer = arguments.GetAttributeValue<bool?>(nameof(GenerateTestDataProviderAttribute.HasContainer)) ?? false;
        bool hasLastModifiedAt =
            arguments.GetAttributeValue<bool?>(nameof(GenerateTestDataProviderAttribute.HasLastModifiedAt)) ?? true;
        var entityName = arguments.GetAttributeValue<string>(nameof(GenerateTestDataProviderAttribute.EntityName));
        var entityNamePlural = arguments.GetAttributeValue<string>(nameof(GenerateTestDataProviderAttribute.EntityNamePlural));
        var commercetoolsSdkModelNamespaceEntityName = arguments.GetAttributeValue<string>(
            nameof(GenerateTestDataProviderAttribute
                .CommercetoolsSdkModelNamespaceEntityName));
        var commercetoolsSdkPagedQueryResponseName = arguments.GetAttributeValue<string>(
            nameof(GenerateTestDataProviderAttribute.CommercetoolsSdkPagedQueryResponseName));

        return (declaration, new TestDataProviderMeta(
            hasKey,
            hasVersion,
            hasContainer,
            hasLastModifiedAt,
            entityName,
            entityNamePlural,
            commercetoolsSdkModelNamespaceEntityName,
            commercetoolsSdkPagedQueryResponseName));
    }
}