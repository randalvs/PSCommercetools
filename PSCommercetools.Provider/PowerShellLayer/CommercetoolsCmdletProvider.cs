using System.Management.Automation;
using System.Management.Automation.Provider;
using PSCommercetools.Provider.PowerShellLayer.Navigation;

// ReSharper disable RedundantTypeDeclarationBody

namespace PSCommercetools.Provider.PowerShellLayer;

[CmdletProvider("PSCommercetools", ProviderCapabilities.Filter | ProviderCapabilities.ShouldProcess)]
public sealed class CommercetoolsCmdletProvider : CommercetoolsNavigationCmdletProvider
{
    protected override ProviderInfo Start(ProviderInfo providerInfo)
    {
        if (!SilentStartup())
        {
            WriteStartupMessage();
        }

        return base.Start(providerInfo);
    }

    private void WriteStartupMessage()
    {
        Host.UI.WriteLine(string.Empty);
        Host.UI.WriteLine("Commercetools PowerShell provider (v0.0.7) started.");
        Host.UI.WriteLine(string.Empty);
    }

    private bool SilentStartup()
    {
        if (SessionState == null)
        {
            return false;
        }

        PSVariable silentStartupVariable = SessionState.PSVariable.Get("SilentStartup");

        object value = silentStartupVariable?.Value ?? false;

        return value is true;
    }
}