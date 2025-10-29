using System;
using commercetools.Sdk.Api.Client;
using commercetools.Sdk.Api.Models.Common;
using commercetools.Sdk.Api.Serialization;
using PSCommercetools.Provider.EntityServiceLayer;

namespace PSCommercetools.Provider.SdkProxyLayer;

public interface ISdkProxy
    // ReSharper disable once RedundantTypeDeclarationBody
{
}

public interface ISdkProxy<T> : ISdkProxy where T : IBaseResource
{
    Func<ProjectApiRoot, string[]?, string?, long?, long?, string?, bool?, EntitiesContainer<T>> GetFunc { get; }
    Func<ProjectApiRoot, string, bool> ExistsByIdFunc { get; }
    Func<ProjectApiRoot, string, string[]?, T> GetByIdFunc { get; }
    Func<ProjectApiRoot, T, string[]?, T> DeleteFunc { get; }
    Func<ProjectApiRoot, SerializerService, string, string[]?, T> CreateFunc { get; }
    Func<ProjectApiRoot, SerializerService, T, object, string[]?, T> UpdateFunc { get; }
}