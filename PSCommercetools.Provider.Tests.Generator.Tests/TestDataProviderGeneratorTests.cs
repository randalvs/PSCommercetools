using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using PSCommercetools.Provider.Generator.Tests.Shared;
using Xunit;
using Xunit.Abstractions;

namespace PSCommercetools.Provider.Tests.Generator.Tests;

public sealed class TestDataProviderGeneratorTests
{
    private readonly ITestOutputHelper testOutputHelper;

    public TestDataProviderGeneratorTests(ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
    }

    [Theory]
    [InlineData("ApiClientTestData")]
    [InlineData("AttributeGroupTestData")]
    [InlineData("BusinessUnitTestData")]
    [InlineData("CartTestData")]
    [InlineData("CartDiscountTestData")]
    [InlineData("CategoryTestData")]
    [InlineData("ChannelTestData")]
    [InlineData("CustomerTestData")]
    [InlineData("CustomObjectTestData")]
    [InlineData("DiscountCodeTestData")]
    [InlineData("InventoryEntryTestData")]
    [InlineData("OrderTestData")]
    [InlineData("ProductTestData")]
    [InlineData("ProductDiscountTestData")]
    [InlineData("ProductSelectionTestData")]
    [InlineData("ProductTypeTestData")]
    [InlineData("ShippingMethodTestData")]
    [InlineData("ShoppingListTestData")]
    [InlineData("StandalonePriceTestData")]
    [InlineData("StoreTestData")]
    [InlineData("TaxCategoryTestData")]
    public void GenerateTestDataProviders(string className)
    {
        var cSharpSourceFileFinder = new CSharpSourceFileFinder();

        var generator = new TestDataProviderGenerator();

        var driver = CSharpGeneratorDriver.Create(generator);

        var compilation = CSharpCompilation.Create(nameof(TestDataProviderGeneratorTests),
            [
                cSharpSourceFileFinder.Find(className).ToSyntaxTree()
            ],
            [
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(GenerateTestDataProviderAttribute).Assembly.Location)
            ]);

        GeneratorDriverRunResult runResult = driver.RunGenerators(compilation).GetRunResult();

        SyntaxTree generatedFileSyntax =
            runResult.GeneratedTrees.Single(t => t.FilePath.EndsWith($"{className}Provider.g.cs"));

        var code = generatedFileSyntax.GetText().ToString();

        testOutputHelper.WriteLine(code);
    }
}