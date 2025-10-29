using System;
using System.Collections.Generic;

namespace PSCommercetools.Provider.Tests.Infrastructure;

internal sealed class CommandBuilder
{
    public List<(string, Dictionary<string, object>)> Commands { get; } = [];

    public CommandBuilder WithCommand(string command, Action<ParameterBuilder>? parameterBuilderAction = null)
    {
        var parameterBuilder = new ParameterBuilder();
        parameterBuilderAction?.Invoke(parameterBuilder);

        Commands.Add((command, parameterBuilder.Parameters));

        return this;
    }
}