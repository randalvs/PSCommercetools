using System;
using System.Collections.Generic;
using System.Management.Automation;
using commercetools.Sdk.Api.Client;
using commercetools.Sdk.Api.Models.Common;
using commercetools.Sdk.Api.Models.CustomObjects;
using commercetools.Sdk.Api.Serialization;
using PSCommercetools.Provider.EntityServiceLayer;
using PSCommercetools.Provider.EntityServiceLayer.Generated;
using PSCommercetools.Provider.SdkProxyLayer;

namespace PSCommercetools.Provider.RepositoryLayer;

public sealed class CommercetoolsEntityRepository
{
    private readonly ProjectApiRoot projectApiRoot;
    private readonly SerializerService serializerService;

    public CommercetoolsEntityRepository(ProjectApiRoot projectApiRoot, SerializerService serializerService)
    {
        this.projectApiRoot = projectApiRoot;
        this.serializerService = serializerService;
    }

    private static Dictionary<Type, ISdkProxy> SdkProxies
    {
        get
        {
            Dictionary<Type, ISdkProxy> sdkProxiesDictionary = Entities.SdkProxiesDictionary;
            if (!sdkProxiesDictionary.ContainsKey(typeof(ICustomObject)))
            {
                sdkProxiesDictionary.Add(typeof(ICustomObject), new CustomObjectSdkProxy());
            }

            return sdkProxiesDictionary;
        }
    }

    public T Remove<T>(T entity, long? version, string[]? expandClauses) where T : IBaseResource
    {
        T removedEntity = GetTypedSdkProxy<T>().DeleteFunc(projectApiRoot, entity, version, expandClauses);
        return removedEntity;
    }

    public bool ExistsById<T>(string id) where T : IBaseResource
    {
        return GetTypedSdkProxy<T>().ExistsByIdFunc(projectApiRoot, id);
    }

    public T GetById<T>(string id, string[]? expandClauses) where T : IBaseResource
    {
        return GetTypedSdkProxy<T>().GetByIdFunc(projectApiRoot, id, expandClauses);
    }

    public EntitiesContainer<T> Get<T>(
        string[]? expandClauses,
        string? whereClause,
        long? limitClause,
        long? offsetClause,
        string? sortClause,
        bool? withCount) where T : IBaseResource
    {
        return GetTypedSdkProxy<T>().GetFunc(projectApiRoot, expandClauses, whereClause, limitClause, offsetClause,
            sortClause, withCount);
    }

    public T Create<T>(object newItemValue, string[]? expandClauses) where T : IBaseResource
    {
        string serializedResource = newItemValue switch
        {
            string stringValue => stringValue,
            PSObject psObject => serializerService.Serialize(psObject.BaseObject),
            _ => throw new ArgumentException("Invalid parameter provided.")
        };

        return GetTypedSdkProxy<T>().CreateFunc(projectApiRoot, serializerService, serializedResource, expandClauses);
    }

    public T Update<T>(T entity, long? version, object actions, string[]? expandClauses) where T : IBaseResource
    {
        T updatedEntity = GetTypedSdkProxy<T>()
            .UpdateFunc(projectApiRoot, serializerService, entity, version, actions, expandClauses);
        return updatedEntity;
    }

    private static ISdkProxy<T> GetTypedSdkProxy<T>() where T : IBaseResource
    {
        if (!SdkProxies.TryGetValue(typeof(T), out ISdkProxy? sdkProxy))
        {
            throw new NotImplementedException(
                $"CommercetoolsEntityRepository repository not implemented for {typeof(T).Name}.");
        }

        return (ISdkProxy<T>)sdkProxy;
    }
}