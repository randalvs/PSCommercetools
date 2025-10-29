using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using commercetools.Sdk.Api.Models.Channels;
using commercetools.Sdk.Api.Models.Common;
using commercetools.Sdk.Api.Models.CustomObjects;
using commercetools.Sdk.Api.Models.Products;
using FluentAssertions.Execution;
using PSCommercetools.Provider.Tests.Infrastructure;
using PSCommercetools.Provider.Tests.TestDataProviders;
using RichardSzalay.MockHttp;
using Xunit;

namespace PSCommercetools.Provider.Tests.CustomCmdLets;

public sealed class UpdateItemTests
{
    private readonly TestHost testHost = new TestHost().Initialize().WithTestPSDrive().Reset();

    [Fact]
    public void Should_Update_Channel_From_Path_Input()
    {
        // Arrange
        IChannel channel = ChannelTestDataProvider.Get();
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Head, $"*/channels/{channel.Id}")
            .Respond(HttpStatusCode.OK, _ => new StringContent(string.Empty));
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Get, $"*/channels/{channel.Id}")
            .Respond(HttpStatusCode.OK, _ => channel.ToCommercetoolsJsonContent());
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Post, $"*/channels/{channel.Id}")
            .WithCommercetoolsJsonContent(new
                {
                    channel.Version,
                    Actions = new IChannelUpdateAction[]
                    {
                        new ChannelChangeKeyAction
                        {
                            Key = "myNewChannelKey"
                        },
                        new ChannelChangeNameAction
                        {
                            Name = new LocalizedString { { "en", "new Channel Name EN" } }
                        }
                    }
                }
            ).Respond(HttpStatusCode.OK, _ => channel.ToCommercetoolsJsonContent());

        // Act
        testHost.InvokeCommand("Update-Item", p => p
            .WithParameter("Path", $@"ct-test:\channels\{channel.Id}")
            .WithParameter("Actions", """
                                      [
                                      	{
                                      		"action": "changeKey",
                                      		"key": "myNewChannelKey"
                                      	},
                                      	{
                                      		"action": "changeName",
                                      		"name": {
                                      			"en": "new Channel Name EN"
                                      		}
                                      	}
                                      ]
                                      """)
        );

        // Assert
        using var _ = new AssertionScope();
        testHost.CommercetoolsMockHttpMessageHandler.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public void Should_Update_Channel_From_Pipe_Input()
    {
        // Arrange
        IChannel channel = ChannelTestDataProvider.Get();
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Head, $"*/channels/{channel.Id}")
            .Respond(HttpStatusCode.OK, _ => new StringContent(string.Empty));
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Get, $"*/channels/{channel.Id}")
            .Respond(HttpStatusCode.OK, _ => channel.ToCommercetoolsJsonContent());
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Get, $"*/channels/{channel.Id}")
            .Respond(HttpStatusCode.OK, _ => channel.ToCommercetoolsJsonContent());
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Head, $"*/channels/{channel.Id}")
            .Respond(HttpStatusCode.OK, _ => new StringContent(string.Empty));
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Get, $"*/channels/{channel.Id}")
            .Respond(HttpStatusCode.OK, _ => channel.ToCommercetoolsJsonContent());
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Post, $"*/channels/{channel.Id}")
            .WithCommercetoolsJsonContent(new
                {
                    channel.Version,
                    Actions = new IChannelUpdateAction[]
                    {
                        new ChannelChangeKeyAction
                        {
                            Key = "myNewChannelKey"
                        },
                        new ChannelChangeNameAction
                        {
                            Name = new LocalizedString { { "en", "new Channel Name EN" } }
                        }
                    }
                }
            ).Respond(HttpStatusCode.OK, _ => channel.ToCommercetoolsJsonContent());

        // Act
        testHost
            .InvokePipeline(cb => cb
                .WithCommand("Get-ChildItem", p => p
                    .WithParameter("Path", @$"ct-test:\channels\{channel.Id}")
                )
                .WithCommand("Update-Item", p => p
                    .WithParameter("Actions", """
                                              [
                                              	{
                                              		"action": "changeKey",
                                              		"key": "myNewChannelKey"
                                              	},
                                              	{
                                              		"action": "changeName",
                                              		"name": {
                                              			"en": "new Channel Name EN"
                                              		}
                                              	}
                                              ]
                                              """)
                )
            );

        // Assert
        using var _ = new AssertionScope();
        testHost.CommercetoolsMockHttpMessageHandler.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public void Should_Update_Channel_From_Script_Input()
    {
        IChannel channel = ChannelTestDataProvider.Get();
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Head, $"*/channels/{channel.Id}")
            .Respond(HttpStatusCode.OK, _ => new StringContent(string.Empty));
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Get, $"*/channels/{channel.Id}")
            .Respond(HttpStatusCode.OK, _ => channel.ToCommercetoolsJsonContent());
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Post, $"*/channels/{channel.Id}")
            .WithCommercetoolsJsonContent(new
                {
                    channel.Version,
                    Actions = new IChannelUpdateAction[]
                    {
                        new ChannelChangeKeyAction
                        {
                            Key = "myNewChannelKey"
                        },
                        new ChannelChangeNameAction
                        {
                            Name = new LocalizedString { { "en", "new Channel Name EN" } }
                        }
                    }
                }
            ).Respond(HttpStatusCode.OK, _ => channel.ToCommercetoolsJsonContent());

//         Collection<PSObject> result = testHost.InvokeScript($"""
//                                                              Add-Type -Path "C:\Projects\CTPS\PSCommercetools\PSCommercetools.Provider.Tests\bin\Debug\net8.0\commercetools.Sdk.Api.dll"
//
//                                                              $changeKeyAction = New-Object -TypeName "commercetools.Sdk.Api.Models.Channels.ChannelChangeKeyAction"
//                                                              $changeKeyAction.Key = "myNewChannelKey"
//
//                                                              $nameLocalizedString = New-Object -TypeName "commercetools.Sdk.Api.Models.Common.LocalizedString"
//                                                              $nameLocalizedString.Add("en", "new Channel Name EN")
//
//                                                              $changeNameAction = New-Object -TypeName "commercetools.Sdk.Api.Models.Channels.ChannelChangeNameAction"
//                                                              $changeNameAction.Name = $nameLocalizedString
//
//                                                              $actionsList = New-Object "System.Collections.Generic.List``1[[commercetools.Sdk.Api.Models.Channels.IChannelUpdateAction]]"
//                                                              $actionsList.Add($changeKeyAction)
//                                                              $actionsList.Add($changeNameAction)
//
//                                                              Update-Item -Path ct-test:\channels\{channel.Id} -Actions $actionsList
//                                                              """);


        var assembly = Assembly.GetExecutingAssembly();
        string assemblyLocation = assembly.Location;
        string? binDirectory = Path.GetDirectoryName(assemblyLocation);
        string commercetoolsSdkApiDll = Path.Join(binDirectory, "commercetools.Sdk.Api.dll");


        testHost.InvokeScript($$"""
                                Add-Type -Path "{{commercetoolsSdkApiDll}}"

                                $nameLocalizedString = [commercetools.Sdk.Api.Models.Common.LocalizedString]::new()
                                $nameLocalizedString.Add("en", "new Channel Name EN")

                                $actionsList = [System.Collections.Generic.List[commercetools.Sdk.Api.Models.Channels.IChannelUpdateAction]]@(
                                    [commercetools.Sdk.Api.Models.Channels.ChannelChangeKeyAction]@{ Key = "myNewChannelKey" },
                                    [commercetools.Sdk.Api.Models.Channels.ChannelChangeNameAction]@{ Name = $nameLocalizedString }
                                )

                                Update-Item -Path ct-test:\channels\{{channel.Id}} -Actions $actionsList
                                """);

        // Assert
        using var _ = new AssertionScope();
        testHost.CommercetoolsMockHttpMessageHandler.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public void Should_Update_Custom_Object_From_Path_Input()
    {
        // Arrange
        IEnumerable<ICustomObject> customObjects = CustomObjectTestDataProvider.Get(1).ToList();
        ICustomObject customObject = customObjects.First();
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Head, "*/custom-objects")
            .WithQueryString("where", $"id=\"{customObject.Id}\"")
            .Respond(HttpStatusCode.OK, _ => new StringContent(string.Empty));
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Get, "*/custom-objects")
            .WithQueryString("where", $"id=\"{customObject.Id}\"")
            .Respond(HttpStatusCode.OK, _ => customObjects.AsPagedQueryResponse().ToCommercetoolsJsonContent());
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Get, $"*/custom-objects/{customObject.Container}/{customObject.Key}")
            .Respond(HttpStatusCode.OK, _ => customObject.ToCommercetoolsJsonContent());
        testHost.CommercetoolsMockHttpMessageHandler
            .Expect(HttpMethod.Post, "*/custom-objects")
            .WithCommercetoolsJsonContent(new CustomObjectDraft
                {
                    Container = customObject.Container,
                    Key = customObject.Key,
                    Value = new
                    {
                        CustomObjectValue = new
                        {
                            Field1 = "value1"
                        }
                    }
                }
            )
            .Respond(HttpStatusCode.OK, _ => customObject.ToCommercetoolsJsonContent());

        // Act
        testHost.InvokeCommand("Update-Item", p => p
            .WithParameter("Path", $@"ct-test:\customobjects\{customObject.Id}")
            .WithParameter("CustomObjectDraft", $$"""
                                                  {
                                                    "container": "{{customObject.Container}}",
                                                    "key": "{{customObject.Key}}",
                                                    "value": {
                                                      "customObjectValue": {
                                                        "field1" : "value1"
                                                      }
                                                    }
                                                  }
                                                  """)
        );

        // Assert
        using var _ = new AssertionScope();
        testHost.CommercetoolsMockHttpMessageHandler.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public void Should_Update_Product_From_Path_Input()
    {
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
            .WithCommercetoolsJsonContent(new
                {
                    product.Version,
                    Actions = new IProductUpdateAction[]
                    {
                        new ProductSetDescriptionAction
                        {
                            Description = new LocalizedString { { "en", "new Product Name EN" } }
                        }
                    }
                }
            ).Respond(HttpStatusCode.OK, _ => product.ToCommercetoolsJsonContent());

        // Act
        testHost.InvokeCommand("Update-Item", p => p
            .WithParameter("Path", $@"ct-test:\products\{product.Id}")
            .WithParameter("Actions", """
                                      [
                                      	{
                                      		"action": "setDescription",
                                      		"description": {
                                      			"en": "new Product Name EN"
                                      		}
                                      	}
                                      ]
                                      """)
        );

        // Assert
        using var _ = new AssertionScope();
        testHost.CommercetoolsMockHttpMessageHandler.VerifyNoOutstandingExpectation();
    }
}