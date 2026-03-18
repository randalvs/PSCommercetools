using System;
using commercetools.Sdk.Api.Models.ApiClients;
using PSCommercetools.Provider.EntityServiceLayer.Parameters;
using PSCommercetools.Provider.RepositoryLayer;

namespace PSCommercetools.Provider.EntityServiceLayer.Services;

internal sealed class ApiClientEntityService : IEntityService
{
    private readonly CommercetoolsApiClientRepository commercetoolsApiClientRepository;

    public ApiClientEntityService(CommercetoolsApiClientRepository commercetoolsApiClientRepository, IApiClient entity)
    {
        this.commercetoolsApiClientRepository = commercetoolsApiClientRepository;
        Entity = entity;
    }

    public bool IsContainer => false;
    public object Entity { get; }

    public object Remove(IEntityServiceParameters? _)
    {
        return commercetoolsApiClientRepository.Remove((IApiClient)Entity);
    }

    public object Update(object actions, IEntityServiceParameters? _)
    {
        throw new NotImplementedException();
    }

    public string Name => ((IApiClient)Entity).Id;
}