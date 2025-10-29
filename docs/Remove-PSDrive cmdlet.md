# Remove-PSDrive cmdlet

Removes a drive pointing to the API of a Commercetools project.

## Syntax

```powershell
Remove-PSDrive [-Name <string>]
```

### Parameters

- **Name** (optional)

  The name of the drive to remove. When omitted the input of this cmdlet must come from a piped drive object (see samples).

### Samples

```powershell
Remove-PSDrive -Name ct-abc
```

```powershell
Get-PsDrive ct-abc | Remove-PSDrive
```
