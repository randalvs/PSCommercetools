using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;

namespace PSCommercetools.Provider.Tests.Extensions;

internal static class PSObjectCollectionExtensions
{
    public static bool BaseObjectsAreAllOfType<T>(this Collection<PSObject> collection)
    {
        return collection.All(o => o.BaseObject is T);
    }

    public static List<T?> GetBaseObjects<T>(this Collection<PSObject> collection) where T : class
    {
        return collection.Select(c => c.BaseObject as T).ToList();
    }

    public static T? GetFirstBaseObjectAs<T>(this Collection<PSObject> collection)
    {
        if (collection.Count == 0)
        {
            return default;
        }

        return (T)collection[0].BaseObject ?? default;
    }
}