using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Text.Json;
using commercetools.Sdk.Api;
using commercetools.Sdk.Api.Serialization;
using Microsoft.Extensions.DependencyInjection;
using RichardSzalay.MockHttp;

namespace PSCommercetools.Provider.Tests.Infrastructure;

internal static class CommercetoolsSerializerExtensions
{
    public static MockedRequest WithCommercetoolsJsonContent<T>(this MockedRequest source, T content)
    {
        return source.WithContent(ToCommercetoolsJson(content));
    }

    public static string ToCommercetoolsJson<T>(this T resource, bool? prettify = null)
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.UseCommercetoolsApiSerialization();
        ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
        var serializerService = serviceProvider.GetRequiredService<SerializerService>();

        string? json = serializerService.Serialize(resource);

        return prettify == true ? PrettifyJson(json) : json;
    }

    public static StringContent ToCommercetoolsJsonContent<T>(this T resource)
    {
        return new StringContent(ToCommercetoolsJson(resource));
    }

    [SuppressMessage("Performance", "CA1869:Cache and reuse \'JsonSerializerOptions\' instances")]
    private static string PrettifyJson(string jsonString)
    {
        using JsonDocument jsonDocument = JsonDocument.Parse(jsonString);
        string prettyJson = JsonSerializer.Serialize(jsonDocument.RootElement, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
        return prettyJson;
    }

    public static T? ToCommercetoolsEntity<T>(this HttpContent? httpContent)
    {
        if (httpContent is null)
        {
            return default;
        }

        string jsonString = httpContent.ReadAsStringAsync().Result;

        var serviceCollection = new ServiceCollection();
        serviceCollection.UseCommercetoolsApiSerialization();
        ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
        var serializerService = serviceProvider.GetRequiredService<SerializerService>();

        return serializerService.Deserialize<T>(jsonString);
    }
}