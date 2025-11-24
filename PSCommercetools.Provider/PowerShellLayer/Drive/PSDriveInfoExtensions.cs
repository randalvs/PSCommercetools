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
        if (string.IsNullOrWhiteSpace(drive.Root))
        {
            return false;
        }

        // Accept both Windows and Unix style after the drive name
        if (!drive.Root.StartsWith($"{drive.Name}:", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (drive.Root.Length == drive.Name.Length + 1)
        {
            // Exactly "name:" is also considered valid
            return true;
        }

        char sep = drive.Root[drive.Name.Length + 1];
        return sep == '\\' || sep == '/';
    }
}