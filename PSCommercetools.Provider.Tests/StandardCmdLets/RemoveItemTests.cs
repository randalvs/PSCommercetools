using System.Net;
using System.Net.Http;
using commercetools.Sdk.Api.Models.Carts;
using PSCommercetools.Provider.Tests.Infrastructure;
using PSCommercetools.Provider.Tests.TestDataProviders;
using RichardSzalay.MockHttp;
using Xunit;

namespace PSCommercetools.Provider.Tests.StandardCmdLets;

public sealed class RemoveItemTests
{
    private readonly TestHost testHost = new TestHost().Initialize().WithTestPSDrive().Reset();

    [Fact]
    public void Should_Remove_Item_From_Path_Input()
    {
        // Arrange
        ICart cart = CartTestDataProvider.Get();
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Head, $"*/carts/{cart.Id}")
            .Respond(HttpStatusCode.OK, _ => new StringContent(string.Empty));
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Get, $"*/carts/{cart.Id}")
            .Respond(HttpStatusCode.OK, _ => cart.ToCommercetoolsJsonContent());
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Get, $"*/carts/{cart.Id}")
            .Respond(HttpStatusCode.OK, _ => cart.ToCommercetoolsJsonContent());
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Delete, $"*/carts/{cart.Id}")
            .Respond(HttpStatusCode.OK, _ => cart.ToCommercetoolsJsonContent());

        // Act
        _ = testHost
            .InvokeCommand("Remove-Item", p => p
                .WithParameter("Path", @$"ct-test:\carts\{cart.Id}")
            );

        // Assert
        testHost.CommercetoolsMockHttpMessageHandler.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public void Should_Remove_Item_From_Pipe_Input()
    {
        // Arrange
        ICart cart = CartTestDataProvider.Get();
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Head, $"*/carts/{cart.Id}")
            .Respond(HttpStatusCode.OK, _ => new StringContent(string.Empty));
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Get, $"*/carts/{cart.Id}")
            .Respond(HttpStatusCode.OK, _ => cart.ToCommercetoolsJsonContent());
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Get, $"*/carts/{cart.Id}")
            .Respond(HttpStatusCode.OK, _ => cart.ToCommercetoolsJsonContent());
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Head, $"*/carts/{cart.Id}")
            .Respond(HttpStatusCode.OK, _ => new StringContent(string.Empty));
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Get, $"*/carts/{cart.Id}")
            .Respond(HttpStatusCode.OK, _ => cart.ToCommercetoolsJsonContent());
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Get, $"*/carts/{cart.Id}")
            .Respond(HttpStatusCode.OK, _ => cart.ToCommercetoolsJsonContent());
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Delete, $"*/carts/{cart.Id}")
            .Respond(HttpStatusCode.OK, _ => cart.ToCommercetoolsJsonContent());

        // Act
        _ = testHost
            .InvokePipeline(cb => cb
                .WithCommand("Get-ChildItem", p => p
                    .WithParameter("Path", @$"ct-test:\carts\{cart.Id}")
                )
                .WithCommand("Remove-Item")
            );

        // Assert
        testHost.CommercetoolsMockHttpMessageHandler.VerifyNoOutstandingExpectation();
    }
}