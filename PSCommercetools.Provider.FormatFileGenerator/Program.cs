using PSCommercetools.Provider.FormatFileGenerator;

using MemoryStream memoryStream = FormatXmlGenerator.Generate(FormatDefinitions.EntitiesGroups);
using var fileStream = new FileStream(
    @"..\..\..\..\PSCommercetools.Provider\PSCommercetools.Provider.format.ps1xml",
    FileMode.Create,
    FileAccess.Write);
memoryStream.CopyTo(fileStream);