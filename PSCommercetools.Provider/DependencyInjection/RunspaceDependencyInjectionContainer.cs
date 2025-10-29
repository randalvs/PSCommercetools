using System;
using System.Collections.Generic;

namespace PSCommercetools.Provider.DependencyInjection;

public sealed class RunspaceDependencyInjectionContainer
{
    public IDictionary<string, IServiceProvider> ServiceProviders { get; set; } = new Dictionary<string, IServiceProvider>();
}