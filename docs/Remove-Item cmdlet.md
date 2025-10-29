# Remove-Item cmdlet

Deletes an entity from Commercetools.

## Syntax

```powershell
    Remove-Item [-Path <string[]>]
                [-Expands <string[]>]         
```

### Parameters

- **Path** (mandatory)

  Array of paths to the entities that need to be removed.

- **Expands** (optional)

  Commercetools expression(s) to expand the results. Returned objects will be expanded with additional data of
  referenced
  entities.
  See: [https://docs.commercetools.com/api/general-concepts#reference-expansion](https://docs.commercetools.com/api/general-concepts#reference-expansion)

### Samples

```powershell
Remove-Item -Path ct-abc:\channels\e81116d0-fb6a-42c2-b38a-e1550720229e
```

```powershell
Remove-Item -Path ct-abc:\channels\ad2fabce-6088-4af0-bfed-2c5415b1ba13, ct-abc:\channels\feaeb2c1-9c2f-400b-95db-bad37e74b868
```

```powershell
Get-Item -Path ct-abc:\channels\e81116d0-fb6a-42c2-b38a-e1550720229e | Remove-Item
```
