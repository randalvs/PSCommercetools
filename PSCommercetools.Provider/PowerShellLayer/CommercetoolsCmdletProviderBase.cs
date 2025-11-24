using System;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Provider;
using System.Runtime.CompilerServices;
using PSCommercetools.Provider.EntityServiceLayer;
using PSCommercetools.Provider.PowerShellLayer.Drive;

namespace PSCommercetools.Provider.PowerShellLayer;

public class CommercetoolsCmdletProviderBase : NavigationCmdletProvider
{
    private const string DebugFlagName = "PSCommercetoolsProviderDebug";
    private EntityServiceFactory? entityServiceFactory;

    public EntityServiceFactory EntityServiceFactory =>
        entityServiceFactory ??= new EntityServiceFactory(GetParentPath, GetChildName);

    private bool DebugEnabled
    {
        get
        {
            object? val = SessionState.PSVariable.GetValue(DebugFlagName);
            return val is true;
        }
    }

    protected override bool IsValidPath(string path)
    {
        return true;
    }

    protected CommercetoolsPSDriveInfo ResolveDriveInfo(string path)
    {
        var drive = (PSDriveInfo ?? SessionState.Drive.Current) as CommercetoolsPSDriveInfo;

        if (drive != null || SessionState == null)
        {
            return drive == null ? throw new InvalidOperationException("Unable to determine PSDriveInfo context.") : drive;
        }

        _ = SessionState.Path.GetResolvedProviderPathFromPSPath(path, out ProviderInfo provider);
        drive = provider?.Drives?.FirstOrDefault() as CommercetoolsPSDriveInfo;

        return drive == null ? throw new InvalidOperationException("Unable to determine PSDriveInfo context.") : drive;
    }

    protected void WriteProviderDebug(string message, [CallerMemberName] string? callerMemberName = "")
    {
        if (DebugEnabled)
        {
            Host.UI.WriteLine(ConsoleColor.Yellow, Console.BackgroundColor, $"[PSCommercetools.{callerMemberName}] :: {message}");
        }
    }
}