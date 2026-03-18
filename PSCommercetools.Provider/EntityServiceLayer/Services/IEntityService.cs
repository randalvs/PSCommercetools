using PSCommercetools.Provider.EntityServiceLayer.Parameters;

namespace PSCommercetools.Provider.EntityServiceLayer.Services;

internal interface IEntityService : IBaseEntityService
{
    object Remove(IEntityServiceParameters? entityServiceParameters);
    object Update(object actions, IEntityServiceParameters? entityServiceParameters);
}