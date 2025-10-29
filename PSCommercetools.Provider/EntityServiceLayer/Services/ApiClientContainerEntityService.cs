using System;
using System.Linq;
using commercetools.Sdk.Api.Models.ApiClients;
using PSCommercetools.Provider.EntityServiceLayer.Models;
using PSCommercetools.Provider.EntityServiceLayer.Parameters;
using PSCommercetools.Provider.RepositoryLayer;

namespace PSCommercetools.Provider.EntityServiceLayer.Services;

internal sealed class ApiClientContainerEntityService : IEntityContainerService
{
    private readonly CommercetoolsApiClientRepository commercetoolsApiClientRepository;

    public ApiClientContainerEntityService(CommercetoolsApiClientRepository commercetoolsApiClientRepository)
    {
        var projectChildEntity = new ProjectChildEntity("apiclients");
        Entity = projectChildEntity;
        this.commercetoolsApiClientRepository = commercetoolsApiClientRepository;
    }

    public bool IsContainer => true;
    public object Entity { get; }

    public string Name => "apiclients";

    public EntityCarrier GetChildEntity(string name, IEntityServiceParameters? _)
    {
        IApiClient apiClient = commercetoolsApiClientRepository.GetById(name);

        return new EntityCarrier
        {
            Item = new ApiClientEntityService(commercetoolsApiClientRepository, apiClient)
        };
    }

    public EntitiesCarrier GetChildEntities(IEntityServiceParameters? entityServiceParameters)
    {
        var commercetoolsEntityServicesParameters = entityServiceParameters as EntityServicesParameters;

        string? whereClause = commercetoolsEntityServicesParameters?.Filter;
        long? limitClause = commercetoolsEntityServicesParameters?.Limit;
        long? offsetClause = commercetoolsEntityServicesParameters?.Offset;
        string? sortClause = commercetoolsEntityServicesParameters?.Sort;
        bool? withTotal = commercetoolsEntityServicesParameters?.WithTotal;

        EntitiesContainer<IApiClient> entitiesContainer =
            commercetoolsApiClientRepository.Get(whereClause, limitClause, offsetClause, sortClause, withTotal);

        return new EntitiesCarrier
        {
            Count = entitiesContainer.Count,
            Limit = entitiesContainer.Limit,
            Offset = entitiesContainer.Offset,
            Total = entitiesContainer.Total,
            Items = entitiesContainer.Items.Select(i => new ApiClientEntityService(commercetoolsApiClientRepository, i))
        };
    }

    public bool HasChild(string name)
    {
        return commercetoolsApiClientRepository.ExistsById(name);
    }

    public void SetChildCount()
    {
        try
        {
            EntitiesContainer<IApiClient> entitiesContainer = commercetoolsApiClientRepository.Get(null, 0, null, null, true);
            ((ProjectChildEntity)Entity).ChildCount = entitiesContainer.Total is not null
                ? entitiesContainer.Total.ToString()
                : "-";
        }
        catch
        {
            ((ProjectChildEntity)Entity).ChildCount = "-";
        }
    }

    public EntityCarrier CreateChildEntity(
        object newItemValue,
        IEntityServiceParameters? _)
    {
        if (newItemValue is not string newItemValueString)
        {
            throw new ArgumentException("Invalid parameters provided. Value should be a Json string.");
        }

        IApiClient apiClient = commercetoolsApiClientRepository.Create(
            newItemValueString);

        return new EntityCarrier
        {
            Item = new ApiClientEntityService(commercetoolsApiClientRepository, apiClient)
        };
    }
}