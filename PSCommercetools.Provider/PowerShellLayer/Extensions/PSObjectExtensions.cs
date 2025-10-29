using System;
using System.Management.Automation;
using commercetools.Sdk.Api.Client;
using commercetools.Sdk.Api.Serialization;
using PSCommercetools.Provider.PowerShellLayer.Drive;

namespace PSCommercetools.Provider.PowerShellLayer.Extensions;

internal static class PSObjectExtensions
{
    public static SerializerService GetCommercetoolsSerializer(this PSObject psObject)
    {
        object? driveObject = psObject.Properties["PSDrive"].Value;

        if (driveObject is not CommercetoolsPSDriveInfo info)
        {
            throw new Exception("Could not find serializer service");
        }

        SerializerService? serializerService = info.SerializerService;

        return serializerService ?? throw new Exception("Could not find serializer service");
    }

    public static ProjectApiRoot GetCommercetoolsProjectApiRoot(this PSObject psObject)
    {
        object? driveObject = psObject.Properties["PSDrive"].Value;

        if (driveObject is not CommercetoolsPSDriveInfo info)
        {
            throw new Exception("Could not find project api root");
        }

        ProjectApiRoot? projectApiRoot = info.ProjectApiRoot;

        return projectApiRoot ?? throw new Exception("Could not find project api root");
    }
}