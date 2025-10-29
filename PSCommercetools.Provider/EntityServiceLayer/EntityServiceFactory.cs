using System;
using Microsoft.Extensions.DependencyInjection;
using PSCommercetools.Provider.EntityServiceLayer.Models;
using PSCommercetools.Provider.EntityServiceLayer.Parameters;
using PSCommercetools.Provider.EntityServiceLayer.Services;
using PSCommercetools.Provider.PowerShellLayer.Drive;
using PSCommercetools.Provider.RepositoryLayer;

namespace PSCommercetools.Provider.EntityServiceLayer;

public sealed class EntityServiceFactory
{
    private readonly Func<string, string> getChildNameCallback;
    private readonly Func<string, string?, string> getParentPathCallback;

    public EntityServiceFactory(
        Func<string, string?, string> getParentPathCallback,
        Func<string, string> getChildNameCallback)
    {
        this.getParentPathCallback = getParentPathCallback;
        this.getChildNameCallback = getChildNameCallback;
    }

    internal IBaseEntityService CreateFromPath(
        CommercetoolsDrivePath commercetoolsPath,
        IEntityServiceParameters? entityServiceParameters = null)
    {
        CommercetoolsPSDriveInfo drive = commercetoolsPath.CommercetoolsPSDriveInfo;

        if (commercetoolsPath.IsDrive)
        {
            return ResolveProjectEntityService(commercetoolsPath);
        }

        string parentPath = getParentPathCallback(commercetoolsPath.Path, null);
        string name = getChildNameCallback(commercetoolsPath.Path);

        var commercetoolsParentPath = CommercetoolsDrivePath.Create(drive, parentPath);
        IBaseEntityService parentCommercetoolsEntityService = CreateFromPath(commercetoolsParentPath, entityServiceParameters);

        if (parentCommercetoolsEntityService is not IEntityContainerService entityContainerService)
        {
            throw new ArgumentException("Error resolving parent container service");
        }

        EntityCarrier entityCarrier = entityContainerService.GetChildEntity(name, entityServiceParameters);

        return entityCarrier.Item;
    }

    private static ProjectEntityService ResolveProjectEntityService(CommercetoolsDrivePath commercetoolsPath)
    {
        CommercetoolsPSDriveInfo drive = commercetoolsPath.CommercetoolsPSDriveInfo;

        return new ProjectEntityService(
            drive.ServiceProvider.GetRequiredService<CommercetoolsEntityRepository>(),
            drive.ServiceProvider.GetRequiredService<CommercetoolsApiClientRepository>());
    }
}