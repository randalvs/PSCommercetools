using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Net.Http;
using commercetools.Sdk.Api.Models.Carts;
using commercetools.Sdk.Api.Models.Products;
using FluentAssertions;
using FluentAssertions.Execution;
using PSCommercetools.Provider.EntityServiceLayer.Models;
using PSCommercetools.Provider.Tests.Extensions;
using PSCommercetools.Provider.Tests.Infrastructure;
using PSCommercetools.Provider.Tests.TestDataProviders;
using RichardSzalay.MockHttp;
using Xunit;

namespace PSCommercetools.Provider.Tests.StandardCmdLets;

public sealed class GetChildItemTests
{
    private readonly TestHost testHost = new TestHost().Initialize().WithTestPSDrive().Reset();

    [Fact]
    public void Should_Return_Root_Children()
    {
        // Act
        Collection<PSObject> psObjects = testHost
            .InvokeCommand("Get-ChildItem", p => p
                .WithParameter("Path", @"ct-test:\")
            );

        // Assert
        using var _ = new AssertionScope();
        psObjects.BaseObjectsAreAllOfType<ProjectChildEntity>().Should().BeTrue();
        psObjects.GetBaseObjects<ProjectChildEntity>().Should().BeEquivalentTo(
        [
            new ProjectChildEntity("products"),
            new ProjectChildEntity("categories"),
            new ProjectChildEntity("inventory"),
            new ProjectChildEntity("carts"),
            new ProjectChildEntity("orders"),
            new ProjectChildEntity("customers"),
            new ProjectChildEntity("businessunits"),
            new ProjectChildEntity("customobjects"),
            new ProjectChildEntity("channels"),
            new ProjectChildEntity("stores"),
            new ProjectChildEntity("taxcategories"),
            new ProjectChildEntity("shoppinglists"),
            new ProjectChildEntity("productselections"),
            new ProjectChildEntity("shippingmethods"),
            new ProjectChildEntity("standaloneprices"),
            new ProjectChildEntity("productdiscounts"),
            new ProjectChildEntity("cartdiscounts"),
            new ProjectChildEntity("discountcodes"),
            new ProjectChildEntity("producttypes"),
            new ProjectChildEntity("attributegroups"),
            new ProjectChildEntity("apiclients")
        ]);
    }

    [Fact]
    public void Should_Return_Root_Children_With_ChildCount()
    {
        // Arrange
        testHost.CommercetoolsMockHttpMessageHandler
            .When(HttpMethod.Get, "*/*")
            .WithQueryString("limit", "0")
            .WithQueryString("withTotal", "True")
            .Respond(HttpStatusCode.OK, request =>
            {
                string? lastSegment = request.RequestUri?.Segments.LastOrDefault();
                ArgumentNullException.ThrowIfNull(lastSegment);

                const int count = 20;

                return lastSegment switch
                {
                    "attribute-groups" => AttributeGroupTestDataProvider.Get(count).AsPagedQueryResponse()
                        .ToCommercetoolsJsonContent(),
                    "business-units" => BusinessUnitTestDataProvider.Get(count).AsPagedQueryResponse()
                        .ToCommercetoolsJsonContent(),
                    "carts" => CartTestDataProvider.Get(count).AsPagedQueryResponse().ToCommercetoolsJsonContent(),
                    "cart-discounts" => CartDiscountTestDataProvider.Get(count).AsPagedQueryResponse()
                        .ToCommercetoolsJsonContent(),
                    "categories" => CategoryTestDataProvider.Get(count).AsPagedQueryResponse().ToCommercetoolsJsonContent(),
                    "channels" => ChannelTestDataProvider.Get(count).AsPagedQueryResponse().ToCommercetoolsJsonContent(),
                    "custom-objects" => CustomObjectTestDataProvider.Get(count).AsPagedQueryResponse()
                        .ToCommercetoolsJsonContent(),
                    "customers" => CustomerTestDataProvider.Get(count).AsPagedQueryResponse().ToCommercetoolsJsonContent(),
                    "discount-codes" => DiscountCodeTestDataProvider.Get(count).AsPagedQueryResponse()
                        .ToCommercetoolsJsonContent(),
                    "inventory" => InventoryEntryTestDataProvider.Get(count).AsPagedQueryResponse().ToCommercetoolsJsonContent(),
                    "orders" => OrderTestDataProvider.Get(count).AsPagedQueryResponse().ToCommercetoolsJsonContent(),
                    "products" => ProductTestDataProvider.Get(count).AsPagedQueryResponse().ToCommercetoolsJsonContent(),
                    "product-discounts" => ProductDiscountTestDataProvider.Get(count).AsPagedQueryResponse()
                        .ToCommercetoolsJsonContent(),
                    "product-selections" => ProductSelectionTestDataProvider.Get(count).AsPagedQueryResponse()
                        .ToCommercetoolsJsonContent(),
                    "product-types" => ProductTypeTestDataProvider.Get(count).AsPagedQueryResponse().ToCommercetoolsJsonContent(),
                    "shipping-methods" => ShippingMethodTestDataProvider.Get(count).AsPagedQueryResponse()
                        .ToCommercetoolsJsonContent(),
                    "shopping-lists" => ShoppingListTestDataProvider.Get(count).AsPagedQueryResponse()
                        .ToCommercetoolsJsonContent(),
                    "standalone-prices" => StandalonePriceTestDataProvider.Get(count).AsPagedQueryResponse()
                        .ToCommercetoolsJsonContent(),
                    "stores" => StoreTestDataProvider.Get(count).AsPagedQueryResponse().ToCommercetoolsJsonContent(),
                    "tax-categories" => TaxCategoryTestDataProvider.Get(count).AsPagedQueryResponse()
                        .ToCommercetoolsJsonContent(),
                    "api-clients" => ApiClientTestDataProvider.Get(count).AsPagedQueryResponse().ToCommercetoolsJsonContent(),
                    _ => throw new NotImplementedException()
                };
            });

        // Act
        Collection<PSObject> psObjects = testHost
            .InvokeCommand("Get-ChildItem", p => p
                .WithParameter("Path", @"ct-test:\")
                .WithParameter("WithCount", true)
            );

        // Assert
        using var _ = new AssertionScope();
        psObjects.BaseObjectsAreAllOfType<ProjectChildEntity>().Should().BeTrue();
        psObjects.GetBaseObjects<ProjectChildEntity>().Should().BeEquivalentTo(
        [
            new ProjectChildEntity("products") { ChildCount = "20" },
            new ProjectChildEntity("categories") { ChildCount = "20" },
            new ProjectChildEntity("inventory") { ChildCount = "20" },
            new ProjectChildEntity("carts") { ChildCount = "20" },
            new ProjectChildEntity("orders") { ChildCount = "20" },
            new ProjectChildEntity("customers") { ChildCount = "20" },
            new ProjectChildEntity("businessunits") { ChildCount = "20" },
            new ProjectChildEntity("customobjects") { ChildCount = "20" },
            new ProjectChildEntity("channels") { ChildCount = "20" },
            new ProjectChildEntity("stores") { ChildCount = "20" },
            new ProjectChildEntity("taxcategories") { ChildCount = "20" },
            new ProjectChildEntity("shoppinglists") { ChildCount = "20" },
            new ProjectChildEntity("productselections") { ChildCount = "20" },
            new ProjectChildEntity("shippingmethods") { ChildCount = "20" },
            new ProjectChildEntity("standaloneprices") { ChildCount = "20" },
            new ProjectChildEntity("productdiscounts") { ChildCount = "20" },
            new ProjectChildEntity("cartdiscounts") { ChildCount = "20" },
            new ProjectChildEntity("discountcodes") { ChildCount = "20" },
            new ProjectChildEntity("producttypes") { ChildCount = "20" },
            new ProjectChildEntity("attributegroups") { ChildCount = "20" },
            new ProjectChildEntity("apiclients") { ChildCount = "20" }
        ]);
    }

    [Fact]
    public void Should_Return_20_Carts()
    {
        // Arrange
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Get, "*/carts")
            .Respond(HttpStatusCode.OK, _ => CartTestDataProvider.Get(20).AsPagedQueryResponse().ToCommercetoolsJsonContent());

        // Act
        Collection<PSObject> psObjects = testHost
            .InvokeCommand("Get-ChildItem", p => p
                .WithParameter("Path", @"ct-test:\carts")
            );

        // Assert
        using var _ = new AssertionScope();
        testHost.CommercetoolsMockHttpMessageHandler.VerifyNoOutstandingExpectation();
        psObjects.BaseObjectsAreAllOfType<ICart>().Should().BeTrue();
        psObjects.GetBaseObjects<ICart>().Should().HaveCount(20);
    }

    [Fact]
    public void Should_Return_20_Products()
    {
        // Arrange
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Get, "*/products")
            .Respond(HttpStatusCode.OK, _ => ProductTestDataProvider.Get(20).AsPagedQueryResponse().ToCommercetoolsJsonContent());

        // Act
        Collection<PSObject> psObjects = testHost
            .InvokeCommand("Get-ChildItem", p => p
                .WithParameter("Path", @"ct-test:\products")
            );

        // Assert
        using var _ = new AssertionScope();
        testHost.CommercetoolsMockHttpMessageHandler.VerifyNoOutstandingExpectation();
        psObjects.BaseObjectsAreAllOfType<IProduct>().Should().BeTrue();
        psObjects.GetBaseObjects<IProduct>().Should().HaveCount(20);
    }

    [Fact]
    public void Should_Apply_Where()
    {
        // Arrange
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Get, "*/carts")
            .WithQueryString("where", "customerEmail=\"john.johnson@example.com\"")
            .Respond(HttpStatusCode.OK, _ => CartTestDataProvider.Get(20).AsPagedQueryResponse().ToCommercetoolsJsonContent());

        // Act
        testHost
            .InvokeCommand("Get-ChildItem", p => p
                .WithParameter("Path", @"ct-test:\carts")
                .WithParameter("Filter", "customerEmail=\"john.johnson@example.com\"")
            );

        // Assert
        using var _ = new AssertionScope();
        testHost.CommercetoolsMockHttpMessageHandler.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public void Should_Apply_Limit()
    {
        // Arrange
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Get, "*/carts")
            .WithQueryString("limit", "75")
            .Respond(HttpStatusCode.OK, _ => CartTestDataProvider.Get(75).AsPagedQueryResponse().ToCommercetoolsJsonContent());

        // Act
        testHost
            .InvokeCommand("Get-ChildItem", p => p
                .WithParameter("Path", @"ct-test:\carts")
                .WithParameter("Limit", "75")
            );

        // Assert
        using var _ = new AssertionScope();
        testHost.CommercetoolsMockHttpMessageHandler.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public void Should_Apply_Offset()
    {
        // Arrange
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Get, "*/carts")
            .WithQueryString("offset", "10")
            .Respond(HttpStatusCode.OK, _ => CartTestDataProvider.Get(75).AsPagedQueryResponse().ToCommercetoolsJsonContent());

        // Act
        testHost
            .InvokeCommand("Get-ChildItem", p => p
                .WithParameter("Path", @"ct-test:\carts")
                .WithParameter("Offset", "10")
            );

        // Assert
        using var _ = new AssertionScope();
        testHost.CommercetoolsMockHttpMessageHandler.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public void Should_Apply_Sort()
    {
        // Arrange
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Get, "*/carts")
            .WithQueryString("sort", "version")
            .Respond(HttpStatusCode.OK, _ => CartTestDataProvider.Get(75).AsPagedQueryResponse().ToCommercetoolsJsonContent());

        // Act
        testHost
            .InvokeCommand("Get-ChildItem", p => p
                .WithParameter("Path", @"ct-test:\carts")
                .WithParameter("Sort", "version")
            );

        // Assert
        using var _ = new AssertionScope();
        testHost.CommercetoolsMockHttpMessageHandler.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public void Should_Apply_Expands()
    {
        // Arrange
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Get, "*/carts")
            .WithQueryString("expand", "paymentInfo.payments[*].paymentStatus.state")
            .Respond(HttpStatusCode.OK, _ => CartTestDataProvider.Get(75).AsPagedQueryResponse().ToCommercetoolsJsonContent());

        // Act
        testHost
            .InvokeCommand("Get-ChildItem", p => p
                .WithParameter("Path", @"ct-test:\carts")
                .WithParameter("Expands", "paymentInfo.payments[*].paymentStatus.state")
            );

        // Assert
        using var _ = new AssertionScope();
        testHost.CommercetoolsMockHttpMessageHandler.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public void Should_Apply_Total()
    {
        // Arrange
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Get, "*/carts")
            .WithQueryString("withTotal", "True")
            .Respond(HttpStatusCode.OK, _ => CartTestDataProvider.Get(75).AsPagedQueryResponse().ToCommercetoolsJsonContent());

        // Act
        testHost
            .InvokeCommand("Get-ChildItem", p => p
                .WithParameter("Path", @"ct-test:\carts")
                .WithParameter("WithCount", true)
                .WithParameter("WithTotal", true)
            );

        // Assert
        using var _ = new AssertionScope();
        testHost.CommercetoolsMockHttpMessageHandler.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public void Should_Apply_No_Total()
    {
        // Arrange
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Get, "*/carts")
            .WithQueryString("withTotal", "False")
            .Respond(HttpStatusCode.OK, _ => CartTestDataProvider.Get(75).AsPagedQueryResponse().ToCommercetoolsJsonContent());

        // Act
        testHost
            .InvokeCommand("Get-ChildItem", p => p
                .WithParameter("Path", @"ct-test:\carts")
                .WithParameter("WithCount", true)
                .WithParameter("WithTotal", false)
            );

        // Assert
        using var _ = new AssertionScope();
        testHost.CommercetoolsMockHttpMessageHandler.VerifyNoOutstandingExpectation();
    }
}