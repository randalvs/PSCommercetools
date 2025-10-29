using System;

namespace PSCommercetools.Provider.PowerShellLayer.Drive;

internal static class DynamicParametersExtensions
{
    public static bool IsCommercetoolsDriveParameters(this object dynamicParameters)
    {
        return dynamicParameters is CommercetoolsDriveParameters;
    }

    public static CommercetoolsDriveParameters GetTyped(this object dynamicParameters)
    {
        var commercetoolsDriveParameters = dynamicParameters as CommercetoolsDriveParameters;

        ArgumentNullException.ThrowIfNull(commercetoolsDriveParameters);

        return commercetoolsDriveParameters;
    }
}