using System.Management.Automation;

// ReSharper disable UnusedMember.Global

namespace PSCommercetools.Provider.PowerShellLayer.Item;

public sealed class NewItemDynamicParameters
{
    [Parameter] public string[]? Expands { get; set; }
}