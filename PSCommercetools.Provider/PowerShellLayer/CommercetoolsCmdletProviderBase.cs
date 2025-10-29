using System.Management.Automation.Provider;
using PSCommercetools.Provider.EntityServiceLayer;

namespace PSCommercetools.Provider.PowerShellLayer;

public class CommercetoolsCmdletProviderBase : NavigationCmdletProvider
{
    private EntityServiceFactory? entityServiceFactory;

    public EntityServiceFactory EntityServiceFactory =>
        entityServiceFactory ??= new EntityServiceFactory(GetParentPath, GetChildName);

    protected override bool IsValidPath(string path)
    {
        return true;
    }
}