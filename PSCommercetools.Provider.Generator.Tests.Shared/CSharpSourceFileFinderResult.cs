namespace PSCommercetools.Provider.Generator.Tests.Shared;

public sealed class CSharpSourceFileFinderResult
{
    internal CSharpSourceFileFinderResult(string filePath)
    {
        FilePath = filePath;
    }

    public string FilePath { get; }
}