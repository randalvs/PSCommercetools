using System.Management.Automation;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace PSCommercetools.Provider.PowerShellLayer.Container;

public sealed class RootEntityGetChildItemDynamicParameters
{
    [Parameter] public SwitchParameter WithCount { get; set; }
}