using System.Collections.Generic;

namespace PSCommercetools.Provider.Tests.Infrastructure;

internal sealed class ParameterBuilder
{
    public Dictionary<string, object> Parameters { get; } = new();

    public ParameterBuilder WithParameter(string name, object value)
    {
        Parameters.Add(name, value);
        return this;
    }
}