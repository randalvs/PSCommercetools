using PSCommercetools.Provider.FormatFileGenerator.Models;
using PSCommercetools.Provider.FormatFileGenerator.Models.Builders;

namespace PSCommercetools.Provider.FormatFileGenerator;

internal static class FormatDefinitions
{
    public static List<EntitiesGroup> EntitiesGroups =>
    [
        new EntitiesGroupBuilder()
            .WithTypeName("PSCommercetools.Provider.EntityServiceLayer.Models.ProjectChildEntity")
            .WithProperty(p => p.WithLabel("Entity").WithName("Name"))
            .WithProperty(p => p.WithLabel("Count").WithName("ChildCount"))
            .WithoutListView()
            .Build(),

        new EntitiesGroupBuilder()
            .WithTypeName("commercetools.Sdk.Api.Models.Products.Product")
            .WithIdProperty()
            .WithVersionProperty()
            .WithProperty(p => p.WithLabel("Key").WithName("Key"))
            .WithProperty(p =>
                p.WithLabel("Product name").WithScriptBlock("$_.MasterData.Current.Name.Values| Select-Object -First 1"))
            .WithProperty(p => p.WithLabel("Variant count").WithScriptBlock("$_.MasterData.Current.Variants.Count + 1"))
            .WithProperty(p => p.WithLabel("Status").WithScriptBlock("$_.MasterData.Published ? \"Published\" :\"Unpublished\""))
            .WithCreatedProperty()
            .WithModifiedProperty()
            .Build(),

        new EntitiesGroupBuilder()
            .WithTypeName("commercetools.Sdk.Api.Models.Products.ProductVariant")
            .WithProperty(p => p.WithLabel("Id").WithName("Id"))
            .WithProperty(p => p.WithLabel("Sku").WithName("Sku"))
            .WithProperty(p => p.WithLabel("Key").WithName("Key"))
            .Build(),

        new EntitiesGroupBuilder()
            .WithTypeName("commercetools.Sdk.Api.Models.Categories.Category")
            .WithIdProperty()
            .WithVersionProperty()
            .WithProperty(p => p.WithLabel("Key").WithName("Key"))
            .WithProperty(p => p.WithLabel("Name").WithScriptBlock("$_.Name.Values| Select-Object -First 1"))
            .WithProperty(p => p.WithLabel("Parent Id").WithScriptBlock("$_.Parent.Id"))
            .WithCreatedProperty()
            .WithModifiedProperty()
            .Build(),

        new EntitiesGroupBuilder()
            .WithTypeName("commercetools.Sdk.Api.Models.Inventories.InventoryEntry")
            .WithIdProperty()
            .WithVersionProperty()
            .WithProperty(p => p.WithLabel("Key").WithName("Key"))
            .WithProperty(p => p.WithLabel("Sku").WithName("Sku"))
            .WithProperty(p => p.WithLabel("Quantity on stock").WithName("QuantityOnStock"))
            .WithProperty(p => p.WithLabel("Available quantity").WithName("AvailableQuantity"))
            .WithCreatedProperty()
            .WithModifiedProperty()
            .Build(),

        new EntitiesGroupBuilder()
            .WithTypeName("commercetools.Sdk.Api.Models.Carts.Cart")
            .WithIdProperty()
            .WithVersionProperty()
            .WithProperty(p => p.WithLabel("Customer email").WithName("CustomerEmail"))
            .WithProperty(p => p.WithLabel("Line item count").WithScriptBlock("$_.LineItems.Count"))
            .WithProperty(p => p.WithLabel("Cart state").WithScriptBlock("$_.CartState.JsonName"))
            .WithCreatedProperty()
            .WithModifiedProperty()
            .Build(),

        new EntitiesGroupBuilder()
            .WithTypeName("commercetools.Sdk.Api.Models.Orders.Order")
            .WithIdProperty()
            .WithVersionProperty()
            .WithProperty(p => p.WithLabel("Order number").WithName("OrderNumber"))
            .WithProperty(p => p.WithLabel("Customer email").WithName("CustomerEmail"))
            .WithProperty(p =>
                p.WithLabel("Total gross").WithScriptBlock("($_.TaxedPrice.TotalGross.CentAmount / 100).ToString(\"F2\")"))
            .WithProperty(p => p.WithLabel("Line item count").WithScriptBlock("$_.LineItems.Count"))
            .WithProperty(p => p.WithLabel("Order status").WithScriptBlock("$_.OrderState.JsonName"))
            .WithProperty(p => p.WithLabel("Payment status").WithScriptBlock("$_.PaymentState.JsonName"))
            .WithProperty(p => p.WithLabel("Shipment status").WithScriptBlock("$_.ShipmentState.JsonName"))
            .WithCreatedProperty()
            .WithModifiedProperty()
            .Build(),

        new EntitiesGroupBuilder()
            .WithTypeName("commercetools.Sdk.Api.Models.Customers.Customer")
            .WithIdProperty()
            .WithVersionProperty()
            .WithProperty(p => p.WithLabel("Customer key").WithName("Key"))
            .WithProperty(p => p.WithLabel("First name").WithName("FirstName"))
            .WithProperty(p => p.WithLabel("Middle name").WithName("MiddleName"))
            .WithProperty(p => p.WithLabel("Last name").WithName("LastName"))
            .WithProperty(p => p.WithLabel("Email").WithName("Email"))
            .WithCreatedProperty()
            .WithModifiedProperty()
            .Build(),

        new EntitiesGroupBuilder()
            .WithTypeName("commercetools.Sdk.Api.Models.CustomerGroups.CustomerGroup")
            .WithIdProperty()
            .WithVersionProperty()
            .WithProperty(p => p.WithLabel("Name").WithName("Name"))
            .WithCreatedProperty()
            .WithModifiedProperty()
            .Build(),

        new EntitiesGroupBuilder()
            .WithTypeName("commercetools.Sdk.Api.Models.BusinessUnits.Company")
            .WithTypeName("commercetools.Sdk.Api.Models.BusinessUnits.Division")
            .WithIdProperty()
            .WithVersionProperty()
            .WithProperty(p => p.WithLabel("Business unit key").WithName("Key"))
            .WithProperty(p => p.WithLabel("Name").WithName("Name"))
            .WithProperty(p => p.WithLabel("Status").WithScriptBlock("$_.Status.JsonName"))
            .WithCreatedProperty()
            .WithModifiedProperty()
            .Build(),

        new EntitiesGroupBuilder()
            .WithTypeName("commercetools.Sdk.Api.Models.CustomObjects.CustomObject")
            .WithIdProperty()
            .WithVersionProperty()
            .WithProperty(p => p.WithLabel("Key").WithName("Key"))
            .WithProperty(p => p.WithLabel("Container").WithName("Container"))
            .WithCreatedProperty()
            .WithModifiedProperty()
            .Build(),

        new EntitiesGroupBuilder()
            .WithTypeName("commercetools.Sdk.Api.Models.ApiClients.ApiClient")
            .WithProperty(p => p.WithLabel("Name").WithName("Name"))
            .WithProperty(p => p.WithLabel("Id").WithName("Id"))
            .WithProperty(p => p.WithLabel("Secret").WithName("Secret"))
            .WithProperty(p => p.WithLabel("Last used").WithName("LastUsed"))
            .WithCreatedProperty()
            .WithProperty(p => p.WithLabel("Scope").WithName("Scope"))
            .Build(),

        new EntitiesGroupBuilder()
            .WithTypeName("commercetools.Sdk.Api.Models.Channels.Channel")
            .WithIdProperty()
            .WithVersionProperty()
            .WithProperty(p => p.WithLabel("Key").WithName("Key"))
            .WithProperty(p => p.WithLabel("Name").WithScriptBlock("$_.Name.Values| Select-Object -First 1"))
            .WithCreatedProperty()
            .WithModifiedProperty()
            .Build(),

        new EntitiesGroupBuilder()
            .WithTypeName("commercetools.Sdk.Api.Models.Stores.Store")
            .WithIdProperty()
            .WithVersionProperty()
            .WithProperty(p => p.WithLabel("Key").WithName("Key"))
            .WithProperty(p => p.WithLabel("Name").WithScriptBlock("$_.Name.Values| Select-Object -First 1"))
            .WithCreatedProperty()
            .WithModifiedProperty()
            .Build(),

        new EntitiesGroupBuilder()
            .WithTypeName("commercetools.Sdk.Api.Models.TaxCategories.TaxCategory")
            .WithIdProperty()
            .WithVersionProperty()
            .WithProperty(p => p.WithLabel("Key").WithName("Key"))
            .WithProperty(p => p.WithLabel("Name").WithName("Name"))
            .WithCreatedProperty()
            .WithModifiedProperty()
            .Build(),

        new EntitiesGroupBuilder()
            .WithTypeName("commercetools.Sdk.Api.Models.ShoppingLists.ShoppingList")
            .WithIdProperty()
            .WithVersionProperty()
            .WithProperty(p => p.WithLabel("Key").WithName("Key"))
            .WithProperty(p => p.WithLabel("Name").WithScriptBlock("$_.Name.Values| Select-Object -First 1"))
            .WithProperty(p => p.WithLabel("Line item count").WithScriptBlock("$_.LineItems.Count"))
            .WithCreatedProperty()
            .WithModifiedProperty()
            .Build(),

        new EntitiesGroupBuilder()
            .WithTypeName("commercetools.Sdk.Api.Models.ProductSelections.ProductSelection")
            .WithIdProperty()
            .WithVersionProperty()
            .WithProperty(p => p.WithLabel("Key").WithName("Key"))
            .WithProperty(p => p.WithLabel("Name").WithScriptBlock("$_.Name.Values| Select-Object -First 1"))
            .WithProperty(p => p.WithLabel("Product count").WithName("ProductCount"))
            .WithCreatedProperty()
            .WithModifiedProperty()
            .Build(),

        new EntitiesGroupBuilder()
            .WithTypeName("commercetools.Sdk.Api.Models.ShippingMethods.ShippingMethod")
            .WithIdProperty()
            .WithVersionProperty()
            .WithProperty(p => p.WithLabel("Name").WithName("Name"))
            .WithProperty(p => p.WithLabel("Active").WithName("Active"))
            .WithProperty(p => p.WithLabel("Default").WithName("IsDefault"))
            .WithCreatedProperty()
            .WithModifiedProperty()
            .Build(),

        new EntitiesGroupBuilder()
            .WithTypeName("commercetools.Sdk.Api.Models.StandalonePrices.StandalonePrice")
            .WithIdProperty()
            .WithVersionProperty()
            .WithProperty(p => p.WithLabel("Key").WithName("Key"))
            .WithProperty(p => p.WithLabel("Sku").WithName("Sku"))
            .WithCreatedProperty()
            .WithModifiedProperty()
            .Build(),

        new EntitiesGroupBuilder()
            .WithTypeName("commercetools.Sdk.Api.Models.ProductDiscounts.ProductDiscount")
            .WithIdProperty()
            .WithVersionProperty()
            .WithProperty(p => p.WithLabel("Key").WithName("Key"))
            .WithProperty(p => p.WithLabel("Name").WithName("Name"))
            .WithCreatedProperty()
            .WithModifiedProperty()
            .Build(),

        new EntitiesGroupBuilder()
            .WithTypeName("commercetools.Sdk.Api.Models.CartDiscounts.CartDiscount")
            .WithIdProperty()
            .WithVersionProperty()
            .WithProperty(p => p.WithLabel("Key").WithName("Key"))
            .WithProperty(p => p.WithLabel("Name").WithScriptBlock("$_.Name.Values| Select-Object -First 1"))
            .WithProperty(p => p.WithLabel("Active").WithName("IsActive"))
            .WithCreatedProperty()
            .WithModifiedProperty()
            .Build(),

        new EntitiesGroupBuilder()
            .WithTypeName("commercetools.Sdk.Api.Models.DiscountCodes.DiscountCode")
            .WithIdProperty()
            .WithVersionProperty()
            .WithProperty(p => p.WithLabel("Key").WithName("Key"))
            .WithProperty(p => p.WithLabel("Code").WithName("Code"))
            .WithProperty(p => p.WithLabel("Active").WithName("IsActive"))
            .WithCreatedProperty()
            .WithModifiedProperty()
            .Build(),

        new EntitiesGroupBuilder()
            .WithTypeName("commercetools.Sdk.Api.Models.ProductTypes.ProductType")
            .WithIdProperty()
            .WithVersionProperty()
            .WithProperty(p => p.WithLabel("Key").WithName("Key"))
            .WithProperty(p => p.WithLabel("Name").WithName("Name"))
            .WithCreatedProperty()
            .WithModifiedProperty()
            .Build(),

        new EntitiesGroupBuilder()
            .WithTypeName("commercetools.Sdk.Api.Models.AttributeGroups.AttributeGroup")
            .WithIdProperty()
            .WithVersionProperty()
            .WithProperty(p => p.WithLabel("Key").WithName("Key"))
            .WithCreatedProperty()
            .WithModifiedProperty()
            .Build(),

        new EntitiesGroupBuilder()
            .WithTypeName("commercetools.Sdk.Api.Models.Payments.Payment")
            .WithIdProperty()
            .WithVersionProperty()
            .WithProperty(p => p.WithLabel("Key").WithName("Key"))
            .WithProperty(p => p.WithLabel("Method").WithScriptBlock("$_.PaymentMethodInfo.Method"))
            .WithCreatedProperty()
            .WithModifiedProperty()
            .Build(),

        new EntitiesGroupBuilder()
            .WithTypeName("commercetools.Sdk.Api.Models.States.State")
            .WithIdProperty()
            .WithVersionProperty()
            .WithProperty(p => p.WithLabel("Key").WithName("Key"))
            .WithProperty(p => p.WithLabel("Type").WithScriptBlock("$_.Type.JsonName"))
            .WithProperty(p => p.WithLabel("Built in").WithName("BuiltIn"))
            .WithCreatedProperty()
            .WithModifiedProperty()
            .Build(),

        new EntitiesGroupBuilder()
            .WithTypeName("commercetools.Sdk.Api.Models.Types.Type")
            .WithIdProperty()
            .WithVersionProperty()
            .WithProperty(p => p.WithLabel("Key").WithName("Key"))
            .WithProperty(p => p.WithLabel("Name").WithScriptBlock("$_.Name.Values| Select-Object -First 1"))
            .WithCreatedProperty()
            .WithModifiedProperty()
            .Build(),

        new EntitiesGroupBuilder()
            .WithTypeName("commercetools.Sdk.Api.Models.Zones.Zone")
            .WithIdProperty()
            .WithVersionProperty()
            .WithProperty(p => p.WithLabel("Name").WithName("Name"))
            .WithProperty(p => p.WithLabel("Key").WithName("Key"))
            .WithCreatedProperty()
            .WithModifiedProperty()
            .Build()
    ];
}