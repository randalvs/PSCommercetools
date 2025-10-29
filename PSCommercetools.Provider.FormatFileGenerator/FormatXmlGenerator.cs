using System.Text;
using System.Xml;
using PSCommercetools.Provider.FormatFileGenerator.Models;

namespace PSCommercetools.Provider.FormatFileGenerator;

internal static class FormatXmlGenerator
{
    public static MemoryStream Generate(List<EntitiesGroup> entitiesGroups)
    {
        var memoryStream = new MemoryStream();
        var xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

        xmlTextWriter.Formatting = Formatting.Indented;

        xmlTextWriter.WriteStartDocument();

        xmlTextWriter.WriteStartElement("Configuration");
        xmlTextWriter.WriteStartElement("ViewDefinitions");

        foreach (EntitiesGroup entitiesGroup in entitiesGroups)
        {
            WriteTableView(xmlTextWriter, entitiesGroup);
            if (entitiesGroup.HasListView)
            {
                WriteListView(xmlTextWriter, entitiesGroup);
            }
        }

        xmlTextWriter.WriteEndElement();
        xmlTextWriter.WriteEndElement();

        xmlTextWriter.WriteEndDocument();
        xmlTextWriter.Flush();

        memoryStream.Seek(0, SeekOrigin.Begin);

        return memoryStream;
    }

    private static void WriteTableView(XmlTextWriter xmlTextWriter, EntitiesGroup entitiesGroup)
    {
        xmlTextWriter.WriteStartElement("View");

        WriteViewName(xmlTextWriter, entitiesGroup);
        WriteViewSelectedBy(xmlTextWriter, entitiesGroup);

        xmlTextWriter.WriteStartElement("TableControl");
        xmlTextWriter.WriteStartElement("TableHeaders");

        foreach (Property property in entitiesGroup.Properties ?? [])
        {
            xmlTextWriter.WriteStartElement("TableColumnHeader");
            WriteLabel(xmlTextWriter, property);
            xmlTextWriter.WriteEndElement();
        }

        xmlTextWriter.WriteEndElement();

        xmlTextWriter.WriteStartElement("TableRowEntries");
        xmlTextWriter.WriteStartElement("TableRowEntry");
        xmlTextWriter.WriteStartElement("TableColumnItems");

        foreach (Property property in entitiesGroup.Properties ?? [])
        {
            WriteTableColumnItem(xmlTextWriter, property);
        }

        xmlTextWriter.WriteEndElement();
        xmlTextWriter.WriteEndElement();
        xmlTextWriter.WriteEndElement();


        xmlTextWriter.WriteEndElement();
        xmlTextWriter.WriteEndElement();
    }

    private static void WriteLabel(XmlTextWriter xmlTextWriter, Property property)
    {
        xmlTextWriter.WriteStartElement("Label");
        xmlTextWriter.WriteString(property.Label);
        xmlTextWriter.WriteEndElement();
    }

    private static void WriteTableColumnItem(XmlTextWriter xmlTextWriter, Property property)
    {
        xmlTextWriter.WriteStartElement("TableColumnItem");
        if (property.Name != null)
        {
            WritePropertyName(xmlTextWriter, property);
        }

        if (property.ScriptBlock != null)
        {
            WriteScriptBlock(xmlTextWriter, property);
        }

        xmlTextWriter.WriteEndElement();
    }

    private static void WriteListView(XmlTextWriter xmlTextWriter, EntitiesGroup entitiesGroup)
    {
        xmlTextWriter.WriteStartElement("View");

        WriteViewName(xmlTextWriter, entitiesGroup);
        WriteViewSelectedBy(xmlTextWriter, entitiesGroup);

        xmlTextWriter.WriteStartElement("ListControl");
        xmlTextWriter.WriteStartElement("ListEntries");
        xmlTextWriter.WriteStartElement("ListEntry");
        xmlTextWriter.WriteStartElement("ListItems");

        foreach (Property property in entitiesGroup.Properties ?? [])
        {
            xmlTextWriter.WriteStartElement("ListItem");
            WriteLabel(xmlTextWriter, property);
            if (property.Name != null)
            {
                WritePropertyName(xmlTextWriter, property);
            }

            if (property.ScriptBlock != null)
            {
                WriteScriptBlock(xmlTextWriter, property);
            }

            xmlTextWriter.WriteEndElement();
        }

        xmlTextWriter.WriteEndElement();
        xmlTextWriter.WriteEndElement();
        xmlTextWriter.WriteEndElement();
        xmlTextWriter.WriteEndElement();
        xmlTextWriter.WriteEndElement();
    }

    private static void WriteViewName(XmlTextWriter xmlTextWriter, EntitiesGroup entitiesGroup)
    {
        xmlTextWriter.WriteStartElement("Name");
        xmlTextWriter.WriteString(entitiesGroup.TypeNames.First().Split('.').Last());
        xmlTextWriter.WriteEndElement();
    }

    private static void WritePropertyName(XmlTextWriter xmlTextWriter, Property property)
    {
        xmlTextWriter.WriteStartElement("PropertyName");
        xmlTextWriter.WriteString(property.Name);
        xmlTextWriter.WriteEndElement();
    }

    private static void WriteScriptBlock(XmlTextWriter xmlTextWriter, Property property)
    {
        xmlTextWriter.WriteStartElement("ScriptBlock");
        xmlTextWriter.WriteString(property.ScriptBlock);
        xmlTextWriter.WriteEndElement();
    }

    private static void WriteViewSelectedBy(XmlTextWriter xmlTextWriter, EntitiesGroup entitiesGroup)
    {
        xmlTextWriter.WriteStartElement("ViewSelectedBy");

        foreach (string typeName in entitiesGroup.TypeNames)
        {
            xmlTextWriter.WriteStartElement("TypeName");
            xmlTextWriter.WriteString(typeName);
            xmlTextWriter.WriteEndElement();
        }

        xmlTextWriter.WriteEndElement();
    }
}