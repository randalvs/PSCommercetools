using PSCommercetools.Provider.EntityServiceLayer.Parameters;

namespace PSCommercetools.Provider.EntityServiceLayer.Services;

internal interface IEntityService : IBaseEntityService
{
    void Remove(IEntityServiceParameters? entityServiceParameters);
    void Update(object actions, IEntityServiceParameters? entityServiceParameters);
}