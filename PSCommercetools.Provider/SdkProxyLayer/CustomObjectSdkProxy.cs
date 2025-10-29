using System;
using System.Linq;
using System.Net;
using commercetools.Sdk.Api.Client;
using commercetools.Sdk.Api.Client.RequestBuilders.CustomObjects;
using commercetools.Sdk.Api.Models.CustomObjects;
using commercetools.Sdk.Api.Serialization;
using PSCommercetools.Provider.EntityServiceLayer;
using PSCommercetools.Provider.SdkProxyLayer.Extensions;

namespace PSCommercetools.Provider.SdkProxyLayer;

internal sealed class CustomObjectSdkProxy : SdkProxyBase, ISdkProxy<ICustomObject>
{
    //public Type EntityType => typeof(ICustomObject);

    public Func<ProjectApiRoot, string[]?, string?, long?, long?, string?, bool?, EntitiesContainer<ICustomObject>>
        GetFunc => (projectApiRoot, expands, where, limit, offset, sort, withCount) =>
    {
        ByProjectKeyCustomObjectsGet? byProjectKeyCustomObjectsGet = projectApiRoot.CustomObjects().Get();

        AddExpandClauses(byProjectKeyCustomObjectsGet, expands);
        AddWhereClause(byProjectKeyCustomObjectsGet, where);
        AddLimitClause(byProjectKeyCustomObjectsGet, limit);
        AddOffsetClause(byProjectKeyCustomObjectsGet, offset);
        AddSortClause(byProjectKeyCustomObjectsGet, sort);
        AddWithTotalClause(byProjectKeyCustomObjectsGet, withCount);

        ICustomObjectPagedQueryResponse? response = byProjectKeyCustomObjectsGet
            .ExecuteAsync()
            .GetAwaiter()
            .GetResult();

        return response.Results.ToEntitiesContainer<ICustomObject>(
            response.Count,
            response.Offset,
            response.Limit,
            response.Total);
    };

    public Func<ProjectApiRoot, ICustomObject, string[]?, ICustomObject> DeleteFunc => (projectApiRoot, customObject, _) =>
        projectApiRoot
            .CustomObjects()
            .WithContainerAndKey(customObject.Container, customObject.Key)
            .Delete()
            .ExecuteAsync()
            .GetAwaiter()
            .GetResult();

    public Func<ProjectApiRoot, SerializerService, string?, string[]?, ICustomObject> CreateFunc =>
        (projectApiRoot, serializerService, json, _) =>
        {
            var draft = serializerService.Deserialize<ICustomObjectDraft>(json);
            return projectApiRoot
                .CustomObjects()
                .Post(draft)
                .ExecuteAsync()
                .GetAwaiter()
                .GetResult();
        };

    public Func<ProjectApiRoot, SerializerService, ICustomObject, object, string[]?, ICustomObject> UpdateFunc =>
        (projectApiRoot, serializerService, _, json, expands) =>
            CreateFunc(projectApiRoot, serializerService, json.ToString(), expands);

    public Func<ProjectApiRoot, string, string[]?, ICustomObject> GetByIdFunc => (projectApiRoot, id, expands) =>
    {
        (string container, string key) = GetContainerAndKeyFromId(projectApiRoot, id);

        ByProjectKeyCustomObjectsByContainerByKeyGet? byProjectKeyCustomObjectsByContainerByKeyGet = projectApiRoot
            .CustomObjects()
            .WithContainerAndKey(container, key)
            .Get();

        byProjectKeyCustomObjectsByContainerByKeyGet = (expands ?? []).Aggregate(byProjectKeyCustomObjectsByContainerByKeyGet,
            (current, expand) => current.WithExpand(expand));

        return byProjectKeyCustomObjectsByContainerByKeyGet.ExecuteAsync()
            .GetAwaiter()
            .GetResult();
    };

    public Func<ProjectApiRoot, string, bool> ExistsByIdFunc => (projectApiRoot, id) =>
        projectApiRoot
            .CustomObjects()
            .Head()
            .WithWhere($"id=\"{id}\"")
            .SendAsync()
            .GetAwaiter()
            .GetResult().StatusCode != HttpStatusCode.NotFound;

    private static (string container, string key) GetContainerAndKeyFromId(ProjectApiRoot projectApiRoot, string id)
    {
        ICustomObjectPagedQueryResponse? customObjectPagedQueryResponse =
            projectApiRoot
                .CustomObjects()
                .Get()
                .WithWhere($"id=\"{id}\"")
                .ExecuteAsync()
                .GetAwaiter()
                .GetResult();

        if (!customObjectPagedQueryResponse.Results.Any())
        {
            throw new ArgumentException($"Could not find custom object with '{id}'");
        }

        ICustomObject? customObject = customObjectPagedQueryResponse.Results.First();

        return (customObject.Container, customObject.Key);
    }
}