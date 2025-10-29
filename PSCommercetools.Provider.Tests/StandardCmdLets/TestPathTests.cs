using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Net;
using System.Net.Http;
using commercetools.Sdk.Api.Models.Carts;
using FluentAssertions;
using FluentAssertions.Execution;
using PSCommercetools.Provider.Tests.Extensions;
using PSCommercetools.Provider.Tests.Infrastructure;
using PSCommercetools.Provider.Tests.TestDataProviders;
using RichardSzalay.MockHttp;
using Xunit;

namespace PSCommercetools.Provider.Tests.StandardCmdLets;

public sealed class TestPathTests
{
    private readonly TestHost testHost = new TestHost().Initialize().WithTestPSDrive().Reset();

    [Fact]
    public void Should_Return_True_For_Valid_Path_To_Child_Item()
    {
        // Arrange
        ICart cart = CartTestDataProvider.Get();
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Head, $"*/carts/{cart.Id}")
            .Respond(HttpStatusCode.OK, _ => new StringContent(string.Empty));

        // Act
        Collection<PSObject> psObjects = testHost
            .InvokeCommand("Test-Path", p => p
                .WithParameter("Path", @$"ct-test:\carts\{cart.Id}")
            );

        // Assert
        using var _ = new AssertionScope();
        psObjects.GetFirstBaseObjectAs<bool>().Should().BeTrue();
        testHost.CommercetoolsMockHttpMessageHandler.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public void Should_Return_True_For_Valid_Path_To_Drive_Root()
    {
        // Act
        Collection<PSObject> psObjects = testHost
            .InvokeCommand("Test-Path", p => p
                .WithParameter("Path", @"ct-test:\")
            );

        // Assert
        using var _ = new AssertionScope();
        psObjects.GetFirstBaseObjectAs<bool>().Should().BeTrue();
    }

    [Fact]
    public void Should_Return_False_For_Invalid_Path_To_Drive_Root()
    {
        // Remark because the drive name is not a valid drive name for the CT provider, the provider will not process it.
        // Find a way to check that no provider code is executed or examine resulting PsObject to check that it is not pointing
        // to this provider.

        // Act
        Collection<PSObject> psObjects = testHost
            .InvokeCommand("Test-Path", p => p
                .WithParameter("Path", @"ct-notexist:\")
            );

        // Assert
        using var _ = new AssertionScope();
        psObjects.GetFirstBaseObjectAs<bool>().Should().BeFalse();
    }

    [Fact]
    public void Should_Return_True_For_Valid_Path_To_Container_Item()
    {
        // Act
        Collection<PSObject> psObjects = testHost
            .InvokeCommand("Test-Path", p => p
                .WithParameter("Path", @"ct-test:\carts")
            );

        // Assert
        using var _ = new AssertionScope();
        psObjects.GetFirstBaseObjectAs<bool>().Should().BeTrue();
        testHost.CommercetoolsMockHttpMessageHandler.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public void Should_Return_False_For_Invalid_Path_To_Container_Item()
    {
        // Act
        Collection<PSObject> psObjects = testHost
            .InvokeCommand("Test-Path", p => p
                .WithParameter("Path", @"ct-test:\baskets")
            );

        // Assert
        using var _ = new AssertionScope();
        psObjects.GetFirstBaseObjectAs<bool>().Should().BeFalse();
        testHost.CommercetoolsMockHttpMessageHandler.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public void Should_Return_False_For_Invalid_Path_To_Child_Item()
    {
        // Arrange
        ICart cart = CartTestDataProvider.Get();
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Head, $"*/carts/{cart.Id}")
            .Respond(HttpStatusCode.OK, _ => new StringContent(null!));

        // Act
        Collection<PSObject> psObjects = testHost
            .InvokeCommand("Test-Path", p => p
                .WithParameter("Path", @$"ct-test:\carts\{cart.Id}")
            );

        // Assert
        using var _ = new AssertionScope();
        psObjects.GetFirstBaseObjectAs<bool>().Should().BeFalse();
        testHost.CommercetoolsMockHttpMessageHandler.VerifyNoOutstandingExpectation();
    }
}