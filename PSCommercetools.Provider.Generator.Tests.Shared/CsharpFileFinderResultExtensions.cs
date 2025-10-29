using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace PSCommercetools.Provider.Generator.Tests.Shared;

public static class CSharpSourceFileFinderExtensions
{
    public static SyntaxTree ToSyntaxTree(this CSharpSourceFileFinderResult result)
    {
        string code = File.ReadAllText(result.FilePath, Encoding.UTF8);

        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);

        return syntaxTree;
    }
}