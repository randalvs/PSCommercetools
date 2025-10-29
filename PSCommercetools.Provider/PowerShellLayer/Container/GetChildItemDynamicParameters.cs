using System.Management.Automation;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace PSCommercetools.Provider.PowerShellLayer.Container;

public sealed class GetChildItemDynamicParameters
{
    [Parameter] public int? Limit { get; set; }
    [Parameter] public string? Sort { get; set; }
    [Parameter] public int? Offset { get; set; }
    [Parameter] public SwitchParameter WithTotal { get; set; }
    [Parameter] public string[]? Expands { get; set; }
    [Parameter] public SwitchParameter WithCount { get; set; }
}