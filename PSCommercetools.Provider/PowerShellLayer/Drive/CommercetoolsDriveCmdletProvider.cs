using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using commercetools.Base.Client;
using commercetools.Sdk.Api;
using commercetools.Sdk.Api.Client;
using commercetools.Sdk.Api.Serialization;
using DotNetEnv;
using DotNetEnv.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PSCommercetools.Provider.DependencyInjection;
using PSCommercetools.Provider.RepositoryLayer;

namespace PSCommercetools.Provider.PowerShellLayer.Drive;

public abstract class CommercetoolsDriveCmdletProvider : CommercetoolsCmdletProviderBase
{
    protected override object NewDriveDynamicParameters()
    {
        return new CommercetoolsDriveParameters();
    }

    protected override Collection<PSDriveInfo>? InitializeDefaultDrives()
    {
        try
        {
            if (SkipInitializeDefaultDrives())
            {
                return null;
            }

            if (!TryGetPowerShellPath(out string? powerShellPath))
            {
                return null;
            }

            string envPath = Path.Combine(powerShellPath, ".env");

            if (!File.Exists(envPath))
            {
                return null;
            }

            var commercetoolsDriveParameters = Env.Load(envPath).ToDotEnvDictionary().ToCommercetoolsDriveParameters();

            var psDriveInfo = new PSDriveInfo(
                "ct-default",
                ProviderInfo,
                @"ct-default:\",
                string.Empty,
                null);

            CommercetoolsPSDriveInfo commercetoolsPSDriveInfo = CreateDrive(commercetoolsDriveParameters, psDriveInfo);

            return new Collection<PSDriveInfo>([commercetoolsPSDriveInfo]);
        }
        catch (Exception exception)
        {
            WriteError(new ErrorRecord(exception, string.Empty, ErrorCategory.NotSpecified, null));
            throw;
        }
    }

    protected override PSDriveInfo? NewDrive(PSDriveInfo drive)
    {
        try
        {
            if (drive.IsCommercetoolsDrive())
            {
                return drive;
            }

            if (!ValidateArguments(drive))
            {
                return null;
            }

            CommercetoolsDriveParameters commercetoolsDriveParameters = DynamicParameters.GetTyped();


            if (!drive.HasValidRoot())
            {
                WriteError(ErrorInfo.InvalidRoot);
                return null;
            }

            CommercetoolsPSDriveInfo commercetoolsPSDriveInfo = CreateDrive(commercetoolsDriveParameters, drive);

            return commercetoolsPSDriveInfo;
        }
        catch (Exception exception)
        {
            WriteError(new ErrorRecord(exception, string.Empty, ErrorCategory.NotSpecified, null));
            throw;
        }
    }

    protected override PSDriveInfo? RemoveDrive(PSDriveInfo drive)
    {
        try
        {
            if (drive is CommercetoolsPSDriveInfo commercetoolsApiPsDriveInfo)
            {
                return commercetoolsApiPsDriveInfo;
            }

            WriteError(ErrorInfo.InvalidDriveParameters);
            return null;
        }
        catch (Exception exception)
        {
            WriteError(new ErrorRecord(exception, string.Empty, ErrorCategory.NotSpecified, null));
            throw;
        }
    }

    private CommercetoolsPSDriveInfo CreateDrive(CommercetoolsDriveParameters commercetoolsDriveParameters,
        PSDriveInfo psDriveInfo)
    {
        IDictionary<string, IServiceProvider>
            serviceProviders = Runspace.DefaultRunspace.GetRunspaceProperties().ServiceProviders;

        if (!serviceProviders.TryGetValue(psDriveInfo.Name, out IServiceProvider? serviceProvider))
        {
            var serviceCollection = new ServiceCollection();
            ConfigureCommercetoolsApi(psDriveInfo.Name, commercetoolsDriveParameters, serviceCollection);
            serviceCollection.AddTransient<CommercetoolsEntityRepository>();
            serviceCollection.AddTransient<CommercetoolsApiClientRepository>();
            serviceProvider = serviceCollection.BuildServiceProvider();
            serviceProviders.Add(psDriveInfo.Name, serviceProvider);
        }

        var projectApiRoot = serviceProvider.GetRequiredService<ProjectApiRoot>();
        var serializerService = serviceProvider.GetRequiredService<SerializerService>();

        var commercetoolsPSDriveInfo = new CommercetoolsPSDriveInfo(psDriveInfo)
        {
            CommercetoolsCmdletProvider = (CommercetoolsCmdletProvider)this,
            ProjectApiRoot = projectApiRoot,
            SerializerService = serializerService,
            ServiceProvider = serviceProvider
        };

        return commercetoolsPSDriveInfo;
    }

    private static void ConfigureCommercetoolsApi(
        string name,
        CommercetoolsDriveParameters commercetoolsDriveParameters,
        ServiceCollection serviceCollection)
    {
        IConfigurationRoot? configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(commercetoolsDriveParameters.ToDictionary(name)).Build();
        serviceCollection.UseCommercetoolsApi(configuration, name, options: new ClientOptions
        {
            HeadNotFoundReturnsDefault = true
        });
    }

    private bool ValidateArguments(PSDriveInfo drive)
    {
        if (!drive.HasRoot())
        {
            WriteError(ErrorInfo.DriveRootNotDefined);
            return false;
        }

        if (DynamicParameters.IsCommercetoolsDriveParameters())
        {
            return true;
        }

        WriteError(ErrorInfo.InvalidDriveParameters);
        return false;
    }

    private static bool TryGetPowerShellPath([MaybeNullWhen(false)] out string powerShellPath)
    {
        powerShellPath = null;

        string userHomePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        powerShellPath = Path.Combine(userHomePath, "PowerShell");

        if (Directory.Exists(powerShellPath))
        {
            return true;
        }

        powerShellPath = null;
        return false;
    }

    private bool SkipInitializeDefaultDrives()
    {
        if (SessionState == null)
        {
            return false;
        }

        PSVariable skipVariable = SessionState.PSVariable.Get("SkipInitializeDefaultCommercetoolsDrive");

        object value = skipVariable?.Value ?? false;

        return value is true;
    }
}