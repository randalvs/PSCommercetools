namespace PSCommercetools.Provider.EntityServiceLayer.Services;

internal interface IBaseEntityService
{
    string Name { get; }
    bool IsContainer { get; }
    object Entity { get; }
}