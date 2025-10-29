using System;
using System.Management.Automation;
using commercetools.Sdk.Api.Client;
using commercetools.Sdk.Api.Serialization;

namespace PSCommercetools.Provider.PowerShellLayer.Drive;

public sealed class CommercetoolsPSDriveInfo : PSDriveInfo
{
    internal CommercetoolsPSDriveInfo(PSDriveInfo driveInfo) : base(driveInfo)
    {
    }

    public required CommercetoolsCmdletProvider CommercetoolsCmdletProvider { get; init; }
    public required ProjectApiRoot ProjectApiRoot { get; init; }
    public required SerializerService SerializerService { get; init; }
    public required IServiceProvider ServiceProvider { get; init; }
}