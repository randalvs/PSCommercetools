using PSCommercetools.Provider.EntityServiceLayer.Parameters;

namespace PSCommercetools.Provider.EntityServiceLayer.Services;

internal interface IEntityService : IBaseEntityService
{
    object Remove(long? version, IEntityServiceParameters? entityServiceParameters);
    object Update(long? version, object actions, IEntityServiceParameters? entityServiceParameters);
}