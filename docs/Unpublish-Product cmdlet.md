# Unpublish-Product cmdlet

Unpublishes a Commercetools product

## Syntax

```powershell
Unpublish-Product [-Path <string[]>]
```

### Parameters

- **Path** (optional)

  Array of paths to the products to unpublish.
  If the path is ommitted the current location will be used or the path value must come from a piped drive entity container object (see samples).

### Samples

```powershell
Unpublish-Product -Path ct-abc:\products\9579e2a9-f122-4d65-b436-8b4f2ff5b0f2
```

```powershell
Unpublish-Product -Path ct-abc:\products\9579e2a9-f122-4d65-b436-8b4f2ff5b0f2, ct-abc:\products\51c09fcd-40d3-4985-95b1-5d8f712f66d5
```

```powershell
Get-ChildItem -Path ct-abc:\products | Unpublish-Product
```
