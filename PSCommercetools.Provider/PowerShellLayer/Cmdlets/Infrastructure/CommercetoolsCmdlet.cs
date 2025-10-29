using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using JetBrains.Annotations;

namespace PSCommercetools.Provider.PowerShellLayer.CmdLets.Infrastructure;

public abstract class CommercetoolsCmdlet : PSCmdlet
{
    [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ParameterSetName = "ObjectParameterSet")]
    [ValidateNotNullOrEmpty]
    public object? InputObject { get; [UsedImplicitly] set; }

    [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ParameterSetName = "PathParameterSet")]
    [Alias("PSPath")]
    [ValidateNotNullOrEmpty]
    public string[]? Paths { get; [UsedImplicitly] set; }

    private bool InputFromPipe => InputObject is PSObject && !InputFromPaths;

    private bool InputFromPaths => Paths is not null && Paths.Length != 0;

    protected override void ProcessRecord()
    {
        try
        {
            if (InputFromPipe)
            {
                PSObject psObject = InputObject as PSObject ?? throw new Exception("Could not cast object to PSObject");

                ProcessRecordByObjectParameterSet(psObject);
                return;
            }

            List<CommercetoolsDrivePath> commercetoolsPaths = ResolvePathsParameter();

            ProcessRecordByInputPathParameterSet(commercetoolsPaths);
        }
        catch (Exception exception)
        {
            WriteError(new ErrorRecord(exception, string.Empty, ErrorCategory.NotSpecified, null));
        }
    }

    protected virtual void ProcessRecordByObjectParameterSet(PSObject psObject)
    {
        throw new NotImplementedException("This cmdlet does not support processing records from the pipeline.");
    }

    internal virtual void ProcessRecordByInputPathParameterSet(List<CommercetoolsDrivePath> commercetoolsPaths)
    {
        throw new NotImplementedException("This cmdlet does not support processing records from the path.");
    }

    private List<CommercetoolsDrivePath> ResolvePathsParameter()
    {
        if (Paths is null || Paths.Length == 0)
        {
            return [];
        }

        List<CommercetoolsDrivePath> resolvedPaths = [];

        foreach (string path in Paths)
        {
            Collection<PathInfo>? pathInfos = SessionState.Path.GetResolvedPSPathFromPSPath(path);
            resolvedPaths.AddRange(pathInfos.Select(pathInfo => pathInfo.ToCommercetoolsDrivePath()).ToList());
        }

        return resolvedPaths;
    }
}