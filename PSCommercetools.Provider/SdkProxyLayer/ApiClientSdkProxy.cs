using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using commercetools.Sdk.Api.Client;
using commercetools.Sdk.Api.Client.RequestBuilders.ApiClients;
using commercetools.Sdk.Api.Models.ApiClients;
using commercetools.Sdk.Api.Serialization;
using PSCommercetools.Provider.EntityServiceLayer;
using PSCommercetools.Provider.SdkProxyLayer.Extensions;

namespace PSCommercetools.Provider.SdkProxyLayer;

internal static class ApiClientSdkProxy
{
    public static Func<ProjectApiRoot, string?, long?, long?, string?, bool?, EntitiesContainer<IApiClient>> GetFunc
        => (projectApiRoot, where, limit, offset, sort, withCount) =>

        {
            ByProjectKeyApiClientsGet? byProjectKeyApiClientsGet = projectApiRoot.ApiClients().Get();

            byProjectKeyApiClientsGet.WithWhereClause(where);
            byProjectKeyApiClientsGet.WithLimitClause(limit);
            byProjectKeyApiClientsGet.WithOffsetClause(offset);
            byProjectKeyApiClientsGet.WithSortClause(sort);
            byProjectKeyApiClientsGet.WithWithTotalClause(withCount);

            IApiClientPagedQueryResponse? response =
                byProjectKeyApiClientsGet
                    .ExecuteAsync()
                    .GetAwaiter()
                    .GetResult();

            return response.Results.ToEntitiesContainer<IApiClient>(response.Count, response.Offset, response.Limit,
                response.Total);
        };

    public static Func<ProjectApiRoot, string, bool> ExistsByIdFunc => (projectApiRoot, id) =>
        TryCaseInsensitiveFindById(projectApiRoot, id, out IApiClient? _);

    public static Func<ProjectApiRoot, string, IApiClient> GetByIdFunc => (projectApiRoot, id) =>
        TryCaseInsensitiveFindById(projectApiRoot, id, out IApiClient? apiClient)
            ? apiClient
            : throw new ArgumentNullException($"Cannot find API client with id '{id}'.");

    public static Func<ProjectApiRoot, IApiClient, IApiClient> DeleteFunc => (projectApiRoot, apiClient) =>
        projectApiRoot.ApiClients().WithId(apiClient.Id).Delete().ExecuteAsync().GetAwaiter().GetResult();

    public static Func<ProjectApiRoot, SerializerService, string, IApiClient> CreateFunc =>
        (projectApiRoot, serializerService, json) =>
        {
            var apiClientDraft = serializerService.Deserialize<IApiClientDraft>(json);
            return projectApiRoot.ApiClients().Post(apiClientDraft).ExecuteAsync().GetAwaiter().GetResult();
        };

    private static bool TryCaseInsensitiveFindById(ProjectApiRoot projectApiRoot, string id,
        [NotNullWhen(true)] out IApiClient? apiClient)
    {
        apiClient = null;
        var stopSearching = false;
        string? lastId = null;

        while (!stopSearching)
        {
            ByProjectKeyApiClientsGet? byProjectKeyApiClientsGet = projectApiRoot
                .ApiClients()
                .Get()
                .WithWithTotal(false)
                .WithLimit(500);

            if (lastId != null)
            {
                byProjectKeyApiClientsGet.WithWhere($"id > {lastId}");
            }

            IApiClientPagedQueryResponse? apiClientPagedQueryResponse = byProjectKeyApiClientsGet.WithSort("id")
                .ExecuteAsync()
                .GetAwaiter()
                .GetResult();

            apiClient = apiClientPagedQueryResponse.Results.FirstOrDefault(client =>
                client.Id.Equals(id, StringComparison.OrdinalIgnoreCase));

            if (apiClient != null)
            {
                return true;
            }

            lastId = apiClientPagedQueryResponse.Results.Last().Id;
            stopSearching = apiClientPagedQueryResponse.Count < 500;
        }

        return false;
    }
}