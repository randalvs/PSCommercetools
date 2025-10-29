using System.Management.Automation.Runspaces;
using System.Runtime.CompilerServices;

namespace PSCommercetools.Provider.DependencyInjection;

public static class RunspaceExtensions
{
    private static readonly ConditionalWeakTable<Runspace, RunspaceDependencyInjectionContainer> Data = new();

    public static RunspaceDependencyInjectionContainer GetRunspaceProperties(this Runspace runspace) =>
        Data.GetOrCreateValue(runspace);
}