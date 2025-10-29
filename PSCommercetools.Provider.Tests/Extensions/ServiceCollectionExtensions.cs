using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using commercetools.Base.Client;
using commercetools.Base.Client.Domain;
using commercetools.Sdk.Api;
using commercetools.Sdk.Api.Client;
using commercetools.Sdk.Api.Models.Errors;
using commercetools.Sdk.Api.Serialization;
using Microsoft.Extensions.DependencyInjection;
using RichardSzalay.MockHttp;

namespace PSCommercetools.Provider.Tests.Extensions;

public static class ServiceCollectionExtensions
{
    private const string ProjectKey = "MockProject";
    private const string ApiBaseAddress = "https://api.mock.commercetools.com/";

    public static void UseCommercetoolsApiMock(this ServiceCollection collection)
    {
        const string clientName = "test";

        collection.AddSingleton(_ =>
        {
            var mock = new MockHttpMessageHandler(BackendDefinitionBehavior.Always);

            mock.When(HttpMethod.Post, "*/oauth/token").Respond(
                HttpStatusCode.OK, JsonContent.Create(new Token()));

            return mock;
        });

        collection.UseCommercetoolsApiSerialization();
        collection.AddLogging();
        collection.SetupClient(
            clientName,
            _ => typeof(ErrorResponse),
            s => s.GetService<IApiSerializerService>()
        ).ConfigurePrimaryHttpMessageHandler<MockHttpMessageHandler>();

        collection.AddHttpClient("CommercetoolsAuth").ConfigurePrimaryHttpMessageHandler<MockHttpMessageHandler>();

        collection.AddTransient<ProjectApiRoot>(s =>
        {
            var clientFactory = s.GetRequiredService<IHttpClientFactory>();

            var config = new ClientConfiguration
            {
                ClientId = string.Empty,
                ClientSecret = string.Empty,
                ProjectKey = ProjectKey,
                ApiBaseAddress = ApiBaseAddress
            };

            IClient? client = new ClientBuilder
            {
                ClientConfiguration = config,
                ClientName = clientName,
                TokenProvider = TokenProviderFactory.CreateClientCredentialsTokenProvider(config, clientFactory),
                SerializerService = s.GetService<IApiSerializerService>(),
                HttpClient = clientFactory.CreateClient(clientName)
            }.Build();

            var projectApiRoot = new ProjectApiRoot(client, config.ProjectKey);

            return projectApiRoot;
        });
    }
}