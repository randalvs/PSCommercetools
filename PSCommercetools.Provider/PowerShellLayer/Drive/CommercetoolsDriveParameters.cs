using System.Collections.Generic;
using System.Management.Automation;
using JetBrains.Annotations;

namespace PSCommercetools.Provider.PowerShellLayer.Drive;

public sealed class CommercetoolsDriveParameters
{
    [UsedImplicitly]
    [Parameter(Mandatory = true)]
    public string ProjectKey { get; init; } = string.Empty;

    [UsedImplicitly]
    [Parameter(Mandatory = true)]
    public string ClientId { get; init; } = string.Empty;

    [UsedImplicitly]
    [Parameter(Mandatory = true)]
    public string ClientSecret { get; init; } = string.Empty;

    [UsedImplicitly]
    [Parameter(Mandatory = true)]
    public string Scopes { get; init; } = string.Empty;

    [UsedImplicitly]
    [Parameter(Mandatory = false)]
    public string ApiBaseAddress { get; set; } = "https://api.europe-west1.gcp.commercetools.com/";

    [UsedImplicitly]
    [Parameter(Mandatory = false)]
    public string AuthorizationBaseAddress { get; set; } = "https://auth.europe-west1.gcp.commercetools.com/";

    public Dictionary<string, string> ToDictionary(string name)
    {
        return new Dictionary<string, string>
        {
            { $"{name}:ApiBaseAddress", EnsureTrailingSlash(ApiBaseAddress) },
            { $"{name}:AuthorizationBaseAddress", EnsureTrailingSlash(AuthorizationBaseAddress) },
            { $"{name}:ClientId", ClientId },
            { $"{name}:ClientSecret", ClientSecret },
            { $"{name}:ProjectKey", ProjectKey },
            { $"{name}:Scope", Scopes }
        };
    }

    private static string EnsureTrailingSlash(string value)
    {
        return value.EndsWith('/') ? value : $"{value}/";
    }
}