namespace PSCommercetools.Provider.FormatFileGenerator.Models.Builders;

internal sealed class EntitiesGroupBuilder
{
    private bool hasListView = true;
    private List<Property>? properties;
    private List<string>? typeNames;

    public EntitiesGroupBuilder WithTypeName(string value)
    {
        typeNames ??= [];
        typeNames.Add(value);
        return this;
    }

    public EntitiesGroupBuilder WithProperty(Action<PropertyBuilder> propertyBuilderAction)
    {
        var propertyBuilder = new PropertyBuilder();
        propertyBuilderAction.Invoke(propertyBuilder);
        Property property = propertyBuilder.Build();
        properties ??= [];
        properties.Add(property);

        return this;
    }

    public EntitiesGroupBuilder WithIdProperty()
    {
        properties ??= [];
        properties.Add(Property.CreateWithName("Id", "Id"));
        return this;
    }

    public EntitiesGroupBuilder WithVersionProperty()
    {
        properties ??= [];
        properties.Add(Property.CreateWithName("Version", "Version"));
        return this;
    }

    public EntitiesGroupBuilder WithCreatedProperty()
    {
        properties ??= [];
        properties.Add(Property.CreateWithName("Created", "CreatedAt"));
        return this;
    }

    public EntitiesGroupBuilder WithModifiedProperty()
    {
        properties ??= [];
        properties.Add(Property.CreateWithName("Modified", "LastModifiedAt"));
        return this;
    }

    public EntitiesGroupBuilder WithoutListView()
    {
        hasListView = false;

        return this;
    }

    public EntitiesGroup Build()
    {
        ArgumentNullException.ThrowIfNull(typeNames);

        return new EntitiesGroup
        {
            TypeNames = typeNames,
            Properties = properties,
            HasListView = hasListView
        };
    }
}