# ConvertTo-CtJson cmdlet

Converts a Commercetools entity to JSON representation.

> **_!_** This cmdlet works similar to the cmdlet [ConvertTo-Json](https://learn.microsoft.com/en-us/powershell/module/microsoft.powershell.utility/convertto-json) from Microsoft.PowerShell.Utility module but uses a configured json serializer from the Commercetools SDK to correctly convert Commercetools objects to Json.

## Syntax

```powershell
ConvertTo-CtJson [-Path <string[]>]
                 [-Prettify]
```

### Parameters

- **Path** (optional)

  Array of paths to the entity to convert to Json.
  If the path is ommitted the current location will be used or the path value must come from a piped drive entity container object (see samples).

- **Prettify** (optional)

Pretty prints the Json in a human readable format. Default is true.

### Samples

```powershell
ConvertTo-CtJson -Path ct-abc:\channel\bccdc935-f3d3-498c-8873-3b72e74a32d4
```

```powershell
ConvertTo-CtJson -Path ct-abc:\channels\bccdc935-f3d3-498c-8873-3b72e74a32d4, ct-abc:\channels\c5d5072a-2604-4c09-a9e4-ea904875a55e
```

```powershell
ConvertTo-CtJson -Path ct-abc:\channels\bccdc935-f3d3-498c-8873-3b72e74a32d4 -Prettify:$false
```

```powershell
Get-ChildItem -Path ct-abc:\channels | ConvertTo-CtJson -Prettify:$false | Set-Clipboard
```
