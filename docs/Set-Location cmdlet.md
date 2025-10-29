# Set-Location cmdlet

Changes the current path location of the drive.

## Syntax

```powershell
Set-Location [-Path <string>]
```

### Parameters

- **Path** (mandatory)

  The path to switch the current location of the drive to.

### Samples

```powershell
Set-Location -Path ct-abc:\
```

```powershell
Set-Location -Path ct-abc:\products
```

```powershell
Set-Location -Path ct-abc:\channels
```

```powershell
Set-Location -Path .\carts
```

```powershell
Set-Location -Path .\carts
```

```powershell
Set-Location -Path ..\carts
```

```powershell
Set-Location -Path ..
```
