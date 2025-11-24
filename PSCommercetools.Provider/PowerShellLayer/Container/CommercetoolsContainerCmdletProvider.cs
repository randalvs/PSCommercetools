using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using PSCommercetools.Provider.EntityServiceLayer.Models;
using PSCommercetools.Provider.EntityServiceLayer.Parameters;
using PSCommercetools.Provider.EntityServiceLayer.Services;
using PSCommercetools.Provider.PowerShellLayer.Drive;
using PSCommercetools.Provider.PowerShellLayer.Item;

namespace PSCommercetools.Provider.PowerShellLayer.Container;

public abstract class CommercetoolsContainerCmdletProvider : CommercetoolsItemCmdletProvider
{
    protected override bool HasChildItems(string path)
    {
        try
        {
            WriteProviderDebug($"path={path}");

            CommercetoolsPSDriveInfo drive = ResolveDriveInfo(path);

            var commercetoolsPath = CommercetoolsDrivePath.Create(drive, path);

            if (commercetoolsPath.HasEmptyPath)
            {
                return true;
            }

            IBaseEntityService commercetoolsEntityService = EntityServiceFactory.CreateFromPath(commercetoolsPath);

            if (!commercetoolsEntityService.IsContainer)
            {
                return false;
            }

            return commercetoolsEntityService is not IEntityContainerService entityContainerService
                ? throw new ArgumentException("Error resolving entity container service")
                : entityContainerService.GetChildEntities(null).Items.Any();
        }
        catch (Exception exception)
        {
            WriteError(new ErrorRecord(exception, string.Empty, ErrorCategory.NotSpecified, null));
            throw;
        }
    }

    protected override void GetChildItems(string path, bool recurse, uint depth)
    {
        try
        {
            WriteProviderDebug($"path={path}");

            CommercetoolsPSDriveInfo drive = ResolveDriveInfo(path);

            var commercetoolsDrivePath = CommercetoolsDrivePath.Create(drive, path);

            IBaseEntityService commercetoolsEntityService = EntityServiceFactory.CreateFromPath(commercetoolsDrivePath);
            IEntityServiceParameters? commercetoolsEntityServiceParameters =
                EntityServiceParametersFactory.CreateFromPsParameters(DynamicParameters, Filter);

            if (commercetoolsEntityService is not IEntityContainerService entityContainerService)
            {
                throw new ArgumentException("Error resolving entity container service");
            }

            EntitiesCarrier entitiesCarrier = entityContainerService.GetChildEntities(commercetoolsEntityServiceParameters);

            IEnumerable<IBaseEntityService> childEntityServices = entitiesCarrier.Items;

            OutputPagingInfo(entitiesCarrier, commercetoolsEntityServiceParameters);

            foreach (IBaseEntityService childEntityService in childEntityServices)
            {
                WriteItemObject(childEntityService.Entity,
                    MakePath(path, childEntityService.Name),
                    childEntityService.IsContainer);
            }
        }
        catch (Exception exception)
        {
            WriteError(new ErrorRecord(exception, string.Empty, ErrorCategory.NotSpecified, null));
        }
    }

    private void OutputPagingInfo(EntitiesCarrier entitiesCarrier,
        IEntityServiceParameters? commercetoolsEntityServiceParametersParameter)
    {
        if (commercetoolsEntityServiceParametersParameter is not EntityServicesParameters { WithCount: true })
        {
            return;
        }

        Host.UI.WriteLine(string.Empty);
        Host.UI.WriteLine($"Limit  : {entitiesCarrier.Limit}");
        Host.UI.WriteLine($"Offset : {entitiesCarrier.Offset}");
        Host.UI.WriteLine($"Count  : {entitiesCarrier.Count}");
        if (entitiesCarrier.Total is not null)
        {
            Host.UI.WriteLine($"Total  : {entitiesCarrier.Total}");
        }

        Host.UI.WriteLine(string.Empty);
    }

    protected override object GetChildItemsDynamicParameters(string path, bool recurse)
    {
        CommercetoolsPSDriveInfo drive = ResolveDriveInfo(path);

        var commercetoolsPath = CommercetoolsDrivePath.Create(drive, path);

        if (commercetoolsPath.IsDrive)
        {
            return new RootEntityGetChildItemDynamicParameters();
        }

        return new GetChildItemDynamicParameters();
    }

    protected override void RemoveItem(string path, bool recurse)
    {
        try
        {
            WriteProviderDebug($"path={path}");

            if (!ShouldProcess(path, "Remove"))
            {
                return;
            }

            CommercetoolsPSDriveInfo drive = ResolveDriveInfo(path);

            var commercetoolsDrivePath = CommercetoolsDrivePath.Create(drive, path);
            IBaseEntityService commercetoolsEntityService = EntityServiceFactory.CreateFromPath(commercetoolsDrivePath);

            IEntityServiceParameters? commercetoolsEntityServiceParameters =
                EntityServiceParametersFactory.CreateFromPsParameters(DynamicParameters, Filter);

            if (commercetoolsEntityService is not IEntityService entityService)
            {
                throw new ArgumentException("Error resolving entity service");
            }

            entityService.Remove(commercetoolsEntityServiceParameters);
        }
        catch (Exception exception)
        {
            WriteError(new ErrorRecord(exception, string.Empty, ErrorCategory.NotSpecified, null));
        }
    }

    protected override object RemoveItemDynamicParameters(string path, bool recurse)
    {
        return new RemoveItemDynamicParameters();
    }
}