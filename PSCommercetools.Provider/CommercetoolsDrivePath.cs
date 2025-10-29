using System;
using System.Management.Automation;
using PSCommercetools.Provider.PowerShellLayer.Drive;

namespace PSCommercetools.Provider;

internal sealed class CommercetoolsDrivePath
{
    private CommercetoolsDrivePath(CommercetoolsPSDriveInfo commercetoolsPSDriveInfo, string path)
    {
        CommercetoolsPSDriveInfo = commercetoolsPSDriveInfo;
        Path = path;
    }

    internal bool HasEmptyPath => string.IsNullOrWhiteSpace(Path);

    internal CommercetoolsPSDriveInfo CommercetoolsPSDriveInfo { get; }
    internal string Path { get; }

    internal bool IsDrive =>
        string.Compare(Path, 0, CommercetoolsPSDriveInfo.Root, 0, Path.Length, StringComparison.OrdinalIgnoreCase) == 0;

    public static CommercetoolsDrivePath Create(PSDriveInfo psDriveInfo, string path)
    {
        return new CommercetoolsDrivePath((CommercetoolsPSDriveInfo)psDriveInfo, path);
    }
}