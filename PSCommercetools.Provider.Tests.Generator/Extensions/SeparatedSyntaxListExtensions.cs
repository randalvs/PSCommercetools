using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PSCommercetools.Provider.Tests.Generator.Extensions;

public static class SeparatedSyntaxListExtensions
{
    public static T? GetAttributeValue<T>(this SeparatedSyntaxList<AttributeArgumentSyntax> source, string attributePropertyName)
    {
        AttributeArgumentSyntax? argument = source.FirstOrDefault(f => f.NameEquals?.Name.ToString() == attributePropertyName);

        if (argument?.Expression is not LiteralExpressionSyntax literal)
        {
            return default;
        }

        object? tokenValue = literal.Token.Value;

        return tokenValue is null ? default : (T)tokenValue;
    }
}