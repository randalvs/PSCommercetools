using System;
using System.Management.Automation;
using PSCommercetools.Provider.EntityServiceLayer.Models;
using PSCommercetools.Provider.EntityServiceLayer.Parameters;
using PSCommercetools.Provider.EntityServiceLayer.Services;
using PSCommercetools.Provider.PowerShellLayer.Drive;

namespace PSCommercetools.Provider.PowerShellLayer.Item;

public abstract class CommercetoolsItemCmdletProvider : CommercetoolsDriveCmdletProvider
{
    protected override void GetItem(string path)
    {
        try
        {
            WriteProviderDebug($"path={path}");

            CommercetoolsPSDriveInfo drive = ResolveDriveInfo(path);

            var commercetoolsPath = CommercetoolsDrivePath.Create(drive, path);

            IEntityServiceParameters? commercetoolsEntityServiceParameters =
                EntityServiceParametersFactory.CreateFromPsParameters(DynamicParameters, Filter);

            IBaseEntityService baseEntityService =
                EntityServiceFactory.CreateFromPath(commercetoolsPath, commercetoolsEntityServiceParameters);

            if (baseEntityService is not IEntityService commercetoolsEntityService)
            {
                throw new ArgumentException("Error resolving entity service");
            }

            object entity = commercetoolsEntityService.Entity;

            WriteItemObject(entity, path, commercetoolsEntityService.IsContainer);
        }
        catch (Exception exception)
        {
            WriteError(new ErrorRecord(exception, string.Empty, ErrorCategory.NotSpecified, null));
            throw;
        }
    }

    protected override object GetItemDynamicParameters(string path)
    {
        return new GetItemDynamicParameters();
    }

    protected override bool ItemExists(string path)
    {
        try
        {
            WriteProviderDebug($"path={path}");

            CommercetoolsPSDriveInfo drive = ResolveDriveInfo(path);

            var commercetoolsPath = CommercetoolsDrivePath.Create(drive, path);
            if (commercetoolsPath.IsDrive)
            {
                return true;
            }

            string parentPath = GetParentPath(path, null);
            string childName = GetChildName(path);

            if (!ItemExists(parentPath))
            {
                return false;
            }

            var parentCommercetoolsPath = CommercetoolsDrivePath.Create(drive, parentPath);

            IBaseEntityService commercetoolsEntityService = EntityServiceFactory.CreateFromPath(parentCommercetoolsPath);

            return commercetoolsEntityService is not IEntityContainerService entityContainerService
                ? throw new ArgumentException("Error resolving entity container service")
                : entityContainerService.HasChild(childName);
        }
        catch (Exception exception)
        {
            WriteError(new ErrorRecord(exception, string.Empty, ErrorCategory.NotSpecified, null));
            throw;
        }
    }

    protected override void NewItem(string path, string itemTypeName, object newItemValue)
    {
        try
        {
            WriteProviderDebug($"path={path}");

            CommercetoolsPSDriveInfo drive = ResolveDriveInfo(path);

            var commercetoolsPath = CommercetoolsDrivePath.Create(drive, path);
            IBaseEntityService entityService = EntityServiceFactory.CreateFromPath(commercetoolsPath);

            IEntityServiceParameters? commercetoolsEntityServiceParameters =
                EntityServiceParametersFactory.CreateFromPsParameters(DynamicParameters, Filter);

            if (entityService is not IEntityContainerService entityContainerService)
            {
                throw new ArgumentException("Cannot create a new item for this path.");
            }

            EntityCarrier entityCarrier = entityContainerService.CreateChildEntity(
                newItemValue,
                commercetoolsEntityServiceParameters);

            WriteItemObject(entityCarrier.Item.Entity, path, entityCarrier.Item.IsContainer);
        }
        catch (Exception exception)
        {
            WriteError(new ErrorRecord(exception, string.Empty, ErrorCategory.NotSpecified, null));
            throw;
        }
    }

    protected override object NewItemDynamicParameters(string path, string itemTypeName, object newItemValue)
    {
        return new NewItemDynamicParameters();
    }
}