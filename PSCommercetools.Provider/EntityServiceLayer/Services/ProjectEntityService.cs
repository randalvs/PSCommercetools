using System;
using System.Linq;
using PSCommercetools.Provider.EntityServiceLayer.Generated;
using PSCommercetools.Provider.EntityServiceLayer.Models;
using PSCommercetools.Provider.EntityServiceLayer.Parameters;
using PSCommercetools.Provider.RepositoryLayer;

namespace PSCommercetools.Provider.EntityServiceLayer.Services;

internal sealed class ProjectEntityService : IEntityContainerService
{
    private readonly CommercetoolsApiClientRepository commercetoolsApiClientRepository;
    private readonly CommercetoolsEntityRepository commercetoolsEntityRepository;

    public ProjectEntityService(
        CommercetoolsEntityRepository commercetoolsEntityRepository,
        CommercetoolsApiClientRepository commercetoolsApiClientRepository)
    {
        this.commercetoolsEntityRepository = commercetoolsEntityRepository;
        this.commercetoolsApiClientRepository = commercetoolsApiClientRepository;
    }

    public bool IsContainer => true;
    public object Entity => new ProjectEntity();

    public string Name => "project";

    public EntityCarrier GetChildEntity(string name, IEntityServiceParameters? entityServiceParameters)
    {
        IBaseEntityService? entityService = name switch
        {
            "apiclients" => new ApiClientContainerEntityService(commercetoolsApiClientRepository),
            _ => Entities.GetCommercetoolsContainerEntityService(commercetoolsEntityRepository, name)
        };

        if (entityService == null)
        {
            throw new ArgumentException("Invalid parameters provided");
        }

        return new EntityCarrier
        {
            Item = entityService
        };
    }

    public EntitiesCarrier GetChildEntities(IEntityServiceParameters? entityServiceParameters)
    {
        var projectCommercetoolsEntityServiceParameters =
            entityServiceParameters as ProjectEntityServiceParameters;

        IEntityContainerService[] entityServices =
            Entities.BuildEntityServicesList(commercetoolsEntityRepository)
                .Append(new ApiClientContainerEntityService(commercetoolsApiClientRepository))
                .ToArray();

        if (!projectCommercetoolsEntityServiceParameters?.WithCount ?? true)
        {
            return new EntitiesCarrier
            {
                Items = entityServices
            };
        }

        foreach (IEntityContainerService entityService in entityServices)
        {
            entityService.SetChildCount();
        }

        return new EntitiesCarrier
        {
            Items = entityServices
        };
    }

    public bool HasChild(string name)
    {
        string lowerName = name.ToLower();

        return Entities.EntitiesList.Contains(lowerName);
    }

    public void SetChildCount()
    {
        throw new NotImplementedException();
    }

    public EntityCarrier CreateChildEntity(
        object newItemValue,
        IEntityServiceParameters? entityServiceParameters)
    {
        throw new NotImplementedException();
    }
}