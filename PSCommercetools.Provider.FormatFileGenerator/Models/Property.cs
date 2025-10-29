namespace PSCommercetools.Provider.FormatFileGenerator.Models;

internal sealed class Property
{
    private Property()
    {
    }

    public required string Label { get; init; }
    public string? Name { get; private init; }
    public string? ScriptBlock { get; private init; }

    public static Property CreateWithName(string label, string name)
    {
        return new Property
        {
            Label = label,
            Name = name
        };
    }

    public static Property CreateWithScriptBlock(string label, string scriptBlock)
    {
        return new Property
        {
            Label = label,
            ScriptBlock = scriptBlock
        };
    }
}