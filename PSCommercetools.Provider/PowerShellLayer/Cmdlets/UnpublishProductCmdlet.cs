using System;
using System.Collections.Generic;
using System.Management.Automation;
using commercetools.Sdk.Api.Client;
using commercetools.Sdk.Api.Models.Products;
using PSCommercetools.Provider.EntityServiceLayer.Services;
using PSCommercetools.Provider.PowerShellLayer.CmdLets.Infrastructure;
using PSCommercetools.Provider.PowerShellLayer.Drive;
using PSCommercetools.Provider.PowerShellLayer.Extensions;

// ReSharper disable UnusedType.Global

namespace PSCommercetools.Provider.PowerShellLayer.CmdLets;

[Cmdlet("Unpublish", "Product", DefaultParameterSetName = "PathParameterSet")]
public class UnpublishProductCmdlet : CommercetoolsCmdlet
{
    protected override void ProcessRecordByObjectParameterSet(PSObject psObject)
    {
        var product = psObject.BaseObject as IProduct;
        ArgumentNullException.ThrowIfNull(product);

        ProjectApiRoot projectApiRoot = psObject.GetCommercetoolsProjectApiRoot();

        UnpublishProduct(projectApiRoot, product);
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

            UnpublishProduct(projectApiRoot, product);
        });
    }

    private static void UnpublishProduct(ProjectApiRoot projectApiRoot, IProduct product)
    {
        _ = projectApiRoot.Products().WithId(product.Id).Post(new ProductUpdate
        {
            Version = product.Version,
            Actions = [new ProductUnpublishAction()]
        }).ExecuteAsync().Result;
    }
}