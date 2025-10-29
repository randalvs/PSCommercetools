using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Net;
using System.Net.Http;
using commercetools.Sdk.Api.Models.ApiClients;
using commercetools.Sdk.Api.Models.Channels;
using commercetools.Sdk.Api.Models.CustomObjects;
using FluentAssertions;
using FluentAssertions.Execution;
using PSCommercetools.Provider.Tests.Extensions;
using PSCommercetools.Provider.Tests.Infrastructure;
using PSCommercetools.Provider.Tests.TestDataProviders;
using RichardSzalay.MockHttp;
using Xunit;

namespace PSCommercetools.Provider.Tests.StandardCmdLets;

public sealed class NewItemTests
{
    private readonly TestHost testHost = new TestHost().Initialize().WithTestPSDrive().Reset();

    [Fact]
    public void Should_Create_Channel()
    {
        // Arrange
        IChannel channel = ChannelTestDataProvider.Get();
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Post, "*/channels")
            .Respond(HttpStatusCode.OK, _ => channel.ToCommercetoolsJsonContent());

        // Act
        Collection<PSObject> psObjects = testHost
            .InvokeCommand("New-Item", p => p
                .WithParameter("Path", @"ct-test:\channels")
                .WithParameter("ItemType", "channel")
                .WithParameter("Value", """
                                        {
                                            "key": "test-channel"
                                        }
                                        """)
            );

        // Assert
        using var _ = new AssertionScope();
        testHost.CommercetoolsMockHttpMessageHandler.VerifyNoOutstandingExpectation();
        psObjects.BaseObjectsAreAllOfType<IChannel>().Should().BeTrue();
        psObjects.GetBaseObjects<IChannel>().Should().HaveCount(1);
    }

    [Fact]
    public void Should_Create_Custom_Object()
    {
        // Arrange
        ICustomObject customObject = CustomObjectTestDataProvider.Get();
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Post, "*/custom-objects")
            .Respond(HttpStatusCode.OK, _ => customObject.ToCommercetoolsJsonContent());

        // Act
        Collection<PSObject> psObjects = testHost
            .InvokeCommand("New-Item", p => p
                .WithParameter("Path", @"ct-test:\customobjects")
                .WithParameter("ItemType", "customobject")
                .WithParameter("Value", """
                                        {
                                            "key": "test-custom-object"
                                        }
                                        """)
            );

        // Assert
        using var _ = new AssertionScope();
        testHost.CommercetoolsMockHttpMessageHandler.VerifyNoOutstandingExpectation();
        psObjects.BaseObjectsAreAllOfType<ICustomObject>().Should().BeTrue();
        psObjects.GetBaseObjects<ICustomObject>().Should().HaveCount(1);
    }

    [Fact]
    public void Should_Create_Api_Client()
    {
        // Arrange
        IApiClient apiClient = ApiClientTestDataProvider.Get();
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Post, "*/api-clients")
            .Respond(HttpStatusCode.OK, _ => apiClient.ToCommercetoolsJsonContent());

        // Act
        Collection<PSObject> psObjects = testHost
            .InvokeCommand("New-Item", p => p
                .WithParameter("Path", @"ct-test:\apiclients")
                .WithParameter("ItemType", "apiclient")
                .WithParameter("Value", """
                                        {
                                            "key": "test-api-client"
                                        }
                                        """)
            );

        // Assert
        using var _ = new AssertionScope();
        testHost.CommercetoolsMockHttpMessageHandler.VerifyNoOutstandingExpectation();
        psObjects.BaseObjectsAreAllOfType<IApiClient>().Should().BeTrue();
        psObjects.GetBaseObjects<IApiClient>().Should().HaveCount(1);
    }
}