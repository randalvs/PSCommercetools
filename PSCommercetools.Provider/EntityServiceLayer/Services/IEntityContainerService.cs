using PSCommercetools.Provider.EntityServiceLayer.Models;
using PSCommercetools.Provider.EntityServiceLayer.Parameters;

namespace PSCommercetools.Provider.EntityServiceLayer.Services;

internal interface IEntityContainerService : IBaseEntityService
{
    EntityCarrier GetChildEntity(string name, IEntityServiceParameters? entityServiceParameters);
    EntitiesCarrier GetChildEntities(IEntityServiceParameters? entityServiceParameters);
    bool HasChild(string name);
    void SetChildCount();
    EntityCarrier CreateChildEntity(object newItemValue, IEntityServiceParameters? entityServiceParameters);
}