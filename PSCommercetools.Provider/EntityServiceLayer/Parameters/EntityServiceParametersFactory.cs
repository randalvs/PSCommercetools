using PSCommercetools.Provider.PowerShellLayer.Container;
using PSCommercetools.Provider.PowerShellLayer.Item;

namespace PSCommercetools.Provider.EntityServiceLayer.Parameters;

public static class EntityServiceParametersFactory
{
    public static IEntityServiceParameters? CreateFromPsParameters(object? dynamicParameters, string? filter)
    {
        return dynamicParameters switch
        {
            RootEntityGetChildItemDynamicParameters rootEntityGetChildItemDynamicParameters => new
                ProjectEntityServiceParameters
                {
                    WithCount = rootEntityGetChildItemDynamicParameters.WithCount
                },
            GetChildItemDynamicParameters getChildItemDynamicParameters => new EntityServicesParameters
            {
                Limit = getChildItemDynamicParameters.Limit,
                Offset = getChildItemDynamicParameters.Offset,
                Filter = filter,
                WithTotal = getChildItemDynamicParameters.WithTotal,
                Sort = getChildItemDynamicParameters.Sort,
                Expands = getChildItemDynamicParameters.Expands,
                WithCount = getChildItemDynamicParameters.WithCount
            },
            NewItemDynamicParameters or RemoveItemDynamicParameters or GetItemDynamicParameters => new EntityServiceParameters
            {
                Expands = ((dynamic)dynamicParameters).Expands
            },
            _ => null
        };
    }
}