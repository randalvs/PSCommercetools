using System.Management.Automation;

// ReSharper disable UnusedMember.Global

namespace PSCommercetools.Provider.PowerShellLayer.Container;

public sealed class RemoveItemDynamicParameters
{
    [Parameter] public string[]? Expands { get; set; }
}