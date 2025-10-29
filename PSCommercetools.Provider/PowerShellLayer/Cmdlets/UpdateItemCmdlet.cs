using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using commercetools.Sdk.Api.Models.CustomObjects;
using PSCommercetools.Provider.EntityServiceLayer.Parameters;
using PSCommercetools.Provider.EntityServiceLayer.Services;
using PSCommercetools.Provider.PowerShellLayer.CmdLets.Infrastructure;
using PSCommercetools.Provider.PowerShellLayer.Drive;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global

namespace PSCommercetools.Provider.PowerShellLayer.CmdLets;

[Cmdlet("Update", "Item", DefaultParameterSetName = "PathParameterSet")]
public class UpdateItemCmdlet : CommercetoolsCmdlet
{
    [Parameter(Mandatory = false)]
    [ValidateNotNullOrEmpty]
    public object? Actions { get; set; }

    [Parameter(Mandatory = false)]
    [ValidateNotNullOrEmpty]
    public object? CustomObjectDraft { get; set; }

    [Parameter(Mandatory = false)] public string[]? Expands { get; set; }

    internal override void ProcessRecordByInputPathParameterSet(List<CommercetoolsDrivePath> commercetoolsPaths)
    {
        commercetoolsPaths.ForEach(commercetoolsPath =>
        {
            CommercetoolsPSDriveInfo drive = commercetoolsPath.CommercetoolsPSDriveInfo;
            CommercetoolsCmdletProvider provider = drive.CommercetoolsCmdletProvider;

            IBaseEntityService commercetoolsEntityService = provider.EntityServiceFactory.CreateFromPath(commercetoolsPath);

            if (commercetoolsEntityService is not IEntityService entityService || entityService.IsContainer)
            {
                throw new ArgumentException("Error resolving entity service");
            }

            UpdateEntity(entityService);
        });
    }

    protected override void ProcessRecordByObjectParameterSet(PSObject psObject)
    {
        var psPath = psObject.Properties["PSPath"].Value.ToString();

        Collection<PathInfo>? pathInfos = SessionState.Path.GetResolvedPSPathFromPSPath(psPath);

        List<CommercetoolsDrivePath> commercetoolsPaths =
            pathInfos.Select(pathInfo => pathInfo.ToCommercetoolsDrivePath()).ToList();

        commercetoolsPaths.ForEach(commercetoolsPath =>
        {
            CommercetoolsCmdletProvider provider = commercetoolsPath.CommercetoolsPSDriveInfo.CommercetoolsCmdletProvider;
            IBaseEntityService commercetoolsEntityService = provider.EntityServiceFactory.CreateFromPath(commercetoolsPath);

            if (commercetoolsEntityService is not IEntityService entityService || entityService.IsContainer)
            {
                throw new ArgumentException("Error resolving entity service");
            }

            UpdateEntity(entityService);
        });
    }

    private void UpdateEntity(IEntityService entityService)
    {
        EntityServiceParameters? entityServiceParameters =
            Expands?.Length > 0 ? new EntityServiceParameters { Expands = Expands } : null;

        if (entityService is CommercetoolsEntityService<ICustomObject>)
        {
            ArgumentNullException.ThrowIfNull(CustomObjectDraft);
            entityService.Update(CustomObjectDraft, entityServiceParameters);
            return;
        }

        ArgumentNullException.ThrowIfNull(Actions);
        entityService.Update(Actions, entityServiceParameters);
    }
}

internal static class PathInfoExtensions
{
    public static CommercetoolsDrivePath ToCommercetoolsDrivePath(this PathInfo pathInfo)
    {
        int driveSeparator = pathInfo.ProviderPath.IndexOf(":", StringComparison.OrdinalIgnoreCase);

        string driveName = pathInfo.ProviderPath[..driveSeparator];

        PSDriveInfo? psDriveInfo =
            pathInfo.Provider.Drives.FirstOrDefault(d => d.Name.Equals(driveName, StringComparison.OrdinalIgnoreCase));

        return psDriveInfo == null
            ? throw new ArgumentException($"Could not find drive '{driveName}'.")
            : CommercetoolsDrivePath.Create(psDriveInfo, pathInfo.ProviderPath);
    }
}