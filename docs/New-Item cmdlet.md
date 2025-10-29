# New-Item cmdlet

Creates a new Commercetools entity.

## Syntax

```powershell
New-Item [-Path <string>]
         [-Value <string>]
         [-Expands <string[]>]
```

### Parameters

- **Path** (mandatory)

  Path to the entity container that is the parent of the entity to be created.

- **Value** (mandatory)

  Json representation of the entity draft to create.

- **Expands** (optional)

  Commercetools expression(s) to expand the results. Returned objects will be expanded with additional data of
  referenced entities.
  See: [https://docs.commercetools.com/api/general-concepts#reference-expansion](https://docs.commercetools.com/api/general-concepts#reference-expansion)

### Samples

```powershell
New-Item -Path ct-abc:\channels -Value '{"key" : "test-channel", "name" : {"en-US" : "Test Channel"}}'
```

```powershell
New-Item -Path ct-abc:\apiclients -Value '{"key" : "test-api-client"}'
```
