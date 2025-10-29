# Get-ChildItem cmdlet

Returns (a subset of) the child items of a Commercetools entity container.

## Syntax

```powershell
Get-ChildItem [-Path <string[]>]
              [-Filter <string>]
              [-Sort <string>]
              [-Expands <string[]>]
              [-Limit <uint>]
              [-OffSet <uint>]
              [-WithNoTotal ]
              [-WithCount]
```

### Parameters

- **Path** (optional)

  Array of paths to the entity containers for which the childs entities need to be fetched.
  If the path is ommitted the current location will be used or the path value must come from a piped drive entity
  container object (see samples).

- **Filter** (optional)

  Commercetools expresssion to filter results returned.
  See: [https://docs.commercetools.com/api/predicates/query](https://docs.commercetools.com/api/predicates/query)

- **Sort** (optional)

  Commercetools expression to sort the results ascending or descending.
  See: [https://docs.commercetools.com/api/general-concepts#sorting](https://docs.commercetools.com/api/general-concepts#sorting)

- **Expands** (optional)

  Commercetools expression(s) to expand the results. Returned objects will be expanded with additional data of
  referenced
  entities.
  See: [https://docs.commercetools.com/api/general-concepts#reference-expansion](https://docs.commercetools.com/api/general-concepts#reference-expansion)

- **Limit** (optional)

  Result set will be limited to number specified. Defaults to 20 and maximum is 500.
  See: [https://docs.commercetools.com/api/general-concepts#limit](https://docs.commercetools.com/api/general-concepts#limit)

- **OffSet** (optional)

  Number of items to skip in the result set.
  See [https://docs.commercetools.com/api/general-concepts#offset](https://docs.commercetools.com/api/general-concepts#offset)

- **WithNoTotal** (optional)

  Don't show totals (more efficient).

- **WithCount** (optional)

  Show totals above list of entities or in case of the root show totals next to each entity container.

### Samples

```powershell
Get-ChildItem -Path ct-abc:\products
```

```powershell
Get-ChildItem -Path ct-abc:\ | Where-Object { $_.Name -eq 'carts'} | Get-ChildItem
```

```powershell
Get-ChildItem -Path ct-abc:\carts -Filter 'customerEmail="michael-williams@ehlt.com"'
```

```powershell
Get-ChildItem -Path ct-abc:\carts -Sort 'lastModifiedAt desc'
```

```powershell
Get-ChildItem -Path ct-abc:\products -Expands 'masterData.current.categories[*]'
```

```powershell
Get-ChildItem -Path ct-abc:\products -Limit 500
```

```powershell
Get-ChildItem -Path ct-abc:\products -Offset 10
```

```powershell
Get-ChildItem -Path ct-abc:\products -WithNoTotal
```

```powershell
Get-ChildItem -Path ct-abc:\products -WithCount
```

```powershell
Get-ChildItem -Path ct-abc:\
```

```powershell
Get-ChildItem -Path ct-abc:\ -WithCount
```

### Root

Executing this cmdlet with the path parameter pointing to the root of the drive (see last two samples above) will return
a list of the entity containers that are supported by the provider. Only the '-WithCount' parameter is allowed in this
case. Providing other parameters will result in an exception. The output will look something like this:

![get-childitem on root](images\get-childitem_root.jpg)
