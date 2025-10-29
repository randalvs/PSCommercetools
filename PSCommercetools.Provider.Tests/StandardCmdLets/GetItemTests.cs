using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Net.Http;
using commercetools.Sdk.Api.Models.ApiClients;
using commercetools.Sdk.Api.Models.Carts;
using commercetools.Sdk.Api.Models.Products;
using FluentAssertions;
using FluentAssertions.Execution;
using PSCommercetools.Provider.Tests.Extensions;
using PSCommercetools.Provider.Tests.Infrastructure;
using PSCommercetools.Provider.Tests.TestDataProviders;
using RichardSzalay.MockHttp;
using Xunit;

namespace PSCommercetools.Provider.Tests.StandardCmdLets;

public sealed class GetItemTests
{
    private readonly TestHost testHost = new TestHost().Initialize().WithTestPSDrive().Reset();

    [Fact]
    public void Should_Return_Cart()
    {
        // Arrange
        ICart cart = CartTestDataProvider.Get();
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Head, $"*/carts/{cart.Id}")
            .Respond(HttpStatusCode.OK, _ => new StringContent(string.Empty));
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Get, $"*/carts/{cart.Id}")
            .Respond(HttpStatusCode.OK, _ => cart.ToCommercetoolsJsonContent());

        // Act
        Collection<PSObject> psObjects = testHost
            .InvokeCommand("Get-Item", p => p
                .WithParameter("Path", @$"ct-test:\carts\{cart.Id}")
            );

        // Assert
        using var _ = new AssertionScope();
        testHost.CommercetoolsMockHttpMessageHandler.VerifyNoOutstandingExpectation();
        psObjects.BaseObjectsAreAllOfType<ICart>().Should().BeTrue();
        psObjects.GetBaseObjects<ICart>().Should().HaveCount(1);
    }

    [Fact]
    public void Should_Return_Product()
    {
        // Arrange
        IProduct product = ProductTestDataProvider.Get();
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Head, $"*/products/{product.Id}")
            .Respond(HttpStatusCode.OK, _ => new StringContent(string.Empty));
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Get, $"*/products/{product.Id}")
            .Respond(HttpStatusCode.OK, _ => product.ToCommercetoolsJsonContent());

        // Act
        Collection<PSObject> psObjects = testHost
            .InvokeCommand("Get-Item", p => p
                .WithParameter("Path", @$"ct-test:\products\{product.Id}")
            );

        // Assert
        using var _ = new AssertionScope();
        testHost.CommercetoolsMockHttpMessageHandler.VerifyNoOutstandingExpectation();
        psObjects.BaseObjectsAreAllOfType<IProduct>().Should().BeTrue();
        psObjects.GetBaseObjects<IProduct>().Should().HaveCount(1);
    }

    [Fact]
    public void Should_Apply_Expands()
    {
        // Arrange
        ICart cart = CartTestDataProvider.Get();
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Head, $"*/carts/{cart.Id}")
            .Respond(HttpStatusCode.OK, _ => new StringContent(string.Empty));
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Get, $"*/carts/{cart.Id}")
            .WithQueryString("expand", "paymentInfo.payments[*].paymentStatus.state")
            .Respond(HttpStatusCode.OK, _ => cart.ToCommercetoolsJsonContent());

        // Act
        testHost
            .InvokeCommand("Get-Item", p => p
                .WithParameter("Path", $@"ct-test:\carts\{cart.Id}")
                .WithParameter("Expands", "paymentInfo.payments[*].paymentStatus.state")
            );

        // Assert
        using var _ = new AssertionScope();
        testHost.CommercetoolsMockHttpMessageHandler.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public void Should_Return_Error()
    {
        // Arrange
        ICart cart = CartTestDataProvider.Get();
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Head, $"*/carts/{cart.Id}")
            .Respond(HttpStatusCode.InternalServerError);

        // Act
        testHost
            .InvokeCommand("Get-Item", p => p
                .WithParameter("Path", $@"ct-test:\carts\{cart.Id}")
            );

        // Assert
        using var _ = new AssertionScope();
        testHost.HasErrors.Should().BeTrue();
    }

    [Fact]
    public void Should_Return_ApiClient_By_Id()
    {
        // Arrange
        IEnumerable<IApiClient> apiClients = ApiClientTestDataProvider.Get(30).ToList();
        IApiClient apiClient = apiClients.Skip(10).First();
        // Resemble real word api client id with mixed casing.
        apiClient.Id = "ABOqM8q1v6aV9PxCyafdvvnL";

        testHost.CommercetoolsMockHttpMessageHandler.Expect(HttpMethod.Get, "*/api-clients")
            .WithQueryString("limit", "500")
            .WithQueryString("sort", "id")
            .WithQueryString("withTotal", "False")
            .Respond(HttpStatusCode.OK, _ => apiClients.AsPagedQueryResponse().ToCommercetoolsJsonContent());

        testHost.CommercetoolsMockHttpMessageHandler.Expect(HttpMethod.Get, "*/api-clients")
            .WithQueryString("limit", "500")
            .WithQueryString("sort", "id")
            .WithQueryString("withTotal", "False")
            .Respond(HttpStatusCode.OK, _ => apiClients.AsPagedQueryResponse().ToCommercetoolsJsonContent());

        // Act
        // Get the api client using a lower case path.
        Collection<PSObject> psObjects = testHost
            .InvokeCommand("Get-Item", p => p
                .WithParameter("Path", @$"ct-test:\apiclients\{apiClient.Id.ToLowerInvariant()}")
            );

        // Assert
        using var _ = new AssertionScope();
        testHost.CommercetoolsMockHttpMessageHandler.VerifyNoOutstandingExpectation();
        psObjects.BaseObjectsAreAllOfType<IApiClient>().Should().BeTrue();
        psObjects.GetBaseObjects<IApiClient>().Should().HaveCount(1);
    }

    [Fact]
    public void Should_Return_ApiClient_By_Id_From_Large_Set()
    {
        // Arrange
        IEnumerable<IApiClient> apiClients = ApiClientTestDataProvider.Get(600).ToList();
        IApiClient apiClient = apiClients.Skip(550).First();
        // Resemble real word api client id with mixed casing.
        apiClient.Id = "ABOqM8q1v6aV9PxCyafdvvnL";

        string? sortId = apiClients.Skip(499).First().Id;

        testHost.CommercetoolsMockHttpMessageHandler.Expect(HttpMethod.Get, "*/api-clients")
            .WithQueryString("limit", "500")
            .WithQueryString("sort", "id")
            .WithQueryString("withTotal", "False")
            .Respond(HttpStatusCode.OK, _ => apiClients.Take(500).AsPagedQueryResponse(500).ToCommercetoolsJsonContent());

        testHost.CommercetoolsMockHttpMessageHandler.Expect(HttpMethod.Get, "*/api-clients")
            .WithQueryString("limit", "500")
            .WithQueryString("sort", "id")
            .WithQueryString("withTotal", "False")
            .WithQueryString("where", $"id > {sortId}")
            .Respond(HttpStatusCode.OK, _ => apiClients.Skip(500).AsPagedQueryResponse(500).ToCommercetoolsJsonContent());

        testHost.CommercetoolsMockHttpMessageHandler.Expect(HttpMethod.Get, "*/api-clients")
            .WithQueryString("limit", "500")
            .WithQueryString("sort", "id")
            .WithQueryString("withTotal", "False")
            .Respond(HttpStatusCode.OK, _ => apiClients.Take(500).AsPagedQueryResponse(500).ToCommercetoolsJsonContent());

        testHost.CommercetoolsMockHttpMessageHandler.Expect(HttpMethod.Get, "*/api-clients")
            .WithQueryString("limit", "500")
            .WithQueryString("sort", "id")
            .WithQueryString("withTotal", "False")
            .WithQueryString("where", $"id > {sortId}")
            .Respond(HttpStatusCode.OK, _ => apiClients.Skip(500).AsPagedQueryResponse(500).ToCommercetoolsJsonContent());

        // Act
        // Get the api client using a lower case path.
        Collection<PSObject> psObjects = testHost
            .InvokeCommand("Get-Item", p => p
                .WithParameter("Path", @$"ct-test:\apiclients\{apiClient.Id.ToLowerInvariant()}")
            );

        // Assert
        using var _ = new AssertionScope();
        testHost.CommercetoolsMockHttpMessageHandler.VerifyNoOutstandingExpectation();
        psObjects.BaseObjectsAreAllOfType<IApiClient>().Should().BeTrue();
        psObjects.GetBaseObjects<IApiClient>().Should().HaveCount(1);
    }
}