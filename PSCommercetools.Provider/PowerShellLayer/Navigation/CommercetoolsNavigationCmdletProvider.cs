using System;
using System.Management.Automation;
using PSCommercetools.Provider.EntityServiceLayer.Services;
using PSCommercetools.Provider.PowerShellLayer.Container;
using PSCommercetools.Provider.PowerShellLayer.Drive;
using PSCommercetools.Provider.PowerShellLayer.Extensions;

namespace PSCommercetools.Provider.PowerShellLayer.Navigation;

public abstract class CommercetoolsNavigationCmdletProvider : CommercetoolsContainerCmdletProvider
{
    public override char ItemSeparator => '\\';

    protected override bool IsItemContainer(string path)
    {
        try
        {
            CommercetoolsPSDriveInfo drive = ResolveDriveInfo(path);

            var commercetoolsPath = CommercetoolsDrivePath.Create(drive, path);

            IBaseEntityService commercetoolsEntityService = EntityServiceFactory.CreateFromPath(commercetoolsPath);

            return commercetoolsEntityService.IsContainer;
        }
        catch (Exception exception)
        {
            WriteError(new ErrorRecord(exception, string.Empty, ErrorCategory.NotSpecified, null));
            throw;
        }
    }

    protected override string GetChildName(string path)
    {
        try
        {
            if (path.EndsWith('\\'))
            {
                path = path[..^1];
            }

            int separatorIndex = path.LastIndexOf(@"\", StringComparison.OrdinalIgnoreCase);
            string retVal = separatorIndex == -1 ? path : path[(separatorIndex + 1)..];

            return retVal;
        }
        catch (Exception exception)
        {
            WriteError(new ErrorRecord(exception, string.Empty, ErrorCategory.NotSpecified, null));
            throw;
        }
    }

    protected override string MakePath(string parent, string child)
    {
        try
        {
            child = child.ToLowerInvariant().RemoveTrailingSlash();
            parent = parent.ToLowerInvariant().RemoveTrailingSlash();

            string? baseMakePath = base.MakePath(parent, child);
            return baseMakePath;
        }
        catch (Exception exception)
        {
            WriteError(new ErrorRecord(exception, string.Empty, ErrorCategory.NotSpecified, null));
            throw;
        }
    }
}