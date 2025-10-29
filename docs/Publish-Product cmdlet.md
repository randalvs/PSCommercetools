# Publish-Product cmdlet

Publishes a Commercetools product

## Syntax

```powershell
Publish-Product [-Path <string[]>]
                [-Scope]
```

### Parameters

- **Path** (optional)

  Array of paths to the products to publish.
  If the path is ommitted the current location will be used or the path value must come from a piped drive entity container object (see samples).

- **Scope** (optional)

The scope controls which part of the product information is published. Defaults to 'All'. See: [https://docs.commercetools.com/api/projects/products#productpublishscope](https://docs.commercetools.com/api/projects/products#productpublishscope)

### Samples

```powershell
Publish-Product -Path ct-abc:\products\9579e2a9-f122-4d65-b436-8b4f2ff5b0f2
```

```powershell
Publish-Product -Path ct-abc:\products\9579e2a9-f122-4d65-b436-8b4f2ff5b0f2, ct-abc:\products\51c09fcd-40d3-4985-95b1-5d8f712f66d5
```

```powershell
Publish-Product -Path ct-abc:\products\9579e2a9-f122-4d65-b436-8b4f2ff5b0f2 -Scope Prices
```

```powershell
Publish-Product -Path ct-abc:\products\9579e2a9-f122-4d65-b436-8b4f2ff5b0f2 -Scope All
```

```powershell
Get-ChildItem -Path ct-abc:\products | Publish-Product
```
