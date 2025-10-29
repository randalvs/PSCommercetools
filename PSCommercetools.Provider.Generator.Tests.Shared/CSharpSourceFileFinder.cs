using System.Reflection;

namespace PSCommercetools.Provider.Generator.Tests.Shared;

public sealed class CSharpSourceFileFinder
{
    private readonly string startDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ??
                                             throw new Exception("Cannot find start location");

    public CSharpSourceFileFinderResult Find(string className)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(className);

        var fileName = $"{className}.cs";

        var currentDir = new DirectoryInfo(startDirectory);

        while (currentDir != null)
        {
            string? found = SearchInDirectoryAndSubdirectories(currentDir.FullName, fileName);
            if (found != null)
            {
                return new CSharpSourceFileFinderResult(found);
            }

            currentDir = currentDir.Parent;
        }

        throw new Exception($"Could not find file for class '{className}' in any of the source directories.");
    }

    private static string? SearchInDirectoryAndSubdirectories(string directory, string fileName)
    {
        try
        {
            string candidate = Path.Combine(directory, fileName);
            if (File.Exists(candidate))
            {
                return candidate;
            }

            foreach (string subdir in Directory.GetDirectories(directory))
            {
                candidate = Path.Combine(subdir, fileName);
                if (File.Exists(candidate))
                {
                    return candidate;
                }

                string? deeper = SearchInDirectoryAndSubdirectories(subdir, fileName);
                if (deeper != null)
                {
                    return deeper;
                }
            }
        }
        catch (UnauthorizedAccessException)
        {
            // Skip directories we cannot access
        }

        return null;
    }
}