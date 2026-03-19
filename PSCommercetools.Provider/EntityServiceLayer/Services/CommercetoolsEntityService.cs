using commercetools.Sdk.Api.Models.Common;
using PSCommercetools.Provider.EntityServiceLayer.Parameters;
using PSCommercetools.Provider.RepositoryLayer;

namespace PSCommercetools.Provider.EntityServiceLayer.Services;

internal sealed class CommercetoolsEntityService<T> : IEntityService where T : IBaseResource
{
    private readonly CommercetoolsEntityRepository commercetoolsEntityRepository;

    public CommercetoolsEntityService(CommercetoolsEntityRepository commercetoolsEntityRepository, T entity)
    {
        this.commercetoolsEntityRepository = commercetoolsEntityRepository;
        Entity = entity;
    }

    public bool IsContainer => false;

    public object Entity { get; }
    public string Name => ((T)Entity).Id;

    public object Remove(long? version, IEntityServiceParameters? entityServiceParameters)
    {
        var commercetoolsEntityServicesParameters = entityServiceParameters as EntityServiceParameters;

        T removedEntity =
            commercetoolsEntityRepository.Remove((T)Entity, version, commercetoolsEntityServicesParameters?.Expands);
        return removedEntity;
    }

    public object Update(long? version, object actions, IEntityServiceParameters? entityServiceParameters)
    {
        var commercetoolsEntityServicesParameters = entityServiceParameters as EntityServiceParameters;

        T updatedEntity =
            commercetoolsEntityRepository.Update((T)Entity, version, actions, commercetoolsEntityServicesParameters?.Expands);
        return updatedEntity;
    }
}