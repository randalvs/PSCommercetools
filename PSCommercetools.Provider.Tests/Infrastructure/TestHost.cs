using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using Microsoft.Extensions.DependencyInjection;
using PSCommercetools.Provider.DependencyInjection;
using PSCommercetools.Provider.PowerShellLayer;
using PSCommercetools.Provider.RepositoryLayer;
using PSCommercetools.Provider.Tests.Extensions;
using RichardSzalay.MockHttp;

namespace PSCommercetools.Provider.Tests.Infrastructure;

internal sealed class TestHost
{
    private const string ProviderName = "PSCommercetools";
    private const string ProjectKey = "ct-project";

    private readonly ServiceProvider serviceProvider;

    private PowerShell? powerShell;

    public TestHost()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.UseCommercetoolsApiMock();
        serviceCollection.AddTransient<CommercetoolsEntityRepository>();
        serviceCollection.AddTransient<CommercetoolsApiClientRepository>();
        serviceProvider = serviceCollection.BuildServiceProvider();
    }

    public MockHttpMessageHandler CommercetoolsMockHttpMessageHandler =>
        serviceProvider.GetRequiredService<MockHttpMessageHandler>();

    public bool HasErrors => Errors.Count > 0;
    private List<ErrorRecord> Errors { get; set; } = [];

    public TestHost Initialize()
    {
        var initialSessionState = InitialSessionState.CreateDefault();
        string powerShellModuleDefinition = typeof(CommercetoolsCmdletProvider).Assembly.Location.Replace(".dll", ".psd1");

        initialSessionState.ImportPSModule(powerShellModuleDefinition);
        initialSessionState.Variables.Add(
            new SessionStateVariableEntry("SkipInitializeDefaultCommercetoolsDrive", true, string.Empty));

        powerShell = PowerShell.Create(initialSessionState);
        powerShell.Runspace.GetRunspaceProperties().ServiceProviders =
            new Dictionary<string, IServiceProvider>
            {
                { "ct-test", serviceProvider }
            };
        return this;
    }

    public TestHost WithTestPSDrive()
    {
        ArgumentNullException.ThrowIfNull(powerShell, nameof(powerShell));

        powerShell.AddCommand("New-PsDrive");
        powerShell.AddParameter("PSProvider", ProviderName)
            .AddParameter("Name", "ct-test")
            .AddParameter("Root", @"ct-test:\")
            .AddParameter("ProjectKey", ProjectKey)
            .AddParameter("ClientId", "id")
            .AddParameter("ClientSecret", "secret")
            .AddParameter("Scopes", "scopes")
            .AddParameter("Scope", "Global");
        powerShell.Invoke();
        powerShell.Commands.Clear();

        return this;
    }

    public TestHost Reset()
    {
        ArgumentNullException.ThrowIfNull(powerShell, nameof(powerShell));

        powerShell.Commands.Clear();
        SetLocationTo(@"ct-test:\");
        Errors = [];

        return this;
    }

    public void SetLocationTo(string path)
    {
        ArgumentNullException.ThrowIfNull(powerShell, nameof(powerShell));

        powerShell.AddCommand("Set-Location");
        powerShell.AddParameter("Path", path);
        powerShell.Invoke();
        powerShell.Commands.Clear();
    }

    public string? GetCurrentLocation()
    {
        ArgumentNullException.ThrowIfNull(powerShell, nameof(powerShell));

        powerShell.AddCommand("Get-Location");
        powerShell.AddParameter("PSProvider", ProviderName);
        powerShell.AddParameter("PSDrive", "ct-test");
        Collection<PSObject>? results = powerShell.Invoke();

        string? currentLocation = results.Count > 0 ? results[0].ToString() : null;

        powerShell.Commands.Clear();

        return currentLocation;
    }

    public Collection<PSObject> InvokeCommand(string command, Action<ParameterBuilder> parameterBuilderAction)
    {
        Collection<PSObject>? psObjects = InvokeInternal(command, parameterBuilderAction);

        ArgumentNullException.ThrowIfNull(psObjects);

        return psObjects;
    }

    public void InvokeScript(string script)
    {
        ArgumentNullException.ThrowIfNull(powerShell, nameof(powerShell));
        powerShell.AddScript(script);
        powerShell.Invoke();

        powerShell.Commands.Clear();
    }

    private Collection<PSObject>? InvokeInternal(string command, Action<ParameterBuilder>? parameterBuilderAction = null)
    {
        ArgumentNullException.ThrowIfNull(powerShell, nameof(powerShell));

        powerShell.AddCommand(command);

        var parameterBuilder = new ParameterBuilder();
        parameterBuilderAction?.Invoke(parameterBuilder);

        foreach (KeyValuePair<string, object> parameter in parameterBuilder.Parameters)
        {
            powerShell.AddParameter(parameter.Key, parameter.Value);
        }

        Collection<PSObject>? psObjects = powerShell.Invoke();

        if (powerShell.Streams.Error.Count > 0)
        {
            Errors.AddRange(powerShell.Streams.Error);
        }

        powerShell.Commands.Clear();

        return psObjects;
    }

    public Collection<PSObject> InvokePipeline(Action<CommandBuilder> commandBuilderAction)
    {
        ArgumentNullException.ThrowIfNull(powerShell, nameof(powerShell));
        Pipeline? pipeline = powerShell.Runspace.CreatePipeline();

        var commandBuilder = new CommandBuilder();
        commandBuilderAction.Invoke(commandBuilder);

        foreach ((string command, Dictionary<string, object> parameters) in commandBuilder.Commands)
        {
            var pipelineCommand = new Command(command);
            foreach (KeyValuePair<string, object> parameter in parameters)
            {
                pipelineCommand.Parameters.Add(new CommandParameter(parameter.Key, parameter.Value));
            }

            pipeline.Commands.Add(pipelineCommand);
        }

        Collection<PSObject>? psObjects = pipeline.Invoke();

        pipeline.Commands.Clear();

        return psObjects;
    }
}