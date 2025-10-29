using System;
using System.Collections.Generic;
using System.Management.Automation;
using commercetools.Sdk.Api.Client;
using commercetools.Sdk.Api.Models.Carts;
using commercetools.Sdk.Api.Models.Products;
using PSCommercetools.Provider.EntityServiceLayer.Services;
using PSCommercetools.Provider.PowerShellLayer.CmdLets.Infrastructure;
using PSCommercetools.Provider.PowerShellLayer.Drive;
using PSCommercetools.Provider.PowerShellLayer.Extensions;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global

namespace PSCommercetools.Provider.PowerShellLayer.CmdLets;

[Cmdlet("Publish", "Product", DefaultParameterSetName = "PathParameterSet")]
public class PublishProductCmdlet : CommercetoolsCmdlet
{
    [Parameter(Mandatory = false)]
    [ValidateNotNullOrEmpty]
    public PublishScope Scope { get; set; } = PublishScope.All;

    protected override void ProcessRecordByObjectParameterSet(PSObject psObject)
    {
        var product = psObject.BaseObject as IProduct;

        ArgumentNullException.ThrowIfNull(product);

        ProjectApiRoot projectApiRoot = psObject.GetCommercetoolsProjectApiRoot();

        PublishProduct(projectApiRoot, product);
    }

    internal override void ProcessRecordByInputPathParameterSet(List<CommercetoolsDrivePath> commercetoolsPaths)
    {
        commercetoolsPaths.ForEach(commercetoolsPath =>
        {
            CommercetoolsPSDriveInfo drive = commercetoolsPath.CommercetoolsPSDriveInfo;
            CommercetoolsCmdletProvider provider = drive.CommercetoolsCmdletProvider;

            IBaseEntityService commercetoolsEntityService = provider.EntityServiceFactory.CreateFromPath(commercetoolsPath);

            if (commercetoolsEntityService is not IEntityService entityService)
            {
                throw new ArgumentException("Error resolving entity service");
            }

            var product = entityService.Entity as IProduct;

            ArgumentNullException.ThrowIfNull(product);

            ProjectApiRoot projectApiRoot = drive.ProjectApiRoot;

            PublishProduct(projectApiRoot, product);
        });
    }

    private void PublishProduct(ProjectApiRoot projectApiRoot, IProduct product)
    {
        _ = projectApiRoot.Products().WithId(product.Id).Post(new ProductUpdate
        {
            Version = product.Version,
            Actions =
            [
                new ProductPublishAction
                {
                    Scope = Scope == PublishScope.Prices
                        ? IProductPublishScope.Prices
                        : IProductPublishScope.All
                }
            ]
        }).ExecuteAsync().Result;
    }
}