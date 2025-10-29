using System.Linq;
using commercetools.Sdk.Api.Models.Common;
using PSCommercetools.Provider.EntityServiceLayer.Models;
using PSCommercetools.Provider.EntityServiceLayer.Parameters;
using PSCommercetools.Provider.RepositoryLayer;

namespace PSCommercetools.Provider.EntityServiceLayer.Services;

internal sealed class CommercetoolsContainerEntityService<T> : IEntityContainerService where T : IBaseResource
{
    private readonly CommercetoolsEntityRepository commercetoolsEntityRepository;

    public CommercetoolsContainerEntityService(CommercetoolsEntityRepository commercetoolsEntityRepository, string name)
    {
        var projectChildEntity = new ProjectChildEntity(name);

        Entity = projectChildEntity;

        this.commercetoolsEntityRepository = commercetoolsEntityRepository;
        Name = name;
    }

    public bool IsContainer => true;
    public object Entity { get; }
    public string Name { get; }

    public EntityCarrier GetChildEntity(string name,
        IEntityServiceParameters? entityServiceParameters)
    {
        string[]? expandClauses =
            entityServiceParameters is EntityServiceParameters commercetoolsEntityServiceParameters
                ? commercetoolsEntityServiceParameters.Expands
                : null;

        var x = commercetoolsEntityRepository.GetById<T>(name, expandClauses);

        return new EntityCarrier
        {
            Item = new CommercetoolsEntityService<T>(commercetoolsEntityRepository, x)
        };
    }

    public EntitiesCarrier GetChildEntities(IEntityServiceParameters? entityServiceParameters)
    {
        var commercetoolsEntityServicesParameters = entityServiceParameters as EntityServicesParameters;

        string[]? expandClause = commercetoolsEntityServicesParameters?.Expands;
        string? whereClause = commercetoolsEntityServicesParameters?.Filter;
        long? limitClause = commercetoolsEntityServicesParameters?.Limit;
        long? offsetClause = commercetoolsEntityServicesParameters?.Offset;
        string? sortClause = commercetoolsEntityServicesParameters?.Sort;
        bool? withTotal = commercetoolsEntityServicesParameters?.WithTotal;

        EntitiesContainer<T> x =
            commercetoolsEntityRepository.Get<T>(expandClause, whereClause, limitClause, offsetClause, sortClause, withTotal);

        return new EntitiesCarrier
        {
            Count = x.Count,
            Limit = x.Limit,
            Offset = x.Offset,
            Total = x.Total,
            Items = x.Items.Select(i => new CommercetoolsEntityService<T>(commercetoolsEntityRepository, i))
        };
    }

    public bool HasChild(string name)
    {
        return commercetoolsEntityRepository.ExistsById<T>(name);
    }

    public void SetChildCount()
    {
        try
        {
            EntitiesContainer<T> entitiesContainer = commercetoolsEntityRepository.Get<T>(null, null, 0, null, null, true);
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
        IEntityServiceParameters? entityServiceParameters)
    {
        var commercetoolsEntityServicesParameters = entityServiceParameters as EntityServiceParameters;

        var newItem = commercetoolsEntityRepository.Create<T>(newItemValue, commercetoolsEntityServicesParameters?.Expands);

        return new EntityCarrier
        {
            Item = new CommercetoolsEntityService<T>(commercetoolsEntityRepository, newItem)
        };
    }
}