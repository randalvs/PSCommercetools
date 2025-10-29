using System;
using System.Management.Automation;

namespace PSCommercetools.Provider.PowerShellLayer;

internal static class ErrorInfo
{
    public static ErrorRecord DriveRootNotDefined =>
        new(new ArgumentNullException(message: "Drive root not defined.", null), "1", ErrorCategory.InvalidArgument, null);

    public static ErrorRecord InvalidDriveParameters =>
        new(new ArgumentNullException(message: "Invalid drive parameters provided.", null), "2", ErrorCategory.InvalidArgument,
            null);

    public static ErrorRecord InvalidRoot =>
        new(new ArgumentException("Root must start with project key follow by ':\'."), "3", ErrorCategory.InvalidArgument,
            null);
}