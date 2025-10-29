using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using commercetools.Sdk.Api.Models.Products;
using FluentAssertions;
using FluentAssertions.Execution;
using PSCommercetools.Provider.Tests.Infrastructure;
using PSCommercetools.Provider.Tests.TestDataProviders;
using RichardSzalay.MockHttp;
using Xunit;

namespace PSCommercetools.Provider.Tests.CustomCmdLets;

public sealed class UnpublishProductTests
{
    private readonly TestHost testHost = new TestHost().Initialize().WithTestPSDrive().Reset();

    [Fact]
    public void Should_Unpublish_Product_From_Pipe_Input()
    {
        ProductUpdate? actualProductUpdate = null;

        // Arrange
        IProduct product = ProductTestDataProvider.Get();
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Head, $"*/products/{product.Id}")
            .Respond(HttpStatusCode.OK, _ => new StringContent(string.Empty));
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Get, $"*/products/{product.Id}")
            .Respond(HttpStatusCode.OK, _ => product.ToCommercetoolsJsonContent());
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Get, $"*/products/{product.Id}")
            .Respond(HttpStatusCode.OK, _ => product.ToCommercetoolsJsonContent());
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Post, $"*/products/{product.Id}")
            .Respond(HttpStatusCode.OK, request =>
            {
                actualProductUpdate = request.Content.ToCommercetoolsEntity<ProductUpdate>();

                return product.ToCommercetoolsJsonContent();
            });

        // Act
        testHost
            .InvokePipeline(cb => cb
                .WithCommand("Get-ChildItem", p => p
                    .WithParameter("Path", @$"ct-test:\products\{product.Id}")
                )
                .WithCommand("Unpublish-Product")
            );

        // Assert
        using var _ = new AssertionScope();
        testHost.CommercetoolsMockHttpMessageHandler.VerifyNoOutstandingExpectation();
        actualProductUpdate.Should().BeEquivalentTo(
            new ProductUpdate
            {
                Version = product.Version,
                Actions = new List<IProductUpdateAction>
                {
                    new ProductUnpublishAction()
                }
            }, options => options.RespectingRuntimeTypes());
    }

    [Fact]
    public void Should_Unpublish_Product_From_Path_Input()
    {
        ProductUpdate? actualProductUpdate = null;

        // Arrange
        IProduct product = ProductTestDataProvider.Get();
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Head, $"*/products/{product.Id}")
            .Respond(HttpStatusCode.OK, _ => new StringContent(string.Empty));
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Get, $"*/products/{product.Id}")
            .Respond(HttpStatusCode.OK, _ => product.ToCommercetoolsJsonContent());
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Post, $"*/products/{product.Id}")
            .Respond(HttpStatusCode.OK, request =>
            {
                actualProductUpdate = request.Content.ToCommercetoolsEntity<ProductUpdate>();

                return product.ToCommercetoolsJsonContent();
            });

        // Act
        testHost
            .InvokeCommand("Unpublish-Product", p => p
                .WithParameter("Path", @$"ct-test:\products\{product.Id}")
            );

        // Assert
        using var _ = new AssertionScope();
        testHost.CommercetoolsMockHttpMessageHandler.VerifyNoOutstandingExpectation();
        actualProductUpdate.Should().BeEquivalentTo(
            new ProductUpdate
            {
                Version = product.Version,
                Actions = new List<IProductUpdateAction>
                {
                    new ProductUnpublishAction()
                }
            }, options => options.RespectingRuntimeTypes());
    }
}