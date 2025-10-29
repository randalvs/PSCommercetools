using System.Net;
using System.Net.Http;
using FluentAssertions;
using PSCommercetools.Provider.Tests.Infrastructure;
using PSCommercetools.Provider.Tests.TestDataProviders;
using RichardSzalay.MockHttp;
using Xunit;

namespace PSCommercetools.Provider.Tests.StandardCmdLets;

public sealed class SetLocationTests
{
    private readonly TestHost testHost = new TestHost().Initialize().WithTestPSDrive().Reset();

    [Theory]
    [InlineData(@"ct-test:\", @"ct-test:\carts", @"ct-test:\carts")]
    [InlineData(@"ct-test:\", @"ct-test:\Carts", @"ct-test:\carts")]
    [InlineData(@"ct-test:\", @"ct-test:\carts\", @"ct-test:\carts")]
    [InlineData(@"ct-test:\", @"ct-test:\orders", @"ct-test:\orders")]
    [InlineData(@"ct-test:\", @".\carts", @"ct-test:\carts")]
    [InlineData(@"ct-test:\carts", @"ct-test:\", @"ct-test:\")]
    [InlineData(@"ct-test:\carts", @"..\orders", @"ct-test:\orders")]
    [InlineData(@"ct-test:\carts", @"..\orders\", @"ct-test:\orders")]
    [InlineData(@"ct-test:\carts", "..", @"ct-test:\")]
    public void Should_Set_Location(string initialLocation, string locationToSwitchTo, string expectedLocation)
    {
        // Arrange
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Get, "*/carts")
            .Respond(HttpStatusCode.OK, _ => CartTestDataProvider.Get(10).AsPagedQueryResponse().ToCommercetoolsJsonContent());
        testHost.SetLocationTo(initialLocation);

        //Act
        testHost
            .InvokeCommand("Set-Location", p => p
                .WithParameter("Path", locationToSwitchTo)
            );
        // Assert
        testHost.GetCurrentLocation().Should().Be(expectedLocation);
    }
}