using System;
using System.Management.Automation;

namespace PSCommercetools.Provider.PowerShellLayer.Drive;

internal static class PSDriveInfoExtensions
{
    public static bool IsCommercetoolsDrive(this PSDriveInfo drive)
    {
        return drive is CommercetoolsPSDriveInfo;
    }

    public static bool HasRoot(this PSDriveInfo drive)
    {
        return drive.Root is not null;
    }

    public static bool HasValidRoot(this PSDriveInfo drive)
    {
        return !string.IsNullOrWhiteSpace(drive.Root) &&
               drive.Root.StartsWith($@"{drive.Name}:\", StringComparison.OrdinalIgnoreCase);
    }
}