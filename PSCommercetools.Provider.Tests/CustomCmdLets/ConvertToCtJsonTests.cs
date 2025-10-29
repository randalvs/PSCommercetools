using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Net;
using System.Net.Http;
using commercetools.Sdk.Api.Models.Products;
using FluentAssertions;
using FluentAssertions.Execution;
using PSCommercetools.Provider.Tests.Extensions;
using PSCommercetools.Provider.Tests.Infrastructure;
using PSCommercetools.Provider.Tests.TestDataProviders;
using RichardSzalay.MockHttp;
using Xunit;

namespace PSCommercetools.Provider.Tests.CustomCmdLets;

public sealed class ConvertToCtJsonTests
{
    private readonly TestHost testHost = new TestHost().Initialize().WithTestPSDrive().Reset();

    [Fact]
    public void Should_Return_Pretty_Json_From_Pipe_Input()
    {
        // Arrange
        IProduct product = ProductTestDataProvider.Get();
        string productJson = product.ToCommercetoolsJson(true);
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Head, $"*/products/{product.Id}")
            .Respond(HttpStatusCode.OK, _ => new StringContent(string.Empty));
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Get, $"*/products/{product.Id}")
            .Respond(HttpStatusCode.OK, _ => product.ToCommercetoolsJsonContent());
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Get, $"*/products/{product.Id}")
            .Respond(HttpStatusCode.OK, _ => product.ToCommercetoolsJsonContent());

        // Act
        Collection<PSObject> psObjects = testHost
            .InvokePipeline(cb => cb
                .WithCommand("Get-ChildItem", p => p
                    .WithParameter("Path", @$"ct-test:\products\{product.Id}")
                )
                .WithCommand("ConvertTo-CtJson")
            );

        // Assert
        using var _ = new AssertionScope();
        var actualJson = psObjects.GetFirstBaseObjectAs<string>();

        actualJson.Should().Be(productJson);
    }

    [Fact]
    public void Should_Return_Pretty_Json_From_Path_Input()
    {
        // Arrange
        IProduct product = ProductTestDataProvider.Get();
        string productJson = product.ToCommercetoolsJson(true);
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Head, $"*/products/{product.Id}")
            .Respond(HttpStatusCode.OK, _ => new StringContent(string.Empty));
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Get, $"*/products/{product.Id}")
            .Respond(HttpStatusCode.OK, _ => product.ToCommercetoolsJsonContent());

        // Act
        Collection<PSObject> psObjects = testHost
            .InvokeCommand("ConvertTo-CtJson", p => p
                .WithParameter("Path", @$"ct-test:\products\{product.Id}")
            );

        // Assert
        using var _ = new AssertionScope();
        var actualJson = psObjects.GetFirstBaseObjectAs<string>();

        actualJson.Should().Be(productJson);
    }

    [Fact]
    public void Should_Return_Json_From_Path_Input()
    {
        // Arrange
        IProduct product = ProductTestDataProvider.Get();
        string productJson = product.ToCommercetoolsJson(false);
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Head, $"*/products/{product.Id}")
            .Respond(HttpStatusCode.OK, _ => new StringContent(string.Empty));
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Get, $"*/products/{product.Id}")
            .Respond(HttpStatusCode.OK, _ => product.ToCommercetoolsJsonContent());

        // Act
        Collection<PSObject> psObjects = testHost
            .InvokeCommand("ConvertTo-CtJson", p => p
                .WithParameter("Path", @$"ct-test:\products\{product.Id}")
                .WithParameter("Prettify", false)
            );

        // Assert
        using var _ = new AssertionScope();
        var actualJson = psObjects.GetFirstBaseObjectAs<string>();

        actualJson.Should().Be(productJson);
    }
}