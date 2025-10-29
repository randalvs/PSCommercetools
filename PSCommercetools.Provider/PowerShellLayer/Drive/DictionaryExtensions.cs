using System.Collections.Generic;

namespace PSCommercetools.Provider.PowerShellLayer.Drive;

internal static class DictionaryExtensions
{
    internal static CommercetoolsDriveParameters ToCommercetoolsDriveParameters(this Dictionary<string, string> source)
    {
        var commercetoolsDriveParameters = new CommercetoolsDriveParameters
        {
            ProjectKey = source["CTP_PROJECT_KEY"],
            ClientId = source["CTP_CLIENT_ID"],
            ClientSecret = source["CTP_CLIENT_SECRET"],
            Scopes = source["CTP_SCOPES"]
        };

        if (source.TryGetValue("CTP_API_URL", out string? apiBaseAddress))
        {
            commercetoolsDriveParameters.ApiBaseAddress = apiBaseAddress;
        }

        if (source.TryGetValue("CTP_AUTH_URL", out string? authorizationBaseAddress))
        {
            commercetoolsDriveParameters.AuthorizationBaseAddress = authorizationBaseAddress;
        }

        return commercetoolsDriveParameters;
    }
}