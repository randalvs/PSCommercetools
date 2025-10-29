using commercetools.Sdk.Api.Client;
using commercetools.Sdk.Api.Models.ApiClients;
using commercetools.Sdk.Api.Serialization;
using PSCommercetools.Provider.EntityServiceLayer;
using PSCommercetools.Provider.SdkProxyLayer;

namespace PSCommercetools.Provider.RepositoryLayer;

public sealed class CommercetoolsApiClientRepository
{
    private readonly ProjectApiRoot projectApiRoot;
    private readonly SerializerService serializerService;

    public CommercetoolsApiClientRepository(ProjectApiRoot projectApiRoot, SerializerService serializerService)
    {
        this.projectApiRoot = projectApiRoot;
        this.serializerService = serializerService;
    }

    public void Remove(IApiClient entity)
    {
        ApiClientSdkProxy.DeleteFunc(projectApiRoot, entity);
    }

    public bool ExistsById(string id)
    {
        return ApiClientSdkProxy.ExistsByIdFunc(projectApiRoot, id);
    }

    public IApiClient Create(string newItemValue)
    {
        return ApiClientSdkProxy.CreateFunc(projectApiRoot, serializerService, newItemValue);
    }

    public IApiClient GetById(string id)
    {
        return ApiClientSdkProxy.GetByIdFunc(projectApiRoot, id);
    }

    public EntitiesContainer<IApiClient> Get(
        string? where,
        long? limit,
        long? offset,
        string? sort,
        bool? withCount)
    {
        return ApiClientSdkProxy.GetFunc(projectApiRoot, where, limit, offset, sort, withCount);
    }
}