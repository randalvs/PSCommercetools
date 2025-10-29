using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PSCommercetools.Provider.Generator;

[Generator]
public class SdkProxyGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<(ClassDeclarationSyntax declaration, SdkProxyMetadata sdkProxyMetadata)> classDeclarations =
            context.SyntaxProvider
                .CreateSyntaxProvider(
                    (node, _) => node is ClassDeclarationSyntax,
                    (transformContext, _) => (ClassDeclarationSyntax)transformContext.Node)
                .Where(c => c.AttributeLists
                    .SelectMany(al => al.Attributes)
                    .Any(a => a.Name.ToString().Contains("GenerateSdkProxy")))
                .Select((declaration, _) => (declaration, SdkProxyMetadata.CreateFrom(declaration)));

        context.RegisterSourceOutput(classDeclarations, (productionContext, source) =>
        {
            (ClassDeclarationSyntax classDeclaration, SdkProxyMetadata sdkProxyMetadata) = source;

            string className = classDeclaration.Identifier.Text;
            string sourceCode = new SdkProxySourceBuilder(sdkProxyMetadata).Build();

            productionContext.AddSource($"{className}SdkProxy.g.cs", sourceCode);
        });
    }
}