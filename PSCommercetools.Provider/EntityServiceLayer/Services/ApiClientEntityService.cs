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

    public void Remove(IEntityServiceParameters? _)
    {
        commercetoolsApiClientRepository.Remove((IApiClient)Entity);
    }

    public void Update(object actions, IEntityServiceParameters? _)
    {
        throw new NotImplementedException();
    }

    public string Name => ((IApiClient)Entity).Id;
}