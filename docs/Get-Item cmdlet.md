# Get-Item cmdlet

Returns a Commercetools entity.

## Syntax

```powershell
Get-ChildItem [-Path <string[]>]
              [-Expands <string>]
```

### Parameters

- **Path** (optional)

  Array of paths to the entity to be fetched.
  If the path is ommitted the current location will be used or the path value must come from a piped drive entity
  container object (see samples).

- **Expands** (optional)

  Commercetools expression(s) to expand the result. Returned object will be expanded with additional data of referenced
  entities.
  See: [https://docs.commercetools.com/api/general-concepts#reference-expansion](https://docs.commercetools.com/api/general-concepts#reference-expansion)

### Samples

```powershell
Get-Item -Path ct-abc:\products\cd7ae8a9-ad1a-40a4-a78e-4b433f716513
```

```powershell
Get-Item -Path ct-abc:\products\cd7ae8a9-ad1a-40a4-a78e-4b433f716513 -Expands 'productType'
```

```powershell
Get-ChildItem -Path ct-abc:\products | Get-Item
```
