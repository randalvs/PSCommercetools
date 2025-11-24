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

    internal bool IsDrive
    {
        get
        {
            // Consider paths like "name:", "name:\" or "name:/" as the drive root
            int columnIndex = Path.IndexOf(':');
            if (columnIndex > 0)
            {
                string driveName = Path[..columnIndex];
                if (driveName.Equals(CommercetoolsPSDriveInfo.Name, StringComparison.OrdinalIgnoreCase))
                {
                    string rest = Path.Length > columnIndex + 1 ? Path[(columnIndex + 1)..] : string.Empty;
                    if (string.IsNullOrEmpty(rest) || rest == "\\" || rest == "/")
                    {
                        return true;
                    }
                }
            }

            // Fallback to comparing with Root, being separator-agnostic
            string normalizedPath = NormalizeSeparators(Path).TrimEnd('\\');
            string normalizedRoot = NormalizeSeparators(CommercetoolsPSDriveInfo.Root ?? string.Empty).TrimEnd('\\');
            return normalizedPath.Equals(normalizedRoot, StringComparison.OrdinalIgnoreCase);
        }
    }

    private static string NormalizeSeparators(string value)
    {
        return value.Replace('/', '\\');
    }

    public static CommercetoolsDrivePath Create(PSDriveInfo psDriveInfo, string path)
    {
        return new CommercetoolsDrivePath((CommercetoolsPSDriveInfo)psDriveInfo, path);
    }
}