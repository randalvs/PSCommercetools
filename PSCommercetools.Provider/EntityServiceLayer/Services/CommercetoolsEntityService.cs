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

    public object Remove(IEntityServiceParameters? entityServiceParameters)
    {
        var commercetoolsEntityServicesParameters = entityServiceParameters as EntityServiceParameters;

        T removedEntity = commercetoolsEntityRepository.Remove((T)Entity, commercetoolsEntityServicesParameters?.Expands);
        return removedEntity;
    }

    public object Update(object actions, IEntityServiceParameters? entityServiceParameters)
    {
        var commercetoolsEntityServicesParameters = entityServiceParameters as EntityServiceParameters;

        T updatedEntity =
            commercetoolsEntityRepository.Update((T)Entity, actions, commercetoolsEntityServicesParameters?.Expands);
        return updatedEntity;
    }
}