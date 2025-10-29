# Test-Path cmdlet

Tests if an entity or entity container exists in Commercetools.

## Syntax

```powershell
Test-Path [-Path <string[]>]
```

### Parameters

- **Path** (mandatory)

  The paths to check if they exist.

### Samples

```powershell
Test-Path -Path ct-abc:\orders\e81116d0-fb6a-42c2-b38a-e1550720229e
```

```powershell
Test-Path -Path ct-abc:\orders\e81116d0-fb6a-42c2-b38a-e1550720229e, ct-abc:\orders\ad2fabce-6088-4af0-bfed-2c5415b1ba13
```
