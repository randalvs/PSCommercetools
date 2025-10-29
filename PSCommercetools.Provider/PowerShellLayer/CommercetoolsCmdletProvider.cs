using System.Management.Automation.Provider;
using PSCommercetools.Provider.PowerShellLayer.Navigation;

// ReSharper disable RedundantTypeDeclarationBody

namespace PSCommercetools.Provider.PowerShellLayer;

[CmdletProvider("PSCommercetools", ProviderCapabilities.Filter | ProviderCapabilities.ShouldProcess)]
public sealed class CommercetoolsCmdletProvider : CommercetoolsNavigationCmdletProvider
{
}