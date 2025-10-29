using PSCommercetools.Provider.Generator;

// ReSharper disable UnusedType.Global
// ReSharper disable RedundantTypeDeclarationBody

namespace PSCommercetools.Provider.Meta;

[GenerateEntity(EntityName = "InventoryEntry", EntityNamePlural = "Inventory",
    CommercetoolsSdkModelNamespaceEntityName = "Inventories")]
[GenerateSdkProxy(EntityName = "InventoryEntry", EntityNamePlural = "Inventory",
    CommercetoolsSdkModelNamespaceEntityName = "Inventories",
    CommercetoolsSdkPagedQueryResponseName = "IInventoryPagedQueryResponse")]
internal class InventoryEntry
{
}