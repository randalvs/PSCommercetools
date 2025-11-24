using System;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Provider;
using System.Runtime.CompilerServices;
using PSCommercetools.Provider.EntityServiceLayer;
using PSCommercetools.Provider.PowerShellLayer.Drive;

namespace PSCommercetools.Provider.PowerShellLayer;

public class CommercetoolsCmdletProviderBase : NavigationCmdletProvider
{
    private const string DebugFlagName = "PSCommercetoolsProviderDebug";
    private EntityServiceFactory? entityServiceFactory;

    public EntityServiceFactory EntityServiceFactory =>
        entityServiceFactory ??= new EntityServiceFactory(GetParentPath, GetChildName);

    private bool DebugEnabled
    {
        get
        {
            object? val = SessionState.PSVariable.GetValue(DebugFlagName);
            return val is true;
        }
    }

    protected override bool IsValidPath(string path)
    {
        return true;
    }

    protected CommercetoolsPSDriveInfo ResolveDriveInfo(string path)
    {
        // Prefer the current drive,, if it is our drive
        var current = (PSDriveInfo ?? SessionState?.Drive.Current) as CommercetoolsPSDriveInfo;
        if (current != null)
        {
            return current;
        }

        if (SessionState == null)
        {
            throw new InvalidOperationException("Unable to determine PSDriveInfo context.");
        }

        // Try to resolve by drive name from the path without doing path resolution (cross-platform safe)
        if (!string.IsNullOrWhiteSpace(path))
        {
            int driveSeparatorIndex = path.IndexOf(':');
            if (driveSeparatorIndex > 0)
            {
                string driveName = path[..driveSeparatorIndex];
                var providerDrives = ProviderInfo?.Drives;
                var namedDrive = providerDrives?
                    .FirstOrDefault(d => d.Name.Equals(driveName, StringComparison.OrdinalIgnoreCase))
                    as CommercetoolsPSDriveInfo;
                if (namedDrive != null)
                {
                    return namedDrive;
                }
            }
        }

        // Fallback to the first drive of this provider if available
        var anyDrive = ProviderInfo?.Drives?.FirstOrDefault() as CommercetoolsPSDriveInfo;
        if (anyDrive != null)
        {
            return anyDrive;
        }

        throw new InvalidOperationException("Unable to determine PSDriveInfo context.");
    }

    protected void WriteProviderDebug(string message, [CallerMemberName] string? callerMemberName = "")
    {
        if (DebugEnabled)
        {
            Host.UI.WriteLine(ConsoleColor.Yellow, Console.BackgroundColor, $"[PSCommercetools.{callerMemberName}] :: {message}");
        }
    }
}