using System.Management.Automation;

// ReSharper disable UnusedMember.Global

namespace PSCommercetools.Provider.PowerShellLayer.Item;

public sealed class GetItemDynamicParameters
{
    [Parameter] public string[]? Expands { get; set; }
}