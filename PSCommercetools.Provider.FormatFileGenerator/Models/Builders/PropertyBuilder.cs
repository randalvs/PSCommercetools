namespace PSCommercetools.Provider.FormatFileGenerator.Models.Builders;

internal sealed class PropertyBuilder
{
    private string? label;
    private string? name;
    private string? scriptBlock;

    public PropertyBuilder WithLabel(string value)
    {
        label = value;
        return this;
    }

    public void WithName(string value)
    {
        name = value;
    }

    public void WithScriptBlock(string value)
    {
        scriptBlock = value;
    }

    public Property Build()
    {
        ArgumentNullException.ThrowIfNull(label);

        return name != null
            ? Property.CreateWithName(label, name)
            : scriptBlock != null
                ? Property.CreateWithScriptBlock(label, scriptBlock)
                : throw new ArgumentException("Either name or scriptBlock must be set.");
    }
}