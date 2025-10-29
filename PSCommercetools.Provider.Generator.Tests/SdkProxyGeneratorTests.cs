using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using PSCommercetools.Provider.Generator.Tests.Shared;
using Xunit;
using Xunit.Abstractions;

namespace PSCommercetools.Provider.Generator.Tests;

public sealed class SdkProxyGeneratorTests
{
    private readonly ITestOutputHelper testOutputHelper;

    public SdkProxyGeneratorTests(ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
    }

    [Theory]
    [InlineData("AttributeGroup")]
    [InlineData("BusinessUnit")]
    [InlineData("Cart")]
    [InlineData("CartDiscount")]
    [InlineData("Category")]
    [InlineData("Channel")]
    [InlineData("Customer")]
    [InlineData("DiscountCode")]
    [InlineData("InventoryEntry")]
    [InlineData("Order")]
    [InlineData("Product")]
    [InlineData("ProductDiscount")]
    [InlineData("ProductSelection")]
    [InlineData("ProductType")]
    [InlineData("ShippingMethod")]
    [InlineData("ShoppingList")]
    [InlineData("StandalonePrice")]
    [InlineData("Store")]
    [InlineData("TaxCategory")]
    public void GenerateSdkProxies(string className)
    {
        var cSharpSourceFileFinder = new CSharpSourceFileFinder();

        var generator = new SdkProxyGenerator();

        var driver = CSharpGeneratorDriver.Create(generator);

        var compilation = CSharpCompilation.Create(nameof(SdkProxyGeneratorTests),
            [
                cSharpSourceFileFinder.Find(className).ToSyntaxTree()
            ],
            [
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(GenerateSdkProxyAttribute).Assembly.Location)
            ]);

        GeneratorDriverRunResult runResult = driver.RunGenerators(compilation).GetRunResult();

        SyntaxTree generatedFileSyntax = runResult.GeneratedTrees.Single(t => t.FilePath.EndsWith($"{className}SdkProxy.g.cs"));

        var code = generatedFileSyntax.GetText().ToString();

        testOutputHelper.WriteLine(code);
    }
}