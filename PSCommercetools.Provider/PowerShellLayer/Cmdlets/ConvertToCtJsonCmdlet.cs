using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text.Encodings.Web;
using System.Text.Json;
using commercetools.Sdk.Api.Serialization;
using PSCommercetools.Provider.EntityServiceLayer.Services;
using PSCommercetools.Provider.PowerShellLayer.CmdLets.Infrastructure;
using PSCommercetools.Provider.PowerShellLayer.Drive;
using PSCommercetools.Provider.PowerShellLayer.Extensions;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global

namespace PSCommercetools.Provider.PowerShellLayer.CmdLets;

[Cmdlet("ConvertTo", "CtJson", DefaultParameterSetName = "PathParameterSet")]
public class ConvertToCtJsonCmdlet : CommercetoolsCmdlet
{
    private readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    [Parameter(Position = 1, Mandatory = false, ValueFromPipeline = true)]
    public SwitchParameter Prettify { get; set; } = true;

    protected override void ProcessRecordByObjectParameterSet(PSObject psObject)
    {
        WriteJson(psObject.GetCommercetoolsSerializer(), psObject.BaseObject);
    }

    internal override void ProcessRecordByInputPathParameterSet(List<CommercetoolsDrivePath> commercetoolsPaths)
    {
        commercetoolsPaths.ForEach(commercetoolsPath =>
        {
            CommercetoolsPSDriveInfo drive = commercetoolsPath.CommercetoolsPSDriveInfo;
            CommercetoolsCmdletProvider provider = drive.CommercetoolsCmdletProvider;

            IBaseEntityService commercetoolsEntityService = provider.EntityServiceFactory.CreateFromPath(commercetoolsPath);

            if (commercetoolsEntityService is not IEntityService entityContainerService)
            {
                throw new ArgumentException("Error resolving entity service");
            }

            WriteJson(drive.SerializerService, entityContainerService.Entity);
        });
    }

    private void WriteJson(SerializerService serializerService, object commercetoolsEntity)
    {
        string? json = serializerService.Serialize(commercetoolsEntity);

        if (json is null)
        {
            throw new Exception("Could not serialize Commercetools entity to json.");
        }

        if (Prettify.IsPresent)
        {
            json = PrettifyJson(json, jsonSerializerOptions);
        }

        WriteObject(json, false);
    }

    private static string PrettifyJson(string jsonString, JsonSerializerOptions options)
    {
        using JsonDocument jsonDocument = JsonDocument.Parse(jsonString);
        string prettyJson = JsonSerializer.Serialize(jsonDocument.RootElement, options);
        return prettyJson;
    }
}